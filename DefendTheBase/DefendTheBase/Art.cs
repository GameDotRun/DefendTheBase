using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefendTheBase
{
    class Art
    {
        // A single white pixel.
        public static Texture2D Pixel { get; private set; }

        // Small font used for debug info.
        public static SpriteFont DebugFont { get; private set; }

        public static void Load(ContentManager content)
        {
            Pixel = content.Load<Texture2D>("Art/Images/Pixel");

            DebugFont = content.Load<SpriteFont>("Art/Fonts/DebugFont");
        }
    }

}
