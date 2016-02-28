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

namespace DefendTheBase
{
    public class GameRoot : Microsoft.Xna.Framework.Game
    {
        // Game Variables
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public static gamestate GameState;

           
        // PATHFINDING CODE
        //Grid Size
        public const int SQUARESIZE = 40;
        public const int HEIGHT = 15;
        public const int WIDTH = 25;

        public const int DEFAULYDIST = 2000; //temp default counter for pathfinding

        //ui Borders
        public const int BORDERTOP = 60;
        public const int BORDERRIGHT = 175;
        public const int BORDERLEFT = 0;

        //game speed
        public const int UPS = 20; // Updates per second
        public const int FPS = 60; //Frames per second

        //public static Art art;
        public static Grid grid;

        //Game states
        public enum gamestate
        {
            StartScreen,
            PlayScreen,
            EndScreen,
            Highscore,
            InfoScreen,
        }




        Vector2 ScreenSize; // ScreenSize
        Random rnd;

        Rectangle mouseRect;

        Enemy enemy;

        UI ui;


        // Constructor
        public GameRoot()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            // PATHFINDING CODE
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
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load Images and Fonts from disk.
            Art.Load(Content);

            // PATHFINDING CODE
            enemy = new Enemy();
            grid = new Grid(SQUARESIZE, HEIGHT, WIDTH, DEFAULYDIST);

            // Set up variables.
            ResetGame();

            //UI
            ui = new UI(Art.uiUp, 0, 0);
            
        }

        // Reset
        public void ResetGame()
        {
            // Reset Variables, or Set if first run.
        }

        // Update
        protected override void Update(GameTime gameTime)
        {
            Input.Update();
            // The above must be called for Input data to update per frame, this allows us to instead of:
            // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            // we can use:
            if (Input.IsButtonDown(Buttons.Back))
                this.Exit();
            if (Input.WasKeyPressed(Keys.Escape))
                this.Exit();

            //UI update
            ui.Update();

            // PATHFINDING CODE
            mouseRect = new Rectangle((int)Input.MousePosition.X, (int)Input.MousePosition.Y, 1, 1);

            grid.Update(mouseRect, gameTime);

            enemy.Update(grid.gridStatus);

            for (int y = 0; y < HEIGHT; y++) //Debug counter Text
                for (int x = 0; x < WIDTH; x++)
                {
                    if (Input.WasLMBClicked || Input.WasRMBClicked && grid.gridSquares[x, y].getSquareEdited)
                    {
                        enemy.pathFound = false;     
                    }

                    if (Input.WasKeyPressed(Keys.G))
                    {
                        enemy.pathFound = false;
                    }
                }


          

            if (Input.WasKeyPressed(Keys.G))
            {
                grid.GenerateNewMap(rnd);
            }

            base.Update(gameTime);
        }

        // Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            // Begin our spriteBatch.
            spriteBatch.Begin();

            

            // PATHFINDING CODE
            
            grid.Draw(spriteBatch, Art.DebugFont);
            enemy.Draw(spriteBatch);

            //draw UI
            ui.Draw(spriteBatch);
#if DEBUG
            // Draw debug text. Shadow on offset, then white text on top for visibility.
            for (int i = 0; i < 2; i++)
            {
                spriteBatch.DrawString(Art.DebugFont,
                    "DEBUG" +
                    "\nFPS: " + (1 / (float)gameTime.ElapsedGameTime.TotalSeconds).ToString("N0") +
                    "\n",
                    i < 1 ? Vector2.One : Vector2.Zero,     // if (i<1) {Vec.One} else {Vec.Zero}
                    i < 1 ? Color.Black : Color.White);     // if (i<1) {C.Black} else {C.White}
            }

            // Aaron what were you doing with this? :P
            spriteBatch.DrawString(Art.DebugFont, enemy.enemyPos.x + " " + enemy.enemyPos.y, enemy.enemyVect, Color.Black);

#endif

            // Finish spriteBatch.
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}