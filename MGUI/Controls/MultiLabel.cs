using System.Collections.Generic;
using MGUI.Core;

namespace MGUI.Controls
{
    public class MultiLabel : PaddedControl
    {
        public List<string> Text = new List<string>();
        private List<Label> labels = new List<Label>();

        public int LineSpacing { get; set; }= 2;
        
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
                label.Text = text;
                label.Invalidate();
                var bounds = label.Bounds;
                bounds.Y = y;
                label.Bounds = bounds;
                y += bounds.Height;
                y += LineSpacing;

                count++;
            }

            
            base.Invalidate();
        }
    }
}