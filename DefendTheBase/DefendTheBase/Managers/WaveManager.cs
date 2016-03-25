using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DefendTheBase
{
    public static class WaveManager
    {
        public static TimeSpan EnemySpawnTimer = TimeSpan.Zero;
        public static bool WaveStarted = false;
        public static int WaveNumber = 1;
        public static int WaveEnemyAmount = 100;
        public static int WaveEnemiesUsed = 0;

        static int WaveEnemiesSpawned = 0;
        static float WaveSpawnInterval = 200f;
        static float WavePower = 2;

        public static void Update(GameTime gameTime)
        {
            if (WaveStarted)
            {
                EnemyManager.Update(gameTime);

                EnemySpawnTimer += gameTime.ElapsedGameTime;

                if (EnemySpawnTimer.TotalMilliseconds >= WaveSpawnInterval)
                {
                    if (WaveEnemiesSpawned != WaveEnemyAmount)
                    {
                        EnemyManager.SpawnEnemy(EnemyManager.TypeIDs[GameRoot.rnd.Next(0, EnemyManager.TypeIDs.Count())], new Vector2(0,0));
                        WaveEnemiesSpawned++;
                    }

                    EnemySpawnTimer = TimeSpan.Zero;
                }

                if (WaveEnemiesUsed >= WaveEnemyAmount)
                    WaveIncrease();
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
