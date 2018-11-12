using System;
using MGUI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Controls
{
    public class Label : PaddedControl
    {
        private string text = string.Empty;
        public string Text
        {
            get => text;
            set
            {
                text = value; 
                Invalidate();
            }
        }

        public SpriteFont SpriteFont { get; set; }
        public Vector2 ActualDrawLocation { get; private set; }

        public enum TextAnchor
        {
            LeftTop,
            CenterTop,
            Center,
            CenterLeft,
            LeftBottom,
            Manual
        }

        public TextAnchor Anchor { get; set; } = TextAnchor.LeftTop;
        
        public Label(Canvas canvas) : base(canvas)
        {
            SpriteFont = canvas.DefaultFont;
        }

        public override void Invalidate()
        {
            
            var size = string.IsNullOrEmpty(Text) ? Vector2.One : SpriteFont.MeasureString(Text);

            var preResizeBounds = Bounds;
            
            this.SizeToParent();
            
            base.Invalidate();
            

            switch (Anchor)
            {
                case TextAnchor.LeftTop:
                    ActualDrawLocation = CanvasBounds.Location.ToVector2();
                    break;
                case TextAnchor.CenterTop:
                    ActualDrawLocation = CanvasBounds.Location.ToVector2();
                    var leftOverWidth = CanvasBounds.Width - size.X;
                    ActualDrawLocation += new Vector2(leftOverWidth/2,0);
                    ActualDrawLocation = ActualDrawLocation.ToPoint().ToVector2();
                    break;
                case TextAnchor.Center:
                    var leftover = CanvasBounds.Size - size.ToPoint();
                    var half = leftover.ToVector2() / 2;
                    ActualDrawLocation = CanvasBounds.Location.ToVector2() + half.ToPoint().ToVector2();
                    break;
                case TextAnchor.LeftBottom:                   
                    var leftoverHeight = CanvasBounds.Size - size.ToPoint();
                    ActualDrawLocation = CanvasBounds.Location.ToVector2() + leftoverHeight.ToVector2();
                    break;
                case TextAnchor.CenterLeft:
                    ActualDrawLocation = CanvasBounds.Location.ToVector2();
                    var leftOverHeight = CanvasBounds.Height - size.Y;
                    ActualDrawLocation += new Vector2(0,leftOverHeight/2);
                    ActualDrawLocation = ActualDrawLocation.ToPoint().ToVector2();
                    break;
                case TextAnchor.Manual:
                    //Don't change it.
                    var withSize = preResizeBounds;
                    withSize.Size = size.ToPoint();
                    Bounds = withSize;
                    ActualDrawLocation = (Parent.CanvasBounds.Location + Bounds.Location).ToVector2();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Draw(SpriteBatch batcher)
        {
            //Buttons use label offset to move text when clicked. Need to apply the offset.
            var newLoc = ActualDrawLocation;
            if (Text == null) return;
            batcher.DrawString(SpriteFont, Text, newLoc, Color);
        }
        
        public Label(IControl parent) : base(parent)
        {
            SpriteFont = Canvas.DefaultFont;
        }
    }
}