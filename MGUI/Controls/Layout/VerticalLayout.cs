using System.Collections.Generic;
using MGUI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Controls.Layout
{
    public class VerticalLayout : Control
    {
        public int InnerPadding { get; set; } = 10;
        public int OuterPadding { get; set; } = 0;

        private readonly List<IControl> subLayouts = new();


        public override void Invalidate()
        {
            SizeToParent();


            foreach (var layout in subLayouts)
            {
                layout.Children.ForEach(child => layout.Remove(child));
            }
            subLayouts.Clear();

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


                var bounds = new Rectangle(GlobalBounds.X + OuterPadding, GlobalBounds.Y + y, Bounds.Width - OuterPadding * 2,
                    newHeight);

                var subLayout = new AbsoluteControl(Canvas)
                {
                    Bounds = bounds
                };
                child.Parent = subLayout;

                // child.Offset = new Point(CanvasBounds.X, CanvasBounds.Y);
                if (i + 1 < Children.Count)
                    y += InnerPadding;

                subLayouts.Add(subLayout);
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

            foreach (var control in subLayouts)
            {
                Canvas.RenderTools.RenderOutline(batcher, control.GlobalBounds, Color.DarkRed);
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