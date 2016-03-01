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
        // A single white pixel.
        public static Texture2D Pixel { get; private set; }

        public static Texture2D[] TrenchTexs { get; private set; }
        public static Texture2D GroundTexs { get; private set; }
        public static Texture2D EnemyTex { get; private set; }
        public static Texture2D uiUp { get; private set; }
        public static Texture2D uiSide { get; private set; }

        // Small font used for debug info.
        public static SpriteFont DebugFont { get; private set; }

        public static void Load(ContentManager content)
        {
            Pixel = content.Load<Texture2D>("Art/Images/Misc/Pixel");

            TrenchTexs = new Texture2D[16];
            for (int i = 0; i < TrenchTexs.Count(); i++)
                TrenchTexs[i] = content.Load<Texture2D>("Art/Images/Trenches/trench_" + i);

            GroundTexs = content.Load<Texture2D>("Art/Images/Terrain/Durt");
            EnemyTex = content.Load<Texture2D>("Art/Images/Misc/ghostSquare");

            DebugFont = content.Load<SpriteFont>("Art/Fonts/DebugFont");
            uiUp = content.Load<Texture2D>("Art/Images/UI/ui-template-topbar");
            uiSide = content.Load<Texture2D>("Art/Images/UI/ui-side");

        }

        public static Texture2D getTrenchTex(string trenchName)
        {
            Texture2D a = TrenchTexs[0];

            switch (trenchName)
            {

                case "Trench_":
                    return TrenchTexs[0];

                case "Trench_N":
                    return TrenchTexs[1];

                case "Trench_NE":
                    return TrenchTexs[2];

                case "Trench_NS":
                    return TrenchTexs[3];

                case "Trench_NW":
                    return TrenchTexs[4];

                case "Trench_NES":
                    return TrenchTexs[5];

                case "Trench_NSW":
                    return TrenchTexs[6];

                case "Trench_NESW":
                    return TrenchTexs[7];

                case "Trench_ES":
                    return TrenchTexs[8];

                case "Trench_EW":
                    return TrenchTexs[9];

                case "Trench_ESW":
                    return TrenchTexs[10];

                case "Trench_SW":
                    return TrenchTexs[11];

                case "Trench_E":
                    return TrenchTexs[12];

                case "Trench_S":
                    return TrenchTexs[13];

                case "Trench_W":
                    return TrenchTexs[14];

                case "Trench_NEW":
                    return TrenchTexs[15];

            }

            return a;
        }
    }

}
