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
        Vector2 buttonDrawPos = new Vector2(1025, 100);
        Vector2 buttonSize = new Vector2(200, 100);

        UiTabs tabs;
        UiButton[] unitBuild;
        UiButton[] baseBuild;

        public UiGameScreen(GraphicsDevice graphicsDevice) : base(GameRoot.WIDTH, GameRoot.HEIGHT)
        {
            tabs = new UiTabs(graphicsDevice, Art.DebugFont, 3, tabDrawPos, new string[3] { "Towers", "Base", "Misc" }, Color.Aquamarine, new Vector2(83, 20));
            unitBuild = new UiButton[6];
            baseBuild = new UiButton[3];

            CreateButtons(graphicsDevice);

        }

        public void Update()
        {
            tabs.Update();
        }


        public void Draw(SpriteBatch sb)
        {
            tabs.Draw(sb);
        }

        public void CreateButtons(GraphicsDevice graphicsDevice)
        {
            //its of UTMOST IMPORTANCE that each button has a unique id

            for (int i = 0; i < unitBuild.Count(); i++)
            {
                unitBuild[i] = new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Color.Red, "button" + i.ToString(), true);
                unitBuild[i].TextBoxLocation = new Vector2(buttonDrawPos.X, buttonDrawPos.Y + (i * (buttonSize.Y + 10)));
                unitBuild[i].StringText = "TOWERS PLACEHOLDER " + i.ToString();
                unitBuild[i].SetStringPos();
                tabs.Add(ref unitBuild[i], 0);
                unitBuild[i].TextBoxRectangleSet();

                
            }

            for (int i = 0; i < baseBuild.Count(); i++)
            {
                baseBuild[i] = new UiButton(graphicsDevice, Art.DebugFont, Vector2.Zero, buttonSize, Color.Red, "button0" + i.ToString(), true);
                baseBuild[i].TextBoxLocation = new Vector2(buttonDrawPos.X, buttonDrawPos.Y + (i * (buttonSize.Y + 10)));
                baseBuild[i].StringText = "BUILD PLACEHOLDER " + i.ToString();
                baseBuild[i].SetStringPos();
                tabs.Add(ref baseBuild[i], 1);
                baseBuild[i].TextBoxRectangleSet();
            }
        }


    }
}
