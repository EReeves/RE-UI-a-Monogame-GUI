using MGUI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Controls.Layout
{
    public class HorizontalLayout : Control
    {
        public int InnerPadding { get; set; } = 10;
        public int OuterPadding { get; set; } = 10;
        
        public HorizontalLayout(Canvas canvas) : base(canvas)
        {
        }

        public override void Invalidate()
        {
            var weightSum = 0;
            foreach (var child in Children)
            {
                weightSum += child.Weight;
            }

            var width = Bounds.Width;
            width -= (Children.Count - 1) * InnerPadding; //inner padding
            width -= OuterPadding * 2; //Outer padding
            var widthPerWeight = width / weightSum;

            var x = OuterPadding;
            for (var i = 0; i < Children.Count; i++)
            {
                var child = Children[i];
                var newWidth = widthPerWeight * child.Weight;
                child.Bounds = new Rectangle(x, OuterPadding, newWidth,
                    Bounds.Height - OuterPadding * 2);
                // child.Offset = new Point(CanvasBounds.X, CanvasBounds.Y);
                if (i+1 < Children.Count)
                    x += OuterPadding;
                x += newWidth;
            }


            base.Invalidate();
        }

        public override void Draw(SpriteBatch batcher)
        {
            foreach (var control in Children)
            {
                control.Draw(batcher);
            }
        }
    }
}