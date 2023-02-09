using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MGUI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGUI.Controls
{
    //For spacing
    public class BlankControl : Control
    {
        public BlankControl(Canvas canvas) : base(canvas)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
  
        }

        public override void Update(GameTime gameTime)
        {
     
        }
    }
}