using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Core
{
    public interface IControl
    {
        List<IControl> Children { get; }
        IControl? Parent { get; set; }
        /// <summary>
        /// Short for LocalBounds
        /// </summary>
        Rectangle Bounds { get; set; }
        Rectangle GlobalBounds { get; }
        Canvas Canvas { get; }

        //Weight
        int Weight { get; set; }

        //Invalidate
        void Invalidate();

        //Add/Remove
        void Add(IControl control);
        void Remove(IControl control);

        //Update/Draw
        void Update(GameTime gameTime);
        void Draw(SpriteBatch batcher);

    }
}