using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Core
{
    /// <summary>
    /// Simply wrap spritebatch to save state in case it needs interrupting.
    /// e.g. Each Canvas is one draw call through RenderTools.Begin/End, but you can end the spritebatch and go custom if you need to. Just start again.
    /// Also provides utilities for culling and ninepatch rendering.
    /// </summary>
    public class RenderTools
    {
        private readonly Canvas canvas;
        private readonly Rectangle canvasBounds;
        public readonly RasterizerState RasterizerState = new() { ScissorTestEnable = false };

        //So drawing can be customized.
        public GraphicsDevice GraphicsDevice { get; }
        public SpriteSortMode SpriteSortMode { get; set; } = SpriteSortMode.Deferred;
        public BlendState? BlendState { get; set; } = null;
        public SamplerState? SamplerState { get; set; } = null;
        public DepthStencilState? DepthStencilState { get; set; } = null;
        public Effect? Effect { get; set; } = null;
        public Matrix? Transform { get; set; }

        public RenderTools(Canvas canvas, GraphicsDevice graphics, Rectangle canvasBounds)
        {
            this.canvas = canvas;
            this.canvasBounds = canvasBounds;

            GraphicsDevice = graphics;
            graphics.ScissorRectangle = canvasBounds;
        }

        public void Begin(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.ScissorRectangle = canvasBounds;
            spriteBatch.Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, Effect, Transform);
        }

        public void End(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
        }

        public void StartCull(SpriteBatch spriteBatch, Rectangle bounds)
        {
            spriteBatch.End();
            spriteBatch.GraphicsDevice.ScissorRectangle = bounds;
            spriteBatch.Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, Effect, Transform);
        }

        public void EndCull(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            Begin(spriteBatch);
        }

        /// <summary>
        /// Draws a rectangle outline
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="rectangle"></param>
        /// <param name="lineWidth"></param>
        /// <param name="color"></param>
        public void RenderOutline(SpriteBatch spriteBatch, Rectangle rectangle, Color color, int lineWidth = 1)
        {
            var pointTexture = canvas.SpriteSheet;
            var source = canvas.SourceRectangles["whiteTexture"].sourceRect;
            spriteBatch.Draw(pointTexture, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height), source, color);
            spriteBatch.Draw(pointTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width - lineWidth, lineWidth), source, color);
            spriteBatch.Draw(pointTexture, new Rectangle(rectangle.X + rectangle.Width - lineWidth, rectangle.Y, lineWidth, rectangle.Height), source, color);
            spriteBatch.Draw(pointTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - lineWidth, rectangle.Width - lineWidth, lineWidth), source, color);
        }

        public static (Rectangle[] sourcePatches, Rectangle[] destPatches) CalculateNinePatch(Rectangle sourceRect, Rectangle destRect, int[]? ninePatchCoords)
        {
            ninePatchCoords ??= new int[] { 12, 12, 12, 12 };

            var sourcePatches = CreatePatches(sourceRect, ninePatchCoords);
            var destinationPatches = CreatePatches(destRect, ninePatchCoords);

            return (sourcePatches, destinationPatches);
        }

        public static void DrawNinePatch(SpriteBatch batcher, Texture2D texture, Rectangle[] sourcePatches, Rectangle[] destinationPatches, Color color, Vector2? scale = null, Point? offset = null)
        {
            for (var i = 0; i < sourcePatches.Length; i++)
            {
                var dest = destinationPatches[i];
                if (scale != null)
                {
                    dest.Size = (dest.Size.ToVector2() * scale.Value).ToPoint();
                }

                if (offset != null)
                {
                    dest.Location += offset.Value;
                }

                batcher.Draw(texture, sourceRectangle: sourcePatches[i],
                    destinationRectangle: dest, color: color);
            }
        }

        //Thanks to Monogame.Extended, helps speed things up.
        private static Rectangle[] CreatePatches(Rectangle rectangle, int[] pad)
        {
            var leftPadding = pad[0];
            var topPadding = pad[1];
            var rightPadding = pad[2];
            var bottomPadding = pad[3];

            var x = rectangle.X;
            var y = rectangle.Y;
            var w = rectangle.Width;
            var h = rectangle.Height;
            var middleWidth = w - leftPadding - rightPadding;
            var middleHeight = h - topPadding - bottomPadding;
            var bottomY = y + h - bottomPadding;
            var rightX = x + w - rightPadding;
            var leftX = x + leftPadding;
            var topY = y + topPadding;
            return new[]
            {
                new Rectangle(x,      y,        leftPadding,  topPadding),      // top left
                new Rectangle(leftX,  y,        middleWidth,  topPadding),      // top middle
                new Rectangle(rightX, y,        rightPadding, topPadding),      // top right
                new Rectangle(x,      topY,     leftPadding,  middleHeight),    // left middle
                new Rectangle(leftX,  topY,     middleWidth,  middleHeight),    // middle
                new Rectangle(rightX, topY,     rightPadding, middleHeight),    // right middle
                new Rectangle(x,      bottomY,  leftPadding,  bottomPadding),   // bottom left
                new Rectangle(leftX,  bottomY,  middleWidth,  bottomPadding),   // bottom middle
                new Rectangle(rightX, bottomY,  rightPadding, bottomPadding)    // bottom right
            };
        }
    }
}