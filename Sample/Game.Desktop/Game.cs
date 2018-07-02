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
                        ["buttonup"] = (spriteSheet.Sprite("buttonup").SourceRectangle, new[] {10, 10, 10, 10}),
                        ["buttondown"] = (spriteSheet.Sprite("buttondown").SourceRectangle, null),
                        ["recessed"] = (spriteSheet.Sprite("recessed").SourceRectangle, null),
                        ["checkbox"] = (spriteSheet.Sprite("checkbox").SourceRectangle, null),
                        ["checkboxclicked"] = (spriteSheet.Sprite("checkboxclicked").SourceRectangle, null)
                    };
            spriteFonts = new Dictionary<string, SpriteFont>
            {
                ["arial"] = Content.Load<SpriteFont>("Arial 11")
            };

            var windowPadding = new[] {12, 16, 12, 15};

            //Off we go!
            canvas = new Canvas(this, screenBounds, texture, sourceRects, spriteFonts, "arial");

            //Window.
            var window = new Window(canvas)
            {
                TitleBarHeight = 20,
                Bounds = new Rectangle(100, 100, 250, 400),
                Color = Color.White,
                PaddingExplicit = windowPadding
            };

            //Give it a layout.
            var verticalLayout = new VerticalLayout(window);

            //Add controls to our layout.

            var blank = new Image(verticalLayout)
            {
                Weight = 4,
                Texture = "corgi"
            };
            var horizontalLayout = new HorizontalLayout(verticalLayout)
            {
               // OuterPadding =  50,
                Weight = 1
            };
            var checkbox = new Checkbox(horizontalLayout)
            {
               Weight = 1,
               Clicked = true
            };
            var label = new Label(horizontalLayout)
            {
                Weight = 4,
                Text = "Checkbox", 
            };
            var horizontalLayout2 = new HorizontalLayout(verticalLayout)
            {
                SidePadding = 10,
                Weight = 1
            };
            var checkbox2= new Checkbox(horizontalLayout2)
            {
                Weight = 1,
     
            };
            var label2 = new Label(horizontalLayout2)
            {
                Weight = 4,
                Text = "Checkbox", 
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

            
            
            
            var multilineWindpw = new Window(canvas)
            {
                Bounds = new Rectangle(400,100,300,200),
                TitleBarHeight = 20,
                PaddingExplicit = windowPadding
            };
            var mlabel = new MultiLabel(multilineWindpw);
            
            var h = new HorizontalLayout(multilineWindpw)
            {
                Bounds = new Rectangle(0, multilineWindpw.Bounds.Height - 80, multilineWindpw.Bounds.Width,80),
                InnerPadding = 10,
                SidePadding = 0
            };
            var i = new InputText(h)
            {
                Weight = 3
            };
            var b = new Button(h)
            {
                Weight = 1,
                Text = "Send"
            };
            
            mlabel.Text.Add("Multiline text");
            mlabel.Text.Add("Hello there");
            mlabel.Text.Add("Hello there");
            mlabel.Text.Add("Hello there");
            mlabel.Text.Add("Hello there");

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