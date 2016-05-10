using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;

namespace DefendTheBase
{
    public class Art
    {
        // A single white pixel.
        public static Texture2D Pixel { get; private set; }

        public static Texture2D Base { get; private set; }
        public static Texture2D Background { get; private set; }

        public static Texture2D[] TrenchTexs { get; private set; }

        public static Texture2D Concrete { get; private set; }
        public static Texture2D ConcreteBlocked { get; private set; }
        public static Texture2D GroundTexs { get; private set; }
        public static Texture2D BlockedSquare { get; private set; }
        public static Texture2D EnemyTex { get; private set; }

        public static Texture2D[] TowerGun { get; private set; }
        public static Texture2D[] TowerRocket { get; private set; }
        public static Texture2D[] TowerSAM { get; private set; }
        public static Texture2D[] TowerTesla { get; private set; }

        public static Texture2D TankBottom { get; private set; }
        public static Texture2D TankTop { get; private set; }
        public static Texture2D JeepBottom { get; private set; }
        public static Texture2D JeepTop { get; private set; }
        public static Texture2D Soldier { get; private set; }
        public static Texture2D FriendlySoldier { get; private set; }
        public static Texture2D FriendlyMechanic { get; private set; }
        public static Texture2D Helicopter { get; private set; }
        public static Texture2D Transport { get; private set; }

        public static Texture2D TankBottomBlue { get; private set; }
        public static Texture2D TankTopBlue { get; private set; }
        public static Texture2D TankBottomRed { get; private set; }
        public static Texture2D TankTopRed { get; private set; }
        public static Texture2D SoldierBlue { get; private set; }
        public static Texture2D SoldierRed { get; private set; }
        public static Texture2D JeepBottomBlue { get; private set; }
        public static Texture2D JeepTopBlue { get; private set; }
        public static Texture2D JeepBottomRed { get; private set; }
        public static Texture2D JeepTopRed { get; private set; }
        public static Texture2D TransportBlue { get; private set; }
        public static Texture2D TransportRed { get; private set; }

        public static Texture2D Projectile { get; private set; }
        public static Texture2D ProjectileGun { get; private set; }
        public static Texture2D ProjectileRocket { get; private set; }
        public static Texture2D ProjectileSAM { get; private set; }
        public static Texture2D ProjectileTesla { get; private set; }

        public static Texture2D uiUp { get; private set; }
        public static Texture2D uiSide { get; private set; }
        public static Texture2D[] ButtonsBase { get; private set; }
        public static Texture2D[] ButtonsTower { get; private set; }
        public static Texture2D[] ButtonsMisc { get; private set; }

        public static Texture2D BloodSplats { get; private set; }
        public static Texture2D Explosions { get; private set; }

        public static Texture2D[] HpBar { get; private set; }
        // Small font used for debug info.
        public static SpriteFont DebugFont { get; private set; }
        public static SpriteFont UiFont { get; private set; }
        public static SpriteFont InfoFont { get; private set; }
        public static SpriteFont HelpFont { get; private set; }

        public static Texture2D TextBoxBackGround { get; private set; }

        public static Texture2D HitlerPort { get; private set; }
        public static Texture2D ChurchilPort { get; private set; }
        public static Texture2D AxisPort { get; private set; }
        public static Texture2D AlliesPort { get; private set; }
        public static Texture2D FrenchiePort { get; private set; }
        public static Texture2D HiroHitoPort { get; private set; }
        public static Texture2D MussoPort { get; private set; }
        public static Texture2D StalinPort { get; private set; }

        public static Texture2D ButtonEffectTexture { get; private set; }
        public static Texture2D tabTestTexture { get; private set; }

        public static Texture2D StartMenuBackground { get; private set; }

        public static Texture2D HelpButton { get; private set; }
        public static Texture2D HelpButtonOff { get; private set; }

        public static Texture2D EndScreenBackground { get; private set; }

        public static Video StartVideo { get; private set; }
        public static Video TutVideo { get; private set; }

        public static void Load(ContentManager content)
        {
            Pixel = content.Load<Texture2D>("Art/Images/Misc/Pixel");   // Flecks Art Contribution.

            // VIDEOS
            StartVideo = content.Load<Video>("Art/Videos/IntroVideo");
            TutVideo = content.Load<Video>("Art/Videos/TutVideo");
            GameManager.videoPlayer = new VideoPlayer();
            GameManager.videoPlayer.IsLooped = false;

            Base = content.Load<Texture2D>("Art/Images/Misc/base");
            Background = content.Load<Texture2D>("Art/Images/UI/background");

            // Trenches
            TrenchTexs = new Texture2D[16];
            for (int i = 0; i < TrenchTexs.Count(); i++)
                TrenchTexs[i] = content.Load<Texture2D>("Art/Images/Trenches/trench_" + i);

            // Terrain
            Concrete = content.Load<Texture2D>("Art/Images/Terrain/concrete");
            ConcreteBlocked = content.Load<Texture2D>("Art/Images/Terrain/concreteBlocked");
            GroundTexs = content.Load<Texture2D>("Art/Images/Terrain/new-grass");
            BlockedSquare = content.Load<Texture2D>("Art/Images/Terrain/squareBlocked");
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
            JeepBottom = content.Load<Texture2D>("Art/Images/UnitsEnemy/jeep_bottom");
            JeepTop = content.Load<Texture2D>("Art/Images/UnitsEnemy/jeep_top");
            Soldier = content.Load<Texture2D>("Art/Images/UnitsEnemy/soldier");
            Helicopter = content.Load<Texture2D>("Art/Images/UnitsEnemy/helicopter");
            Transport = content.Load<Texture2D>("Art/Images/UnitsEnemy/transport");
            TankBottomBlue = content.Load<Texture2D>("Art/Images/UnitsEnemy/Tank_bottom_blue");
            TankTopBlue = content.Load<Texture2D>("Art/Images/UnitsEnemy/Tank_top_blue");
            TankBottomRed = content.Load<Texture2D>("Art/Images/UnitsEnemy/Tank_bottom_red");
            TankTopRed = content.Load<Texture2D>("Art/Images/UnitsEnemy/Tank_top_red");
            JeepBottomBlue = content.Load<Texture2D>("Art/Images/UnitsEnemy/jeep_bottom_blue");
            JeepTopBlue = content.Load<Texture2D>("Art/Images/UnitsEnemy/jeep_top");
            JeepTopRed = content.Load<Texture2D>("Art/Images/UnitsEnemy/jeep_top");
            JeepBottomRed = content.Load<Texture2D>("Art/Images/UnitsEnemy/jeep_bottom_red");
            SoldierBlue = content.Load<Texture2D>("Art/Images/UnitsEnemy/soldier_blue");
            SoldierRed = content.Load<Texture2D>("Art/Images/UnitsEnemy/soldier_red");
            TransportBlue = content.Load<Texture2D>("Art/Images/UnitsEnemy/transport_blue");
            TransportRed = content.Load<Texture2D>("Art/Images/UnitsEnemy/transport_red");

            //Friendlies
            FriendlySoldier = content.Load<Texture2D>("Art/Images/UnitsPlayer/army_man_good");
            FriendlyMechanic = content.Load<Texture2D>("Art/Images/UnitsPlayer/mechanic");

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
            ButtonsTower = new Texture2D[5];
            for (int i = 0; i < 5; i++)
                ButtonsTower[i] = content.Load<Texture2D>("Art/Images/UI/Buttons/ButtonTower" + i);
            ButtonsMisc = new Texture2D[1];
            for (int i = 0; i < 1; i++)
                ButtonsMisc[i] = content.Load<Texture2D>("Art/Images/UI/Buttons/ButtonMisc" + i);

            HpBar = new Texture2D[2];
            for (int i = 0; i < 2; i++)
                HpBar[i] = content.Load<Texture2D>("Art/Images/UI/hpBar" + i);

            //effects
            BloodSplats = content.Load<Texture2D>("Art/Images/Effect/BLOOD");
            Explosions = content.Load<Texture2D>("Art/Images/Effect/explosion-sprite");
            ButtonEffectTexture = content.Load<Texture2D>("Art/Images/UI/Buttons/ButtonEffect");
            tabTestTexture = content.Load<Texture2D>("Art/Images/UI/Buttons/tabTestTex");

            //tab portraits
            HitlerPort = content.Load<Texture2D>("Art/Images/Portraits/Hitler");
            ChurchilPort = content.Load<Texture2D>("Art/Images/Portraits/Churchil");
            AxisPort = content.Load<Texture2D>("Art/Images/Portraits/AxisFlag");
            AlliesPort = content.Load<Texture2D>("Art/Images/Portraits/AlliesFlag");
            FrenchiePort = content.Load<Texture2D>("Art/Images/Portraits/Frenchie");
            HiroHitoPort = content.Load<Texture2D>("Art/Images/Portraits/Hirohito");
            MussoPort = content.Load<Texture2D>("Art/Images/Portraits/Musso");
            StalinPort = content.Load<Texture2D>("Art/Images/Portraits/Stalin");

            // Fonts
            DebugFont = content.Load<SpriteFont>("Art/Fonts/DebugFont");
            UiFont = content.Load<SpriteFont>("Art/Fonts/UiFont");
            HelpFont = content.Load<SpriteFont>("Art/Fonts/HelpFont");
            InfoFont = content.Load<SpriteFont>("Art/Fonts/InfoFont");

            TextBoxBackGround = content.Load<Texture2D>("Art/Images/Misc/TextBoxBackground");

            StartMenuBackground = content.Load<Texture2D>("Art/Images/Misc/StartMenu");
            EndScreenBackground = content.Load<Texture2D>("Art/Images/Misc/EndScreen");

            HelpButton = content.Load<Texture2D>("Art/Images/Misc/HelpButton");
            HelpButtonOff = content.Load<Texture2D>("Art/Images/Misc/HelpButtonOff");
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
