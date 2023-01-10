using MGUI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Controls.Layout
{
    public class VerticalLayout : Control
    {
        public int InnerPadding { get; set; } = 10;
        public int OuterPadding { get; set; } = 0;



        public override void Invalidate()
        {

            var weightSum = 0;
            foreach (var child in Children)
            {
                weightSum += child.Weight;
            }

            var height = Bounds.Height;
            height -= (Children.Count - 1) * InnerPadding; //inner padding
            height -= OuterPadding * 2; //Outer padding
            var heightPerWeight = height / weightSum;

            var y = OuterPadding;
            for (var i = 0; i < Children.Count; i++)
            {
                var child = Children[i];
                var newHeight = heightPerWeight * child.Weight;
                child.Bounds = new Rectangle(OuterPadding, y, Bounds.Width - OuterPadding * 2,
                    newHeight);
                // child.Offset = new Point(CanvasBounds.X, CanvasBounds.Y);
                if (i + 1 < Children.Count)
                    y += InnerPadding;
                y += newHeight;
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

        public VerticalLayout(Canvas canvas) : base(canvas)
        {
        }

        public VerticalLayout(IControl parent) : base(parent)
        {
        }
    }
}