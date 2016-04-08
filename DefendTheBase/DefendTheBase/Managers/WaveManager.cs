using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RPGEx;

namespace DefendTheBase
{
    public static class WaveManager
    {
        public static TimeSpan EnemySpawnTimer = TimeSpan.Zero;
        public static TimeSpan WaveStartTimer = TimeSpan.FromMinutes(1);
        public static bool WaveStarted = false;
        public static int WaveNumber = 1;
        public static int WaveEnemyAmount = 100;
        public static int WaveEnemiesUsed = 0;

        static int WaveEnemiesSpawned = 0;
        static float WaveSpawnInterval = 500f;
        static float WavePower = 2;

        static bool waveCountPop = false;
        static float fade = 1f;

        public static void Update(GameTime gameTime)
        {
            if (!WaveStarted)
            {
                WaveStartTimer -= gameTime.ElapsedGameTime;

                if (!waveCountPop)
                {
                    UiManager.UiScreens[1].StringList[3].Add(new UiTextString(Art.DebugFont, "Wave in 60 seconds", new Vector2(GameRoot.ScreenSize.X / 3, GameRoot.ScreenSize.Y / 2), Color.Red));
                    UiManager.UiScreens[1].StringList[3][UiManager.UiScreens[1].StringList[3].Count - 1].StringScale = 5f;

                    waveCountPop = true;
                }

                
                UiManager.UiScreens[1].StringList[3][0].StringColour *=  fade;
                fade -= 0.001f;

                if (fade <= 0)
                    UiManager.UiScreens[1].StringList[3].RemoveAt(UiManager.UiScreens[1].StringList[3].Count - 1);
            }


            if (WaveStartTimer <= TimeSpan.Zero || WaveStarted)
            {
                WaveStarted = true;

                if (WaveStarted)
                {
                    EnemyManager.Update(gameTime);

                    EnemySpawnTimer += gameTime.ElapsedGameTime;

                    if (EnemySpawnTimer.TotalMilliseconds >= WaveSpawnInterval)
                    {
                        if (WaveEnemiesSpawned != WaveEnemyAmount)
                        {
                            EnemyManager.SpawnEnemy(EnemyManager.TypeIDs[GameRoot.rnd.Next(0, EnemyManager.TypeIDs.Count())], new Vector2(0, 0));
                            WaveEnemiesSpawned++;
                        }

                        EnemySpawnTimer = TimeSpan.Zero;
                    }

                    if (WaveEnemiesUsed >= WaveEnemyAmount)
                        WaveIncrease();
                }

                WaveStartTimer = TimeSpan.FromMinutes(1);
            }
        }

        static void WaveIncrease()
        {
            WaveStarted = false;
            WaveNumber++;
            WavePower++;
            //WaveEnemyAmount = (WaveNumber * 75) + (int)(WavePower * 0.5f);
            //WaveSpawnInterval = (WaveEnemyAmount / WaveNumber) * 10f;
            WaveEnemiesUsed = 0;
            WaveEnemiesSpawned = 0;
            WaveEnemyAmount+=2;
            if (WaveNumber < 200)
                WaveSpawnInterval-=2;
            GameManager.ModifyManpower(25);
           
        }
    }
}
