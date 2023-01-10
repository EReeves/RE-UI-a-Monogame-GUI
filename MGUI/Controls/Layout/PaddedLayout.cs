using System;
using MGUI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Controls.Layout
{
    public class PaddedLayout : Control
    {
        private int allSidePadding = 0;
        public int Padding
        {
            get => allSidePadding;
            set
            {
                allSidePadding = value;
                PaddingExplicit = new int[] { value, value, value, value };
            }
        }

        /// <summary>
        /// Set padding per side,
        /// Starts on the left going clockewize, 4 integers.
        /// </summary>
        /// <returns></returns>
        public int[] PaddingExplicit { get; set; } = { 0, 0, 0, 0 };


        public override void Invalidate()
        {
            Bounds = Parent != null ? new Rectangle(Bounds.X, Bounds.Y, Parent.Bounds.Width, Parent.Bounds.Height) : Canvas.Bounds;

            AddPadding();
            base.Invalidate();
        }

        //Add padding to all children.
        private void AddPadding()
        {
            SizeToParent();

            //Also make children this big.
            foreach (var control in Children)
            {
                var bounds = control.Bounds;
                bounds.Width = Bounds.Width;
                bounds.Height = Bounds.Height;
                bounds.Location = Point.Zero;
                control.Bounds = bounds;

            }

            //Then add padding.
            var pad = PaddingExplicit;
            var padl = pad[0];
            var padu = pad[1];
            var padr = pad[2];
            var padb = pad[3];
            var offset = new Point(padl, padu);
            var sizeOffset = new Point(padr, padb);
            sizeOffset += offset; //we also have to take the left padding off the right/bottom.

            foreach (var child in Children)
            {
                /*
                //Left padding.
                if (child.Bounds.X < padl)
                    offset.X = padl - child.Bounds.X;
                
                //Top padding.
                if (child.Bounds.Y < allSidePadding)
                    offset.Y = padu - child.Bounds.Y;*/

                var bounds = child.Bounds;
                bounds.Location += offset;
                bounds.Size -= sizeOffset;


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

        public PaddedLayout(Canvas canvas) : base(canvas)
        {
        }

        public PaddedLayout(IControl parent) : base(parent)
        {
        }
    }
}