using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MGUI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Controls.Layout
{
    public class CenterLayout : Control, ILayout
    {
        public CenterLayout(Canvas canvas) : base(canvas)
        {
        }

        private void LayoutChildren()
        {
            var halfWidth = (GlobalBounds.Width-Padding.Sides[0]-Padding.Sides[2])/2;
            var halfHeight = (GlobalBounds.Height-Padding.Sides[1]-Padding.Sides[3])/2;

            foreach(var child in Children)
            {
                var newBounds = child.Bounds;
                newBounds.X = halfWidth - (child.GlobalBounds.Width/2);
                newBounds.Y = halfHeight - (child.GlobalBounds.Height/2);
                child.Bounds = newBounds;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
         //
        }

        public override void Update(GameTime gameTime)
        {
            //
        }

        public void Invalidate()
        {
            LayoutChildren();
        }
    }
}