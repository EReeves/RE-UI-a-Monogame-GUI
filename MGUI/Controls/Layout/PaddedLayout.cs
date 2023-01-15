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
            SizeToParent();
            AddPadding();
            
            base.Invalidate();
        }

        //Add padding to all children.
        private void AddPadding()
        {
            var pad = PaddingExplicit;
            var padl = pad[0];
            var padt = pad[1];
            var padr = pad[2];
            var padb = pad[3];

            var width = Parent!.Bounds.Width - padl - padr;
            var height = Parent.Bounds.Height - padt - padb;
            Bounds = new Rectangle(padl, padt, width, height);
        }

        public override void Draw(SpriteBatch batcher)
        {
            base.Draw(batcher);
        }

        public PaddedLayout(Canvas canvas) : base(canvas)
        {
        }

        public PaddedLayout(IControl parent) : base(parent)
        {
        }
    }
}