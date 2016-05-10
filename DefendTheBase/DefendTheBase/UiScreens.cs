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
            if (GameManager.HelpMode)
            {
                foreach (UiButton button in UiButtonMessenger.ButtonListeners)
                {
                    if (button.IsMouseHovering(Input.MousePosition.ToPoint()) && !button.TabButton && !HelpDialogManager.Hovering)
                    {
                        HelpDialogManager.Hovering = true;
                        HelpDialogManager.Add(new HelpDialog(button.TextBoxInfo, Input.MousePosition));
                        break;
                    }

                    else HelpDialogManager.Hovering = false;
                }
            }


            if (GameManager.UnlockedTowers.HasFlag(GameManager.Unlocks.RocketTower))
            {
                if (unitBuild.Count < 2)
                {
                    unitBuild.Add(new UiButton(Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsTower[1], Art.ButtonEffectTexture, "btn0TowerRocket", true));
                    buttonSingleInit(unitBuild[unitBuild.Count - 1], unitBuild.Count - 1, 0);
                    unitBuild[1].TextBoxInfo = "Rocket Tower\n Effective vs Vehicles";
                }
            }

            if (GameManager.UnlockedTowers.HasFlag(GameManager.Unlocks.SamTower))
            {
                if (unitBuild.Count < 3)
                {
                    unitBuild.Add(new UiButton(Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsTower[2], Art.ButtonEffectTexture, "btn0TowerSAM", true));
                    buttonSingleInit(unitBuild[unitBuild.Count - 1], unitBuild.Count - 1, 0);
                    unitBuild[2].TextBoxInfo = "SAM Tower\n Effective vs Helicopters";
                }
            }

            if (GameManager.UnlockedTowers.HasFlag(GameManager.Unlocks.TeslaTower))
            {
                if (unitBuild.Count < 4)
                {
                    unitBuild.Add(new UiButton(Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsTower[3], Art.ButtonEffectTexture, "btn0TowerTesla", true));
                    buttonSingleInit(unitBuild[unitBuild.Count - 1], unitBuild.Count - 1, 0);
                    unitBuild[3].TextBoxInfo = "Tesla Tower\n Neutral vs All";
                }
            }

            if (GameManager.UnlockedTowers.HasFlag(GameManager.Unlocks.Upgrade))
            {
                if (unitBuild.Count < 5)
                {
                    unitBuild.Add(new UiButton(Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsTower[4], Art.ButtonEffectTexture, "btn0Upgrade", true));
                    buttonSingleInit(unitBuild[unitBuild.Count - 1], unitBuild.Count - 1, 0);
                    unitBuild[4].TextBoxInfo = "Upgrade Tower\n Use this to upgrade an existing tower!";
                }
            }

            
            if(GameManager.GameState == GameManager.GameStates.PlayScreen)
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
            unitBuild[0].TextBoxInfo = "Gun Tower\n Effective vs Soldiers";
            //unitBuild[0].StringText = "Gun Tower";

            buttonsInit(ref unitBuild, 0);

            //Base Buttons Here
            baseBuild.Add(new UiButton(Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsBase[0], Art.ButtonEffectTexture, "btn0Trench", true));
            baseBuild[0].TextBoxInfo = "Use this to build trenches!";

            baseBuild.Add(new UiButton(Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsBase[1], Art.ButtonEffectTexture, "btn0Concrete", true));
            baseBuild[1].TextBoxInfo = "Use this to build foundations to place your towers on!";
            //baseBuild[1].StringText = "Build Concrete";

            baseBuild.Add(new UiButton(Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsBase[2], Art.ButtonEffectTexture, "btn0Destroy", true));
            baseBuild[2].TextBoxInfo = "Use this to Destroy a placed building!";
            //baseBuild[2].StringText = "Destroy Building";

            baseBuild.Add(new UiButton(Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsBase[3], Art.ButtonEffectTexture, "btn0Repair", true));
            baseBuild[3].TextBoxInfo = "Use this to repair a tower!";
            baseBuild[3].StringText = "Repair Tower";


            buttonsInit(ref baseBuild, 1);


            //Misc Buttons Here


            miscBuild.Add(new UiButton(Art.DebugFont, Vector2.Zero, buttonSize, Art.ButtonsMisc[0], Art.ButtonEffectTexture, "btn1NextWave", true));
            //miscBuild[0].StringText = "Next Wave";
            miscBuild[0].TextBoxInfo = "Start Next Wave";

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
        public List<UiButton> topScreenButtons;

        public UiTopGameScreen(GraphicsDevice graphicsDevice) : base(GameManager.WIDTH, GameManager.HEIGHT)
        {
            healthBar = new UiStatusBars(100, new Vector2(437, 39), new Vector2(384, 45), Art.HpBar[0], Art.HpBar[1]);
            waveStats = new List<UiTextBox>();
            currencyStats = new List<UiTextBox>();
            timers = new List<UiTextBox>();
            popUpText = new List<UiTextString>();
            topScreenButtons = new List<UiButton>();

            Add(ref waveStats);
            Add(ref currencyStats);
            Add(ref timers);
            Add(ref popUpText);
            Add(ref healthBar);
            Add(ref topScreenButtons);

            CreateUi(graphicsDevice);
        }

        public void CreateUi(GraphicsDevice graphicsDevice)
        {

            topScreenButtons.Add(new UiButton(Art.UiFont, new Vector2(1200,  5), new Vector2(30, 30), Art.HelpButton, Art.ButtonEffectTexture, "HelpButton", true));

            waveStats.Add(new UiTextBox(Art.UiFont, "Wave: ", new Vector2(10, 10), Color.White, Art.TextBoxBackGround, true));
            waveStats[0].TextBoxInfo = "Current Wave Number";

            waveStats.Add(new UiTextBox(Art.UiFont, "Kills: " + WaveManager.EnemiesKilled, new Vector2(110, 10), Color.White, Art.TextBoxBackGround, true));
            waveStats[1].TextBoxInfo = "Number of Enemies killed";

            waveStats.Add(new UiTextBox(Art.UiFont, "Questions Completed: " + WaveManager.questionsAnsweredCorrect + "/" + QuestionPopUpManager.QuestionTotal, new Vector2(925, 50), Color.White, Art.TextBoxBackGround, true));
            waveStats[2].TextBoxInfo = "Questions Completed";

            currencyStats.Add(new UiTextBox(Art.UiFont, "Manpower: " + GameManager.Manpower, new Vector2(10, 50), Color.White, Art.TextBoxBackGround, true));
            currencyStats[0].TextBoxInfo = "Number of men available";

            currencyStats.Add(new UiTextBox(Art.UiFont, "Resources: " + GameManager.Resources, new Vector2(10, 90), Color.White, Art.TextBoxBackGround, true));
            currencyStats[1].TextBoxInfo = "Number of resources available.";

            timers.Add(new UiTextBox(Art.UiFont, "Next Wave in: ", new Vector2(1000, GameManager.BORDERTOP - 40), Color.White, Art.TextBoxBackGround, true));
            timers[0].TextBoxInfo = "Time till next wave";

            foreach (UiButton button in topScreenButtons)
            {
                UiButtonMessenger.RegisterButton(button);
                button.TextBoxRectangleSet();
                button.SetButtonState = UiButton.UiButtonStates.Button_Up;
            }

            foreach (UiTextBox box in waveStats)
            {
                box.StringOffset = new Vector2(5, 0);
            }

            if (!GameManager.HelpMode)
            {
                topScreenButtons[0].TextBoxTexture = Art.HelpButtonOff;
            }

            topScreenButtons[0].TextBoxInfo = "Help Button";
        }

        public void Update()
        {
            healthBar.Update(GameManager.BaseHealth);

            waveStats[0].StringText = "Wave: " + WaveManager.WaveNumber;
            waveStats[1].StringText = "Kills: " + WaveManager.EnemiesKilled;
            waveStats[2].StringText = "Questions Completed: " + WaveManager.questionsAnsweredCorrect + "/" + QuestionPopUpManager.QuestionTotal;
            

            currencyStats[0].StringText = "Manpower: " + GameManager.Manpower;
            currencyStats[1].StringText = "Resources: " + GameManager.Resources;

            timers[0].StringText = "Next Wave in: " + WaveManager.WaveStartTimer.TotalSeconds.ToString("N0");

            foreach (List<UiTextBox> boxList in TextBoxList)
            {
                foreach (UiTextBox box in boxList)
                {
                    if (box.IsMouseHovering(Input.MousePosition.ToPoint())  && !HelpDialogManager.Hovering)
                    {
                        HelpDialogManager.Hovering = true;
                        HelpDialogManager.Add(new HelpDialog(box.TextBoxInfo, Input.MousePosition));
                        break;
                    }
                }
            }


            if (topScreenButtons[0].IsButtonDown() && GameManager.HelpMode)
            {
                topScreenButtons[0].TextBoxTexture = Art.HelpButtonOff;
                GameManager.HelpMode = false;
            }

            else if (topScreenButtons[0].IsButtonDown() && !GameManager.HelpMode)
            {
                topScreenButtons[0].TextBoxTexture = Art.HelpButton;
                GameManager.HelpMode = true;
            }
        }
    }

    public static class MessageBoxManager
    {
        public static string RocketTowerUnlock = "New Tower Unlocked! Rocket Tower, use this to make short work of vehicles";
        public static string SAMTowerUnlock = "New Tower Unlocked! SAM Tower, use this to make short work of helicopters";
        public static string TeslaTowerUnlock = "New Tower Unlocked! Tesla Tower, has no weakness but no strength either";
        public static string UpgradeTowerUnlock = "New Tower Unlocked! Upgrade Tower, use this to upgrade your Towers";
        public static string Introduction = "Welcome Commander!\nWe have some problems. The Axis are attacking our base and we dont have enough defenses!\nSurvive as long as you can commander.\nAnd good luck.";

        public static List<MessageBox> MessageBox = new List<MessageBox>();

        public static bool MessageDisplayed = false;

        public static void Add(MessageBox messagebox)
        {
            MessageBox.Add(messagebox);
            MessageDisplayed = true;
        }

        public static void Remove(MessageBox messagebox)
        {
            MessageBox.Remove(messagebox);
            MessageDisplayed = false;
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
        public UiTimer timer = new UiTimer(30000f);

        public MessageBox(string stringText)
            : base(Art.UiFont, stringText, new Vector2(250, 150), Color.White, Art.TextBoxBackGround, false)
        {
            TextBoxSize = new Vector2(500, 200);
            StringOffset = new Vector2(10, 0);
            LineWrapper();
        }

        public void Update(GameTime gt)
        {
            if (!timer.GetActive)
                timer.ActivateTimer();

            timer.TimerUpdate(gt);

            if (TextBox.Contains(Input.MousePosition.ToPoint()) && Input.WasLMBClicked)
            {
                timer.ManualReset();
            }
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
        public static string Neutral = "Neutral";

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
                while(PopUps.Count > 10)
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
            new string[] { "Who won World War II?", "Britain", "Germany", "Allied Forces", "Ans3" },
            new string[] { "Who was the leader of the Soviet Union during World War II?", "Stalin", "Trotski", "Lenin", "Ans1" },
            new string[] { "Who was the leader of Germany during World War II?", "Churchill", "Mussolini", "Hitler", "Ans3" },
            new string[] { "Who was the leader of Italy during World War II?", "Hirohito", "Mussolini", "Eisenhower", "Ans2" },
            new string[] { "When did WWII begin?", "1939", "1914", "1941", "Ans1" },
            new string[] { "Which country did Germany invade to start WWII?", "Austria", "Russia", "Poland", "Ans3" },
            new string[] { "What was the Nazi 'Lightning War' called?", "The Blitz", "Blitzkrieg", "Operation Barbarossa", "Ans2" },
            new string[] { "What was the Battle of Britain?", "The Royal Air Force defeated the Luftwaffe.", "The Luftwaffe bombed London and other British cities.", "The British withdrew from France by sea.", "Ans1" },
            new string[] { "What kind of bomb did the Americans drop on Hiroshima?", "A V-1 rocket", "Blitzkrieg", "An atomic bomb", "Ans3" },
            new string[] { "What was Hitlers wife called?", "Eva Braun", "Isla Braun", "Emma Braun", "Ans1" },
            new string[] { "Where was Hitler Born?", "Poland", "Austria", "Germany", "Ans2" },
            new string[] { "What type of car did Hitler promise to build?", "Beatle", "Volkswagen ", "Ford", "Ans2" },
            new string[] { "What country did Hitler want to send the Jews to?", "Madagascar ", "America", "Poland", "Ans1" },
            new string[] { "What was Kristallnacht?", "Day of Dreams", "Night of Crystals", "Night of  Broken Glass", "Ans3" },
            new string[] { "What was Hitler's original desired profession?", "Artist ", "Politician", "Soldier", "Ans1" },
            new string[] { "Roughly how many Jews were killed in Concentration Camps?", "6 mil", "1 mil ", "10 mil", "Ans1" },
            new string[] { "Who were the two main sides of WWII? Allies and?", "Axis ", "Bad guys", "Enemies", "Ans1" },
            new string[] { "What did Hitler use to persuade the people to vote for him?", "Forced at gun point", "Bribes", "Propaganda", "Ans3" },
            new string[] { "What country was Auschwitz(Concentration Camp) in?", "Italy", "Germany", "Poland", "Ans3" }
        };

        public static int QuestionTotal = QuestionsArray.Count();

        static List<QuestionPopUp> QuestionPopUps = new List<QuestionPopUp>();

        public static bool QuestionUp = false;

        public static void Init()
        {

            QuestionsArray = new List<string[]>
            {
                new string[] { "Who won World War II?", "Britain", "Germany", "Allied Forces", "Ans3" },
                new string[] { "Who was the leader of the Soviet Union during World War II?", "Stalin", "Trotski", "Lenin", "Ans1" },
                new string[] { "Who was the leader of Germany during World War II?", "Churchill", "Mussolini", "Hitler", "Ans3" },
                new string[] { "Who was the leader of Italy during World War II?", "Hirohito", "Mussolini", "Eisenhower", "Ans2" },
                new string[] { "When did WWII begin?", "1939", "1914", "1941", "Ans1" },
                new string[] { "Which country did Germany invade to start WWII?", "Austria", "Russia", "Poland", "Ans3" },
                new string[] { "What was the Nazi 'Lightning War' called?", "The Blitz", "Blitzkrieg", "Operation Barbarossa", "Ans2" },
                new string[] { "What was the Battle of Britain?", "The Royal Air Force defeated the Luftwaffe.", "The Luftwaffe bombed London and other British cities.", "The British withdrew from France by sea.", "Ans1" },
                new string[] { "What kind of bomb did the Americans drop on Hiroshima?", "A V-1 rocket", "Blitzkrieg", "An atomic bomb", "Ans3" },
                new string[] { "What was Hitlers wife called?", "Eva Braun", "Isla Braun", "Emma Braun", "Ans1" },
                new string[] { "Where was Hitler Born?", "Poland", "Austria", "Germany", "Ans2" },
                new string[] { "What type of car did Hitler promise to build?", "Beatle", "Volkswagen ", "Ford", "Ans2" },
                new string[] { "What country did Hitler want to send the Jews to?", "Madagascar ", "America", "Poland", "Ans1" },
                new string[] { "What was Kristallnacht?", "Day of Dreams", "Night of Crystals", "Night of  Broken Glass", "Ans3" },
                new string[] { "What was Hitler's original desired profession?", "Artist ", "Politician", "Soldier", "Ans1" },
                new string[] { "Roughly how many Jews were killed in Concentration Camps?", "6 mil", "1 mil ", "10 mil", "Ans1" },
                new string[] { "Who were the two main sides of WWII? Allies and?", "Axis ", "Bad guys", "Enemies", "Ans1" },
                new string[] { "What did Hitler use to persuade the people to vote for him?", "Forced at gun point", "Bribes", "Propaganda", "Ans3" },
                new string[] { "What country was Auschwitz(Concentration Camp) in?", "Italy", "Germany", "Poland", "Ans3" }
            };


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
            QuestionBox.StringOffset = new Vector2(10, 0);
            QuestionBox.LineWrapper();

            CorrectBox.TextBoxSize = new Vector2(500, 200);
            CorrectBox.StringOffset = new Vector2(10, 0);
            CorrectBox.LineWrapper();

            WrongBox.TextBoxSize = new Vector2(500, 200);
            WrongBox.StringOffset = new Vector2(10, 0);
            WrongBox.LineWrapper();


            Answers[0].StringText = questionDetails[1];
            Answers[0].TextBoxInfo = "An answer box.";
            Answers[1].StringText = questionDetails[2];
            Answers[1].TextBoxInfo = "An answer box.";
            Answers[2].StringText = questionDetails[3];
            Answers[2].TextBoxInfo = "An answer box.";

            foreach (UiButton button in Answers)
            {
                UiButtonMessenger.RegisterButton(button);
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
                            GameManager.ModifyResources(1000);


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
                    Input.Update();
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

    public static class HelpDialogManager
    {
        public static List<HelpDialog> DialogBox = new List<HelpDialog>();

        public static bool Hovering = false;

        public static void Add(HelpDialog messagebox)
        {
            DialogBox.Add(messagebox);
        }

        public static void Remove(HelpDialog messagebox)
        {
            DialogBox.Remove(messagebox);
        }

        public static void Update()
        {
            if (DialogBox.Count > 1)
                DialogBox.RemoveAt(0);


            if (Hovering == false && DialogBox.Count != 0)
                DialogBox.RemoveAt(0);

            foreach (HelpDialog message in DialogBox)
            {
                message.Update();
            }
        }

        public static void Draw(SpriteBatch sb)
        {
            foreach (HelpDialog message in DialogBox)
            {
                message.Draw(sb);
            }
        }
    }

    public class HelpDialog : UiTextBox
    { 
        public HelpDialog(string stringText, Vector2 mousePos)
            : base(Art.HelpFont, stringText, mousePos, Color.White, Art.TextBoxBackGround, false)
        {
            TextBoxSize = new Vector2(200, 300);
            LineWrapper();

            SizeToTextY();

            StringOffset = new Vector2(10, 0);
        }

        public void Update()
        {
            if (Input.MousePosition.X + TextBoxSize.X > GameManager.ScreenSize.X + GameManager.BORDERRIGHT)
            {
                float Tempvalue = GameManager.ScreenSize.X + GameManager.BORDERRIGHT - TextBoxSize.X - Input.MousePosition.X;
                TextBoxLocation = new Vector2(Input.MousePosition.X + Tempvalue, TextBoxLocation.Y + 20);
            }

            else TextBoxLocation = Input.MousePosition;
        }

        public void DrawBox(SpriteBatch sb)
        {
            Draw(sb);
        }
    
    }

    public class StartScreen
    {
        Vector2 StartScreenModifier = new Vector2(0, -5);
        Texture2D backgroundTex = Art.StartMenuBackground;

        List<UiButton> StartMenuButtons = new List<UiButton>();
        UiTextBox HiScores = new UiTextBox(Art.UiFont, "", new Vector2(50, 250), Color.White, Art.TextBoxBackGround, false);

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

            HiScores.TextBoxSize = new Vector2(400, 150);
            HiScores.StringOffset = new Vector2(10, 0);

            GameManager.Scores = GameManager.Scores.LoadData();
        
        }

        public void Update(GameTime gt)
        {
            HiScores.StringText = "Highest Wave: " + GameManager.Scores.HighestWave.ToString() + "\n"
                + "Highest Wave Kills: " + GameManager.Scores.HighestWaveKills + "\n\n"
                    + "All time kills: " + GameManager.Scores.AllTimeKills;

            if (fade < 1 && GameManager.GameState == GameManager.GameStates.StartScreen)
            {
                
                EnableScreen();
                fade = 1;
            }

            if (StartMenuButtons[0].IsButtonDown())
            {
                GameRoot.resetgame = true;

                DisableScreen();
                GameManager.GameState = GameManager.GameStates.PlayScreen;
                FadeOutTimer.ActivateTimer();
                fadeout = true;

                StartMenuButtons[0].SetButtonState = UiButton.UiButtonStates.Button_Up;

                Input.Update(); 

            }

            else if (StartMenuButtons[1].IsButtonDown())
            {
                GameManager.GameState = GameManager.GameStates.InfoScreen;
                Input.Update();
            }

            else if (StartMenuButtons[2].IsButtonDown())
            {
                GameManager.GameState = GameManager.GameStates.TutScreen;
                Input.Update();
            
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

                HiScores.Draw(sb);
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

    public class InfoScreen
    {
        const string Hitler = "Adolf Hitler was the leader of the Nazi Party and Chancellor of Germany from 1933 to 1945, and Fuhrer (leader) of Nazi Germany from 1934 to 1945. As dictator of Nazi Germany, he initiated World War II in Europe with the invasion of Poland in September 1939. \n Hitler was born in German speaking Austria and moved to and from Germany in his youngest years. Hitler's father's farming efforts at Hafeld ended in failure in 1897 and the family moved to Lambach. The eight-year-old Hitler took singing lessons, sang in the church choir, and even considered becoming a priest. \nIgnoring his son's desire to attend a classical high school and become an artist, Alois (Hitlers Father) sent Hitler to the Realschule in Linz in September 1900.Hitler rebelled against this decision, and in Mein Kampf revealed that he intentionally did poorly in school, hoping that once his father saw 'what little progress I was making at the technical school he would let me devote myself to my dream'. \nFrom 1905, Hitler lived a bohemian life in Vienna, financed by orphan's benefits and support from his mother. He worked as a casual labourer and eventually as a painter, selling watercolours of Vienna's sights. The Academy of Fine Arts Vienna rejected him in 1907 and again in 1908, citing 'unfitness for painting'. \nHitler created a public image as a celibate man without a domestic life, dedicated entirely to his political mission and the nation. He met his lover, Eva Braun, in 1929, and married her in April 1945. \n \n \n\n\nSourced from:  https://en.wikipedia.org/wiki/Adolf_Hitler";
        const string Churchill = "Britain's war leader\nWinston Churchill was Britain's prime minister for most of World War II. He was famous for his speeches, and for his refusal to give in, even when things were going badly. For a time he was the most famous person in Britain. People all over the world know the name Winston Churchill.\n\nWhen did he live?\nChurchill was born in 1874. He lived through two world wars. He saw the first cars, the first planes, and the first astronauts in space. He was at the crowning of Elizabeth II as Queen in 1953. He was an MP for over 60 years. Winston Churchill died in 1965.\n\nWhat was special about Churchill?\nPeople remember Churchill as a war leader. But he did other important jobs in a long life full of adventures. Winston Churchill loved history and in his life he made history.\n\n\n\n\n\n\n\n\n\n\nSourced from:  http://www.bbc.co.uk/schools/primaryhistory/famouspeople/winston_churchill/";
        const string Mussolini = "Italian dictator Benito Mussolini (1883 to 1945) rose to power in the wake of World War I as a leading proponent of Facism. Originally a revolutionary Socialist, he forged the paramilitary Fascist movement in 1919 and became prime minister in 1922. Mussolini's military expenditures in Libya, Somalia, Ethiopia and Albania made Italy predominant in the Mediterranean region, though they exhausted his armed forces by the late 1930s. Mussolini allied himself with Hitler, relying on the German dictator to prop up his leadership during World War II, but he was killed shortly after the German surrender in Italy in 1945.\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\nSourced from   http://www.history.com/topics/world-war-ii/benito-mussolini";
        const string Allies = "The Allies of World War II, called the United Nations from the 1 January 1942 declaration, were the countries that together opposed the Axis powers during the Second World War. The Allies promoted the alliance as seeking to stop German, Japanese and Italian aggression.\n\nThe anti-German coalition at the start of the war (1 September 1939) consisted of France, Poland and Great Britain, soon to be joined by the British Commonwealth (Canada, Australia, New Zealand and South Africa). Poland was a minor factor after its defeat in 1939; France was a minor factor after its defeat in 1940. After first having cooperated with Germany in partitioning Poland whilst remaining neutral in the Allied-Axis conflict, the Soviet Union perforce joined the Allies in June 1941 after being invaded by Germany. The United States provided war materiel and money all along, and officially joined in December 1941 after the Japanese attack on Pearl Harbor. As of 1942, the 'Big Three' leaders of the United Kingdom, the Soviet Union, and the United States controlled Allied policy; relations between the United Kingdom and the United States were especially close. China had already been at war with Japan since the Marco Polo Bridge Incident of 1937, but officially joined the Allies in 1941. The Big Three and China were referred as a 'trusteeship of the powerful', then were recognized as the Allied 'Big Four' in Declaration by United Nations and later the 'Four Policemen' of United Nations for the Allies. Other key Allies included British India, the Netherlands, and Yugoslavia as well as Free France; there were numerous others. Together they called themselves the 'United Nations' and in 1945 created the modern UN.\n\n\n\nSourced from   https://en.wikipedia.org/wiki/Allies_of_World_War_II";
        const string Axis = "The Axis powers (German: Achsenmachte, Japanese: Sujikukoku, Italian: Potenze dell'Asse), also known as the Axis, were the nations that fought in the Second World War against the Allied forces. The Axis powers agreed on their opposition to the Allies, but did not completely coordinate their activity.\n\nThe Axis grew out of the diplomatic efforts of Germany, Italy and Japan to secure their own specific expansionist interests in the mid 1930s. The first step was the treaty signed by Germany and Italy in October 1936. Mussolini declared on 1 November that all other European countries would from then on rotate on the Rome-Berlin axis, thus creating the term 'Axis'. The almost simultaneous second step was the signing in November 1936 of the Anti-Comintern Pact, an anti communist treaty between Germany and Japan. Italy joined the Pact in 1937. The 'Rome/Berlin Axis' became a military alliance in 1939 under the so-called 'Pact of Steel', with the Tripartite Pact of 1940 leading to the integration of the military aims of Germany and its two treaty-bound allies.\n\nAt its zenith during World War II, the Axis presided over territories that occupied large parts of Europe, North Africa, and East Asia. There were no three-way summit meetings and cooperation and coordination was minimal, with a bit more between Germany and Italy. The war ended in 1945 with the defeat of the Axis powers and the dissolution of their alliance. As in the case of the Allies, membership of the Axis was fluid, with some nations switching sides or changing their degree of military involvement over the course of the war.\n\n\nSourced from   https://en.wikipedia.org/wiki/Axis_powers";
        const string HiroHito = "Hirohito (1901 to 1989) was emperor of Japan from 1926 until his death in 1989. He took over at a time of rising democratic sentiment, but his country soon turned toward ultra-nationalism and militarism. During World War II (1939 to 1945), Japan attacked nearly all of its Asian neighbors, allied itself with Nazi Germany and launched a surprise assault on the U.S. naval base at Pearl Harbor. Though Hirohito later portrayed himself as a virtually powerless constitutional monarch, many scholars have come to believe he played an active role in the war effort. After Japan's surrender in 1945, he became a figurehead with no political power.\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\nSourced from    http://www.history.com/topics/world-war-ii/hirohito";
        const string Stalin = "Joseph Stalin (1878 to 1953) was the dictator of the Union of Soviet Socialist Republics (USSR) from 1929 to 1953. Under Stalin, the Soviet Union was transformed from a peasant society into an industrial and military superpower. However, he ruled by terror, and millions of his own citizens died during his brutal reign. Born into poverty, Stalin became involved in revolutionary politics, as well as criminal activities, as a young man. After Bolshevik leader Vladimir Lenin (1870 to 1924) died, Stalin outmaneuvered his rivals for control of the party. Once in power, he collectivized farming and had potential enemies executed or sent to forced labor camps. Stalin aligned with the United States and Britain in World War II (1939 to 1945) but afterward engaged in an increasingly tense relationship with the West known as the Cold War (1946 to 1991). After his death, the Soviets initiated a deStalinization process.\n\n\n\n\n\n\n\n\n\n\n\n\n\nSourced from    http://www.history.com/topics/joseph-stalin";
        const string CharlesDeGaulle = "Charles de Gaulle rose from French soldier in World War I to exiled leader and, eventually, president of the Fifth Republic, a position he held until 1969. De Gaulle's time as a commander in World War II would later influence his political career, providing him with a tenacious drive. His time as president was marked by the student and worker uprisings in 1968, which he responded to with an appeal for civil order.\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\nSourced from    http://www.biography.com/people/charles-de-gaulle-9269794";

        Texture2D backgroundTex = Art.StartMenuBackground;
        UiTabs tabs;
        UiButton backButton;

        List<UiTextBox> InfoBoxes = new List<UiTextBox>();

        public InfoScreen(GraphicsDevice graphicsDevice)
        {
            tabs = new UiTabs(graphicsDevice, Art.DebugFont, 8, new Vector2(200, 200), new string[8] { "", "", "", "", "", "", "", "" }, Art.tabTestTexture, Art.ButtonEffectTexture, new Vector2(100, 50));
            backButton = new UiButton(Art.UiFont, new Vector2(20, 800), new Vector2(100, 50 ), Art.TextBoxBackGround, Art.ButtonEffectTexture, "backButton", true);

            backButton.SetButtonState = UiButton.UiButtonStates.Button_Up;

            tabs.tabList[0].tabButton.TextBoxTexture = Art.ChurchilPort;
            tabs.tabList[1].tabButton.TextBoxTexture = Art.StalinPort;
            tabs.tabList[2].tabButton.TextBoxTexture = Art.FrenchiePort;
            tabs.tabList[3].tabButton.TextBoxTexture = Art.HitlerPort;
            tabs.tabList[4].tabButton.TextBoxTexture = Art.MussoPort;
            tabs.tabList[5].tabButton.TextBoxTexture = Art.HiroHitoPort;
            tabs.tabList[6].tabButton.TextBoxTexture = Art.AlliesPort;
            tabs.tabList[7].tabButton.TextBoxTexture = Art.AxisPort;

            backButton.StringText = "Back";
            backButton.StringOffset = new Vector2(10);

            InfoBoxes.Add(new UiTextBox(Art.InfoFont, Churchill, new Vector2(200, 255), Color.White, Art.TextBoxBackGround, false));
            tabs.Add(InfoBoxes[0], 0);

            InfoBoxes.Add(new UiTextBox(Art.InfoFont, Stalin, new Vector2(200, 255), Color.White, Art.TextBoxBackGround, false));
            tabs.Add(InfoBoxes[1], 1);

            InfoBoxes.Add(new UiTextBox(Art.InfoFont, CharlesDeGaulle, new Vector2(200, 255), Color.White, Art.TextBoxBackGround, false));
            tabs.Add(InfoBoxes[2], 2);

            InfoBoxes.Add(new UiTextBox(Art.InfoFont, Hitler, new Vector2(200, 255), Color.White, Art.TextBoxBackGround, false));
            tabs.Add(InfoBoxes[3], 3);

            InfoBoxes.Add(new UiTextBox(Art.InfoFont, Mussolini, new Vector2(200, 255), Color.White, Art.TextBoxBackGround, false));
            tabs.Add(InfoBoxes[4], 4);

            InfoBoxes.Add(new UiTextBox(Art.InfoFont, HiroHito, new Vector2(200, 255), Color.White, Art.TextBoxBackGround, false));
            tabs.Add(InfoBoxes[5], 5);

            InfoBoxes.Add(new UiTextBox(Art.InfoFont, Allies, new Vector2(200, 255), Color.White, Art.TextBoxBackGround, false));
            tabs.Add(InfoBoxes[6], 6);

            InfoBoxes.Add(new UiTextBox(Art.InfoFont, Axis, new Vector2(200, 255), Color.White, Art.TextBoxBackGround, false));
            tabs.Add(InfoBoxes[7], 7);

            
            foreach (UiTextBox box in InfoBoxes)
            {
                box.TextBoxSize = new Vector2(900, 500);
                box.StringOffset = new Vector2(20, 10);
                box.LineWrapper();
            }

        }

        public void Update()
        {
            if (GameManager.GameState == GameManager.GameStates.InfoScreen)
            {
                EnableScreen();
                Input.Update();
            }

            if (backButton.IsButtonDown())
            {
                backButton.SetButtonState = UiButton.UiButtonStates.Button_Up;
                DisableScreen();
                GameManager.GameState = GameManager.GameStates.StartScreen;
            }

            tabs.Update();
        }

        public void Draw(SpriteBatch sb)
        {

            sb.Draw(backgroundTex, Vector2.Zero, Color.White);
            backButton.DrawButton(sb);
            tabs.Draw(sb);
        }

        public void EnableScreen()
        {
            tabs.AddTabsToButtonMessenger();

            UiButtonMessenger.RegisterButton(backButton);
        }

        public void DisableScreen()
        {
            UiButtonMessenger.RemoveButton(backButton.GetButtonID);
        }

    }

    public class EndScreen
    {
        Vector2 EndScreenModifier = new Vector2(0, -5);
        Texture2D backgroundTex = Art.EndScreenBackground;

        UiTimer FadeOutTimer = new UiTimer(5000f);

        float fade = 0f;

        public bool fadeout = false;
        public bool fadein = true;

        public EndScreen()
        {


        }

        public void Update(GameTime gt)
        {
          
            if (fade >= 1)
            {
                FadeOutTimer.ActivateTimer();
                fadeout = true;
                fadein = false;
                GameManager.GameState = GameManager.GameStates.StartScreen;
            }

            else if (fade < 1 && !fadeout && GameManager.GameState == GameManager.GameStates.LoseScreen)
            {
                fade += 0.001f;
                fadein = true;
            }

            if (FadeOutTimer.GetActive)
            {
                FadeOutTimer.TimerUpdate(gt);
                fade -= 0.01f;
            }

            if (FadeOutTimer.TimeReached() )
            {
                fadeout = false;
            }

            if (fade < 0)
                fade = 0;

            
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(backgroundTex, Vector2.Zero, Color.White * fade);
        }

    }



    /*public class GameOverScreen : Ui
    { }*/

}
