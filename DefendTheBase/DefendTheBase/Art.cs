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

        public static Texture2D Concrete { get; private set; }
        public static Texture2D GroundTexs { get; private set; }
        public static Texture2D EnemyTex { get; private set; }

        public static Texture2D[] TowerGun { get; private set; }
        public static Texture2D[] TowerRocket { get; private set; }
        public static Texture2D[] TowerSAM { get; private set; }
        public static Texture2D[] TowerTesla { get; private set; }

        public static Texture2D TankBottom { get; private set; }
        public static Texture2D TankTop { get; private set; }

        public static Texture2D Projectile { get; private set; }
        public static Texture2D ProjectileGun { get; private set; }
        public static Texture2D ProjectileRocket { get; private set; }
        public static Texture2D ProjectileSAM { get; private set; }
        public static Texture2D ProjectileTesla { get; private set; }

        public static Texture2D uiUp { get; private set; }
        public static Texture2D uiSide { get; private set; }
        public static Texture2D[] ButtonsBase { get; private set; }
        public static Texture2D[] ButtonsTower { get; private set; }

        // Small font used for debug info.
        public static SpriteFont DebugFont { get; private set; }

        public static void Load(ContentManager content)
        {
            Pixel = content.Load<Texture2D>("Art/Images/Misc/Pixel");   // Flecks Art Contribution.
            // Trenches
            TrenchTexs = new Texture2D[16];
            for (int i = 0; i < TrenchTexs.Count(); i++)
                TrenchTexs[i] = content.Load<Texture2D>("Art/Images/Trenches/trench_" + i);

            // Terrain
            Concrete = content.Load<Texture2D>("Art/Images/Terrain/concrete");
            GroundTexs = content.Load<Texture2D>("Art/Images/Terrain/Durt");
            EnemyTex = content.Load<Texture2D>("Art/Images/Misc/ghostSquare");

            // Towers
            TowerGun = new Texture2D[4];
            for (int i = 0; i < TowerGun.Length; i++)
                TowerGun[i] = content.Load<Texture2D>("Art/Images/Towers/GunTower/minigun_lvl" + (i + 1) + "Turret");
            TowerRocket = new Texture2D[4];
            for (int i = 0; i < TowerRocket.Length; i++)
                TowerRocket[i] = content.Load<Texture2D>("Art/Images/Towers/RocketTower/RocketTurretLvl" + (i + 1));
            TowerSAM = new Texture2D[4];
            for (int i = 0; i < TowerSAM.Length; i++)
                TowerSAM[i] = content.Load<Texture2D>("Art/Images/Towers/SAMTower/SAMmk" + (i + 1));
            TowerTesla = new Texture2D[4];
            for (int i = 0; i < TowerTesla.Length; i++)
                TowerTesla[i] = content.Load<Texture2D>("Art/Images/Towers/TeslaTower/teslatower-mk" + (i + 1));

            //Enemies
            TankBottom = content.Load<Texture2D>("Art/Images/UnitsEnemy/Tank_bottom");
            TankTop = content.Load<Texture2D>("Art/Images/UnitsEnemy/Tank_top");


            // Projectiles
            Projectile = content.Load<Texture2D>("Art/Images/Projectiles/Projectile");
            ProjectileGun = content.Load<Texture2D>("Art/Images/Projectiles/bullet");
            ProjectileRocket = content.Load<Texture2D>("Art/Images/Projectiles/missile");
            ProjectileSAM = content.Load<Texture2D>("Art/Images/Projectiles/sam-missile");
            ProjectileTesla = content.Load<Texture2D>("Art/Images/Projectiles/tesla-zappy");

            // UI
            uiUp = content.Load<Texture2D>("Art/Images/UI/ui-template-topbar");
            uiSide = content.Load<Texture2D>("Art/Images/UI/ui-side");
            ButtonsBase = new Texture2D[4];
            for (int i = 0; i < 4; i++)
                ButtonsBase[i] = content.Load<Texture2D>("Art/Images/UI/Buttons/ButtonBase" + i);
            ButtonsTower = new Texture2D[4];
            for (int i = 0; i < 4; i++)
                ButtonsTower[i] = content.Load<Texture2D>("Art/Images/UI/Buttons/ButtonTower" + i);

            // Fonts
            DebugFont = content.Load<SpriteFont>("Art/Fonts/DebugFont");

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
