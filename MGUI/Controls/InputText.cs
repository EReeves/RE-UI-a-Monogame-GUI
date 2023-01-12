using System;
using System.Linq;
using MGUI.Core;
using MGUI.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Color = Microsoft.Xna.Framework.Color;
using KeyboardInput = MGUI.Core.Utility.KeyboardInput;

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
                if (label != null)
                    SetLabelText(value);
            }
        }

        //Used to set label
        private Label? label;
        private Color labelColor = Color.White;

        private bool focused = false;
        public Point TextOffset { get; set; } = new Point(4, 0);

        public event EventHandler<string> OnReturn;

        public Color TextColor
        {
            get => labelColor;
            set
            {
                labelColor = value;
                if (label != null)
                    label.Color = value;
            }
        }

        private void SetLabelText(string text)
        {
            if (focused)
                text += "|";

            var size = label!.SpriteFont.MeasureString(text);
            if (size.X > Bounds.Width)
            {
                while (label.SpriteFont.MeasureString(text).X > Bounds.Width - (TextOffset.X))
                {
                    text = text[1..];
                }
            }

            label.Text = text;
        }


        private void KeyboardInputOnKeyPressed(object sender, KeyboardInput.KeyEventArgs e, KeyboardState ks)
        {
            if (!focused) return;
            switch (e.KeyCode)
            {
                case Keys.Back when Text.Length >= 1:
                    Text = Text[..^1];
                    break;
                case Keys.Enter:
                    OnReturn?.Invoke(this, Text);
                    Text = string.Empty; ;
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
            label ??= new Label(this)
            {
                Text = Text,
                Anchor = Label.TextAnchor.CenterLeft,
                Bounds = new Rectangle(TextOffset.X, TextOffset.Y, 1, 1)
            };

            label.Color = labelColor;

            if (Text.Length != 0)
                SetLabelText(Text);

            base.Invalidate();


            NinePatchCache = RenderTools.CalculateNinePatch(Canvas.SourceRectangles["recessed"].sourceRect, GlobalBounds,
                Canvas.SourceRectangles["recessed"].ninePatch);
        }


        private void MouseInput()
        {
            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton != ButtonState.Pressed) return;
            var mouseRect = new Rectangle(mouseState.X, mouseState.Y, 1, 1);
            var f = mouseRect.Intersects(GlobalBounds);
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
            RenderTools.DrawNinePatch(batcher, Canvas.SpriteSheet, NinePatchCache.SourcePatches, NinePatchCache.DestinationPatches, Color);

            base.Draw(batcher);
            if (focused)
                Canvas.RenderTools.RenderOutline(batcher, GlobalBounds, Color.Goldenrod, 2);
            label.Draw(batcher);
        }

        private void Listen()
        {
            Core.Utility.KeyboardInput.CharPressed += KeyboardInputOnCharPressed;
            Core.Utility.KeyboardInput.KeyPressed += KeyboardInputOnKeyPressed;
        }

        public InputText(Canvas canvas) : base(canvas)
        {
            Listen();
        }

        public InputText(IControl parent) : base(parent)
        {
            Listen();
        }
    }
}