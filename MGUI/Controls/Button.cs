using System;
using System.Data;
using MGUI.Controls.Layout;
using MGUI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MGUI.Controls
{
    /// <summary>
    /// A basic button.
    /// </summary>
    public class Button : PaddedControl
    {
        public bool Clicked { get; private set; } = false;
        private bool ClickedPrevious { get; set; } = false;
        public Color ClickedTintColor { get; set; } = new Color(180,180,180);
        public Color HoverTintColor { get; set; } = Color.White;

        protected Color actualColor;
        protected Color color = new Color(200,200,200);
        
        //Textures
        private (Rectangle[] SourcePatches, Rectangle[] DestinationPatches) NinePatchCacheButtonDown;
        public string ButtonUpTexture { get; set; } = "buttonup";
        public string ButtonDownTexture { get; set; } = "buttondown";

        public bool ToggleButton { get; set; } = false;
        
        
        public override Color Color
        {
            get => color;
            set
            {
                actualColor = value;
                color = value;
            }
        }

        /// <summary>
        /// This offset will be applied to the label text when the button is pressed.
        /// For example, by default the button gets smaller as it is depressed, so the label needs to move down or it looks weird.
        /// </summary>
        public int ButtonClickYOffset { get; set; } = 5;

        public Label Label { get; set; }
        private string text = string.Empty;


        public string Text
        {
            get => text;
            set
            {
                text = value;
                if(Label!= null)
                    Label.Text = value;            
            }
        }

        public event EventHandler OnClick;
        


        public override void Update(GameTime gameTime)
        {
            ClickUpdate();
            base.Update(gameTime);
        }

        public override void Invalidate()
        {
            actualColor = color;
            
            if (Label == null)
            {
                Label = new Label(Canvas) { Text = string.Empty }; //Label to go in the button. 
                var center = new CenteredLayout(Canvas); //Center it
                center.ShouldSizeToParent = true;
                center.Add(Label);
                Add(center);
            }
     
            Label.Text = text;

            NinePatchCache = Canvas.RenderTools.CalculateNinePatch(Canvas.SourceRectangles["buttonup"].sourceRect, CanvasBounds,
                Canvas.SourceRectangles["buttonup"].ninePatch);
            
            NinePatchCacheButtonDown = Canvas.RenderTools.CalculateNinePatch(Canvas.SourceRectangles["buttondown"].sourceRect, CanvasBounds,
                Canvas.SourceRectangles["buttondown"].ninePatch);
            
            base.Invalidate();
        }
        
        public void ClickUpdate()
        {
            var mouseState = Mouse.GetState();
            var pos = mouseState.Position;
            var mousePosRect = new Rectangle(pos.X,pos.Y,1,1);

            if (mousePosRect.Intersects(CanvasBounds))
            {
                var clicked = mouseState.LeftButton == ButtonState.Pressed;

                if (clicked && ToggleButton && !ClickedPrevious)
                {
                    Clicked = !Clicked;
                }

                if (clicked && !ToggleButton)
                    Clicked = true;
                
                ClickedPrevious = clicked;

                color = Clicked ? ClickedTintColor : HoverTintColor;
            }
            else
                color = actualColor;
        }

        public override void Draw(SpriteBatch batcher)
        {
            var ninePatch = Clicked ? NinePatchCacheButtonDown : NinePatchCache;
            Canvas.RenderTools.DrawNinePatch(batcher, Canvas.SpriteSheet, ninePatch.SourcePatches, NinePatchCache.DestinationPatches, Color);

            var offset = Clicked ? ButtonClickYOffset : 0;
            var original = Label.Offset;
            original.Y += offset;
            Label.Offset = original;
            Label.Draw(batcher);
            original.Y -= offset;
            Label.Offset = original;
        }

        public Button(Canvas canvas) : base(canvas)
        {
        }

        public Button(IControl parent) : base(parent)
        {
        }
    }
}