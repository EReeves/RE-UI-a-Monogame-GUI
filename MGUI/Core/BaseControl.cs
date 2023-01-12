using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Core
{
    public abstract class BaseControl : IControl
    {
        public List<IControl> Children { get; set; } = new List<IControl>();
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
                if (Parent == null) return Bounds;

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
        /// Used in some layouts to determine the size or location of the control.
        /// </summary>
        public int Weight { get; set; } = 1;


        public BaseControl(Canvas canvas)
        {
            this.Canvas = canvas;
            canvas.Add(this);
        }

        /// <summary>
        ///  Simultaneously create UI and add to parent.
        /// </summary>
        /// <param name="parent"></param>
        public BaseControl(IControl parent)
        {
            Canvas = parent.Canvas;
            parent.Add(this);
        }

        //BaseControl has nothing to invalidate.
        public abstract void Invalidate();

        //Resize to parent, or canvas if there is no parent.
        public void SizeToParent()
        {
            if (Parent != null)
            {
                var newBounds = Bounds;
                newBounds.Width = Parent.Bounds.Width;
                newBounds.Height = Parent.Bounds.Height;
                Bounds = newBounds;
            }
            else
            {
                var newBounds = Bounds;
                newBounds.Width = Canvas.Bounds.Width;
                newBounds.Height = Canvas.Bounds.Height;
                Bounds = newBounds;
            }
        }

        //Called automatically by parent or canvas.
        public virtual void Update(GameTime gameTime) => Children.ForEach(x => x.Update(gameTime));

        //Called automatically by parent or canvas.
        public virtual void Draw(SpriteBatch spriteBatch) => Children.ForEach(x => x.Draw(spriteBatch));

        //Add a child control
        public virtual void Add(IControl control)
        {
            control.Parent = this;
            Children.Add(control);
        }

        //Remove a child control
        public virtual void Remove(IControl control)
        {
            control.Parent = null;
            Children.Remove(control);
        }

    }
}