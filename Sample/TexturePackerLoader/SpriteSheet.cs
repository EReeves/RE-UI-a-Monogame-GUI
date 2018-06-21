using Microsoft.Xna.Framework;

namespace TexturePackerLoader
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework.Graphics;

    public class SpriteSheet
    {
        private readonly IDictionary<string, SpriteFrame> spriteList;
        public readonly List<Rectangle> SourceRectangles = new List<Rectangle>();
        public Texture2D Texture { get; set; }

        public SpriteSheet()
        {
            spriteList = new Dictionary<string, SpriteFrame>();
        }

        public void Add(string name, SpriteFrame sprite)
        {
            sprite.Name = name;
            spriteList.Add(name, sprite);
            SourceRectangles.Add(sprite.SourceRectangle);
        }

        public void Add(SpriteSheet otherSheet)
        {
            foreach (var sprite in otherSheet.spriteList)
            {
                spriteList.Add(sprite);
                SourceRectangles.Add(sprite.Value.SourceRectangle);
            }
        }

        public SpriteFrame Sprite(string sprite)
        {
            return this.spriteList[sprite];
        }

    }
}