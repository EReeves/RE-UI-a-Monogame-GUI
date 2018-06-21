using MGUI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MGUI.Controls
{
    public class Window : Control
    {
        public Window(Canvas canvas) : base(canvas)
        {
        }
        
        //We need to invalidate the window's bar rectangle to save instantiating a new one every frame.
        
        private Rectangle bounds;
        public override Rectangle Bounds
        {
            get => bounds;
            set { bounds = value;
                Invalidate();
            }
        }

        private int barHeight = 30;
        public int BarHeight
        {
            get => barHeight;
            set { 
                barHeight = value;
                Invalidate();
            }
        }
        private Rectangle BarRectangle { get; set; }

        public override void Invalidate()
        {
            BarRectangle = new Rectangle(CanvasBounds.X,CanvasBounds.Y,Bounds.Width,BarHeight);
            foreach (var child in Children)
            {
                child.Offset = new Point(0, BarHeight);
            }
            
            base.Invalidate();
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
            //Offset if we have a parent or have an offset.
            var destRect = CanvasBounds;
            
            //Draw self
            batcher.Draw(Canvas.SpriteSheet, destRect, Color);
            
            //Draw children minding the bar.
            foreach (var control in Children)
            {
                control.Draw(batcher);
            }
            
            //Bar
            batcher.Draw(Canvas.SpriteSheet,BarRectangle,Color.Black * 0.5f);
        }
    }
}