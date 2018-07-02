using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public Control(IControl parent) : base(parent)
        {
            
        }

        private bool invalidatedBounds = true;

        /// <summary>
        /// Draw children if outside of bounds.
        /// Disabling this introduces another draw call so best to use it only when needed.
        /// </summary>
        public bool DrawOverflow { get; set; } = true;
        
        /// <summary>
        /// Used to cache nine patch information.
        /// </summary>
        protected (Rectangle[] SourcePatches, Rectangle[] DestinationPatches) NinePatchCache { get; set; }

        public override void Invalidate()
        {
            invalidatedBounds = true;

            foreach (var child in Children)
            {
                child.Invalidate();
            }
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

        public override void Draw(SpriteBatch batcher)
        {
            if (!DrawOverflow)
            {
                Canvas.RenderTools.StartCull(batcher, CanvasBounds);
                base.Draw(batcher);
                Canvas.RenderTools.EndCull(batcher);
            }
            else base.Draw(batcher);
        }
    }
}