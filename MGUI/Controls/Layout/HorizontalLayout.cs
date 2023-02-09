using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MGUI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Controls.Layout
{
    public class HorizontalLayout : Control, ILayout
    {

        public int InnerPadding { get; set; } = 3;

        public HorizontalLayout(Canvas canvas) : base(canvas)
        {
        }

            private void LayoutChildren()
    {
        var weightSum = Children.Aggregate(0, (total, child) => total + child.Weight);

        var width = GlobalBounds.Width;
        width -= (Children.Count - 1) * InnerPadding; //inner padding
        var xPadding = Padding.Sides[0] + Padding.Sides[2];
        width -= xPadding;

        var widthPerWeight = width / weightSum;

        var x = 0;
        for (var i = 0; i < Children.Count; i++)
        {
            var child = Children[i];
            var newWidth = widthPerWeight * child.Weight;


            var bounds = new Rectangle(0 + x, 0, newWidth, GlobalBounds.Height);
            Children[i].Bounds = bounds;

            if (i + 1 < Children.Count)
                x += InnerPadding;

            x += newWidth;
        }
    }


        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public void Invalidate()
        {
            LayoutChildren();
        }
    }
}