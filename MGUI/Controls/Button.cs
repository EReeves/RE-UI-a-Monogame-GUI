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
    public class Button : Control
    {
        public bool Clicked { get; private set; } = false;
        public Color ClickedTintColor { get; set; } = Color.Goldenrod;
        public Color HoverTintColor { get; set; } = Color.DarkOliveGreen;
        
        private Color actualColor;
        private Color color;
        
        public override Color Color
        {
            get => color;
            set
            {
                actualColor = value;
                color = value;
            }
        }

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
            if (Label == null)
            {
                Label = new Label(Canvas) { Text = string.Empty }; //Label to go in the button. 
                var center = new CenteredLayout(Canvas); //Center it
                center.Add(Label);
                Add(center);
            }
     
            Label.Text = text;

            NinePatchCache = Canvas.RenderTools.CalculateNinePatch(Canvas.SourceRectangles["buttonup"].sourceRect, CanvasBounds,
                Canvas.SourceRectangles["buttonup"].ninePatch);
            
            base.Invalidate();
        }

        public void ClickUpdate()
        {
            var mouseState = Mouse.GetState();
            var pos = mouseState.Position;
            var mousePosRect = new Rectangle(pos.X,pos.Y,1,1);
            
            if (!mousePosRect.Intersects(CanvasBounds))
            {
                Clicked = false;
                color = actualColor;
                return;
            }

            var clicked = mouseState.LeftButton == ButtonState.Pressed;
            if (!Clicked && clicked) //State not clicked, but clicked.
            {
                OnClick?.Invoke(this, EventArgs.Empty);
                Clicked = true;
                return;
            }

            if (!clicked)
            {
                Clicked = false;
                color = actualColor;
            }
            color = clicked ? ClickedTintColor : HoverTintColor;
        }

        public override void Draw(SpriteBatch batcher)
        {
            Canvas.RenderTools.DrawNinePatch(batcher, Canvas.SpriteSheet, NinePatchCache.SourcePatches, NinePatchCache.DestinationPatches, Color);

            base.Draw(batcher);
        }

        public Button(Canvas canvas) : base(canvas)
        {
        }

        public Button(IControl parent) : base(parent)
        {
        }
    }
}