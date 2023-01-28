using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using MGUI.Controls;
using MGUI.Controls.Layout;
using MGUI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TexturePackerLoader;

namespace Game.Desktop
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private readonly GraphicsDeviceManager graphics;

        //MGUI
        private Canvas canvas;
        private SpriteBatch? spriteBatch;
        private Texture2D? texture;
        private Dictionary<string, SpriteFont>? spriteFonts;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsFixedTimeStep = false;
            IsMouseVisible = true;
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            base.Initialize();
        }


        protected override void LoadContent()
        {
            //Load textures, you can do this however you like. Textuer packer isn't free afterall.
            var loader = new SpriteSheetLoader(Content);
            var spriteSheet = loader.Load("texture");

            //GUI Setup, can be adapted to suit game's texture atlas system.
            var screenBounds = new Rectangle(0, 0, 800, 600);
            texture = spriteSheet.Texture; //Load texture atlas
            var sourceRects = //Source rectangles and nine patch coordinates.
                new
                    Dictionary<string, (Rectangle, int[]?)>
                {
                    //texture name              //source rect                 //nine patch, defaults to 10,10,10,10 if null.
                    ["whiteTexture"] = (spriteSheet.Sprite("whitetexture").SourceRectangle, null),
                    ["background"] = (spriteSheet.Sprite("background").SourceRectangle, null),
                    ["window"] = (spriteSheet.Sprite("floatingbackground").SourceRectangle,
                            new[] { 20, 30, 20, 20 }),
                    ["corgi"] = (spriteSheet.Sprite("Corgi").SourceRectangle, null),
                    ["buttonup"] = (spriteSheet.Sprite("buttonup").SourceRectangle, new[] { 10, 10, 10, 10 }),
                    ["buttondown"] = (spriteSheet.Sprite("buttondown").SourceRectangle, null),
                    ["recessed"] = (spriteSheet.Sprite("recessed").SourceRectangle, null),
                    ["checkbox"] = (spriteSheet.Sprite("checkbox").SourceRectangle, null),
                    ["checkboxclicked"] = (spriteSheet.Sprite("checkboxclicked").SourceRectangle, null)
                };

            canvas = new Canvas(this, screenBounds, texture, sourceRects);

            var window = new Window(canvas)
            {
                Bounds = new Rectangle(0, 0, 400, 600)
            };

            var vert = new VerticalLayout(canvas)
            {
                Bounds = new Rectangle(0, 0, Int32.MaxValue, Int32.MaxValue)
            };
            window.Add(vert);

            var inner = new Window(canvas)
            {
                Bounds = new Rectangle(0, 0, 100, 100)
            };
            var inner2 = new Window(canvas)
            {
                Bounds = new Rectangle(0, 0, 100, 100),
                Weight = 2
            };

            vert.Add(inner);
            vert.Add(inner2);

            vert.LayoutChildren();


            spriteBatch = new SpriteBatch(GraphicsDevice);


            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            canvas.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            canvas?.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            texture?.Dispose();

            base.UnloadContent();
        }
    }
}