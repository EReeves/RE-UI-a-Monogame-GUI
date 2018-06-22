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

            //Setup finished.

            //Set up a few controls.

            //Just a basic control that draws a background with no extra functionality until I put some in.
            var control = new Window(Canvas)
            {
                Bounds = new Rectangle(100, 100, 300, 210), //Relative to Canvas
                Color = Color.DarkGray,
            };
            
            var win = new Window(Canvas)
            {
                BarHeight = 15,
                Bounds = new Rectangle(200, 100, 200, 250),
                Color = Color.White
            };
            
            var max = new Maximize(Canvas); //To Maximize layout inside window.    
            var vertical = new VerticalLayout(Canvas);

            var blank = new BlankControl(Canvas)
            {
                Weight = 2,
                Color = Color.Aqua
            };
            vertical.Add(blank);
            
            var inputText = new InputText(Canvas)
            {
                Weight = 1,
                Color = Color.Red
            };
            vertical.Add(inputText);
            
            var btn = new Button(Canvas)
            {
                Weight = 1,
                Color = Color.Brown,
                Text = "Go!"
            };
            vertical.Add(btn);
            
            max.Add(vertical); //Resize to the window.
            win.Add(max); //Add to window.
            Canvas.Add(win); //Add to canvas.
            

            var input = new InputText(Canvas)
            {
                Bounds = new Rectangle(110,30,160,30),
                Color = Color.Black
            };
            control.Add(input);

            //Create a control to go inside it.
            var innerControl = new Button(Canvas)
            {
                Bounds = new Rectangle(30, 30, 70, 30), //Relative to parent control
                Color = Color.DarkSlateBlue,
                DrawOverflow = false
            };
            var buttonLabel = new Label(Canvas)
            {
                Bounds = new Rectangle(22, 6, 1, 1),
                Color = Color.WhiteSmoke,
                Text = "Go!"
            };
            innerControl.Add(buttonLabel);

            var label = new Label(Canvas)
            {
                Bounds = new Rectangle(30, 80, 1, 1),
                Color = Color.WhiteSmoke,
                Text = "Hello World~!"
            };
            
            control.Add(innerControl);
            control.Add(label);

            innerControl.OnClick += (sender, args) => { label.Text = input.Text; };
            input.OnReturn += (sender, s) => label.Text = s;
            //Finally add the control to the canvas.
            Canvas.Add(control);

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