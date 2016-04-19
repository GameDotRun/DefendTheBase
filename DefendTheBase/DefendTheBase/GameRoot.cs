using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Flextensions;
using RPGEx;

namespace DefendTheBase
{
    public class GameRoot : Microsoft.Xna.Framework.Game
    {
        // Game Variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Grid Size
        public const int SQUARESIZE = 50;
        public const int HEIGHT = 15;
        public const int WIDTH = 20;

       
        public const int DEFAULYDIST = 2000; //temp default counter for pathfinding
        public static Coordinates STARTPOINT = new Coordinates(0,0);
        public static Coordinates ENDPOINT = new Coordinates(18, 13, 0);
              
        //ui Borders
        public const int BORDERTOP = 125;
        public const int BORDERRIGHT = 250;
        public const int BORDERLEFT = 0;

        //game speed
        public const int UPS = 20; // Updates per second
        public const int FPS = 60; //Frames per second

        public static Grid grid;
        public static Random rnd;

        public static Vector2 ScreenSize; // ScreenSize

        UiSideGameScreen UiSideScreen;
        UiTopGameScreen UiTopScreen;
        // Constructor
        public GameRoot()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / FPS);

            ScreenSize = new Vector2(WIDTH, HEIGHT) * SQUARESIZE;
            graphics.PreferredBackBufferWidth = (int)ScreenSize.X + BORDERRIGHT + BORDERLEFT;
            graphics.PreferredBackBufferHeight = (int)ScreenSize.Y + BORDERTOP;
        }

        // Init
        protected override void Initialize()
        {
            rnd = new Random();

            base.Initialize();
        }

        // Load
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Load Images and Fonts from disk.
            Art.Load(Content);
            // Set up variables.
            ResetGame();
        }

        // Reset
        public void ResetGame()
        {
            // Reset Variables, or Set if first run.
            UiButtonMessenger.InitiliseListenerList();
            EnemyListener.InitiliseListener();
            TowerListener.InitiliseListener();
            grid = new Grid(SQUARESIZE, DEFAULYDIST);
            UiSideScreen = new UiSideGameScreen(GraphicsDevice);
            UiTopScreen = new UiTopGameScreen(GraphicsDevice);
            GameManager.ResetValues();
        }

        // Update
        protected override void Update(GameTime gameTime)
        {
            Input.Update();
            // The above must be called for Input data to update per frame, this allows us to instead of:
            // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            // we can use:
            if (Input.IsButtonDown(Buttons.Back) || Input.WasKeyPressed(Keys.Escape))
                this.Exit();

            UiButtonMessenger.ButtonResponder(Input.GetMouseState, Input.GetMouseStateOld);
            UiSideScreen.Update();
            UiTopScreen.Update();
            WaveManager.Update(gameTime);
            TowerManager.Update();
            EffectManager.Update(gameTime);

            // Using the last button pressed ID, as long as it exists,
            // see if it is a "btn0" and then set the BuildState using the rest of the ID.
            if (UiButtonMessenger.ButtonPressedId != null)
            {
                if (UiButtonMessenger.ButtonPressedId.Contains("btn0"))
                {
                    // Create String from id by removing the "btn0". Then Parse String to enum.
                    string bStateString = UiButtonMessenger.ButtonPressedId.Substring(4);
                    GameManager.BuildState = (GameManager.BuildStates)Enum.Parse(typeof(GameManager.BuildStates), bStateString);
                }

                else if (UiButtonMessenger.ButtonPressedId.Contains("btn1"))
                {
                    // Create String from id by removing the "btn0". Then Parse String to enum.
                    string bStateString = UiButtonMessenger.ButtonPressedId.Substring(4);
                    WaveManager.StartWave();
                }
            }

            grid.Update(gameTime);

            /*if(grid.gridStatus.HasFlag(Grid.gridFlags.endPoint)) //CREATE A WAVE COUNT DOWN
                WaveManager.WaveStarted = true;*/

            for (int y = 0; y < HEIGHT; y++) // get if a square has been edited 
                for (int x = 0; x < WIDTH; x++)
                {
                    if (grid.gridSquares[x, y].getSquareEdited)
                    {
                            EnemyManager.ResetEnemyAI();
                            grid.pathFound = false;
                            grid.pathFound = GridManager.GridPaths(grid.gridSquares);

                    }

                }

            // Wipe grid when BackSpace is pressed. REMOVE LATER
            if (Input.WasKeyPressed(Keys.Back))
            {
                grid.resetGrid();
                //tanks.pathFound = false;
            }

            base.Update(gameTime);
        }

        // Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkOliveGreen);
            spriteBatch.Begin();

            grid.Draw(spriteBatch, Art.DebugFont);
            UiManager.Draw(spriteBatch);
            EffectManager.Draw(spriteBatch, 0);
            EnemyManager.Draw(spriteBatch);
            TowerManager.Draw(spriteBatch);
            EffectManager.Draw(spriteBatch, 1);
            
#if DEBUG
            // Draw debug text. Shadow on offset, then white text on top for visibility.
            for (int i = 0; i < 2; i++)
            {
                spriteBatch.DrawString(Art.DebugFont,
                    "DEBUG" +
                    "\nFPS: " + (1 / (float)gameTime.ElapsedGameTime.TotalSeconds).ToString("N0") +
                    "\nBuild: " + GameManager.BuildState +
                    "\nEnemySpawn: " + WaveManager.EnemySpawnTimer,
                    i < 1 ? Vector2.One : Vector2.Zero,     // if (i<1) {Vec.One} else {Vec.Zero}
                    i < 1 ? Color.Black : Color.White);     // if (i<1) {C.Black} else {C.White}
            }

            //spriteBatch.DrawString(Art.DebugFont, tanks.ScreenPos.X + " " + tanks.ScreenPos.Y, tanks.ScreenPos, Color.Black);

#endif
            // Finish spriteBatch.
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}