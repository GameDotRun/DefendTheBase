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
        private const int DEFAULT_RESOURCES = 2500;

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

        private static float m_manPower = DEFAULT_MANPOWER;
        private static int m_resources = DEFAULT_RESOURCES;

        public static Coordinates mouseSqrCoords;
        public static GameStates GameState;
        public static BuildStates BuildState;
        public static float Manpower { get { return m_manPower; } }
        public static int Resources { get { return m_resources; } }

        static UiSideGameScreen UiSideScreen;
        static UiTopGameScreen UiTopScreen;


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

        public static void Init(GraphicsDevice graphics )
        {
            rnd = new Random();
            UiSideScreen = new UiSideGameScreen(graphics);
            UiTopScreen = new UiTopGameScreen(graphics);
            EnemyListener.InitiliseListener();
            TowerListener.InitiliseListener();
            TroopListener.InitiliseListener();
        }

        public static void Update(GameTime gameTime)
        {
            m_manPower = TroopListener.TroopList.Count();

            UiSideScreen.Update();
            UiTopScreen.Update();

            WaveManager.Update(gameTime);
            TowerManager.Update();
            TroopManager.Update(gameTime);
            EffectManager.Update(gameTime);
            QuestionPopUpManager.Update();
        }

        public static void Draw(SpriteBatch sb)
        {
            
            EffectManager.Draw(sb, 0);
            TowerManager.Draw(sb);
            EnemyManager.Draw(sb);
            TroopManager.Draw(sb);
            EffectManager.Draw(sb, 1);
            QuestionPopUpManager.Draw(sb);

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
            //Add modifications for each enemy here
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

        public static float DamageCalculator(int Damage, Enemy enemy, Projectile.Type projectile)
        {
            float totalDamage = 0f;
            int baseDmg = Damage;
            Projectile.Type proj = projectile;
            string enemyType = enemy.EnemyType;


            if (enemyType == "Soldier")
            {
                if (projectile == Projectile.Type.Gun)
                {
                    totalDamage = baseDmg * 3f;
                }

                else
                    totalDamage = baseDmg / 2f;      
            }

            else if (enemyType == "Tank")
            {
                if (projectile == Projectile.Type.Rocket)
                {
                    totalDamage = baseDmg * 4f;
                }

                else
                    totalDamage = baseDmg / 10f;     
            }

            else if (enemyType == "Helicopter")
            {
                if (projectile == Projectile.Type.SAM)
                {
                    totalDamage = baseDmg * 2.5f;
                }

                else
                    totalDamage = baseDmg / 1.5f;
            
            }

            else if (enemyType == "Jeep")
            {
                if (projectile == Projectile.Type.Rocket)
                {
                    totalDamage = baseDmg * 2.5f;
                }

                else
                    totalDamage = baseDmg / 2f;
            }

            else if (enemyType == "Transport")
            {
                if (projectile == Projectile.Type.Rocket)
                {
                    totalDamage = baseDmg * 5f;
                }

                else
                    totalDamage = baseDmg / 3f;
            
            }

            return totalDamage;
        }

    }
}
