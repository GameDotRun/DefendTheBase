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
        public static int EnemiesKilled = 0;

        static int WaveEnemiesSpawned = 0;
        static float WaveSpawnInterval = 1000f;
        static int WavePower = 2;

        static bool WaveEndInit = false;
        static bool FirstWaveIntro = false;


        static bool spawnTroop = true;
        static float fade = 1f;

        public static List<string> WaveComposition = new List<string>();

        static int compositionIndex = 0;

        public static void Update(GameTime gameTime)
        {
            if (WaveNumber == 1 && FirstWaveIntro)
            {
                FirstWaveIntro = false;
                if(MessageBoxManager.MessageBox.Count == 0)
                    MessageBoxManager.Add(new MessageBox(MessageBoxManager.Introduction));
            
            }


            if (!WaveStarted && !MessageBoxManager.MessageDisplayed && GameManager.GameState == GameManager.GameStates.PlayScreen)
            {
                if (!QuestionPopUpManager.QuestionUp)
                    WaveStartTimer -= gameTime.ElapsedGameTime;

                if (!WaveEndInit)
                {
                    WaveEndInit = true;

                    WaveCompositionCreator();

                    GameManager.ModifyResources(1000);

                    if(QuestionPopUpManager.QuestionsArray.Count != 0)
                        GenerateQuestion();
                    if (GameManager.Manpower != 0 && GameManager.BaseHealth < 100)
                    { 
                        float temp = 100 - GameManager.BaseHealth;
                        temp = temp * (GameManager.Manpower / 100);
                        GameManager.BaseHealth += temp * 3;

                        if (GameManager.BaseHealth > 100)
                            GameManager.BaseHealth = 100;
                    }

                    for(int i = 0; i < 3; i++)
                        TroopManager.SpawnTroop();
                }
            }


            if (WaveStartTimer <= TimeSpan.Zero || WaveStarted)
            {
                WaveStarted = true;

                if (WaveStarted)
                {
                    EnemyManager.Update(gameTime);

                    float ttMili = (GameManager.FPS * 1000 / GameRoot.targetTime.Milliseconds);
                    TimeSpan timeAdjust = new TimeSpan(0, 0, 0, 0, (int)ttMili / 1000);

                    EnemySpawnTimer += gameTime.ElapsedGameTime;

                    if (GameRoot.SpeedUp)
                    {
                        EnemySpawnTimer += timeAdjust;
                    }

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
            WaveEndInit = false;
            WaveStarted = false;
            WaveNumber++;
            WavePower++;
            //WaveEnemyAmount = (WaveNumber * 75) + (int)(WavePower * 0.5f);
            //WaveSpawnInterval = (WaveEnemyAmount / WaveNumber) * 10f;
            WaveEnemiesSpawned = 0;
            WaveEnemyAmount+=2;
            if (WaveNumber < 50)
                WaveSpawnInterval-= 10;
           
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

            if(WaveManager.WaveNumber > 10)
                UseableEnemies.Add("SoldierBlue");

            if (WaveManager.WaveNumber > 12)
                UseableEnemies.Add("TransportBlue");

            if (WaveManager.WaveNumber > 14)
                UseableEnemies.Add("JeepBlue");

            if (WaveManager.WaveNumber > 16)
                UseableEnemies.Add("TankBlue");

            if (WaveManager.WaveNumber > 20)
                UseableEnemies.Add("SoldierRed");

            if (WaveManager.WaveNumber > 22)
                UseableEnemies.Add("TransportRed");

            if (WaveManager.WaveNumber > 24)
                UseableEnemies.Add("JeepRed");

            if (WaveManager.WaveNumber > 26)
                UseableEnemies.Add("TankRed");

                // UseableEnemies.Add("TankBlue");
               //   UseableEnemies.Add("TankRed");
               // UseableEnemies.Add("JeepBlue");
               // UseableEnemies.Add("JeepRed");
               // UseableEnemies.Add("TransportBlue");
               // UseableEnemies.Add("TransportRed");
               // UseableEnemies.Add("SoldierBlue");
              //  UseableEnemies.Add("SoldierRed");

            WavePower = (WaveNumber * 20);

            if (WaveNumber > 26)
                WavePower = WaveNumber * 40;

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
                    CurrentWavePower += 8;
                }

                else if (UseableEnemies[index] == "Jeep")
                {
                    CurrentWavePower += 10;
                }

                else if (UseableEnemies[index] == "Tank")
                {
                    CurrentWavePower += 16;
                }

                else if (UseableEnemies[index] == "Helicopter")
                {
                    CurrentWavePower += 10;
                }

                if (UseableEnemies[index] == "SoldierBlue")
                {
                    CurrentWavePower++;
                }

                else if (UseableEnemies[index] == "TransportBlue")
                {
                    CurrentWavePower += 8;
                }

                else if (UseableEnemies[index] == "JeepBlue")
                {
                    CurrentWavePower += 10;
                }

                else if (UseableEnemies[index] == "TankBlue")
                {
                    CurrentWavePower += 16;
                }

                if (UseableEnemies[index] == "SoldierRed")
                {
                    CurrentWavePower++;
                }

                else if (UseableEnemies[index] == "TransportRed")
                {
                    CurrentWavePower += 8;
                }

                else if (UseableEnemies[index] == "JeepRed")
                {
                    CurrentWavePower += 10;
                }

                else if (UseableEnemies[index] == "TankRed")
                {
                    CurrentWavePower += 16;
                }

               WaveComposition.Add(UseableEnemies[index]);

               //WaveComposition.Add("Tank");
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

        }

        public static void Reset()
        {
            questionsAnsweredCorrect = 0;
            WaveStarted = false;
            WaveNumber = 1;
            WaveEnemyAmount = 20;
            EnemiesKilled = 0;
            WaveEnemiesSpawned = 0;
            WaveSpawnInterval = 1000f;
            WavePower = 2;

            WaveEndInit = false;
            FirstWaveIntro = true;

        }
    }
}
