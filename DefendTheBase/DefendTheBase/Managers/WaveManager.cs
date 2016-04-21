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
        [Flags]
        public enum Questions
        { 
            WWIIWinner,
            SovietLeader,
            GermanLeader,
            ItalianLeader,
            WWIIStartDate,
            GermanPolandInvasion,
            NaziLightningWar,
            BattleOfBritain,
            AmericanBomb
        }



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
        static bool spawnTroop = true;
        static float fade = 1f;

        public static void Update(GameTime gameTime)
        {
            if (!WaveStarted)
            {
                WaveStartTimer -= gameTime.ElapsedGameTime;

                if (!waveCountPop)
                {
                    UiManager.UiScreens[1].StringList[3].Add(new UiTextString(Art.DebugFont, "Wave in 60 seconds", new Vector2(GameManager.ScreenSize.X / 3, GameManager.ScreenSize.Y / 2), Color.Red));
                    UiManager.UiScreens[1].StringList[3][UiManager.UiScreens[1].StringList[3].Count - 1].StringScale = 5f;
                    waveCountPop = true;

                    GenerateQuestion();
                }

                if (UiManager.UiScreens[1].StringList[3].Count != 0)
                {
                    UiManager.UiScreens[1].StringList[3][0].StringColour *= fade;
                    fade -= 0.001f;
                }

                if (fade <= 0)
                {
                    if (UiManager.UiScreens[1].StringList[3].Count != 0)
                        UiManager.UiScreens[1].StringList[3].RemoveAt(UiManager.UiScreens[1].StringList[3].Count - 1);
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
                            EnemyManager.SpawnEnemy(EnemyManager.TypeIDs[GameManager.rnd.Next(0, EnemyManager.TypeIDs.Count())], new Vector2(0, 0));
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

        public static void GenerateQuestion()
        {
            string CurrentQuestion;
            string[] CurrentAnswers;
            string CorrectAnswer;

            CurrentAnswers = new string[3];

            Questions nextQuestion = (Questions)GameManager.rnd.Next(1, 10);

            switch (nextQuestion)
            {
                case Questions.AmericanBomb:
                    CurrentQuestion = QuestionStrings.AmericanBomb;
                    CurrentAnswers = QuestionStrings.AmericanBombAnswers;
                    CorrectAnswer = QuestionStrings.AmericanBombCorrect;
                    break;

                case Questions.BattleOfBritain:
                    CurrentQuestion = QuestionStrings.BattleOfBritain;
                    CurrentAnswers = QuestionStrings.BattleOfBritainAnswers;
                    CorrectAnswer = QuestionStrings.BattleOfBritainCorrect;
                    break;

                case Questions.GermanLeader:
                    CurrentQuestion = QuestionStrings.GermanLeader;
                    CurrentAnswers = QuestionStrings.GermanLeaderAnswers;
                    CorrectAnswer = QuestionStrings.GermanLeaderCorrect;
                    break;

                case Questions.GermanPolandInvasion:
                    CurrentQuestion = QuestionStrings.GermanPolandInvasion;
                    CurrentAnswers = QuestionStrings.GermanPolandInvasionAnswers;
                    CorrectAnswer = QuestionStrings.GermanPolandInvasionCorrect;
                    break;

                case Questions.ItalianLeader:
                    CurrentQuestion = QuestionStrings.ItalianLeader;
                    CurrentAnswers = QuestionStrings.ItalianLeaderAnswers;
                    CorrectAnswer = QuestionStrings.ItalianLeaderCorrect;
                    break;

                case Questions.NaziLightningWar:
                    CurrentQuestion = QuestionStrings.NaziLightningWar;
                    CurrentAnswers = QuestionStrings.NaziLightningWarAnswers;
                    CorrectAnswer = QuestionStrings.NaziLightningWarCorrect;
                    break;

                case Questions.SovietLeader:
                    CurrentQuestion = QuestionStrings.SovietLeader;
                    CurrentAnswers = QuestionStrings.SovietLeaderAnswers;
                    CorrectAnswer = QuestionStrings.SovietLeaderCorrect;
                    break;

                case Questions.WWIIStartDate:
                    CurrentQuestion = QuestionStrings.WWIIStartDate;
                    CurrentAnswers = QuestionStrings.WWIIStartDateAnswers;
                    CorrectAnswer = QuestionStrings.WWIIStartDateCorrect;
                    break;

                case Questions.WWIIWinner:
                    CurrentQuestion = QuestionStrings.WWIIWinner;
                    CurrentAnswers = QuestionStrings.WWIIWinnerAnswers;
                    CorrectAnswer = QuestionStrings.WWIIWinnerCorrect;
                    break;

                default:
                     CurrentQuestion = QuestionStrings.WWIIWinner;
                    CurrentAnswers = QuestionStrings.WWIIWinnerAnswers;
                    CorrectAnswer = QuestionStrings.WWIIWinnerCorrect;
                    break;
            }

            QuestionPopUpManager.Add(new QuestionPopUp(CurrentQuestion, CurrentAnswers[0], CurrentAnswers[1], CurrentAnswers[2], CorrectAnswer ));
        
        
        
        }

    }
}
