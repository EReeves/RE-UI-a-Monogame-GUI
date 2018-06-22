using System;
using System.Linq;
using MGUI.Core;
using MGUI.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Color = Microsoft.Xna.Framework.Color;
using KeyboardInput =  MGUI.Core.Utility.KeyboardInput;

namespace MGUI.Controls
{
    public class InputText : Control
    {
        private string text = string.Empty;
        public string Text
        {
            get => text;
            set
            {
                text = value;
                if(label!=null)
                    SetLabelText(value);
            }
        }

        private Label label;
        private Color textColor = Color.White;
        private bool focused = false;
        public int TextPadding { get; set; } = 5;

        public event EventHandler<string> OnReturn;

        public Color TextColor
        {
            get => textColor;
            set
            {
                textColor = value;
                if (label != null)
                    label.Color = value;
            }
        }

        private void SetLabelText(string text)
        {
            if(focused)
                text += "|";
            
            var size = label.SpriteFont.MeasureString(text);
            if (size.X > Bounds.Width)
            {
                while (label.SpriteFont.MeasureString(text).X > Bounds.Width - (TextPadding*2))
                {
                    text = text.Substring(1);
                }
            }

            label.Text = text;
        }

        public InputText(Canvas canvas) : base(canvas)
        {
            Core.Utility.KeyboardInput.CharPressed += KeyboardInputOnCharPressed;
            Core.Utility.KeyboardInput.KeyPressed += KeyboardInputOnKeyPressed;
        }

        private void KeyboardInputOnKeyPressed(object sender, KeyboardInput.KeyEventArgs e, KeyboardState ks)
        {
            if (!focused) return;
            switch (e.KeyCode)
            {
                case Keys.Back when Text.Length >= 1:
                    Text = Text.Substring(0, Text.Length - 1);
                    break;
                case Keys.Enter:
                    OnReturn?.Invoke(this, Text);
                    Text = string.Empty;;
                    focused = false;
                    break;
            }
        }

        private void KeyboardInputOnCharPressed(object sender, KeyboardInput.CharacterEventArgs e, KeyboardState ks)
        {
            if (!focused) return;
            var c = e.Character;
            Text += c;
        }

        public override void Invalidate()
        {
            label.Color = textColor;
            label.Offset = new Point(CanvasBounds.X + TextPadding, CanvasBounds.Y);
            
            
            //Centre text in bounds, or try to.
            var stringSize = label.SpriteFont.MeasureString("Hello");
            var difference = Bounds.Height - stringSize.Y;
            var offset = label.Offset;
            offset.Y += (int)(difference / 2);
            label.Offset = offset;
            
            SetLabelText(Text);         
            
            base.Invalidate();
        }

        public override void Layout()
        {
            label = new Label(Canvas)
            {
                Text = Text,
                Bounds = new Rectangle(5,0,1,1)
            };
            base.Layout();
        }

        private void MouseInput()
        {
            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton != ButtonState.Pressed) return;
            var mouseRect = new Rectangle(mouseState.X,mouseState.Y,1,1);
            var f = mouseRect.Intersects(CanvasBounds);
            if (f == !focused)
            {
                focused = f;
                SetLabelText(Text); //Redraw text, it changes from focused/unfocused.
                return;
            }

            focused = f;
        }

        public override void Update(GameTime gameTime)
        {
            MouseInput();
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch batcher)
        {
            base.Draw(batcher);
            if(focused)
                Canvas.RenderTools.RenderOutline(batcher, CanvasBounds, Color.Goldenrod,2);
            label.Draw(batcher);
        }
    }
}