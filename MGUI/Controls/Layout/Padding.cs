using System;
using MGUI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Controls.Layout
{
    public class Padding : Control
    {
        private int allSidePadding;

        public Padding(Canvas canvas, int padding) : base(canvas)
        {
            allSidePadding = padding;
        }
        
        public Padding(Canvas canvas, int[] padding) : base(canvas)
        {
            throw new NotImplementedException();
        }

        public override void Layout()
        {
            base.Layout();
        }

        public override void Invalidate()
        {
            Bounds = Parent != null ? new Rectangle(0, 0, Parent.Bounds.Width - Offset.X, Parent.Bounds.Height - Offset.Y) : Canvas.Bounds;

            AddPadding();
            base.Invalidate();
        }

        //Add padding to all children.
        private void AddPadding()
        {
            var offset = new Point(allSidePadding);
            foreach (var child in Children)
            {
                //Left padding.
                if (child.Bounds.X < allSidePadding)
                    offset.X = allSidePadding - child.Bounds.X;
                
                //Top padding.
                if (child.Bounds.Y < allSidePadding)
                    offset.Y = allSidePadding - child.Bounds.Y;
                
                //Apply top and left.
                child.Offset = offset;
                
                //Far right padding.
                var bounds = child.Bounds;
                if (child.CanvasBounds.Right > CanvasBounds.Right - allSidePadding)
                    bounds.Width -= (child.CanvasBounds.Right - (CanvasBounds.Right-allSidePadding));
                
                //Far left.
                if (child.CanvasBounds.Bottom > CanvasBounds.Bottom - allSidePadding)
                    bounds.Height -= (child.CanvasBounds.Bottom - (CanvasBounds.Bottom-allSidePadding));

                child.Bounds = bounds;
            }           
        }

        public override void Draw(SpriteBatch batcher)
        {
            foreach (var child in Children)
            {
                child.Draw(batcher);
            }
        }
    }
}