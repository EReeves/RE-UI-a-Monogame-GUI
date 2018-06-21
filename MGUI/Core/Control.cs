using Microsoft.Xna.Framework;

namespace MGUI.Core
{
    /// <summary>
    /// Adds caching and invalidation on top of the base control.
    /// </summary>
    public class Control : BaseControl
    {
        public Control(Canvas canvas) : base(canvas)
        {
        }

        private bool invalidatedBounds = true;

        public override void Invalidate()
        {
            invalidatedBounds = true;
            foreach (var child in Children)
            {
                child.Invalidate();
            }
        }

        public override void Layout()
        {
            
        }

        private Point offset = Point.Zero;
        public override Point Offset
        {
            get
            {
                invalidatedBounds = true;
                return offset;
            }
            set => offset = value;
        }

        private Rectangle bounds;
        public override Rectangle Bounds
        {
            get
            {
                invalidatedBounds = true;
                return bounds;
            }
            set => bounds = value;
        }

        //Cache this.
        private Rectangle canvasBounds;
        public override Rectangle CanvasBounds
        {
            get
            {
                if (invalidatedBounds)
                    canvasBounds = base.CanvasBounds;

                invalidatedBounds = false;
                return canvasBounds;
            }
        }
    }
}