using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Core
{
    public abstract class BaseControl : IControl
    {
        public List<IControl> Children { get; set; } = new List<IControl>();
        public IControl Parent { get; set; }
        
        /// <summary>
        /// Optional offset applied to the control, often by the parent.
        /// Shouldn't be touched unless you know what you're doing.
        /// </summary>
        public virtual Point Offset { get; set; }

        public Canvas Canvas { get; set; }


        /// <summary>
        /// Size and location of the control, relative to parent, or canvas in the case parent == null.
        /// </summary>
        public virtual Rectangle Bounds { get; set; }

        /// <summary>
        /// The bounds in canvas space.
        /// </summary>
        public virtual Rectangle CanvasBounds
        {
            get
            {
                var rect = Bounds;
                rect.X += Offset.X;
                rect.Y += Offset.Y;
                
                if (Parent == null) return rect;
                
                //Need to offset for parent.
                rect.X += Parent.CanvasBounds.X;
                rect.Y += Parent.CanvasBounds.Y;

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
        /// <exception cref="Exception"></exception>
        public BaseControl(IControl parent)
        {
            if(parent.Canvas == null) throw new Exception("Canvas must be set on the parent control before adding to a parent");
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
                var newBounds = Parent.Bounds;
                newBounds.X = 0;
                newBounds.Y = 0;
                newBounds.Width -= Offset.X;
                newBounds.Height -= Offset.Y;
                Bounds = newBounds;
            }
            else
                Bounds = Canvas.Bounds;
        }

        //Called automatically by parent or canvas.
        public virtual void Update(GameTime gameTime)
        {
            foreach (var control in Children)
            {
                control.Update(gameTime);
            }
        }

        //Called automatically by parent or canvas.
        public virtual void Draw(SpriteBatch batcher)
        {
            foreach (var control in Children)
            {
                control.Draw(batcher);
            }
        }

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