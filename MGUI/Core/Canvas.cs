using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MGUI.Core.Trait;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Core;

public class Canvas : IControl
{

    public Texture2D SpriteSheetTexture { get; }
    public Dictionary<string, (Rectangle sourceRect, int[]? ninePatch)> SourceRectangles { get; }
    public RenderTools RenderTools { get; set; }



    //INewControl Impl
    public Canvas System => this;
    public List<IControl> Children { get; } = new();
    public IControl Parent { get; set; }
    public Rectangle Bounds { get; set; }
    public int Weight { get; set; }
    public Rectangle GlobalBounds => Bounds;
    public PaddingTrait Padding { get; set; } = new();

    public Canvas(Game game, Rectangle bounds, Texture2D spriteSheetTexture, Dictionary<string, (Rectangle, int[]?)> sourceRectangles)
    {
        SpriteSheetTexture = spriteSheetTexture;
        SourceRectangles = sourceRectangles;
        Bounds = bounds;
        RenderTools = new RenderTools(this, game.GraphicsDevice, bounds);
    }

    public void Update(GameTime gameTime)
    {
        foreach (var control in Children)
        {
            RecursiveUpdate(gameTime, control);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        RenderTools.Begin(spriteBatch);

        foreach (var control in Children)
        {
            RecursiveDraw(spriteBatch, control);
        }

        RenderTools.End(spriteBatch);
    }

    private void RecursiveDraw(SpriteBatch spriteBatch, IControl control)
    {
        control.Draw(spriteBatch);

        foreach (var child in control.Children)
        {
            RecursiveDraw(spriteBatch, child);
        }
    }

    private void RecursiveUpdate(GameTime gameTime, IControl control)
    {
        control.Update(gameTime);

        foreach (var child in control.Children)
        {
            RecursiveUpdate(gameTime, child);
        }
    }

    public void Add(IControl control)
    {
        Children.Add(control);
        control.Parent = this;
    }

    public void Remove(IControl control)
    {
        Children.Add(control);
        control.Parent = null;
    }
}
