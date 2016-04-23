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

        // Constructor
        public GameRoot()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / GameManager.FPS);

            GameManager.ScreenSize = new Vector2(GameManager.WIDTH, GameManager.HEIGHT) * GameManager.SQUARESIZE;
            graphics.PreferredBackBufferWidth = (int)GameManager.ScreenSize.X + GameManager.BORDERRIGHT + GameManager.BORDERLEFT;
            graphics.PreferredBackBufferHeight = (int)GameManager.ScreenSize.Y + GameManager.BORDERTOP;
        }

        // Init
        protected override void Initialize()
        {
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
            GameManager.GameState = GameManager.GameStates.StartScreen;
        }

        // Reset
        public void ResetGame()
        {
            // Reset Variables, or Set if first run.
            UiButtonMessenger.InitiliseListenerList();
            GameManager.grid = new Grid(GameManager.SQUARESIZE, GameManager.DEFAULYDIST);
            GameManager.Init(GraphicsDevice);
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

            if (GameManager.GameState == GameManager.GameStates.StartScreen)
            {
                if (Input.LMBDown)
                    GameManager.GameState = GameManager.GameStates.PlayScreen;
            }

            if (GameManager.GameState == GameManager.GameStates.PlayScreen)
            {
                GameManager.Update(gameTime);
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

                GameManager.grid.Update(gameTime);

                for (int y = 0; y < GameManager.HEIGHT; y++) // get if a square has been edited 
                    for (int x = 0; x < GameManager.WIDTH; x++)
                    {
                        if (GameManager.grid.gridSquares[x, y].getSquareEdited)
                        {
                            EnemyManager.ResetEnemyAI();
                            GameManager.grid.pathFound = false;
                            GameManager.grid.pathFound = GridManager.GridPaths(GameManager.grid.gridSquares);
                        }
                    }
                base.Update(gameTime);
            }
        }

        // Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkOliveGreen);
            spriteBatch.Begin();
            spriteBatch.Draw(Art.Background, Vector2.Zero, Color.DarkGray);
            if (GameManager.GameState == GameManager.GameStates.PlayScreen)
            {
                GameManager.grid.Draw(spriteBatch, Art.DebugFont);
                UiManager.Draw(spriteBatch);
                GameManager.Draw(spriteBatch);
                
            }
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