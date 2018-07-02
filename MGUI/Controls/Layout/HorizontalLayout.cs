using MGUI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Controls.Layout
{
    public class HorizontalLayout : Control
    {
        public int InnerPadding { get; set; } = 10;
        public int OuterPadding { get; set; } = 0;
        public int SidePadding { get; set; } = 0;
        


        public override void Invalidate()
        {
            var weightSum = 0;
            foreach (var child in Children)
            {
                weightSum += child.Weight;
            }
            
         

            var width = Bounds.Width;
            width -= (Children.Count - 1) * InnerPadding; //inner padding
            width -= SidePadding * 2; //Outer padding
            var widthPerWeight = width / weightSum;

            var x = SidePadding;
            for (var i = 0; i < Children.Count; i++)
            {
                var child = Children[i];
                var newWidth = widthPerWeight * child.Weight;
                child.Bounds = new Rectangle(x, OuterPadding, newWidth,
                    Bounds.Height - (OuterPadding * 2));
                // child.Offset = new Point(CanvasBounds.X, CanvasBounds.Y);
                if (i+1 < Children.Count)
                    x += InnerPadding;
                x += newWidth;
            }
            
            base.Invalidate();

            //Some children might have resized to smaller children. Center them.
            foreach (var child in Children)
            {
                var h = CanvasBounds.Height - child.CanvasBounds.Height;
            
                child.Bounds = new Rectangle(child.Bounds.X,h/2,child.Bounds.Width,child.Bounds.Height);
            }
        }

        public override void Draw(SpriteBatch batcher)
        {
            foreach (var control in Children)
            {
                control.Draw(batcher);
            }
        }

        public HorizontalLayout(Canvas canvas) : base(canvas)
        {
        }

        public HorizontalLayout(IControl parent) : base(parent)
        {
        }
    }
}