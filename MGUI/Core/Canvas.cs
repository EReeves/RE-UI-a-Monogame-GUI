using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Core
{
    /// <summary>
    /// The base point for all UI, handles updating and drawing at the top level.
    /// </summary>
    public class Canvas
    {
        public List<IControl> Children = new List<IControl>();
        public Texture2D SpriteSheet { get; }
        public Dictionary<string,(Rectangle sourceRect, int[] ninePatch)> SourceRectangles { get; } //Also contains nine patch information if relevant.
        public Dictionary<string,SpriteFont> SpriteFonts { get; }
        public Rectangle Bounds { get; set; }
        
        public RenderTools RenderTools { get; set; }

        private string defaultFont;
        public SpriteFont DefaultFont => SpriteFonts[defaultFont];

        /// <summary>
        /// The base point for all UI.
        /// </summary>
        /// <param name="bounds">The window bounds as a Rectangle</param>
        /// <param name="spriteSheet">A spritesheet or atlas with all the textures used in UI.</param>
        /// <param name="sourceRectangles">A Dictionary with names and source rectanges for UI textures.</param>
        /// <param name="spriteFonts">A dictionary with names for fonts and the SpriteFonts themselves</param>
        /// <param name="defaultFont">The name of the default SpriteFont to use for text</param>
        public Canvas(Game game, Rectangle bounds, Texture2D spriteSheet, Dictionary<string, (Rectangle, int[])> sourceRectangles, Dictionary<string,SpriteFont> spriteFonts, string defaultFont)
        {
            this.defaultFont = defaultFont;
            this.SpriteSheet = spriteSheet;
            this.SourceRectangles = sourceRectangles;
            this.Bounds = bounds;
            this.SpriteFonts = spriteFonts;
            
            RenderTools = new RenderTools(this, game.GraphicsDevice, bounds);
            
            Core.Utility.KeyboardInput.Initialize(game,50,10);
        }
        
        /// <summary>
        /// Updates all children in the canvas recursively.
        /// </summary>
        /// <param name="time"></param>
        public void Update(GameTime time)
        {
            Core.Utility.KeyboardInput.Update();
                
            foreach (var child in Children)
            {
                child.Update(time);
            }
        }

        /// <summary>
        /// Draws all children in the canvas recursively.
        /// </summary>
        /// <param name="batcher"></param>
        public void Draw(SpriteBatch batcher)
        {
            RenderTools.Start(batcher);
            
            foreach (var child in Children)
            {
                child.Draw(batcher);
            }
            
            batcher.End();
        }

        /// <summary>
        /// Will invalidate all children.
        /// </summary>
        public void Invalidate()
        {
            for (var i = 0; i < Children.Count; i++)
            {
                var child = Children[i];
                child.Invalidate();
            }
        }

        /// <summary>
        /// Add a control to the canvas.
        /// </summary>
        /// <param name="control"></param>
        public void Add(IControl control)
        {
            control.Parent = null;
            Children.Add(control);

        }

        /// <summary>
        /// Remove a control from the canvas.
        /// </summary>
        /// <param name="control"></param>
        public void Remove(IControl control)
        {
            Children.Remove(control);
        }
    }
}