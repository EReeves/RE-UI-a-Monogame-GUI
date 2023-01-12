using System;
using MGUI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Controls
{
    /// <summary>
    /// Draws a custom texture.
    /// </summary>
    public class Image : PaddedControl
    {
        public string Texture { get; set; }
        private Rectangle sourceRect;

        public Image(Canvas canvas) : base(canvas)
        {
        }

        public Image(IControl parent) : base(parent)
        {
        }

        public override void Invalidate()
        {
            if (Texture == null)
                throw new Exception("Texture property must be set on an Image control.");

            sourceRect = Canvas.SourceRectangles[Texture].sourceRect;

            base.Invalidate();
        }

        public override void Draw(SpriteBatch batcher)
        {
            batcher.Draw(Canvas.SpriteSheet, GlobalBounds, sourceRect, Color);

            base.Draw(batcher);
        }
    }
}