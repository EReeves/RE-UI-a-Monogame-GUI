using MGUI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Controls.Layout
{
    public class Center : Control
    {
        public Center(Canvas canvas) : base(canvas)
        {
        }

        public override void Draw(SpriteBatch batcher)
        {
            foreach (var control in Children)
            {
                control.Draw(batcher);
            }
        }

        public override void Invalidate()
        {
            if (Parent != null)
            {
                var newBounds = Parent.Bounds;
                newBounds.X = 0;
                newBounds.Y = 0;
                Bounds = newBounds;
            }
            else
                Bounds = Canvas.Bounds;
            //can only have one child.
            var child = Children[0];
            var height = Bounds.Height - child.Bounds.Height;
            var width = Bounds.Width - child.Bounds.Width;
            
            Children[0].Bounds = new Rectangle(width/2,height/2,child.Bounds.Width,child.Bounds.Height);
            base.Invalidate();
        }
    }
}