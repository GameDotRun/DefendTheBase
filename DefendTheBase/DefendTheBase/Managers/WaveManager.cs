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
        public static TimeSpan WaveStartTimer = TimeSpan.FromMinutes(1);
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

                    if(QuestionPopUpManager.questionsList.Count != 0)
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
                        if (WaveEnemiesSpawned != WaveEnemyAmount)
                        {
                            EnemyManager.SpawnEnemy(WaveComposition[compositionIndex], new Vector2(0, 0));
                            compositionIndex++;
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

            if (TowerListener.TowersList.Count /2   != 0)
                WavePower *= TowerListener.TowersList.Count / 2;

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
                    CurrentWavePower += 16;
                }

               WaveComposition.Add(UseableEnemies[index]);
                    
            }

            WaveEnemyAmount = WaveComposition.Count;


        }

        public static void GenerateQuestion()
        {
            string CurrentQuestion;
            string[] CurrentAnswers;
            string CorrectAnswer;

            CurrentAnswers = new string[3];

            int nextQuestionindex = GameManager.rnd.Next(0, QuestionPopUpManager.questionsList.Count());
            QuestionPopUpManager.Questions nextQuestion = QuestionPopUpManager.questionsList[nextQuestionindex];

            QuestionPopUpManager.questionsList.Remove(nextQuestion);

            switch (nextQuestion)
            {
                case QuestionPopUpManager.Questions.AmericanBomb:
                    CurrentQuestion = QuestionPopUpManager.AmericanBomb;
                    CurrentAnswers = QuestionPopUpManager.AmericanBombAnswers;
                    CorrectAnswer = QuestionPopUpManager.AmericanBombCorrect;
                    break;

                case QuestionPopUpManager.Questions.BattleOfBritain:
                    CurrentQuestion = QuestionPopUpManager.BattleOfBritain;
                    CurrentAnswers = QuestionPopUpManager.BattleOfBritainAnswers;
                    CorrectAnswer = QuestionPopUpManager.BattleOfBritainCorrect;
                    break;

                case QuestionPopUpManager.Questions.GermanLeader:
                    CurrentQuestion = QuestionPopUpManager.GermanLeader;
                    CurrentAnswers = QuestionPopUpManager.GermanLeaderAnswers;
                    CorrectAnswer = QuestionPopUpManager.GermanLeaderCorrect;
                    break;

                case QuestionPopUpManager.Questions.GermanPolandInvasion:
                    CurrentQuestion = QuestionPopUpManager.GermanPolandInvasion;
                    CurrentAnswers = QuestionPopUpManager.GermanPolandInvasionAnswers;
                    CorrectAnswer = QuestionPopUpManager.GermanPolandInvasionCorrect;
                    break;

                case QuestionPopUpManager.Questions.ItalianLeader:
                    CurrentQuestion = QuestionPopUpManager.ItalianLeader;
                    CurrentAnswers = QuestionPopUpManager.ItalianLeaderAnswers;
                    CorrectAnswer = QuestionPopUpManager.ItalianLeaderCorrect;
                    break;

                case QuestionPopUpManager.Questions.NaziLightningWar:
                    CurrentQuestion = QuestionPopUpManager.NaziLightningWar;
                    CurrentAnswers = QuestionPopUpManager.NaziLightningWarAnswers;
                    CorrectAnswer = QuestionPopUpManager.NaziLightningWarCorrect;
                    break;

                case QuestionPopUpManager.Questions.SovietLeader:
                    CurrentQuestion = QuestionPopUpManager.SovietLeader;
                    CurrentAnswers = QuestionPopUpManager.SovietLeaderAnswers;
                    CorrectAnswer = QuestionPopUpManager.SovietLeaderCorrect;
                    break;

                case QuestionPopUpManager.Questions.WWIIStartDate:
                    CurrentQuestion = QuestionPopUpManager.WWIIStartDate;
                    CurrentAnswers = QuestionPopUpManager.WWIIStartDateAnswers;
                    CorrectAnswer = QuestionPopUpManager.WWIIStartDateCorrect;
                    break;

                case QuestionPopUpManager.Questions.WWIIWinner:
                    CurrentQuestion = QuestionPopUpManager.WWIIWinner;
                    CurrentAnswers = QuestionPopUpManager.WWIIWinnerAnswers;
                    CorrectAnswer = QuestionPopUpManager.WWIIWinnerCorrect;
                    break;

                default:
                    CurrentQuestion = QuestionPopUpManager.WWIIWinner;
                    CurrentAnswers = QuestionPopUpManager.WWIIWinnerAnswers;
                    CorrectAnswer = QuestionPopUpManager.WWIIWinnerCorrect;
                    break;
            }

            QuestionPopUpManager.Add(new QuestionPopUp(CurrentQuestion, CurrentAnswers[0], CurrentAnswers[1], CurrentAnswers[2], CorrectAnswer));
        }

        public static void QuestionsCorrectCheck()
        {
            if(questionsAnsweredCorrect > 2)
            {
                GameManager.UnlockedTowers |= GameManager.Unlocks.RocketTower;
            }

            if (questionsAnsweredCorrect > 5) 
            {
                GameManager.UnlockedTowers |= GameManager.Unlocks.SamTower;
            }

            if (questionsAnsweredCorrect > 8) 
            {
                GameManager.UnlockedTowers |= GameManager.Unlocks.TeslaTower;
            }

            else if (questionsAnsweredCorrect > 11)
            { }

            else if (questionsAnsweredCorrect > 14)
            { }

            else if (questionsAnsweredCorrect > 18)
            { }

            else if (questionsAnsweredCorrect == 20)
            { }
        }
    }
}
