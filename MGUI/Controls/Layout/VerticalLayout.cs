using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MGUI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Controls.Layout;

public class VerticalLayout : Control, ILayout
{
    public int InnerPadding { get; set; } = 3;

    public VerticalLayout(Canvas canvas) : base(canvas)
    {
    }

    private void LayoutChildren()
    {
        var weightSum = Children.Aggregate(0, (total, child) => total + child.Weight);

        var height = GlobalBounds.Height;
        height -= (Children.Count - 1) * InnerPadding; //inner padding
        var yPadding = Padding.Sides[1] + Padding.Sides[3];
        height -= yPadding;

        var heightPerWeight = height / weightSum;

        var y = 0;
        for (var i = 0; i < Children.Count; i++)
        {
            var child = Children[i];
            var newHeight = heightPerWeight * child.Weight;


            var bounds = new Rectangle(0, 0 + y, GlobalBounds.Width, newHeight);
            Children[i].Bounds = bounds;

            if (i + 1 < Children.Count)
                y += InnerPadding;

            y += newHeight;
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        //Don't draw
    }

    public override void Update(GameTime gameTime)
    {

    }

    public void Invalidate()
    {
        LayoutChildren();
    }
}
