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
        protected readonly Canvas Canvas;
        
        /// <summary>
        /// Optional offset applied to the control, often by the parent.
        /// Shouldn't be touched unless you know what you're doing.
        /// </summary>
        public virtual Point Offset { get; set; }
        
        
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
        public Color Color { get; set; } = Color.White;
        
        /// <summary>
        /// Used in some layouts to determine the size or location of the control.
        /// </summary>
        public int Weight { get; set; } = 1;

      
        public BaseControl(Canvas canvas)
        {
            this.Canvas = canvas;
        }

        /// <summary>
        /// Called after control is added to a parent.
        /// Set up and sub controls can go in here.
        /// </summary>
        public abstract void Layout();

        //BaseControl has nothing to invalidate.
        public abstract void Invalidate();

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
            //Offset if we have a parent or have an offset.
            var destRect = CanvasBounds;
                     
            batcher.Draw(Canvas.SpriteSheet, destRect, Canvas.SourceRectangles["whiteTexture"].sourceRect, Color);
            
            foreach (var control in Children)
            {
                control.Draw(batcher);
            }
        }

        //Add a child control   
        public void Add(IControl control)
        {
            control.Parent = this;
            Children.Add(control);
            
            //Layout will 
            control.Layout();
        }
        
        //Remove a child control
        public void Remove(IControl control)
        {
            control.Parent = null;
            Children.Remove(control);
        }

    }
}