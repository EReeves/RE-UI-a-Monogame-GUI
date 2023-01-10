using System.Collections.Generic;
using MGUI.Core;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Controls
{
    public class MultiLabel : PaddedControl
    {
        public List<string> Text = new();
        private readonly List<Label> labels = new();
        public SpriteFont SpriteFont { get; set; } = null;

        public int LineSpacing { get; set; } = 2;

        public MultiLabel(Canvas canvas) : base(canvas)
        {
        }

        public MultiLabel(IControl parent) : base(parent)
        {
        }

        public override void Invalidate()
        {
            var count = 0;
            var y = 0;

            for (var i = 0; i < Text.Count; i++)
            {
                var text = Text[i];
                if (count >= labels.Count)
                    labels.Add(new Label(this));

                var label = labels[count];
                label.Anchor = Label.TextAnchor.Manual;
                label.Text = text;

                if (SpriteFont != null)
                    label.SpriteFont = SpriteFont;

                var bounds = label.Bounds;
                bounds.Y = y;
                label.Bounds = bounds;
                y += bounds.Height;
                y += LineSpacing;
                label.Invalidate();

                count++;
            }


            base.Invalidate();
        }
    }
}