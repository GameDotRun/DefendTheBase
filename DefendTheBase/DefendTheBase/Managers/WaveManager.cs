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
        public static int questionsAnsweredCorrect = 0;
        public static TimeSpan EnemySpawnTimer = TimeSpan.Zero;
        public static TimeSpan WaveStartTimer = TimeSpan.FromSeconds(60);
        public static bool WaveStarted = false;
        public static int WaveNumber = 1;
        public static int WaveEnemyAmount = 20;
        public static int WaveEnemiesUsed = 0;

        static int WaveEnemiesSpawned = 0;
        static float WaveSpawnInterval = 500f;
        static int WavePower = 2;

        static bool waveCountPop = false;
        static bool spawnTroop = true;
        static float fade = 1f;

        static List<string> WaveComposition = new List<string>();

        static int compositionIndex = 0;

        public static void Update(GameTime gameTime)
        {
            if (!WaveStarted)
            {
                if (!QuestionPopUpManager.QuestionUp)
                    WaveStartTimer -= gameTime.ElapsedGameTime;

                if (!waveCountPop)
                {
                    waveCountPop = true;

                    WaveCompositionCreator();

                    if(QuestionPopUpManager.QuestionsArray.Count != 0)
                        GenerateQuestion();
                }

                if (WaveStartTimer.Seconds % 5 == 0 && spawnTroop)
                {
                    TroopManager.SpawnTroop();
                    spawnTroop = false;
                }

                else if (WaveStartTimer.Seconds % 5 != 0)
                {
                    spawnTroop = true;
                }
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
                
                            if (WaveComposition.Count != 0)
                            {
                                EnemyManager.SpawnEnemy(WaveComposition[0], new Vector2(0, 0));
                                WaveComposition.RemoveAt(0);
                                WaveEnemiesSpawned++;
                            }
                        

                        EnemySpawnTimer = TimeSpan.Zero;
                    }

                    if (EnemyListener.EnemyList.Count == 0 && WaveComposition.Count == 0)
                        WaveIncrease();
                }

                WaveStartTimer = TimeSpan.FromMinutes(1);
            }
        }

        static void WaveIncrease()
        {
            fade = 1f;
            waveCountPop = false;
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

        public static void StartWave()
        {
            WaveStartTimer = TimeSpan.Zero;
        }

        public static void WaveCompositionCreator()
        {
            compositionIndex = 0;

            WaveComposition.Clear();

            int CurrentWavePower = 0;

            List<string> UseableEnemies = new List<string>();

            UseableEnemies.Add("Soldier");

            if (GameManager.UnlockedTowers.HasFlag(GameManager.Unlocks.RocketTower) && WaveManager.WaveNumber > 2)
                UseableEnemies.Add("Transport");

            if (GameManager.UnlockedTowers.HasFlag(GameManager.Unlocks.RocketTower) && WaveManager.WaveNumber > 4)
                UseableEnemies.Add("Jeep");

            if (GameManager.UnlockedTowers.HasFlag(GameManager.Unlocks.RocketTower) && WaveManager.WaveNumber > 6)
                UseableEnemies.Add("Tank");

            if (GameManager.UnlockedTowers.HasFlag(GameManager.Unlocks.SamTower) && WaveManager.WaveNumber > 8)
                UseableEnemies.Add("Helicopter");

            WavePower = (WaveNumber * 10);

            /*if (TowerListener.TowersList.Count /3   != 0)
                WavePower *= TowerListener.TowersList.Count / 3;*/

            while (CurrentWavePower <= WavePower)
            {
                int index = GameManager.rnd.Next(0, UseableEnemies.Count);

                int difference = WavePower - CurrentWavePower;

                if (UseableEnemies[index] == "Soldier")
                {
                    CurrentWavePower++;
                }

                else if (UseableEnemies[index] == "Transport")
                {
                    CurrentWavePower += 2;
                }

                else if (UseableEnemies[index] == "Jeep")
                {
                    CurrentWavePower += 4;
                }

                else if (UseableEnemies[index] == "Tank")
                {
                    CurrentWavePower += 8;
                }

                else if (UseableEnemies[index] == "Helicopter")
                {
                    CurrentWavePower += 3;
                }

               WaveComposition.Add(UseableEnemies[index]);
                    
            }

            WaveEnemyAmount = WaveComposition.Count;


        }

        public static void GenerateQuestion()
        {
            string[] question = QuestionPopUpManager.QuestionsArray[GameManager.rnd.Next(0, QuestionPopUpManager.QuestionsArray.Count)];
            QuestionPopUpManager.Add(new QuestionPopUp(question));
        }

        public static void QuestionsCorrectCheck()
        {
            if(questionsAnsweredCorrect > 2 && !GameManager.UnlockedTowers.HasFlag(GameManager.Unlocks.RocketTower))
            {
                GameManager.UnlockedTowers |= GameManager.Unlocks.RocketTower;
                MessageBoxManager.Add(new MessageBox(MessageBoxManager.RocketTowerUnlock));
            }

            if (questionsAnsweredCorrect > 5 && !GameManager.UnlockedTowers.HasFlag(GameManager.Unlocks.SamTower)) 
            {
                GameManager.UnlockedTowers |= GameManager.Unlocks.SamTower;
                MessageBoxManager.Add(new MessageBox(MessageBoxManager.SAMTowerUnlock));
            }

            if (questionsAnsweredCorrect > 8 && !GameManager.UnlockedTowers.HasFlag(GameManager.Unlocks.TeslaTower)) 
            {
                GameManager.UnlockedTowers |= GameManager.Unlocks.TeslaTower;
                MessageBoxManager.Add(new MessageBox(MessageBoxManager.TeslaTowerUnlock));

            }

            else if (questionsAnsweredCorrect > 11 && !GameManager.UnlockedTowers.HasFlag(GameManager.Unlocks.Upgrade))
            { 
                GameManager.UnlockedTowers |= GameManager.Unlocks.Upgrade;
                MessageBoxManager.Add(new MessageBox(MessageBoxManager.UpgradeTowerUnlock));
            }

            else if (questionsAnsweredCorrect > 14)
            { }

            else if (questionsAnsweredCorrect > 18)
            { }

            else if (questionsAnsweredCorrect == 20)
            { }
        }
    }
}
