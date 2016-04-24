using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using RPGEx;
using Microsoft.Xna.Framework;

namespace DefendTheBase
{
    public class GameManager
    {
        private const float DEFAULT_MANPOWER = 100f;
        private const int DEFAULT_RESOURCES = 1000;

        public enum GameStates
        {
            StartScreen,
            PlayScreen,
            WinScreen,
            LoseScreen,
            InfoScreen,
        }

        public enum BuildStates
        {
            // This decides what a mouse click does.
            Nothing,
            Destroy,
            Upgrade,
            Trench,
            Concrete,
            TowerGun,
            TowerRocket,
            TowerSAM,
            TowerTesla
        }

        [Flags]
        public enum Unlocks
        { 
            None = 0,
            RocketTower = 1,
            SamTower = 2,
            TeslaTower = 4
        }

        private static float m_manPower = DEFAULT_MANPOWER;
        private static int m_resources = DEFAULT_RESOURCES;

        public static Coordinates mouseSqrCoords;
        public static GameStates GameState;
        public static BuildStates BuildState;
        public static Unlocks UnlockedTowers;
        public static float Manpower { get { return m_manPower; } }
        public static int Resources { get { return m_resources; } }

        public static UiSideGameScreen UiSideScreen;
        public static UiTopGameScreen UiTopScreen;


        //Grid Size
        public const int SQUARESIZE = 50;
        public const int HEIGHT = 15;
        public const int WIDTH = 20;
        public const int DEFAULYDIST = 2000; //temp default counter for pathfinding
        public static Coordinates STARTPOINT = new Coordinates(0, 0);
        public static Coordinates ENDPOINT = new Coordinates(18, 13, 0);

        //ui Borders
        public const int BORDERTOP = 125;
        public const int BORDERRIGHT = 250;
        public const int BORDERLEFT = 0;

        //game speed
        public const int UPS = 20; // Updates per second
        public const int FPS = 60; //Frames per second

        public static Grid grid;
        public static Random rnd;

        public static Vector2 ScreenSize; // ScreenSize
        public static Vector2 MouseScreenPos;

        public static void Init(GraphicsDevice graphics )
        {
            rnd = new Random();
            UiSideScreen = new UiSideGameScreen(graphics);
            UiTopScreen = new UiTopGameScreen(graphics);
            EnemyListener.InitiliseListener();
            TowerListener.InitiliseListener();
            TroopListener.InitiliseListener();
            QuestionPopUpManager.Init();

        }

        public static void Update(GameTime gameTime)
        {
            if(mouseSqrCoords != null)
                MouseScreenPos = new Vector2(mouseSqrCoords.x * SQUARESIZE, mouseSqrCoords.y * SQUARESIZE + BORDERTOP);

            m_manPower = TroopListener.TroopList.Count();

            UiSideScreen.Update();
            UiTopScreen.Update();

            QuestionPopUpManager.Update();
            WaveManager.Update(gameTime);
            TowerManager.Update();
            TroopManager.Update(gameTime);
            EffectManager.Update(gameTime);
            PopUpTextManager.Update(gameTime);
            PopUpNotificationManager.Update(gameTime);
            MessageBoxManager.Update(gameTime);
            
        }

        public static void Draw(SpriteBatch sb)
        {
            
            EffectManager.Draw(sb, 0);
            TowerManager.Draw(sb);
            EnemyManager.Draw(sb);
            TroopManager.Draw(sb);
            EffectManager.Draw(sb, 1);
            PopUpTextManager.Draw(sb);
            QuestionPopUpManager.Draw(sb);
            PopUpNotificationManager.Draw(sb);
            MessageBoxManager.Draw(sb);

        }

        public static void ModifyManpower(float value)
        {
            m_manPower += value;
        }

        public static void ModifyResources(int value)
        {
            m_resources += value;
        }

        public static void ResetValues()
        {
            m_manPower = DEFAULT_MANPOWER;
            m_resources = DEFAULT_RESOURCES;
        }

        public static void EnemyWasDestroyed(string EnemyType)
        {
            //Add modifications for each enemy here (what resources you recieve from enemies after kill)
            if (EnemyType == "Tank")
                m_resources += 100;
            else if (EnemyType == "Helicopter")
                m_resources += 100;
            else if (EnemyType == "Transport")
                m_resources += 100;
            else if (EnemyType == "Jeep")
                m_resources += 100;
            else if (EnemyType == "Soldier")
                m_resources += 100;
        }

        //costs stuff
        public static void TowerWasBuilt(string TowerType)
        {
            if (TowerType == "Gun")
            {
                m_resources -= BuildManager.Resources;
                TroopManager.DestroyTroop(BuildManager.ManPower);
            }
            else if (TowerType == "Rocket")
            {
                m_resources -= BuildManager.Resources;
                TroopManager.DestroyTroop(BuildManager.ManPower);
            }
            else if (TowerType == "SAM")
            {
                m_resources -= BuildManager.Resources;
                TroopManager.DestroyTroop(BuildManager.ManPower);
            }
            else if (TowerType == "Tesla")
            {
                m_resources -= BuildManager.Resources;
                TroopManager.DestroyTroop(BuildManager.ManPower);
            }
        
        }

        public static void BaseWasBuilt(string BaseType)
        {
            if (BaseType == "Trench")
                m_resources -= BuildManager.Resources;

            else if (BaseType == "Concrete")
                m_resources -= BuildManager.Resources;
        }

        // modify the values here for different costs
        public static void CostGet()
        {
            switch (GameManager.BuildState)
            {
                case GameManager.BuildStates.TowerGun:
                    BuildManager.ManPower = 1;
                    BuildManager.Resources = 100;
                    break;

                case GameManager.BuildStates.TowerRocket:
                    BuildManager.ManPower = 2;
                    BuildManager.Resources = 300;
                    break;

                case GameManager.BuildStates.TowerSAM:
                    BuildManager.ManPower = 4;
                    BuildManager.Resources = 1000;
                    break; ;

                case GameManager.BuildStates.TowerTesla:
                    BuildManager.ManPower = 1;
                    BuildManager.Resources = 100;
                    break;

                case GameManager.BuildStates.Concrete:
                    BuildManager.ManPower = 1;
                    BuildManager.Resources = 100;
                    break;

                case GameManager.BuildStates.Upgrade:
                    BuildManager.ManPower = 1;
                    BuildManager.Resources = 100;
                    break;

                case GameManager.BuildStates.Trench:
                    BuildManager.ManPower = 1;
                    BuildManager.Resources = 50;
                    break;
            }

        }


        //damage stuffs
        public static float DamageCalculator(int Damage, Enemy enemy, Projectile.Type projectile)
        {
            float totalDamage = 0f;
            float resistedDamage;
            int baseDmg = Damage;
            Projectile.Type proj = projectile;
            string enemyType = enemy.EnemyType;

            Color textColour = Color.Black;
            string popUpText = "";

            resistedDamage = Resist(enemy );

 
                if (enemyType == "Soldier")
                {
                    if (projectile == Projectile.Type.Gun)
                    {
                        popUpText = PopUpTextManager.Effective;
                        textColour = Color.DarkKhaki;
                        totalDamage = TotalDamageCrunch(baseDmg, resistedDamage, enemy, ref popUpText, ref textColour) * 2;
                    }

                    else
                    {
                        popUpText = PopUpTextManager.Resist;
                        textColour = Color.Yellow;
                        totalDamage = TotalDamageCrunch(baseDmg, resistedDamage, enemy, ref popUpText, ref textColour) / 2;
                    }
                }

                else if (enemyType == "Tank")
                {
                    if (projectile == Projectile.Type.Rocket)
                    {
                        popUpText = PopUpTextManager.Effective;
                        textColour = Color.DarkKhaki;
                        totalDamage = TotalDamageCrunch(baseDmg, resistedDamage, enemy, ref popUpText, ref textColour) * 2;
                    }

                    else
                    {
                        popUpText = PopUpTextManager.Resist;
                        textColour = Color.Yellow;
                        totalDamage = TotalDamageCrunch(baseDmg, resistedDamage, enemy, ref popUpText, ref textColour) / 4;
                    }
                }

                else if (enemyType == "Helicopter")
                {
                    if (projectile == Projectile.Type.SAM)
                    {
                        popUpText = PopUpTextManager.Effective;
                        textColour = Color.DarkKhaki;
                        totalDamage = TotalDamageCrunch(baseDmg, resistedDamage, enemy, ref popUpText, ref textColour) * 2;
                    }

                    else
                    {
                        popUpText = PopUpTextManager.Resist;
                        textColour = Color.Yellow;
                        totalDamage = TotalDamageCrunch(baseDmg, resistedDamage, enemy, ref popUpText, ref textColour) / 2;
                    }

                }

                else if (enemyType == "Jeep")
                {
                    if (projectile == Projectile.Type.Rocket)
                    {
                        popUpText = PopUpTextManager.Effective;
                        textColour = Color.DarkKhaki;
                        totalDamage = TotalDamageCrunch(baseDmg, resistedDamage, enemy, ref popUpText, ref textColour) * 4;
                    }

                    else
                    {
                        popUpText = PopUpTextManager.Resist;
                        textColour = Color.Yellow;
                        totalDamage = TotalDamageCrunch(baseDmg, resistedDamage, enemy, ref popUpText, ref textColour) / 2;
                    }
                }

                else if (enemyType == "Transport")
                {
                    if (projectile == Projectile.Type.Rocket)
                    {
                        popUpText = PopUpTextManager.Effective;
                        textColour = Color.DarkKhaki;
                        totalDamage = TotalDamageCrunch(baseDmg, resistedDamage, enemy, ref popUpText, ref textColour) * 3;
                    }

                    else
                    {
                        popUpText = PopUpTextManager.Resist;
                        textColour = Color.Yellow;
                        totalDamage = TotalDamageCrunch(baseDmg, resistedDamage, enemy, ref popUpText, ref textColour) / 2;
                    }

                }
            


            PopUpTextManager.Add(new PopUpText(popUpText, enemy.ScreenPos, textColour));

            return totalDamage;
        }

        static float TotalDamageCrunch(float baseDam, float resistedDam, Enemy enemy, ref string text, ref Color textColour)
        {
            float totalDamage;
            float critical;

            totalDamage = baseDam - resistedDam;

            critical = Critical(enemy);

            totalDamage += totalDamage * critical;

            if (critical != 0)
            {
                text += " " + PopUpTextManager.Critical;
            }

            if (totalDamage == 0)
            {
                text += " " + PopUpTextManager.NoEffect;
                textColour = Color.Red;

            }

            return totalDamage;
        }

        static float Critical(Enemy enemy)
        {
            int CriticalValue = rnd.Next(1, 101);

            if (CriticalValue < enemy.criticalResist)
                return 0;

            else return 2;

        }

        static float Resist(Enemy enemy)
        {
            float ResistValue = rnd.Next(1, (int)enemy.resistance);

            return ResistValue / 10;
        
        }
    }
}
