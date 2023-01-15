using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Core
{
    /// <summary>
    /// The base point for all UI, handles updating and drawing at the top level.
    /// </summary>
    public class Canvas
    {
        public List<IControl> Children { get; } = new();
        public Texture2D SpriteSheet { get; }
        public Dictionary<string, (Rectangle sourceRect, int[]? ninePatch)> SourceRectangles { get; }
        public Dictionary<string, SpriteFont> SpriteFonts { get; }
        public Rectangle Bounds { get; set; }

        public RenderTools RenderTools { get; set; }

        private readonly string defaultFontName;
        public SpriteFont DefaultFont => SpriteFonts[defaultFontName];

        /// <summary>
        /// The base point for all UI.
        /// </summary>
        /// <param name="bounds">The window bounds as a Rectangle</param>
        /// <param name="spriteSheet">A spritesheet or atlas with all the textures used in UI.</param>
        /// <param name="sourceRectangles">A Dictionary with names and source rectanges for UI textures.</param>
        /// <param name="spriteFonts">A dictionary with names for fonts and the SpriteFonts themselves</param>
        /// <param name="defaultFont">The name of the default SpriteFont to use for text</param>
        public Canvas(Game game, Rectangle bounds, Texture2D spriteSheet, Dictionary<string, (Rectangle, int[]?)> sourceRectangles, Dictionary<string, SpriteFont> spriteFonts, string defaultFont)
        {
            this.defaultFontName = defaultFont;
            this.SpriteSheet = spriteSheet;
            this.SourceRectangles = sourceRectangles;
            this.Bounds = bounds;
            this.SpriteFonts = spriteFonts;

            RenderTools = new RenderTools(this, game.GraphicsDevice, bounds);

            Core.Utility.KeyboardInput.Initialize(game, 350, 10);
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
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            RenderTools.Begin(spriteBatch);

            foreach (var child in Children)
            {
                child.Draw(spriteBatch);
            }

            RenderTools.End(spriteBatch);
        }

        /// <summary>
        /// Will invalidate all children.
        /// </summary>
        public void Invalidate()
        {
            for (int i = Children.Count-1; i >= 0; i--)
            {
                Children[i].Invalidate();
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
            control.Parent = null;
            Children.Remove(control);
        }

    }
}