using MGUI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Controls
{
    /// <summary>
    /// A blank control for drawing or debugging purposes.
    /// </summary>
    public class BlankControl : Control
    {
        public override void Layout()
        {
            
        }

        public BlankControl(Canvas canvas) : base(canvas)
        {
        }
    }
}