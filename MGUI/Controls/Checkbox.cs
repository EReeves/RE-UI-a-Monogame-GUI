using MGUI.Core;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Controls
{
    public class Checkbox : Button
    {
        public Checkbox(Canvas canvas) : base(canvas)
        {
        }

        public Checkbox(IControl parent) : base(parent)
        {
        }

        public override void Invalidate()
        {
            actualColor = color;
            ToggleButton = true;
            var sourceRec =  Canvas.SourceRectangles["checkbox"].sourceRect;
            var newBounds = Bounds;
            newBounds.Width = sourceRec.Width;
            newBounds.Height = sourceRec.Height;
            Bounds = newBounds;
        }

        public override void Draw(SpriteBatch batcher)
        {
            var source = Clicked
                ? Canvas.SourceRectangles["checkboxclicked"].sourceRect
                : Canvas.SourceRectangles["checkbox"].sourceRect;
            batcher.Draw(Canvas.SpriteSheet,CanvasBounds,source,Color);
        }
    }
}