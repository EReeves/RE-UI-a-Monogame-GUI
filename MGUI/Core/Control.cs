using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MGUI.Core.Trait;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Core;

public abstract class Control
{
    internal Control Parent { get; set; }
    internal Canvas Canvas { get; set; }
    internal List<Control> Children { get; } = new();

    public Rectangle Bounds { get; set; }
    public Rectangle GlobalBounds => GetGlobalBounds();
    public int Weight { get; set; } = 1;
    public PaddingTrait Padding { get; set; } = new();


    public Control(Canvas system)
    {
        Canvas = system;
        system.Add(this);
    }

    public abstract void Draw(SpriteBatch spriteBatch);
    public abstract void Update(GameTime gameTime);


    public void Add(Control control)
    {
        Children.Add(control);
        control.Parent = this;
    }

    public void Remove(Control control)
    {
        Children.Remove(control);
        control.Parent = null;
    }

    private Rectangle GetGlobalBounds()
    {
        return Parent != null ? GetParentPaddingOffsetBounds() : Bounds;
    }

    private Rectangle GetParentPaddingOffsetBounds()
    {
        var x = Parent.GlobalBounds.X + Bounds.X + Parent.Padding.Sides[0];
        var y = Parent.GlobalBounds.Y + Bounds.Y + Parent.Padding.Sides[1];
        var width = Math.Clamp(Bounds.Width, 0, Math.Abs(Parent.GlobalBounds.Width - Parent.Padding.Sides[2] - Parent.Padding.Sides[0]));
        var height = Math.Clamp(Bounds.Height, 0, Math.Abs(Parent.GlobalBounds.Height - Parent.Padding.Sides[3] - Parent.Padding.Sides[1]));

        return new Rectangle(x, y, width, height);
    }



}
