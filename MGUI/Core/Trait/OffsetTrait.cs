using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MGUI.Core.Trait;

public class OffsetTrait
{
    private readonly IControl control;

    public OffsetTrait(IControl control)
    {
        this.control = control;
    }

    public Rectangle GetBounds(Rectangle bounds)
    {
        var rect = control.Bounds;
        rect.X += bounds.X;
        rect.Y += bounds.Y;

        return rect;
    }
}
