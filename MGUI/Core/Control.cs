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

        public bool Hide { get; set; } = false;

        /// <summary>
        /// Draw children if outside of bounds.
        /// Disabling this introduces another draw call so best to use it only when needed.
        /// </summary>
        public bool DrawOverflow { get; set; } = true;

        private bool invalidatedBounds = true;

        /// <summary>
        /// Used to cache nine patch information.
        /// </summary>
        protected (Rectangle[] SourcePatches, Rectangle[] DestinationPatches) NinePatchCache { get; set; }

        public override void Invalidate()
        {
            invalidatedBounds = true;
            Children.ForEach(x => x.Invalidate());
        }

        private Rectangle localBounds;
        public override Rectangle Bounds
        {
            get
            {
                invalidatedBounds = true;
                return localBounds;
            }
            set => localBounds = value;
        }

        //Cache this.
        private Rectangle canvasBounds;
        public override Rectangle GlobalBounds
        {
            get
            {
                if (invalidatedBounds)
                    canvasBounds = base.GlobalBounds;

                invalidatedBounds = false;
                return canvasBounds;
            }
        }

        public override void Draw(SpriteBatch batcher)
        {
            if (Hide) return;

            if (!DrawOverflow)
            {
                Canvas.RenderTools.StartCull(batcher, GlobalBounds);
                base.Draw(batcher);
                Canvas.RenderTools.EndCull(batcher);
            }
            else base.Draw(batcher);
        }
    }
}