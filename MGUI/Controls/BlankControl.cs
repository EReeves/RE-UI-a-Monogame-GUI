using MGUI.Controls.Layout;
using MGUI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Controls
{
    /// <summary>
    /// A blank control for drawing or debugging purposes.
    /// </summary>
    public class BlankControl : PaddedControl
    {

        public override void Draw(SpriteBatch batcher)
        {
            batcher.Draw(Canvas.SpriteSheet, GlobalBounds, Canvas.SourceRectangles["whiteTexture"].sourceRect, Color);

            base.Draw(batcher);
        }

        public BlankControl(Canvas canvas) : base(canvas)
        {
        }

        public BlankControl(IControl parent) : base(parent)
        {
        }

    }
}