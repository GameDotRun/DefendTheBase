using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RPGEx;

namespace DefendTheBase
{
    public class UiGameScreen : Ui
    {
        //these variables hopefully wont be needed when i've made some planned ui functions
        Vector2 tabDrawPos = new Vector2(1000, 60);
        Vector2 buttonDrawPos = new Vector2(1025, 120);
        Vector2 buttonSize = new Vector2(200, 100);

        UiTabs tabs;

        //Group Elements up with Lists, allows the ui controller to manipulate them
        public List<UiTextString> waveStats;
        public List<UiTextString> currencyStats;

        public List<UiButton> unitBuild;
        public List<UiButton> baseBuild;
        public List<UiButton> miscBuild;

        public UiGameScreen(GraphicsDevice graphicsDevice) : base(GameRoot.WIDTH, GameRoot.HEIGHT)
        {
            tabs = new UiTabs(graphicsDevice, Art.DebugFont, 3, tabDrawPos, new string[3] { "Towers", "Base", "Misc" }, Color.Aquamarine, new Vector2(83, 40));
            waveStats = new List<UiTextString>();
            currencyStats = new List<UiTextString>();
            
            unitBuild = new List<UiButton>();
            baseBuild = new List<UiButton>();
            miscBuild = new List<UiButton>();

            //add grouped elements to uicontrollers
            Add(ref waveStats);
            Add(ref currencyStats);
            Add(ref tabs);
            Add(ref unitBuild);
            Add(ref baseBuild);
            Add(ref miscBuild);

            CreateButtons(graphicsDevice);
        }

        public void Update()
        {
            tabs.Update();


            waveStats[0].StringText = "Wave: " + LevelWaves.WaveNumber;
            waveStats[1].StringText = "Enemies: " + LevelWaves.WaveEnemiesUsed + "/" + LevelWaves.WaveEnemyAmount;

            currencyStats[0].StringText = "Manpower: " + GameManager.Manpower;
            currencyStats[1].StringText = "Resources: " + GameManager.Resources;
        }


        public void Draw(SpriteBatch sb)
        {
            tabs.Draw(sb);

            foreach (UiTextString text in waveStats)
                text.DrawString(sb);
            foreach (UiTextString text in currencyStats)
                text.DrawString(sb);
        }

        public void CreateButtons(GraphicsDevice graphicsDevice)
        {
            waveStats.Add(new UiTextString(Art.DebugFont, "Wave: " + LevelWaves.WaveNumber, new Vector2(100, 0), Color.Black));
            waveStats.Add(new UiTextString(Art.DebugFont, "Enemies: " + LevelWaves.WaveEnemiesUsed + "/" + LevelWaves.WaveEnemyAmount, new Vector2(200, 0), Color.Black));

            currencyStats.Add(new UiTextString(Art.DebugFont, "Manpower: " + GameManager.Manpower, new Vector2(300, 0), Color.Black));
            currencyStats.Add(new UiTextString(Art.DebugFont, "Resources: " + GameManager.Resources, new Vector2(300, 20), Color.Black));

            //its of UTMOST IMPORTANCE that each button has a unique id

            //Units buttons Here
            unitBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Color.Red, "btn0TowerGun", true));
            unitBuild[0].StringText = "Gun Tower";
            unitBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Color.Red, "btn0TowerRocket", true));
            unitBuild[1].StringText = "Rocket Tower";
            unitBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Color.Red, "btn0TowerSAM", true));
            unitBuild[2].StringText = "SAM Tower";
            unitBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Color.Red, "btn0TowerTesla", true));
            unitBuild[3].StringText = "Tesla Tower";

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

            for (int i = 0; i < miscBuild.Count(); i++)
            {
                miscBuild[i].TextBoxLocation = new Vector2(buttonDrawPos.X, buttonDrawPos.Y + (i * (buttonSize.Y + 10)));
                miscBuild[i].SetStringPos();
                tabs.Add(miscBuild[i], 2);
                miscBuild[i].TextBoxRectangleSet();
            }

           
        }


    }
}
