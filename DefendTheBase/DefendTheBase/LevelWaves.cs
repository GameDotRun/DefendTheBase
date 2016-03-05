using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DefendTheBase
{
    public static class LevelWaves
    {
        public static bool WaveStarted = false;

        public static int WaveNumber = 1;
        static int WaveEnemyAmount = 25;
        public static int WaveEnemiesUsed = 0;
        static int WaveEnemiesSpawned = 0;
        static TimeSpan WaveSpawnTimer = TimeSpan.Zero;
        static float WaveSpawnInterval = (WaveEnemyAmount / WaveNumber) * 10f;
        static float WavePower = 2;

        public static void Update(GameTime gameTime)
        {
            if (WaveStarted)
            {
                EnemyCreator.Update();

                WaveSpawnTimer += gameTime.ElapsedGameTime;

                if (WaveSpawnTimer.TotalMilliseconds >= WaveSpawnInterval)
                {
                    if (WaveEnemiesSpawned != WaveEnemyAmount)
                    {
                        EnemyCreator.SpawnEnemy(EnemyCreator.TypeIDs[GameRoot.rnd.Next(0, EnemyCreator.TypeIDs.Count())]);
                        WaveEnemiesSpawned++;
                    }

                    WaveSpawnTimer = TimeSpan.Zero;
                }

                if (WaveEnemiesUsed == WaveEnemyAmount)
                    WaveIncrease();
            }
        }

        static void WaveIncrease()
        {
            WaveStarted = false;
            WaveNumber++;
            WavePower++;
            WaveEnemyAmount = (WaveNumber * 75) + (int)(WavePower * 0.5f);
            WaveSpawnInterval = (WaveEnemyAmount / WaveNumber) * 10f;
           
        }
    }
}
