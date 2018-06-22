using System;
using MGUI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Controls
{
    public class Label : Control
    {
        public string Text { get; set; } = string.Empty;
        public SpriteFont SpriteFont { get; set; }
        
        public Label(Canvas canvas) : base(canvas)
        {
            SpriteFont = canvas.DefaultFont;
        }



        public override void Draw(SpriteBatch batcher)
        {
            batcher.DrawString(SpriteFont, Text, CanvasBounds.Location.ToVector2(), Color);
        }
    }
}