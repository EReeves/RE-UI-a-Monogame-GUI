using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Core;

public class AbsoluteControl : IControl
{

    void IControl.Invalidate()
    {

    }

    void IControl.Add(IControl control)
    {
        throw new NotImplementedException();
    }

    void IControl.Remove(IControl control)
    {
        throw new NotImplementedException();
    }

    void IControl.Update(GameTime gameTime)
    {

    }

    void IControl.Draw(SpriteBatch batcher)
    {

    }

    public List<IControl> Children { get; set; } = new();
    public IControl? Parent { get; set; }
    public Rectangle Bounds { get; set; } = Rectangle.Empty;
    public Rectangle GlobalBounds => this.Bounds;
    public Canvas Canvas => throw new NotImplementedException();
    public int Weight { get; set; } = 1;
}
