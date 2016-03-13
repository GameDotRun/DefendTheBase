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

            // Build Gun Tower Button
            unitBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Color.Red, "btn0TowerGun", true));
            unitBuild[0].TextBoxLocation = new Vector2(buttonDrawPos.X, buttonDrawPos.Y + (0 * (buttonSize.Y + 10)));
            unitBuild[0].StringText = "Gun Tower";
            unitBuild[0].SetStringPos();
            tabs.Add(unitBuild[0], 0);
            unitBuild[0].TextBoxRectangleSet();
            // Build Rocket Tower Button
            unitBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Color.Red, "btn0TowerRocket", true));
            unitBuild[1].TextBoxLocation = new Vector2(buttonDrawPos.X, buttonDrawPos.Y + (1 * (buttonSize.Y + 10)));
            unitBuild[1].StringText = "Rocket Tower";
            unitBuild[1].SetStringPos();
            tabs.Add(unitBuild[1], 0);
            unitBuild[1].TextBoxRectangleSet();
            // Build SAM Tower Button
            unitBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Color.Red, "btn0TowerSAM", true));
            unitBuild[2].TextBoxLocation = new Vector2(buttonDrawPos.X, buttonDrawPos.Y + (2 * (buttonSize.Y + 10)));
            unitBuild[2].StringText = "SAM Tower";
            unitBuild[2].SetStringPos();
            tabs.Add(unitBuild[2], 0);
            unitBuild[2].TextBoxRectangleSet();
            // Build Tesla Tower Button
            unitBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Color.Red, "btn0TowerTesla", true));
            unitBuild[3].TextBoxLocation = new Vector2(buttonDrawPos.X, buttonDrawPos.Y + (3 * (buttonSize.Y + 10)));
            unitBuild[3].StringText = "Tesla Tower";
            unitBuild[3].SetStringPos();
            tabs.Add(unitBuild[3], 0);
            unitBuild[3].TextBoxRectangleSet();

            //for (int i = 2; i < 6; i++) // where 6 is number of buttons to createl
            //{
            //    unitBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Color.Red, "button" + i.ToString(), true));
            //    unitBuild[i].TextBoxLocation = new Vector2(buttonDrawPos.X, buttonDrawPos.Y + (i * (buttonSize.Y + 10)));
            //    unitBuild[i].StringText = "TOWERS PLACEHOLDER " + i.ToString();
            //    unitBuild[i].SetStringPos();
            //    tabs.Add(unitBuild[i], 0);
            //    unitBuild[i].TextBoxRectangleSet();
            //}
            // Build Trench Button
            baseBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Color.Red, "btn0Trench", true));
            baseBuild[0].TextBoxLocation = new Vector2(buttonDrawPos.X, buttonDrawPos.Y);
            baseBuild[0].StringText = "Build Trench";
            baseBuild[0].SetStringPos();
            tabs.Add(baseBuild[0], 1);
            baseBuild[0].TextBoxRectangleSet();
            // Build Concrete Button
            baseBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Color.Red, "btn0Concrete", true));
            baseBuild[1].TextBoxLocation = new Vector2(buttonDrawPos.X, buttonDrawPos.Y + (1 * (buttonSize.Y + 10)));
            baseBuild[1].StringText = "Build Concrete";
            baseBuild[1].SetStringPos();
            tabs.Add(baseBuild[1], 1);
            baseBuild[1].TextBoxRectangleSet();
            // PLACEHOLDER BUTTON
            baseBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Color.Red, "btn0Destroy", true));
            baseBuild[2].TextBoxLocation = new Vector2(buttonDrawPos.X, buttonDrawPos.Y + (2 * (buttonSize.Y + 10)));
            baseBuild[2].StringText = "Destroy Building";
            baseBuild[2].SetStringPos();
            tabs.Add(baseBuild[2], 1);
            baseBuild[2].TextBoxRectangleSet();

            //for (int i = 1; i < 3; i++)
            //{
            //    baseBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Color.Red, "button0" + i.ToString(), true));
            //    baseBuild[i].TextBoxLocation = new Vector2(buttonDrawPos.X, buttonDrawPos.Y + (i * (buttonSize.Y + 10)));
            //    baseBuild[i].StringText = "BUILD PLACEHOLDER " + i.ToString();
            //    baseBuild[i].SetStringPos();
            //    tabs.Add(baseBuild[i], 1);
            //    baseBuild[i].TextBoxRectangleSet();
            //}

            // Misc
            // Upgrade button
            miscBuild.Add(new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Color.Red, "btn0Upgrade", true));
            miscBuild[0].TextBoxLocation = new Vector2(buttonDrawPos.X, buttonDrawPos.Y + (0 * (buttonSize.Y + 10)));
            miscBuild[0].StringText = "Upgrade Tower";
            miscBuild[0].SetStringPos();
            tabs.Add(miscBuild[0], 2);
            miscBuild[0].TextBoxRectangleSet();
        }


    }
}
