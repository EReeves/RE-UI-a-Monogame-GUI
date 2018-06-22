using System;
using MGUI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Controls
{
    public class Label : Control
    {
        private string text = string.Empty;
        public string Text
        {
            get => text;
            set
            {
                text = value; 
                Invalidate();
            }
        }

        public SpriteFont SpriteFont { get; set; }
        
        public Label(Canvas canvas) : base(canvas)
        {
            SpriteFont = canvas.DefaultFont;
        }

        public override void Invalidate()
        {
            var size = SpriteFont.MeasureString(Text);
            var newB = Bounds;
            newB.Width = (int) size.X;
            newB.Height = (int) size.Y;
            Bounds = newB;
            base.Invalidate();
        }

        public override void Draw(SpriteBatch batcher)
        {
            batcher.DrawString(SpriteFont, Text, CanvasBounds.Location.ToVector2(), Color);
        }
    }
}