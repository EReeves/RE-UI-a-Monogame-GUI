using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Core
{
    /// <summary>
    /// Adds caching, invalidation, and optional culling and hiding to BareControl.
    /// </summary>
    public class Control : BareControl
    {
        public Control(Canvas canvas) : base(canvas)
        {
        }

        public Control(IControl parent) : base(parent)
        {

        }

        /// <summary>
        /// Draw children if outside of bounds.
        /// Disabling this introduces another draw call so best to use it only when needed.
        /// </summary>
        public bool DrawOverflow { get; set; } = true;
        public bool Hide { get; set; } = false;
        private bool invalidatedBounds = true;
        protected (Rectangle[] SourcePatches, Rectangle[] DestinationPatches) NinePatchCache { get; set; }

        public override void Invalidate()
        {
            if (Parent is AbsoluteControl)
            {
                SizeToParent();
            }
            invalidatedBounds = true;
            Children.ForEach(x => x.Invalidate());
        }

        private Rectangle localBounds = new Rectangle(0, 0, 50, 50);
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