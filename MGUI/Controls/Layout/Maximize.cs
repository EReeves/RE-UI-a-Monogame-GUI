using MGUI.Core;
using Microsoft.Xna.Framework;

namespace MGUI.Controls.Layout
{
    //Resizes to parent's size. 
    public class Maximize : Control
    {
        private readonly bool resizeChildren;

        public Maximize(Canvas canvas, bool resizeChildren = true) : base(canvas)
        {
            this.resizeChildren = resizeChildren;
        }

        public override void Invalidate()
        {
            Bounds = Parent != null ? new Rectangle(0, 0, Parent.Bounds.Width - Offset.X, Parent.Bounds.Height - Offset.Y) : Canvas.Bounds;
            if (resizeChildren)
            {
                foreach (var control in Children)
                {
                    control.Bounds = Bounds;
                }
            }

            base.Invalidate();
        }
    }
}