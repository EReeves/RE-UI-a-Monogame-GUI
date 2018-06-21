using System;
using System.Data;
using MGUI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MGUI.Controls
{
    /// <summary>
    /// A basic button.
    /// </summary>
    public class Button : Control
    {
        public bool Clicked { get; private set; } = false;
        public Color ClickedTintColor { get; set; } = Color.Goldenrod;
        public Color HoverTintColor { get; set; } = Color.DarkOliveGreen;
        private Color actualColor;

        public event EventHandler OnClick;
        
        public Button(Canvas canvas) : base(canvas)
        {
        }

        public override void Update(GameTime gameTime)
        {
            ClickUpdate();
            base.Update(gameTime);
        }

        public override void Layout()
        {
            actualColor = Color;
        }

        public void ClickUpdate()
        {
            var mouseState = Mouse.GetState();
            var pos = mouseState.Position;
            var mousePosRect = new Rectangle(pos.X,pos.Y,1,1);
            
            if (!mousePosRect.Intersects(CanvasBounds))
            {
                Clicked = false;
                Color = actualColor;
                return;
            }

            var clicked = mouseState.LeftButton == ButtonState.Pressed;
            if (!Clicked && clicked) //State not clicked, but clicked.
            {
                OnClick?.Invoke(this, EventArgs.Empty);
                Clicked = true;
                return;
            }

            if (!clicked)
            {
                Clicked = false;
                Color = actualColor;
            }
            Color = clicked ? ClickedTintColor : HoverTintColor;
        }
    }
}