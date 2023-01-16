using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Core
{
    /// <summary>
    /// Barevones IControl implementation
    /// Calls draw on children, but own drawing must be implemented
    /// </summary>
    public abstract class BareControl : IControl
    {
        public List<IControl> Children { get; } = new List<IControl>();
        public IControl? Parent { get; set; }
        public Canvas Canvas { get; set; }


        /// <summary>
        /// Size and location of the control, relative to parent, or canvas in the case parent == null.
        /// </summary>
        public virtual Rectangle Bounds { get; set; }

        /// <summary>
        /// The bounds in canvas space.
        /// </summary>
        public virtual Rectangle GlobalBounds
        {
            get
            {
                if (Parent == null) return this.Bounds;

                //Global is just an offset from parent.
                var rect = Bounds;
                rect.X += Parent.GlobalBounds.X;
                rect.Y += Parent.GlobalBounds.Y;

                return rect;
            }
        }

        /// <summary>
        /// Tint colour
        /// </summary>
        public virtual Color Color { get; set; } = Color.White;

        /// <summary>
        /// The purpose of weight is defined by the parent layout if any.
        /// </summary>
        public int Weight { get; set; } = 1;


        public BareControl(Canvas canvas)
        {
            this.Canvas = canvas;
            canvas.Add(this);
        }

        /// <summary>
        ///  Create with Control as parent
        /// </summary>
        /// <param name="parent"></param>
        public BareControl(IControl parent)
        {
            Canvas = parent.Canvas;
            parent.Add(this);
        }

        public abstract void Invalidate();

        /// <summary>
        /// Resize to parent, or canvas if there is no parent.
        /// </summary>
        public void SizeToParent()
        {
            var parentBounds = Parent == null ? Canvas.Bounds : Parent.Bounds;

            var newBounds = this.Bounds;
            newBounds.Width = parentBounds.Width;
            newBounds.Height = parentBounds.Height;
            this.Bounds = newBounds;

        }

        public virtual void Update(GameTime gameTime) => Children.ForEach(x => x.Update(gameTime));
        public virtual void Draw(SpriteBatch spriteBatch) => Children.ForEach(x => x.Draw(spriteBatch));

        /// <summary>
        /// Called after a child is added
        /// </summary>
        /// <param name="control"></param>
        public virtual void Add(IControl control)
        {
            control.Parent = this;
            Children.Add(control);
        }

        public virtual void Remove(IControl control)
        {
            control.Parent = null;
            Children.Remove(control);
        }

    }
}