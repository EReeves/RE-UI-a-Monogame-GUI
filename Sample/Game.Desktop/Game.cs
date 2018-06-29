using System.Collections.Generic;
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
        private SpriteBatch spriteBatch;
        private Texture2D texture;
        private Dictionary<string, SpriteFont> spriteFonts;

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
                    Dictionary<string, (Rectangle, int[])>
                    {
                        //texture name              //source rect                 //nine patch, defaults to 10,10,10,10 if null.
                        ["whiteTexture"] = (spriteSheet.Sprite("whitetexture").SourceRectangle, null),
                        ["background"] = (spriteSheet.Sprite("background").SourceRectangle, null),
                        ["windowBackground"] = (spriteSheet.Sprite("floatingbackground").SourceRectangle,
                            new[] {20, 20, 20, 20}),
                        ["corgi"] = (spriteSheet.Sprite("Corgi").SourceRectangle, null),
                        ["buttonup"] = (spriteSheet.Sprite("buttonup").SourceRectangle, new[] {20, 20, 20, 30}),
                        ["buttondown"] = (spriteSheet.Sprite("buttondown").SourceRectangle, null),
                        ["recessed"] = (spriteSheet.Sprite("recessed").SourceRectangle, null)
                    };
            spriteFonts = new Dictionary<string, SpriteFont>
            {
                ["arial"] = Content.Load<SpriteFont>("Arial 11")
            };

            //Off we go!
            canvas = new Canvas(this, screenBounds, texture, sourceRects, spriteFonts, "arial");

            //Window.
            var window = new Window(canvas)
            {
                TitleBarHeight = 20,
                Bounds = new Rectangle(200, 100, 250, 350),
                Color = Color.White,
                PaddingExplicit = new[] {12, 16, 12, 15} 
            };

            //Give it a layout.
            var verticalLayout = new VerticalLayout(window);

            //Add controls to our layout.

            var blank = new Image(verticalLayout)
            {
                Weight = 3,
                Texture = "corgi"
            };
            var inputText = new InputText(verticalLayout)
            {
                Weight = 1,
            };
            var button = new Button(verticalLayout)
            {
                Weight = 1,
                Text = "Go!",
            };

            //Invalidae the whole UI and we're done.
            canvas.Invalidate();


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
            
            canvas.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            spriteFonts["arial"].Texture.Dispose();
            texture.Dispose();
            
            base.UnloadContent();
        }
    }
}