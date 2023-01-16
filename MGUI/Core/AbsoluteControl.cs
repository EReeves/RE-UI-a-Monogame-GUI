using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Core;

/// <summary>
/// Stays absolute on Bounds no matter the parent. 
/// </summary> 
public class AbsoluteControl : BareControl
{
    public AbsoluteControl(Canvas canvas) : base(canvas)
    {
    }

    public AbsoluteControl(IControl parent) : base(parent)
    {
    }

    public override void Invalidate()
    {
        //Absolute. Don't invalidate.
    }

    public override Rectangle GlobalBounds => this.Bounds;
}
