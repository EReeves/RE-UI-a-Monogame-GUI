using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MGUI.Controls.Layout;
using MGUI.Core.Trait;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Core;

public class Canvas : Control, ILayout
{
    public Texture2D SpriteSheetTexture { get; }
    public Dictionary<string, (Rectangle sourceRect, int[]? ninePatch)> SourceRectangles { get; }
    public RenderTools RenderTools { get; set; }

    //INewControl Impl
    public Canvas(Game game, Rectangle bounds, Texture2D spriteSheetTexture, Dictionary<string, (Rectangle, int[]?)> sourceRectangles) : base(null)
    {
        SpriteSheetTexture = spriteSheetTexture;
        SourceRectangles = sourceRectangles;
        Bounds = bounds;
        RenderTools = new RenderTools(this, game.GraphicsDevice);
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var control in Children)
        {
            RecursiveUpdate(gameTime, control);
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        foreach (var control in Children)
        {
            RecursiveDraw(spriteBatch, control);
        }
    }

    private void RecursiveDraw(SpriteBatch spriteBatch, Control control)
    {
        control.Draw(spriteBatch);

        foreach (var child in control.Children)
        {
            RecursiveDraw(spriteBatch, child);
        }
    }

    private void RecursiveUpdate(GameTime gameTime, Control control)
    {
        control.Update(gameTime);

        foreach (var child in control.Children)
        {
            RecursiveUpdate(gameTime, child);
        }
    }

    public void Invalidate()
    {
        foreach (var child in Children)
        {
            RecursiveInvalidate(child);
        }
    }

    private void RecursiveInvalidate(Control control)
    {
        if(control is ILayout layout)
        {
            layout.Invalidate();
        }

        foreach (var child in control.Children)
        {
            RecursiveInvalidate(child);
        }
    }

}
