using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MGUI.Core;
using MGUI.Core.Trait;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MGUI.Controls
{
    public class Button : Control
    {
        private DrawTrait upDraw;
        private DrawTrait downDraw;
        private ButtonState wasLeftDown = ButtonState.Released;
        private bool tint = false;
        public Button(Canvas canvas) : base(canvas)
        {
            upDraw = new(canvas)
            {
                NinePatchTexture = "buttonup"
            };
            downDraw = new(canvas)
            {
                NinePatchTexture = "buttondown",
                Hide = true
            };
        }

        public Color Color
        {
            get => upDraw.Color;
            set
            {
                upDraw.Color = value;
                downDraw.Color = value;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            upDraw.Draw(spriteBatch, GlobalBounds);
            downDraw.Draw(spriteBatch, GlobalBounds);

            if (tint)
                Canvas.RenderTools.RenderOutline(spriteBatch, GlobalBounds, Color.Goldenrod, 2);
        }

        public override void Update(GameTime gameTime)
        {
            ClickUpdate();
        }

        private void ClickUpdate()
        {
            var mouseState = Mouse.GetState();
            var mouseRect = new Rectangle(mouseState.X, mouseState.Y, 1, 1);
            if (!mouseRect.Intersects(GlobalBounds))
            {
                ButtonUp();
                UnTint();
            }
            else
            {
                Tint();

                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (wasLeftDown == ButtonState.Released)
                    {
                        //click
                    }
                    else
                    {
                        //hold
                    }

                    ButtonDown();
                    UnTint();
                }
                else
                {
                    ButtonUp();
                }
            }

            wasLeftDown = mouseState.LeftButton;
        }

        private void ButtonDown()
        {
            upDraw.Hide = true;
            downDraw.Hide = false;
        }

        private void Tint()
        {
            tint = true;
        }

        private void UnTint()
        {
            tint = false;
        }


        private void ButtonUp()
        {
            upDraw.Hide = false;
            downDraw.Hide = true;
        }
    }
}