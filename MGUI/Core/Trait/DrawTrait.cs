using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Core.Trait;

public class DrawTrait
{
    public bool Hide { get; set; } = false;
    public bool DrawDebug { get; set; } = true;
    public bool DrawOverflow { get; set; } = true;
    public Color Color { get; set; } = Color.White;
    public Color DebugColor { get; set; } = Color.Red;

    public string NinePatchTexture { get; set; } = "default";
    private (Rectangle[] source, Rectangle[] dest)? ninepatchSourceDest = null;

    private readonly Canvas system;

    public DrawTrait(Canvas system)
    {
        this.system = system;
    }

    public void CacheNinePatch(Rectangle bounds)
    {
        var (sourceRect, ninePatch) = system.SourceRectangles[NinePatchTexture];
        ninepatchSourceDest = RenderTools.CalculateNinePatch(sourceRect, bounds, ninePatch);
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle bounds)
    {

        if (!Hide)
        {
            var (source, dest) = ninepatchSourceDest ??= RenderTools.CalculateNinePatch(system.SourceRectangles[NinePatchTexture].sourceRect, bounds, system.SourceRectangles[NinePatchTexture].ninePatch);
            RenderTools.DrawNinePatch(spriteBatch, system.SpriteSheetTexture, source, dest, Color);
        }

        if (DrawDebug)
        {
            system.RenderTools.RenderOutline(spriteBatch, bounds, DebugColor);
        }
    }

}
