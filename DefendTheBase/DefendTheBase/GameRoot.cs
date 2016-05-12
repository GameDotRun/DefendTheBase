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

        StartScreen startScreen;
        EndScreen endScreen;
        InfoScreen infoScreen;

        public static bool resetgame = false;

        public static bool exit = false;

        public static TimeSpan targetTime;

        public static bool SpeedUp = false;
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
            IsFixedTimeStep = true;
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
            GameManager.GameState = GameManager.GameStates.StartVideo;

            startScreen = new StartScreen();
            endScreen = new EndScreen();
            infoScreen = new InfoScreen(GraphicsDevice);
        }

        // Reset
        public void ResetGame()
        {
            // Reset Variables, or Set if first run.
            GameManager.ResetValues();
            UiButtonMessenger.InitiliseListenerList();
            GameManager.grid = new Grid(GameManager.SQUARESIZE, GameManager.DEFAULYDIST);
            GameManager.Init(GraphicsDevice);
           
        }

        // Update
        protected override void Update(GameTime gameTime)
        {
            targetTime = this.TargetElapsedTime;

            if (Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.NumPad0))
            {
                this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 180.0f);
                SpeedUp = true;
            }
            else
            {
                SpeedUp = false;
                this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60.0f);

            }
            

            Input.Update();
            // The above must be called for Input data to update per frame, this allows us to instead of:
            // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            // we can use:
            if (Input.IsButtonDown(Buttons.Back) || Input.WasKeyPressed(Keys.Escape) || exit)
                this.Exit();

            

            UiButtonMessenger.ButtonResponder(Input.GetMouseState, Input.GetMouseStateOld);

            if (GameManager.GameState == GameManager.GameStates.StartScreen || startScreen.fadeout)
            {
                startScreen.Update(gameTime);
            }

            if (GameManager.GameState == GameManager.GameStates.InfoScreen)
            {
                infoScreen.Update();
            }

            if (GameManager.GameState == GameManager.GameStates.LoseScreen || endScreen.fadeout)
            {
                endScreen.Update(gameTime);
            }

            if (resetgame)
            {
                ResetGame();
                resetgame = false;
            }

            if (GameManager.GameState == GameManager.GameStates.PlayScreen || endScreen.fadein)
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
                    else if (UiButtonMessenger.ButtonPressedId.Contains("btn1") && !QuestionPopUpManager.QuestionUp && !MessageBoxManager.MessageDisplayed)
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

            if (GameManager.GameState == GameManager.GameStates.StartVideo)
            {
                if (GameManager.FIRSTRUN)
                {
                    GameManager.videoPlayer.Play(Art.StartVideo);
                    GameManager.FIRSTRUN = false;
                }
                if (GameManager.videoPlayer.State == MediaState.Stopped || Input.LMBDown)
                {
                    GameManager.GameState = GameManager.GameStates.StartScreen;
                    GameManager.videoPlayer.Stop();
                    PlayMusic();
                }
            }

            if (GameManager.GameState == GameManager.GameStates.TutScreen)
            {
                GameManager.videoPlayer.Play(Art.TutVideo);
                if (GameManager.videoPlayer.State == MediaState.Stopped || Input.WasLMBClicked)
                {
                    GameManager.GameState = GameManager.GameStates.StartScreen;
                    GameManager.videoPlayer.Stop();
                }
            }
        }

        private void PlayMusic()
        {
            if (MediaPlayer.State != MediaState.Playing)
            {
                // Play music
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = GameManager.MASTER_VOL * GameManager.MUSIC_VOL;
                MediaPlayer.Play(Sound.Music);
            }
        }

        // Draw
        protected override void Draw(GameTime gameTime)
        {
            graphics.SynchronizeWithVerticalRetrace = false;

            GraphicsDevice.Clear(Color.DarkOliveGreen);
            spriteBatch.Begin();

            if (GameManager.GameState == GameManager.GameStates.PlayScreen || endScreen.fadein)
            {
                spriteBatch.Draw(Art.Background, Vector2.Zero, Color.DarkGray);
                GameManager.grid.Draw(spriteBatch, Art.DebugFont);
                UiManager.Draw(spriteBatch);
                GameManager.Draw(spriteBatch);
                
            }

            if (GameManager.GameState == GameManager.GameStates.InfoScreen)
            {
                infoScreen.Draw(spriteBatch);
            }

            if (GameManager.GameState == GameManager.GameStates.StartVideo || GameManager.GameState == GameManager.GameStates.TutScreen)
            {
                if (GameManager.videoPlayer.State != MediaState.Stopped)
                {
                    Texture2D texture = GameManager.videoPlayer.GetTexture();
                    if (texture != null)
                    {
                        spriteBatch.Draw(texture, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight),
                            Color.White);
                    }
                }
            }

            if (GameManager.GameState == GameManager.GameStates.StartScreen || startScreen.fadeout)
                startScreen.Draw(spriteBatch);

            if (GameManager.GameState == GameManager.GameStates.LoseScreen || endScreen.fadeout)
                endScreen.Draw(spriteBatch);
                

#if DEBUG
            // Draw debug text. Shadow on offset, then white text on top for visibility.

            /*if (!float.IsInfinity(1 / (float)gameTime.ElapsedGameTime.TotalSeconds))
            {
                for (int i = 0; i < 2; i++)
                {
                    spriteBatch.DrawString(Art.DebugFont,
                        "DEBUG" +
                        "\nFPS: " + (1 / (float)gameTime.ElapsedGameTime.TotalSeconds).ToString("") +
                        "\nBuild: " + GameManager.BuildState +
                        "\nEnemySpawn: " + WaveManager.EnemySpawnTimer,
                        i < 1 ? Vector2.One : Vector2.Zero,     // if (i<1) {Vec.One} else {Vec.Zero}
                        i < 1 ? Color.Black : Color.White);     // if (i<1) {C.Black} else {C.White}
                }
            }

            //spriteBatch.DrawString(Art.DebugFont, tanks.ScreenPos.X + " " + tanks.ScreenPos.Y, tanks.ScreenPos, Color.Black);*/

#endif
 

            // Finish spriteBatch.
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}