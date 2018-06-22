using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Core
{
    public class RenderTools
    {
        private readonly Canvas canvas;
        private readonly GraphicsDevice graphics;
        private readonly Rectangle canvasBounds;
        private RasterizerState rasterizerState;
        private RasterizerState rasterizerStateNoScissor;
        
        //So drawing can be customized.

        public SpriteSortMode SpriteSortMode { get; set; } = SpriteSortMode.Deferred;
        public BlendState BlendState { get; set; } = null;
        public SamplerState SamplerState { get; set; } = null;
        public DepthStencilState DepthStencilState { get; set; } = null;
        public Effect Effect { get; set; } = null;
        
        public RenderTools(Canvas canvas, GraphicsDevice graphics, Rectangle canvasBounds)
        {
            this.canvas = canvas;
            this.graphics = graphics;
            this.canvasBounds = canvasBounds;
            rasterizerState = new RasterizerState { ScissorTestEnable = true };

            graphics.ScissorRectangle = canvasBounds;
        }

        public void Start(SpriteBatch batcher)
        {
            batcher.GraphicsDevice.ScissorRectangle = canvasBounds;
            batcher.Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState, rasterizerState, Effect);
        }

        public void StartCull(SpriteBatch batcher, Rectangle bounds)
        {
            batcher.End();
            batcher.GraphicsDevice.ScissorRectangle = bounds;
            batcher.Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState, rasterizerState, Effect);
        }

        public void EndCull(SpriteBatch batcher)
        {
            batcher.End();
            Start(batcher);
        }

        /// <summary>
        /// Draws a rectangle outline
        /// </summary>
        /// <param name="batcher"></param>
        /// <param name="rectangle"></param>
        /// <param name="lineWidth"></param>
        /// <param name="color"></param>
        public void RenderOutline(SpriteBatch batcher, Rectangle rectangle, Color color, int lineWidth = 1)
        {
            var pointTexture = canvas.SpriteSheet;
            var source = canvas.SourceRectangles["whiteTexture"].sourceRect;
            batcher.Draw(pointTexture, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height + lineWidth), source, color);
            batcher.Draw(pointTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + lineWidth, lineWidth), source, color);
            batcher.Draw(pointTexture, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, lineWidth, rectangle.Height + lineWidth), source, color);
            batcher.Draw(pointTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + lineWidth, lineWidth), source, color);
        }

        public (Rectangle[] sourcePatches, Rectangle[] destPatches) CalculateNinePatch(Rectangle sourceRect, Rectangle destRect, int[] ninePatchCoords)
        {          
            if (ninePatchCoords == null)
                ninePatchCoords = new int[]{20,20,20,20};
            
            var sourcePatches = CreatePatches(sourceRect, ninePatchCoords);
            var destinationPatches = CreatePatches(destRect, ninePatchCoords);

            return (sourcePatches, destinationPatches);
        }

        public void DrawNinePatch(SpriteBatch batcher, Texture2D texture, Rectangle[] sourcePatches, Rectangle[] destinationPatches, Color color)
        {
            for (var i = 0; i < sourcePatches.Length; i++)
            {
                batcher.Draw(texture, sourceRectangle: sourcePatches[i],
                    destinationRectangle: destinationPatches[i], color: color);
            }
        }
        
        //Thanks to Monogame.Extended, helps speed things up.
        private Rectangle[] CreatePatches(Rectangle rectangle, int[] pad)
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
            var patches = new[]
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
            return patches;
        }
    }
}