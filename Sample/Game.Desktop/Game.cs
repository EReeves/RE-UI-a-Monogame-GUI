using System.CodeDom;
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
        private Canvas Canvas;
        private SpriteBatch spriteBatch;
        private RasterizerState uIRasterizerState;

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
            TexturePackerLoader.SpriteSheetLoader loader = new SpriteSheetLoader(Content);
            var spriteSheet = loader.Load("texture");
            
            //GUI Setup, can be adapted to suit game's texture atlas system.
            var screenBounds = new Rectangle(0, 0, 800, 600);
            var texture = spriteSheet.Texture; //Load texture atlas
            var sourceRects =  //Source rectangles and nine patch coordinates.
                new
                    Dictionary<string, (Rectangle, int[])>  
                    {      //texture name              //source rect                 //nine patch, defaults to 10,10,10,10 if null.
                        ["whiteTexture"] = (spriteSheet.Sprite("whitetexture").SourceRectangle, null),
                        ["background"] = (spriteSheet.Sprite("background").SourceRectangle, null),
                        ["windowBackground"] = (spriteSheet.Sprite("floatingbackground").SourceRectangle, new[] {20, 20, 20, 20})
                    };
            var spriteFonts = new Dictionary<string, SpriteFont>
            {
                ["arial"] = Content.Load<SpriteFont>("Arial 11")
            };
            
            //Off we go!
            Canvas = new Canvas(this, screenBounds, texture, sourceRects, spriteFonts, "arial");

            //Window.
            var window = new Window(Canvas)
            {
                TitleBarHeight = 15,
                Bounds = new Rectangle(200, 100, 200, 250),
                Color = Color.White,
            };
            //Give it some padding
            var paddedLayout = new PaddedLayout(window)
            {
                Padding = 10
            };
            //Give it a layout.
            var verticalLayout = new VerticalLayout(paddedLayout);
            
            //Add controls to our layout.
            
            var blank = new BlankControl(verticalLayout)
            {
                Weight = 2,
                Color = Color.Aqua
            };
            var inputText = new InputText(verticalLayout)
            {
                Weight = 1,
                Color = Color.Red
            };          
            var button = new Button(verticalLayout)
            {
                Weight = 1,
                Color = Color.Brown,
                Text = "Go!"
            };
           
            //Invalidae the whole UI and we're done.
            Canvas.Invalidate();

            
            spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            Canvas.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            Canvas.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
    }
}