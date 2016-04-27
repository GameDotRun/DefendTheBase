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

            if (GameManager.UnlockedTowers.HasFlag(GameManager.Unlocks.Upgrade))
            {
                if (unitBuild.Count < 5)
                {
                    unitBuild.Add(new UiButton(Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsTower[4], Art.ButtonEffectTexture, "btn0Upgrade", true));
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
            waveStats.Add(new UiTextBox(Art.UiFont, "Kills: " + WaveManager.WaveEnemiesUsed , new Vector2(110, 10), Color.White, Art.TextBoxBackGround, true ));
            currencyStats.Add(new UiTextBox(Art.UiFont, "Manpower: " + GameManager.Manpower, new Vector2(10, 50), Color.White, Art.TextBoxBackGround, true));
            currencyStats.Add(new UiTextBox(Art.UiFont, "Resources: " + GameManager.Resources, new Vector2(10, 90), Color.White, Art.TextBoxBackGround, true));
            timers.Add(new UiTextBox(Art.UiFont, "Next Wave in: ", new Vector2(1000, GameManager.BORDERTOP - 40), Color.White, Art.TextBoxBackGround, true));

            
        }

        public void Update()
        {
            healthBar.Update(50);

            waveStats[0].StringText = "Wave: " + WaveManager.WaveNumber;
            waveStats[1].StringText = "Kills: " + WaveManager.WaveEnemiesUsed ;

            currencyStats[0].StringText = "Manpower: " + GameManager.Manpower;
            currencyStats[1].StringText = "Resources: " + GameManager.Resources;

            timers[0].StringText = "Next Wave in: " + WaveManager.WaveStartTimer.TotalSeconds.ToString("N0");
            
        }
    }

    public static class MessageBoxManager
    {
        public static string RocketTowerUnlock = "New Tower Unlocked!\nRocket Tower, use this to make\nshort work of vehicles";
        public static string SAMTowerUnlock = "New Tower Unlocked!\nSAM Tower, use this to make\nshort work of helicopters";
        public static string TeslaTowerUnlock = "New Tower Unlocked!\nTesla Tower, has no weakness\nbut no strength either";
        public static string UpgradeTowerUnlock = "New Tower Unlocked!\nUpgrade Tower, use this to upgrade\nyour Towers";
        public static string Introduction = "";

        public static List<MessageBox> MessageBox = new List<MessageBox>();

        public static void Add(MessageBox messagebox)
        {
            MessageBox.Add(messagebox);
        }

        public static void Remove(MessageBox messagebox)
        {
            MessageBox.Remove(messagebox);
        }

        public static void Update(GameTime gt)
        {
            if (MessageBox.Count > 2)
            {
                MessageBox.RemoveAt(0);
            }

            foreach (MessageBox message in MessageBox)
            {
                message.Update(gt);

                if (message.timer.TimeReached())
                {
                    Remove(message);
                    break;
                }

            }
        }

        public static void Draw(SpriteBatch sb)
        {
            foreach (MessageBox message in MessageBox)
            {
                message.Draw(sb);
            }
        }
    
    }

    public class MessageBox : UiTextBox
    {
        public UiTimer timer = new UiTimer(5000f);

        public MessageBox(string stringText)
            : base(Art.UiFont, stringText, new Vector2(250, 150), Color.White, Art.TextBoxBackGround, false)
        {
            TextBoxSize = new Vector2(500, 200);
            TextBoxColour = Color.Black;
            StringOffset = new Vector2(10, 0);
        }

        public void Update(GameTime gt)
        {
            if (!timer.GetActive)
                timer.ActivateTimer();

            timer.TimerUpdate(gt);
        }

        public void DrawBox(SpriteBatch sb)
        {
            Draw(sb);
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
        public static string Misfire = "Misfire";

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
            if (PopUps.Count > 100000)
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

    public static class QuestionPopUpManager
    {
        public static List<string[]> QuestionsArray = new List<string[]>
        {
            new string[] { "Who won World War 2?", "Britain", "Germany", "Allied Forces", "Ans3" },
            new string[] { "Who was the leader of the Soviet Union during World War II?", "Stalin", "Trotski", "Lenin", "Ans1" },
            new string[] { "Who was the leader of Germany during World War II?", "Churchill", "Mussolini", "Hitler", "Ans3" },
            new string[] { "Who was the leader of Italy during World War II?", "Hirohito", "Mussolini", "Eisenhower", "Ans2" },
            new string[] { "When did WW2 begin?", "1939", "1914", "1941", "Ans1" },
            new string[] { "Which country did germany invade to start WW2?", "Austria", "Russia", "Poland", "Ans3" },
            new string[] { "What were the Nazi 'lightning war' tactics which conquered Denmark, Norway, Holland, Belgium and France in April-June 1940 called?", "The Blitz", "Blitzkrieg", "Operation Barbarossa", "Ans2" },
            new string[] { "What was the Battle of Britain?", "The Royal Air Force defeated the Luftwaffe.", "The Luftwaffe bombed London and other British cities.", "The British withdrew from France by sea.", "Ans1" },
            new string[] { "What kind of bomb did the Americans drop on Hiroshima?", "A V-1 rocket", "Blitzkrieg", "An atomic bomb", "Ans3" },
            new string[] { "What was Hitlers wife called?", "Eva Braun", "Isla Braun", "Emma Braun", "Ans1" },
            new string[] { "Where was Hitler Born?", "Poland", "Austria", "Germany", "Ans2" },
            new string[] { "What type of car did Hitler Promise to build?", "Beatle", "Volkswagen ", "Ford", "Ans2" },
            new string[] { "What country did Hitler want to send the jews to?", "Madagascar ", "America", "Poland", "Ans1" },
            new string[] { "What was Kristallnacht?", "Day of dreams", "Night of crystals", "Night of  broken glass", "Ans3" },
            new string[] { "What did hitler want to be?", "Artist ", "Politician", "Soldier", "Ans1" },
            new string[] { "Roughly how many jews were killed in concentration camps?", "6 mil", "1 mil ", "10 mil", "Ans1" },
            new string[] { "Who were the 2 main sides of ww2? Allies and?", "Axis ", "Bad guys", "enemies", "Ans1" },
            new string[] { "What did hitler use to persuade people to vote for him?", "Forced at gun point", "Bribes", "Propaganda", "Ans3" },
            new string[] { "What country was Auschwitz in?", "Italy", "Germany", "Poland", "Ans3" }
        };
        
        static List<QuestionPopUp> QuestionPopUps = new List<QuestionPopUp>();

        public static bool QuestionUp = false;

        public static void Init()
        {
            

            
        
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

        string[] QuestionDetails = new string[5];

        public QuestionPopUp(string[] questionDetails)
        {
            QuestionDetails = questionDetails;
            correctAnsID = questionDetails[4];

            QuestionBox = new UiTextBox(Art.UiFont, questionDetails[0], new Vector2(250, 150), Color.White, Art.TextBoxBackGround, false);
            CorrectBox = new UiTextBox(Art.UiFont, "Correct! A soldier joins your cause!\n\nClick to continue", new Vector2(250, 150), Color.White, Art.TextBoxBackGround, false);
            WrongBox = new UiTextBox(Art.UiFont, "Wrong! Better luck next time!\n\nClick to continue", new Vector2(250, 150), Color.White, Art.TextBoxBackGround, false);
            Answers.Add(new UiButton(Art.UiFont, new Vector2(350, 400), new Vector2(300, 100), Art.TextBoxBackGround, Art.ButtonEffectTexture, "Ans1", true));
            Answers.Add(new UiButton(Art.UiFont, new Vector2(350, 520), new Vector2(300, 100), Art.TextBoxBackGround, Art.ButtonEffectTexture, "Ans2", true));
            Answers.Add(new UiButton(Art.UiFont, new Vector2(350, 640), new Vector2(300, 100), Art.TextBoxBackGround, Art.ButtonEffectTexture, "Ans3", true));

            QuestionBox.TextBoxSize = new Vector2(500, 200);
            QuestionBox.TextBoxColour = Color.Black;
            //QuestionBox.StringScale = 2f;
            QuestionBox.StringOffset = new Vector2(10, 0);
            QuestionBox.LineWrapper();

            CorrectBox.TextBoxSize = new Vector2(500, 200);
            CorrectBox.TextBoxColour = Color.Black;
            //CorrectBox.StringScale = 2f;
            CorrectBox.StringOffset = new Vector2(10, 0);
            CorrectBox.LineWrapper();

            WrongBox.TextBoxSize = new Vector2(500, 200);
            WrongBox.TextBoxColour = Color.Black;
            //WrongBox.StringScale = 2f;
            WrongBox.StringOffset = new Vector2(10, 0);
            WrongBox.LineWrapper();


            Answers[0].StringText = questionDetails[1];
            Answers[1].StringText = questionDetails[2];
            Answers[2].StringText = questionDetails[3];

            foreach (UiButton button in Answers)
            {
                UiButtonMessenger.RegisterButton(button);
                button.TextBoxColour = Color.Black;
                //button.StringScale = 2f;
                button.StringOffset = new Vector2(10, 0);
                button.TextBoxRectangleSet();
                button.LineWrapper();

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
                            QuestionPopUpManager.QuestionsArray.Remove(QuestionDetails);

                        }

                        else State = QuestionState.Wrong;

                        foreach (UiButton buttons in Answers)
                        {
                            UiButtonMessenger.RemoveButton(buttons.GetButtonID);
                        }
                    }
                }
            }

            else if (State == QuestionState.Correct || State == QuestionState.Wrong)
            { 
                if(Input.WasLMBClicked)
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

        UiTimer FadeOutTimer = new UiTimer(2000f);

        float fade = 1f;

        public bool fadeout = false;

        public StartScreen()
        { 
            for(int i = 0; i < 4; i++)
            {
                StartMenuButtons.Add(new UiButton(Art.UiFont, new Vector2(525, 250 + i * 150), new Vector2(200, 100), Art.TextBoxBackGround, Art.ButtonEffectTexture, "str" + i, true));
                UiButtonMessenger.RegisterButton(StartMenuButtons[i]);
            }

            StartMenuButtons[0].StringText = "Start";
            StartMenuButtons[0].StringOffset = new Vector2(70, 30);
            StartMenuButtons[1].StringText = "Info";
            StartMenuButtons[1].StringOffset = new Vector2(70, 30);
            StartMenuButtons[2].StringText = "Tutorial";
            StartMenuButtons[2].StringOffset = new Vector2(55, 30);
            StartMenuButtons[3].StringText = "Exit";
            StartMenuButtons[3].StringOffset = new Vector2(75, 30);
        
        }

        public void Update(GameTime gt)
        {
            if (StartMenuButtons[0].IsButtonDown())
            {
                DisableScreen();
                GameManager.GameState = GameManager.GameStates.PlayScreen;

                FadeOutTimer.ActivateTimer();
                fadeout = true;
            }

            else if (StartMenuButtons[1].IsButtonDown())
            {
                //load info box and whatever else

            }

            else if (StartMenuButtons[2].IsButtonDown())
            { 
                //load tutorial vid and whatever else
            
            }

            else if (StartMenuButtons[3].IsButtonDown())
                GameRoot.exit = true;

            if (FadeOutTimer.GetActive)
            {
                FadeOutTimer.TimerUpdate(gt);
                fade -= 0.01f;
            }

            if (FadeOutTimer.TimeReached())
                fadeout = false;
        }

        public void Draw(SpriteBatch sb)
        { 
            sb.Draw(backgroundTex, Vector2.Zero, Color.White * fade);

            if (GameManager.GameState == GameManager.GameStates.StartScreen)
            {
                foreach (UiButton button in StartMenuButtons)
                    button.DrawButton(sb);
            }
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
