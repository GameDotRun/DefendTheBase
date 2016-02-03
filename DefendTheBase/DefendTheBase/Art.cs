using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefendTheBase
{
    public class Art
    {
        // PATHFINDING CODE
        public enum TrenchEnum
        {
            Horizontal,
            Vertical,
            TUp,
            TDown,
            TLeft,
            TRight,
            EUp,
            EDown,
            ERight,
            ELeft,
            trenchX
        }

        // A single white pixel.
        public static Texture2D Pixel { get; private set; }

        public static Texture2D[] TrenchTexs { get; private set; }
        public static Texture2D GroundTexs { get; private set; }
        public static Texture2D EnemyTex { get; private set; }

        // Small font used for debug info.
        public static SpriteFont DebugFont { get; private set; }

        public static void Load(ContentManager content)
        {
            Pixel = content.Load<Texture2D>("Art/Images/Pixel");

            TrenchTexs = new Texture2D[11];
            for (int i = 0; i < TrenchTexs.Count(); i++)
                TrenchTexs[i] = content.Load<Texture2D>("Art/Images/Trenches/trench_" + i);

            GroundTexs = content.Load<Texture2D>("Art/Images/dirt");
            EnemyTex = content.Load<Texture2D>("Art/Images/ghostSquare");

            DebugFont = content.Load<SpriteFont>("Art/Fonts/DebugFont");
        }

        public static Texture2D getTrenchTex(TrenchEnum trenchEnum)
        {
            Texture2D a = TrenchTexs[0];

            switch (trenchEnum)
            {

                case TrenchEnum.EUp:
                    return TrenchTexs[0];
                    break;

                case TrenchEnum.ERight:
                    return TrenchTexs[1];
                    break;

                case TrenchEnum.ELeft:
                    return TrenchTexs[2];
                    break;

                case TrenchEnum.EDown:
                    return TrenchTexs[3];
                    break;

                case TrenchEnum.TUp:
                    return TrenchTexs[4];
                    break;

                case TrenchEnum.TRight:
                    return TrenchTexs[5];
                    break;

                case TrenchEnum.TLeft:
                    return TrenchTexs[6];
                    break;

                case TrenchEnum.TDown:
                    return TrenchTexs[7];
                    break;

                case TrenchEnum.trenchX:
                    return TrenchTexs[8];
                    break;

                case TrenchEnum.Vertical:
                    return TrenchTexs[9];
                    break;

                case TrenchEnum.Horizontal:
                    return TrenchTexs[10];
                    break;


            }

            return a;
        }
    }

}
