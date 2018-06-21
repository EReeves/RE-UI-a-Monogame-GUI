using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Core
{
    public class RenderTools
    {
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
        
        public RenderTools(GraphicsDevice graphics, Rectangle canvasBounds)
        {
            this.graphics = graphics;
            this.canvasBounds = canvasBounds;
            rasterizerState = new RasterizerState { ScissorTestEnable = true };

            graphics.ScissorRectangle = canvasBounds;
        }

        public void Start(SpriteBatch batcher)
        {
            batcher.GraphicsDevice.ScissorRectangle = canvasBounds;
            batcher.Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState, rasterizerState);
        }

        public void StartCull(SpriteBatch batcher, Rectangle bounds)
        {
            batcher.End();
            batcher.GraphicsDevice.ScissorRectangle = bounds;
            batcher.Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState, rasterizerState);
        }

        public void EndCull(SpriteBatch batcher)
        {
            batcher.End();
            Start(batcher);
        }
    }
}