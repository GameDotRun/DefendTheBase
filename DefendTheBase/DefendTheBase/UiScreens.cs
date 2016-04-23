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
            tabs = new UiTabs(graphicsDevice, Art.DebugFont, 3, tabDrawPos, new string[3] { "Towers", "Base", "Misc" }, Art.tabTestTexture, Art.ButtonEffectTexture, new Vector2(83, 40));

            unitBuild = new List<UiButton>();
            baseBuild = new List<UiButton>();
            miscBuild = new List<UiButton>();

            //add grouped elements to uicontrollers, do not add anything that is part of a tab. Just add the tab
            Add(ref tabs);

            CreateUi(graphicsDevice);
        }

        public void Update()
        {
            if (GameManager.UnlockedTowers.HasFlag(GameManager.Unlocks.RocketTower))
            {
                if (unitBuild.Count < 2)
                {
                    unitBuild.Add(new UiButton(Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsTower[1], Art.ButtonEffectTexture, "btn0TowerRocket", true));
                    buttonSingleInit(unitBuild[unitBuild.Count - 1], unitBuild.Count - 1, 0);
                }
            }

            if (GameManager.UnlockedTowers.HasFlag(GameManager.Unlocks.SamTower))
            {
                if (unitBuild.Count < 3)
                {
                    unitBuild.Add(new UiButton(Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsTower[2], Art.ButtonEffectTexture, "btn0TowerSAM", true));
                    buttonSingleInit(unitBuild[unitBuild.Count - 1], unitBuild.Count - 1, 0);
                }
            }

            if (GameManager.UnlockedTowers.HasFlag(GameManager.Unlocks.TeslaTower))
            {
                if (unitBuild.Count < 4)
                {
                    unitBuild.Add(new UiButton(Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsTower[3], Art.ButtonEffectTexture, "btn0TowerTesla", true));
                    buttonSingleInit(unitBuild[unitBuild.Count - 1], unitBuild.Count - 1, 0);
                }
            }

            tabs.Update();
        }

        public void buttonSingleInit(UiButton Button, int buttonNumber, int tabPage)
        {
            Button.TextBoxLocation = new Vector2(buttonDrawPos.X, buttonDrawPos.Y + (buttonNumber * (buttonSize.Y + 10)));
            Button.SetStringPos();
            tabs.Add(Button, tabPage);
            Button.TextBoxRectangleSet();
        
        }

        public void buttonsInit(ref List<UiButton> buttonList, int tabPage)
        {
            for (int i = 0; i < buttonList.Count(); i++)
            {
                buttonList[i].TextBoxLocation = new Vector2(buttonDrawPos.X, buttonDrawPos.Y + (i * (buttonSize.Y + 10)));
                buttonList[i].SetStringPos();
                tabs.Add(buttonList[i], tabPage);
                buttonList[i].TextBoxRectangleSet();
            }
        
        }

        public void CreateUi(GraphicsDevice graphicsDevice)
        {
          
            //its of UTMOST IMPORTANCE that each button has a unique id

            //Units buttons Here
            unitBuild.Add(new UiButton(Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsTower[0], Art.ButtonEffectTexture, "btn0TowerGun", true));
            //unitBuild[0].StringText = "Gun Tower";

            buttonsInit(ref unitBuild, 0);

            //Base Buttons Here
            baseBuild.Add(new UiButton(Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsBase[0], Art.ButtonEffectTexture, "btn0Trench", true));
            //baseBuild[0].StringText = "Build Trench";
            baseBuild.Add(new UiButton(Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsBase[1], Art.ButtonEffectTexture, "btn0Concrete", true));
            //baseBuild[1].StringText = "Build Concrete";
            baseBuild.Add(new UiButton(Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsBase[2], Art.ButtonEffectTexture, "btn0Destroy", true));
            //baseBuild[2].StringText = "Destroy Building";

            buttonsInit(ref baseBuild, 1);


            //Misc Buttons Here


            miscBuild.Add(new UiButton(Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsMisc[0], Art.ButtonEffectTexture, "btn1NextWave", true));
            //miscBuild[0].StringText = "Next Wave";

            buttonsInit(ref miscBuild, 2);
        }
    }

    public class UiTopGameScreen : Ui
    {
        public UiStatusBars healthBar;

        public List<UiTextBox> waveStats;
        public List<UiTextBox> currencyStats;
        public List<UiTextBox> timers;
        public List<UiTextString> popUpText;

        public UiTopGameScreen(GraphicsDevice graphicsDevice) : base(GameManager.WIDTH, GameManager.HEIGHT)
        {
            healthBar = new UiStatusBars(100, new Vector2(100, 25), new Vector2(300, 10), Art.HpBar[0], Art.HpBar[1]);
            waveStats = new List<UiTextBox>();
            currencyStats = new List<UiTextBox>();
            timers = new List<UiTextBox>();
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
            waveStats.Add(new UiTextBox(Art.UiFont, "Wave: ", new Vector2(10, 10), Color.White, Art.TextBoxBackGround, true));
            waveStats.Add(new UiTextBox(Art.UiFont, "Enemies: " + WaveManager.WaveEnemiesUsed + "/" + WaveManager.WaveEnemyAmount, new Vector2(110, 10), Color.White, Art.TextBoxBackGround, true ));
            currencyStats.Add(new UiTextBox(Art.UiFont, "Manpower: " + GameManager.Manpower, new Vector2(10, 50), Color.White, Art.TextBoxBackGround, true));
            currencyStats.Add(new UiTextBox(Art.UiFont, "Resources: " + GameManager.Resources, new Vector2(10, 90), Color.White, Art.TextBoxBackGround, true));
            timers.Add(new UiTextBox(Art.UiFont, "Next Wave in: ", new Vector2(1000, GameManager.BORDERTOP - 40), Color.White, Art.TextBoxBackGround, true));

            
        }

        public void Update()
        {
            healthBar.Update(50);

            waveStats[0].StringText = "Wave: " + WaveManager.WaveNumber;
            waveStats[1].StringText = "Enemies: " + WaveManager.WaveEnemiesUsed + "/" + WaveManager.WaveEnemyAmount;

            currencyStats[0].StringText = "Manpower: " + GameManager.Manpower;
            currencyStats[1].StringText = "Resources: " + GameManager.Resources;

            timers[0].StringText = "Next Wave in: " + WaveManager.WaveStartTimer.TotalSeconds;
            
        }
    }

    public static class PopUpNotificationManager
    {
        public static string NoResources = "Not enough resources";
        public static string NoManpower = "Not enough manpower";
        public static string CantPlaceTrench = "You can't block off squares";
        public static string NeedConcrete = "Must be placed on concrete";
        public static string NextToTrench = "Must be placed next to trench";

        public static List<PopUpNotificationText> PopUps = new List<PopUpNotificationText>();

        public static void Add(PopUpNotificationText popup)
        {
            PopUps.Add(popup);
        }

        public static void Remove(PopUpNotificationText popup)
        {
            PopUps.Remove(popup);
        }

        public static void Update(GameTime gt)
        {
            if (PopUps.Count > 2)
            {
                PopUps.RemoveAt(0);
            }

            foreach (PopUpNotificationText popup in PopUps)
            {
                popup.Update(gt);

                if (popup.timer.TimeReached())
                {
                    Remove(popup);
                    break;
                }

            }
        }

        public static void Draw(SpriteBatch sb)
        {
            foreach (PopUpNotificationText popup in PopUps)
            {
                popup.Draw(sb);
            }
        }

    }

    public class PopUpNotificationText : UiTextString
    {
        public UiTimer timer = new UiTimer(750f);

        public PopUpNotificationText(string stringText, Vector2 StringPos, Color color)
            : base(Art.DebugFont, stringText, StringPos, color)
        {
            StringScale = 2;
        }

        public void Update(GameTime gt)
        {
            if (!timer.GetActive)
                timer.ActivateTimer();

            StringScale += 0.01f;
            StringPosition += new Vector2(0, -1);

            timer.TimerUpdate(gt);
        }

        public void Draw(SpriteBatch sb)
        {
            DrawString(sb, Vector2.Zero);
        }
    }

    public static class PopUpTextManager
    {
        public static string Critical = "Critical";
        public static string Resist = "Resist";
        public static string Effective = "Effective";
        public static string NoEffect = "No Effect";

        public static List<PopUpText> PopUps = new List<PopUpText>();

        public static void Add(PopUpText popup)
        {
            PopUps.Add(popup);
        }

        public static void Remove(PopUpText popup)
        {
            PopUps.Remove(popup);
        }

        public static void Update(GameTime gt)
        {
            if (PopUps.Count > 10)
            {
                PopUps.RemoveAt(0);
            }

            foreach (PopUpText popup in PopUps)
            {
                popup.Update(gt);

                if (popup.timer.TimeReached())
                {
                    Remove(popup);
                    break;
                }

            }
        }

        public static void Draw(SpriteBatch sb)
        {
            foreach (PopUpText popup in PopUps)
            {
                popup.Draw(sb);
            }
        }  
    
    }

    public class PopUpText : UiTextString
    {
        public UiTimer timer = new UiTimer(750f);

        public PopUpText(string stringText, Vector2 StringPos, Color color)
            : base(Art.DebugFont, stringText, StringPos, color)
        {  }

        public void Update(GameTime gt)
        {
            if (!timer.GetActive)
                timer.ActivateTimer();

            StringScale += 0.01f;
            StringPosition += new Vector2(0, -1);

            timer.TimerUpdate(gt);
        }

        public void Draw(SpriteBatch sb)
        {
            DrawString(sb, Vector2.Zero);
        }
    }

    //4 things needed to make a question -
    //1: make questions and answers below, along with the correct answer. use the same names as enums for the question
    //2: add enums to question enums... should prob move that too here
    //3: in init add the new enums to the list
    //4: add it to the case in questiongenerator, located in waveManager.

    public static class QuestionPopUpManager
    {
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

        public static string WWIIWinner = "Who won World War 2?";
        public static string SovietLeader = "Who was the leader of the Soviet Union during\nWorld War II?";
        public static string GermanLeader = "Who was the leader of Germany during\nWorld War II?";
        public static string ItalianLeader = "who was the leader of Italy during World War II?";
        public static string WWIIStartDate = "When did ww2 begin?";
        public static string GermanPolandInvasion = "Which country did germany invade to start ww2?";
        public static string NaziLightningWar = "What were the Nazi 'lightning war' tactics which\nconquered Denmark, Norway, Holland, Belgium\nand France in April-June 1940 called?";
        public static string BattleOfBritain = "What was the Battle of Britain?";
        public static string AmericanBomb = "What kind of bomb did the Americans drop on\nHiroshima?";

        public static string[] WWIIWinnerAnswers = { "Britain", "Germany", "Allied Forces" };
        public static string[] SovietLeaderAnswers = { "Stalin", "Trotski", "Lenin" };
        public static string[] GermanLeaderAnswers = { "Churchill", "Mussolini", "Hitler" };
        public static string[] ItalianLeaderAnswers = { "Hirohito", "Mussolini", "Eisenhower" };
        public static string[] WWIIStartDateAnswers = { "1939", "1914", "1941" };
        public static string[] GermanPolandInvasionAnswers = { "Austria", "Russia", "Poland" };
        public static string[] NaziLightningWarAnswers = { "The Blitz", "Blitzkrieg", "Operation Barbarossa" };
        public static string[] BattleOfBritainAnswers = { "The Royal Air Force\ndefeated the Luftwaffe.", "The Luftwaffe bombed London\nand other British cities.", "The British withdrew\nfrom France by sea." };
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

        public static List<Questions> questionsList = new List<Questions>();
        static List<QuestionPopUp> QuestionPopUps = new List<QuestionPopUp>();

        public static bool QuestionUp = false;

        public static void Init()
        {
            questionsList.Add(Questions.AmericanBomb);
            questionsList.Add(Questions.BattleOfBritain);
            questionsList.Add(Questions.GermanLeader);
            questionsList.Add(Questions.GermanPolandInvasion);
            questionsList.Add(Questions.ItalianLeader);
            questionsList.Add(Questions.NaziLightningWar);
            questionsList.Add(Questions.SovietLeader);
            questionsList.Add(Questions.WWIIStartDate);
            questionsList.Add(Questions.WWIIWinner);
        
        }

        public static void Add(QuestionPopUp Question)
        {
            QuestionPopUps.Add(Question);
            QuestionUp = true;
        }

        public static void RemoveQuestion(QuestionPopUp question)
        {
            QuestionPopUps.Remove(question);
            QuestionUp = false;
            WaveManager.QuestionsCorrectCheck();
        }

        public static void Update()
        {
            foreach (QuestionPopUp question in QuestionPopUps)
            {
                question.Update();

                if (question.State == QuestionPopUp.QuestionState.Done)
                {
                    RemoveQuestion(question);

                    break;
                }
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

    public class QuestionPopUp
    {
        public enum QuestionState
        { 
            Asking,
            Correct,
            Wrong,
            Done
        }

        public QuestionState State;

        UiTextBox QuestionBox, CorrectBox, WrongBox;
        List<UiButton> Answers = new List<UiButton>();
        string correctAnsID;

        public QuestionPopUp(string Question, string Answer1, string Answer2, string Answer3, string correctAnswerID)
        {
            correctAnsID = correctAnswerID;

            QuestionBox = new UiTextBox(Art.UiFont, Question, new Vector2(250, 150), Color.White, Art.TextBoxBackGround, false);
            CorrectBox = new UiTextBox(Art.UiFont, "Correct! A soldier joins your cause!\n\nPress enter to continue", new Vector2(250, 150), Color.White, Art.TextBoxBackGround, false);
            WrongBox = new UiTextBox(Art.UiFont, "Wrong! Better luck next time!\n\nPress enter to continue", new Vector2(250, 150), Color.White, Art.TextBoxBackGround, false);
            Answers.Add(new UiButton(Art.UiFont, new Vector2(350, 400), new Vector2(300, 100), Art.TextBoxBackGround, Art.ButtonEffectTexture, "Ans1", true));
            Answers.Add(new UiButton(Art.UiFont, new Vector2(350, 520), new Vector2(300, 100), Art.TextBoxBackGround, Art.ButtonEffectTexture, "Ans2", true));
            Answers.Add(new UiButton(Art.UiFont, new Vector2(350, 640), new Vector2(300, 100), Art.TextBoxBackGround, Art.ButtonEffectTexture, "Ans3", true));

            QuestionBox.TextBoxSize = new Vector2(500, 200);
            QuestionBox.TextBoxColour = Color.Black;
            //QuestionBox.StringScale = 2f;
            QuestionBox.StringOffset = new Vector2(10, 0);

            CorrectBox.TextBoxSize = new Vector2(500, 200);
            CorrectBox.TextBoxColour = Color.Black;
            //CorrectBox.StringScale = 2f;
            CorrectBox.StringOffset = new Vector2(10, 0);

            WrongBox.TextBoxSize = new Vector2(500, 200);
            WrongBox.TextBoxColour = Color.Black;
            //WrongBox.StringScale = 2f;
            WrongBox.StringOffset = new Vector2(10, 0);

            Answers[0].StringText = Answer1;
            Answers[1].StringText = Answer2;
            Answers[2].StringText = Answer3;

            foreach (UiButton button in Answers)
            {
                UiButtonMessenger.RegisterButton(button);
                button.TextBoxColour = Color.Black;
                //button.StringScale = 2f;
                button.StringOffset = new Vector2(10, 0);
                button.TextBoxRectangleSet();

                button.SetButtonState = UiButton.UiButtonStates.Button_Up;
            }

            State = QuestionState.Asking;
        }

        public void Update()
        {
            if (State == QuestionState.Asking)
            {
                foreach (UiButton button in Answers)
                {
                    if (button.IsButtonDown())
                    {
                        if (button.GetButtonID == correctAnsID)
                        {
                            State = QuestionState.Correct;
                            WaveManager.questionsAnsweredCorrect++;
                            TroopManager.SpawnTroop();

                        }

                        else State = QuestionState.Wrong;

                        foreach (UiButton buttons in Answers)
                        {
                            UiButtonMessenger.RemoveButton(buttons.GetButtonID);
                        }
                    }
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
                    button.DrawButton(sb);
                }
            }

            else if (State == QuestionState.Correct)
                CorrectBox.Draw(sb);

            else if (State == QuestionState.Wrong)
                WrongBox.Draw(sb);
        }
    }

    public class StartScreen
    {
        Vector2 StartScreenModifier = new Vector2(0, -5);
        Texture2D backgroundTex = Art.StartMenuBackground;

        List<UiButton> StartMenuButtons = new List<UiButton>();

        public StartScreen()
        { 
            for(int i = 0; i < 3; i++)
            {
                StartMenuButtons.Add(new UiButton(Art.UiFont, new Vector2(525, 300 + i * 150), new Vector2(200, 100), Art.TextBoxBackGround, Art.ButtonEffectTexture, "str" + i, true));
                UiButtonMessenger.RegisterButton(StartMenuButtons[i]);
            }

            StartMenuButtons[0].StringText = "Start";
            StartMenuButtons[0].StringOffset = new Vector2(70, 30);
            StartMenuButtons[1].StringText = "Tutorial";
            StartMenuButtons[1].StringOffset = new Vector2(55, 30);
            StartMenuButtons[2].StringText = "Exit";
            StartMenuButtons[2].StringOffset = new Vector2(75, 30);
        
        }

        public void Update()
        {
            if (StartMenuButtons[0].IsButtonDown())
            {
                DisableScreen();
                GameManager.GameState = GameManager.GameStates.PlayScreen;
            }

            else if (StartMenuButtons[1].IsButtonDown())
            { 
                //load tutorial vid and whatever else
            
            }

            else if (StartMenuButtons[2].IsButtonDown())
                GameRoot.exit = true;
        
        }

        public void Draw(SpriteBatch sb)
        { 
            sb.Draw(backgroundTex, Vector2.Zero, Color.White);

            foreach (UiButton button in StartMenuButtons)
                button.DrawButton(sb);
        }

        public void EnableScreen()
        {
            foreach (UiButton button in StartMenuButtons)
                UiButtonMessenger.RegisterButton(button);
        
        }

        public void DisableScreen()
        {
            foreach (UiButton button in StartMenuButtons)
                UiButtonMessenger.RemoveButton(button.GetButtonID);
        }

    }

    /*public class GameOverScreen : Ui
    { }*/

}
