﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Core
{
    public class RenderTools
    {
        private readonly Canvas canvas;
        private readonly Rectangle canvasBounds;
        //Off by default, could mess with rendering of a lot of games.
        public readonly RasterizerState RasterizerState = new() { ScissorTestEnable = false };


        //So drawing can be customized.

        public SpriteSortMode SpriteSortMode { get; set; } = SpriteSortMode.Deferred;
        public BlendState BlendState { get; set; } = null;
        public SamplerState SamplerState { get; set; } = null;
        public DepthStencilState DepthStencilState { get; set; } = null;
        public Effect Effect { get; set; } = null;
        public Matrix? Transform { get; set; }


        public RenderTools(Canvas canvas, GraphicsDevice graphics, Rectangle canvasBounds)
        {
            this.canvas = canvas;
            this.canvasBounds = canvasBounds;

            graphics.ScissorRectangle = canvasBounds;
        }

        public void Start(SpriteBatch batcher)
        {
            batcher.GraphicsDevice.ScissorRectangle = canvasBounds;
            batcher.Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, Effect, Transform);
        }

        public void StartCull(SpriteBatch batcher, Rectangle bounds)
        {
            batcher.End();
            batcher.GraphicsDevice.ScissorRectangle = bounds;
            batcher.Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, Effect, Transform);
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
            batcher.Draw(pointTexture, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height), source, color);
            batcher.Draw(pointTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width - lineWidth, lineWidth), source, color);
            batcher.Draw(pointTexture, new Rectangle(rectangle.X + rectangle.Width - lineWidth, rectangle.Y, lineWidth, rectangle.Height), source, color);
            batcher.Draw(pointTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - lineWidth, rectangle.Width - lineWidth, lineWidth), source, color);
        }

        public static (Rectangle[] sourcePatches, Rectangle[] destPatches) CalculateNinePatch(Rectangle sourceRect, Rectangle destRect, int[] ninePatchCoords)
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