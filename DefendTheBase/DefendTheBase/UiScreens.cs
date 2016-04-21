using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RPGEx;

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

            for (int i = 0; i < unitBuild.Count(); i++)
            {
                unitBuild[i].TextBoxLocation = new Vector2(buttonDrawPos.X, buttonDrawPos.Y + (i * (buttonSize.Y + 10)));
                unitBuild[i].SetStringPos();
                tabs.Add(unitBuild[i], 0);
                unitBuild[i].TextBoxRectangleSet();
            }

            //Base Buttons Here
            baseBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Color.Red, "btn0Trench", true));
            baseBuild[0].StringText = "Build Trench";
            baseBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Color.Red, "btn0Concrete", true));
            baseBuild[1].StringText = "Build Concrete";
            baseBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Color.Red, "btn0Destroy", true));
            baseBuild[2].StringText = "Destroy Building";

            for (int i = 0; i < baseBuild.Count(); i++)
            {
                baseBuild[i].TextBoxLocation = new Vector2(buttonDrawPos.X, buttonDrawPos.Y + (i * (buttonSize.Y + 10)));
                baseBuild[i].SetStringPos();
                tabs.Add(baseBuild[i], 1);
                baseBuild[i].TextBoxRectangleSet();
            }

            //Misc Buttons Here
            miscBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Color.Red, "btn0Upgrade", true));
            miscBuild[0].StringText = "Upgrade Tower";
            miscBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Color.Blue, "btn1NextWave", true));
            miscBuild[1].StringText = "Next Wave";

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


    public class QuestionPopUp
    {
        UiTextBox QuestionBox;
        List<UiButton> Answers = new List<UiButton>();
        int correctAnsIndex;

        public QuestionPopUp(string Question, string Answer1, string Answer2, string Answer3)
        {
            QuestionBox = new UiTextBox(Art.DebugFont, Question, new Vector2(250, 100), Color.White, Art.TextBoxBackGround, false);
            Answers.Add(new UiButton(Art.DebugFont, new Vector2(400, 400), new Vector2(200, 100), Art.TextBoxBackGround, "Ans1", true));
            Answers.Add(new UiButton(Art.DebugFont, new Vector2(400, 500), new Vector2(200, 100), Art.TextBoxBackGround, "Ans2", true));
            Answers.Add(new UiButton(Art.DebugFont, new Vector2(400, 600), new Vector2(200, 100), Art.TextBoxBackGround, "Ans3", true));

            QuestionBox.TextBoxSize = new Vector2(500, 200);
            QuestionBox.StringScale = 2f;
            QuestionBox.StringOffset = new Vector2(10, 0);

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
            }
        }

        public void Update()
        {
            if (UiButtonMessenger.CheckButtonIsDown("Ans1"))
            {
                string one = Answers[0].GetButtonID;
            
            }

        }

        public void Draw(SpriteBatch sb)
        {

            QuestionBox.Draw(sb);

            foreach (UiButton button in Answers)
            {
                button.Draw(sb);
            }
        
        }
    }

    /*public class StartScreen : Ui
    { }

    public class GameOverScreen : Ui
    { }*/

}
