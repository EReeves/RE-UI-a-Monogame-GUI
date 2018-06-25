using MGUI.Controls.Layout;
using MGUI.Core;

namespace MGUI.Controls
{
    public class PaddedControl : Control
    {
        private PaddedLayout paddedLayout;
        
        private int allSidePadding = 0;        
        //Set padding on all sides to one value.
        public int Padding
        {
            get => allSidePadding;
            set
            {
                allSidePadding = value; 
                PaddingExplicit = new int[] {value, value, value, value};
            }
        }
        
        /// <summary>
        /// Set padding per side,
        /// Starts on the left going clockewize, 4 integers.
        /// </summary>
        /// <returns></returns>
        public int[] PaddingExplicit { get; set; }  = {0, 0, 0, 0};
        
        public PaddedControl(Canvas canvas) : base(canvas)
        {
            paddedLayout = new PaddedLayout(this);
        }

        public PaddedControl(IControl parent) : base(parent)
        {
            paddedLayout = new PaddedLayout(this);
        }

        public override void Invalidate()
        {

            if (paddedLayout != null)
            {
                paddedLayout.Padding = Padding;
                paddedLayout.PaddingExplicit = PaddingExplicit;
                paddedLayout.Invalidate();
            }

            base.Invalidate();
        }
        public override void Add(IControl control)
        {
            if (paddedLayout == null)
            {
                base.Add(control);
                return;
            }

            control.Parent = paddedLayout;
            paddedLayout.Add(control);

        }
    }
}