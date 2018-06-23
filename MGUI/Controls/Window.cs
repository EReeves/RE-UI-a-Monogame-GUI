using MGUI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MGUI.Controls
{
    /// <inheritdoc />
    /// <summary>
    /// Draggable window control.
    /// </summary>
    public class Window : Control
    {


        //We need to invalidate the window's bar rectangle to save instantiating a new one every frame.
        
        private Rectangle bounds;
        public override Rectangle Bounds
        {
            get => bounds;
            set { bounds = value;
                Invalidate();
            }
        }

        private int titleBarHeight = 20;
        public int TitleBarHeight
        {
            get => titleBarHeight;
            set { 
                titleBarHeight = value;
                Invalidate();
            }
        }
        private Rectangle BarRectangle { get; set; }

        public bool Draggable { get; set; } = true;

        public override void Invalidate()
        {
            BarRectangle = new Rectangle(CanvasBounds.X,CanvasBounds.Y,Bounds.Width,TitleBarHeight);
            foreach (var child in Children)
            {
                child.Offset = new Point(0, TitleBarHeight);
            }

            base.Invalidate();
            
            //Cache nine patch
            var texture = Canvas.SourceRectangles["windowBackground"];
            NinePatchCache = Canvas.RenderTools.CalculateNinePatch(texture.sourceRect, CanvasBounds, texture.ninePatch);
        }
        
        public override void Layout()
        {

            Invalidate();
        }

        public override void Update(GameTime gameTime)
        {
            DragInput();
            base.Update(gameTime);
        }

        //Window drag input state.
        private bool lMouseDown;
        private Point lastPosition;
        
        //Handles mouse input/window drag.
        private void DragInput()
        {
            if (!Draggable) return;
            
            var mouseState = Mouse.GetState();
            var lDown = mouseState.LeftButton == ButtonState.Pressed;
            if (!lDown)
            {
                lMouseDown = false;
                return;
            }
            
            var mouseRect = new Rectangle(mouseState.X,mouseState.Y,1,1);

            
            if (!lMouseDown)
            {
                if (!mouseRect.Intersects(BarRectangle)) return; //Drag on the bar only.
                //First pressed
                lastPosition = mouseState.Position;
                lMouseDown = true;
                return;
            }

            var newRect = bounds;
            var difference = mouseState.Position - lastPosition;
            var window = Canvas.Bounds;
            newRect.X += difference.X;
            newRect.Y += difference.Y;

           //Clamp to Canvas bounds.
            if (window.X > newRect.X)
                newRect.X = window.X;
            if (newRect.Right > window.Width)
                newRect.X = window.Right - bounds.Width;
            if (window.Bottom < newRect.Bottom)
                newRect.Y = window.Bottom - bounds.Height;
            if (newRect.Y < window.Y)
                newRect.Y = window.Y;

            
            Bounds = newRect;
            
            Invalidate();
            //Reset state
            lastPosition = mouseState.Position;
        }
        //Children draw from the bottom of the window bar.
        public override void Draw(SpriteBatch batcher)
        {
            Canvas.RenderTools.DrawNinePatch(batcher,Canvas.SpriteSheet,NinePatchCache.SourcePatches, NinePatchCache.DestinationPatches, Color);
            
            //Draw children minding the bar.
            foreach (var control in Children)
            {
                control.Draw(batcher);
            }
            
        }

        public Window(Canvas canvas) : base(canvas)
        {
        }

        public Window(IControl parent) : base(parent)
        {
        }
    }
}