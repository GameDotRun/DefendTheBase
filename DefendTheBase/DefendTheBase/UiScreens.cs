using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RPGEx;
using Flextensions;

namespace DefendTheBase
{
    public class UiSideGameScreen : Ui
    {
        //these variables hopefully wont be needed when i've made some planned ui functions
        Vector2 tabDrawPos = new Vector2(1000, GameManager.BORDERTOP);
        Vector2 buttonDrawPos = new Vector2(1025, GameManager.BORDERTOP + 60);
        Vector2 buttonSize = new Vector2(200, 100);

        UiTabs tabs;

        //Group Elements up with Lists, allows the ui controller to manipulate them
        public List<UiButton> unitBuild;
        public List<UiButton> baseBuild;
        public List<UiButton> miscBuild;

        public UiSideGameScreen(GraphicsDevice graphicsDevice) : base(GameManager.WIDTH, GameManager.HEIGHT)
        {
            tabs = new UiTabs(graphicsDevice, Art.DebugFont, 3, tabDrawPos, new string[3] { "Towers", "Base", "Misc" }, Color.Aquamarine, new Vector2(83, 40));

            unitBuild = new List<UiButton>();
            baseBuild = new List<UiButton>();
            miscBuild = new List<UiButton>();

            //add grouped elements to uicontrollers, do not add anything that is part of a tab. Just add the tab
            Add(ref tabs);

            CreateUi(graphicsDevice);
        }

        public void Update()
        {
            tabs.Update();
        }

        public void CreateUi(GraphicsDevice graphicsDevice)
        {
          
            //its of UTMOST IMPORTANCE that each button has a unique id

            //Units buttons Here
            unitBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsTower[0], "btn0TowerGun", true));
            //unitBuild[0].StringText = "Gun Tower";
            unitBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsTower[1], "btn0TowerRocket", true));
            //unitBuild[1].StringText = "Rocket Tower";
            unitBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsTower[2], "btn0TowerSAM", true));
            //unitBuild[2].StringText = "SAM Tower";
            unitBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsTower[3], "btn0TowerTesla", true));
            //unitBuild[3].StringText = "Tesla Tower";
            unitBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsTower[4], "btn0Upgrade", true));

            for (int i = 0; i < unitBuild.Count(); i++)
            {
                unitBuild[i].TextBoxLocation = new Vector2(buttonDrawPos.X, buttonDrawPos.Y + (i * (buttonSize.Y + 10)));
                unitBuild[i].SetStringPos();
                tabs.Add(unitBuild[i], 0);
                unitBuild[i].TextBoxRectangleSet();
            }

            //Base Buttons Here
            baseBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsBase[0], "btn0Trench", true));
            baseBuild[0].StringText = "Build Trench";
            baseBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsBase[1], "btn0Concrete", true));
            baseBuild[1].StringText = "Build Concrete";
            baseBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsBase[2], "btn0Destroy", true));
            baseBuild[2].StringText = "Destroy Building";


            for (int i = 0; i < baseBuild.Count(); i++)
            {
                baseBuild[i].TextBoxLocation = new Vector2(buttonDrawPos.X, buttonDrawPos.Y + (i * (buttonSize.Y + 10)));
                baseBuild[i].SetStringPos();
                tabs.Add(baseBuild[i], 1);
                baseBuild[i].TextBoxRectangleSet();
            }

            //Misc Buttons Here

            
            miscBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsMisc[0], "btn1NextWave", true));
            miscBuild[0].StringText = "Next Wave";

            for (int i = 0; i < miscBuild.Count(); i++)
            {
                miscBuild[i].TextBoxLocation = new Vector2(buttonDrawPos.X, buttonDrawPos.Y + (i * (buttonSize.Y + 10)));
                miscBuild[i].SetStringPos();
                tabs.Add(miscBuild[i], 2);
                miscBuild[i].TextBoxRectangleSet();
            }
        }
    }

    public class UiTopGameScreen : Ui
    {
        public UiStatusBars healthBar;

        public List<UiTextString> waveStats;
        public List<UiTextString> currencyStats;
        public List<UiTextString> timers;
        public List<UiTextString> popUpText;

        public UiTopGameScreen(GraphicsDevice graphicsDevice) : base(GameManager.WIDTH, GameManager.HEIGHT)
        {
            healthBar = new UiStatusBars(100, new Vector2(100, 25), new Vector2(300, 10), Art.HpBar[0], Art.HpBar[1]);
            waveStats = new List<UiTextString>();
            currencyStats = new List<UiTextString>();
            timers = new List<UiTextString>();
            popUpText = new List<UiTextString>();

            Add(ref waveStats);
            Add(ref currencyStats);
            Add(ref timers);
            Add(ref popUpText);
            Add(ref healthBar);

            CreateUi(graphicsDevice);
        }

        public void CreateUi(GraphicsDevice graphicsDevice)
        {
            waveStats.Add(new UiTextString(Art.DebugFont, "Wave: " + WaveManager.WaveNumber, new Vector2(100, 0), Color.Black));
            waveStats.Add(new UiTextString(Art.DebugFont, "Enemies: " + WaveManager.WaveEnemiesUsed + "/" + WaveManager.WaveEnemyAmount, new Vector2(200, 0), Color.Black));
            currencyStats.Add(new UiTextString(Art.DebugFont, "Manpower: " + GameManager.Manpower, new Vector2(300, 40), Color.Black));
            currencyStats.Add(new UiTextString(Art.DebugFont, "Resources: " + GameManager.Resources, new Vector2(300, 60), Color.Black));
            timers.Add(new UiTextString(Art.DebugFont, "Next Wave in: ", new Vector2(420, 10), Color.Black));

            
        }

        public void Update()
        {
            healthBar.Update(50);

            waveStats[0].StringText = "Wave: " + WaveManager.WaveNumber;
            waveStats[1].StringText = "Enemies: " + WaveManager.WaveEnemiesUsed + "/" + WaveManager.WaveEnemyAmount;

            currencyStats[0].StringText = "Manpower: " + GameManager.Manpower;
            currencyStats[1].StringText = "Resources: " + GameManager.Resources;

            timers[0].StringText = "Next Wave in: " + WaveManager.WaveStartTimer;
            
        }
    }

    public static class QuestionPopUpManager
    {
        static List<QuestionPopUp> QuestionPopUps = new List<QuestionPopUp>();

        public static void Add(QuestionPopUp Question)
        {
            QuestionPopUps.Add(Question);
        }

       /* public static RemoveQuestion()
        {
            QuestionPopUps.Remove(
        }*/

        public static void Update()
        {
            foreach (QuestionPopUp question in QuestionPopUps)
            {
                question.Update();
            }
        
        }

        public static void Draw(SpriteBatch sb)
        {
            foreach(QuestionPopUp question in QuestionPopUps)
            {
                question.Draw(sb);
            }
        }  
    }

    public static class QuestionStrings
    {
        public static string WWIIWinner = "Who won World War 2?";
        public static string SovietLeader = "Who was the leader of the Soviet Union during World War II?";
        public static string GermanLeader = "Who was the leader of Germany during World War II?";
        public static string ItalianLeader = "who was the leader of Italy during World War II?";
        public static string WWIIStartDate = "When did ww2 begin?";
        public static string GermanPolandInvasion = "Which country did germany invade to start ww2?";
        public static string NaziLightningWar = "What were the Nazi 'lightning war' tactics which\nconquered Denmark, Norway, Holland, Belgium and France in April-June 1940 called?";
        public static string BattleOfBritain = "What was the Battle of Britain?";
        public static string AmericanBomb = "What kind of bomb did the Americans drop on Hiroshima?";

        public static string[] WWIIWinnerAnswers = { "Britain", "Germany", "Allied Forces" };
        public static string[] SovietLeaderAnswers = { "Stalin", "Trotski", "Lenin" };
        public static string[] GermanLeaderAnswers = { "Churchill", "Mussolini", "Hitler" };
        public static string[] ItalianLeaderAnswers = { "Hirohito", "Mussolini", "Eisenhower" };
        public static string[] WWIIStartDateAnswers = { "1939", "1914", "1941" };
        public static string[] GermanPolandInvasionAnswers = { "Austria", "Russia", "Poland" };
        public static string[] NaziLightningWarAnswers = { "The Blitz", "Blitzkrieg", "Operation Barbarossa" };
        public static string[] BattleOfBritainAnswers = { "The Royal Air Force defeated the Luftwaffe.", "The Luftwaffe bombed London and other British cities.", "The British withdrew from France by sea." };
        public static string[] AmericanBombAnswers = { "A V-1 rocket", "Blitzkrieg", "An atomic bomb" };

        public static string WWIIWinnerCorrect = "Ans3";
        public static string SovietLeaderCorrect = "Ans1";
        public static string GermanLeaderCorrect = "Ans3";
        public static string ItalianLeaderCorrect = "Ans2";
        public static string WWIIStartDateCorrect = "Ans1";
        public static string GermanPolandInvasionCorrect = "Ans3";
        public static string NaziLightningWarCorrect = "Ans2";
        public static string BattleOfBritainCorrect = "Ans1";
        public static string AmericanBombCorrect = "Ans3";

    }


    public class QuestionPopUp
    {
        enum QuestionState
        { 
            Asking,
            Correct,
            Wrong,
            Done
        }

        QuestionState State;

        UiTextBox QuestionBox, CorrectBox, WrongBox;
        List<UiButton> Answers = new List<UiButton>();
        string correctAnsID;

        public QuestionPopUp(string Question, string Answer1, string Answer2, string Answer3, string correctAnswerID)
        {
            correctAnsID = correctAnswerID;

            QuestionBox = new UiTextBox(Art.DebugFont, Question, new Vector2(250, 100), Color.White, Art.TextBoxBackGround, false);
            CorrectBox = new UiTextBox(Art.DebugFont, "Correct! A soldier joins your cause!\n\nPress enter to continue", new Vector2(250, 100), Color.White, Art.TextBoxBackGround, false);
            WrongBox = new UiTextBox(Art.DebugFont, "Wrong! Better luck next time!\n\nPress enter to continue", new Vector2(250, 100), Color.White, Art.TextBoxBackGround, false);
            Answers.Add(new UiButton(Art.DebugFont, new Vector2(400, 400), new Vector2(200, 100), Art.TextBoxBackGround, "Ans1", true));
            Answers.Add(new UiButton(Art.DebugFont, new Vector2(400, 500), new Vector2(200, 100), Art.TextBoxBackGround, "Ans2", true));
            Answers.Add(new UiButton(Art.DebugFont, new Vector2(400, 600), new Vector2(200, 100), Art.TextBoxBackGround, "Ans3", true));

            QuestionBox.TextBoxSize = new Vector2(500, 200);
            QuestionBox.StringScale = 2f;
            QuestionBox.StringOffset = new Vector2(10, 0);

            CorrectBox.TextBoxSize = new Vector2(500, 200);
            CorrectBox.StringScale = 2f;
            CorrectBox.StringOffset = new Vector2(10, 0);

            WrongBox.TextBoxSize = new Vector2(500, 200);
            WrongBox.StringScale = 2f;
            WrongBox.StringOffset = new Vector2(10, 0);

            Answers[0].StringText = Answer1;
            Answers[1].StringText = Answer2;
            Answers[2].StringText = Answer3;

            foreach (UiButton button in Answers)
            {
                UiButtonMessenger.RegisterButton(button);
                button.TextBoxColour = Color.Black;
                button.StringScale = 2f;
                button.StringOffset = new Vector2(10, 0);
                button.TextBoxRectangleSet();

                button.SetButtonState = UiButton.UiButtonStates.Button_Up;
            }

            State = QuestionState.Asking;
        }

        public void Update()
        {
            foreach (UiButton button in Answers)
            {
                if (button.IsButtonDown())
                {
                    if (button.GetButtonID == correctAnsID)
                    {
                        State = QuestionState.Correct;
                    }

                    else State = QuestionState.Wrong;
                }
            }

            if (State == QuestionState.Correct || State == QuestionState.Wrong)
            { 
                if(Input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter))
                {
                    State = QuestionState.Done;
                }
            }

        }

        public void Draw(SpriteBatch sb)
        {

            if (State == QuestionState.Asking)
            {
                QuestionBox.Draw(sb);

                foreach (UiButton button in Answers)
                {
                    button.Draw(sb);
                }
            }

            else if (State == QuestionState.Correct)
                CorrectBox.Draw(sb);

            else if (State == QuestionState.Wrong)
                WrongBox.Draw(sb);
        }
    }

    /*public class StartScreen : Ui
    { }

    public class GameOverScreen : Ui
    { }*/

}
