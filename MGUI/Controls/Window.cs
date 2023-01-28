using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MGUI.Core;
using MGUI.Core.Trait;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Controls;

public class Window : Control
{
    public Window(Canvas system) : base(system)
    {
        Drawable = new DrawTrait(system)
        {
            NinePatchTexture = "window"
        };

        Padding.Sides[1] = DragBarPadding;
    }

    public int DragBarPadding { get; set; } = 20;
    private DrawTrait Drawable { get; set; }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Drawable.Draw(spriteBatch, GlobalBounds);
    }

    public override void Update(GameTime gameTime)
    {

    }
}
