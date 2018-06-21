using System.CodeDom;
using System.Collections.Generic;
using MGUI.Controls;
using MGUI.Controls.Layout;
using MGUI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            //GUI Setup, can be adapted to suit game's texture atlas system.
            var screenBounds = new Rectangle(0, 0, 800, 600);
            var texture = Content.Load<Texture2D>("texture"); //Load texture atlas
            var sourceRects =  //Source rectangles and nine patch coordinates.
                new
                    Dictionary<string, (Rectangle, int[])>  
                    {      //texture name              //source rect                 //nine patch, defaults to 10,10,10,10 if null.
                        ["defaultBackground"] = (new Rectangle(0, 0, 40, 40), null),
                        ["windowBackground"] = (new Rectangle(40, 0, 40, 40), new[] {10, 10, 10, 15})
                    };
            var spriteFonts = new Dictionary<string, SpriteFont>
            {
                ["arial"] = Content.Load<SpriteFont>("Arial 11")
            };
            
            //Off we go!
            Canvas = new Canvas(GraphicsDevice, screenBounds, texture, sourceRects, spriteFonts, "arial");

            //Setup finished.

            //Set up a few controls.

            //Just a basic control that draws a background with no extra functionality until I put some in.
            var control = new Window(Canvas)
            {
                Bounds = new Rectangle(100, 100, 300, 200), //Relative to Canvas
                Color = Color.DarkGray,
            };

            var pad = new Padding(Canvas, 10);
            
            pad.Add(new BlankControl(Canvas)
            {
                Bounds =  new Rectangle(5,0,300,200),
                Color = Color.Bisque
            });
            
            control.Add(pad);

            //Create a control to go inside it.
            var innerControl = new Button(Canvas)
            {
                Bounds = new Rectangle(30, 30, 70, 30), //Relative to parent control
                Color = Color.DarkGreen,
                DrawOverflow = false
            };
            var buttonLabel = new Label(Canvas)
            {
                Bounds = new Rectangle(22, 6, 1, 1),
                Color = Color.WhiteSmoke,
                Text = "Go! aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaah"
            };
            innerControl.Add(buttonLabel);

            var label = new Label(Canvas)
            {
                Bounds = new Rectangle(30, 80, 1, 1),
                Color = Color.Black,
                Text = "Hello World~!"
            };
            
            control.Add(innerControl);
            control.Add(label);


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