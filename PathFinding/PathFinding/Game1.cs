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

namespace PathFinding
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public const int UPS = 20; // Updates per second
        public const int FPS = 60;

        public Vector2 ScreenSize;
        public static Art art;


        const int SQUARESIZE = 25;
        const int HEIGHT = 25;
        const int WIDTH = 25;
        const int DEFAULYDIST = 2000;
        Coordinates aiStart = new Coordinates(2, 2);

        Random rnd;
        SpriteBatch spriteBatch;
        Grid grid;
       
        Texture2D squareTex, squareMove, trenchTex;
        MouseState mouse;
        KeyboardState keyboard, old;
        Rectangle mouseRect;
        SpriteFont debug;
        ai Ai;

        bool pathFound;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / FPS);

            ScreenSize = new Vector2(WIDTH, HEIGHT) * SQUARESIZE;

            graphics.PreferredBackBufferWidth = (int)ScreenSize.X;
            graphics.PreferredBackBufferHeight = (int)ScreenSize.Y;

        }

 
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            pathFound = false;

            rnd = new Random();

            base.Initialize();



        }

  
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            debug = Content.Load<SpriteFont>("debug");
            squareTex = Content.Load<Texture2D>("dirt");
            squareMove = Content.Load<Texture2D>("square");
            grid = new Grid(SQUARESIZE, HEIGHT, WIDTH, squareTex, trenchTex, DEFAULYDIST);
            Ai = new ai(HEIGHT, WIDTH, aiStart, DEFAULYDIST);
            art = new Art();
            art.Load(Content);

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

      
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            mouse = Mouse.GetState();
            keyboard = Keyboard.GetState();
            mouseRect = new Rectangle(mouse.X, mouse.Y, 1, 1);

            grid.Update(mouseRect, mouse, Ai, gameTime);

            if(grid.gridStatus.HasFlag(Grid.gridFlags.endPoint))
            {
                if (!pathFound )
                    pathFound = Ai.FindPath(grid.stopPointCoord, grid.gridSquares, HEIGHT, WIDTH);
            }

            if (pathFound)
                Ai.Update(grid.stopPointCoord, grid.gridSquares, HEIGHT, WIDTH);

            if (keyboard.IsKeyDown(Keys.G) && old != keyboard)
            {
                grid.GenerateNewMap(Ai, rnd);
            }

            old = keyboard;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            grid.Draw(spriteBatch, debug);
            spriteBatch.DrawString(debug, "fps: " + (1 / (float)gameTime.ElapsedGameTime.TotalSeconds).ToString("N0"), Vector2.Zero, Color.Red);
            


            spriteBatch.End();
           
            base.Draw(gameTime);
        }
    }
}
