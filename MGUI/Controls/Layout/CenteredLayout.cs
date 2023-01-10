using System.Net.NetworkInformation;
using MGUI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Controls.Layout
{
    public class CenteredLayout : Control
    {



        public override void Draw(SpriteBatch batcher)
        {
            foreach (var control in Children)
            {
                control.Draw(batcher);
            }
        }

        public override void Invalidate()
        {

            foreach (var child in Children)
            {
                var height = Bounds.Height - child.Bounds.Height;
                var width = Bounds.Width - child.Bounds.Width;

                child.Bounds = new Rectangle(width / 2, height / 2, child.Bounds.Width, child.Bounds.Height);
            }

            base.Invalidate();
        }


        public CenteredLayout(Canvas canvas) : base(canvas)
        {
        }

        public CenteredLayout(IControl parent) : base(parent)
        {
        }
    }
}