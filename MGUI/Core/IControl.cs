using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MGUI.Core.Trait;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Core;

public interface IControl
{
    Canvas System { get; }
    List<IControl> Children { get; }
    IControl Parent { get; set; }
    Rectangle Bounds { get; set; }
    Rectangle GlobalBounds { get; }
    PaddingTrait Padding { get; set; }

    //Weight
    int Weight { get; set; }

    //Update/Draw
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
}
