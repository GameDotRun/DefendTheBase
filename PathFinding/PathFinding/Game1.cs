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

        //Grid Size
        public const int SQUARESIZE = 25;
        public const int HEIGHT = 25;
        public const int WIDTH = 30;
        
        public const int DEFAULYDIST = 2000; //temp default counter for pathfinding

        //ui Borders
        public const int BORDERTOP = 60;
        public const int BORDERRIGHT = 175;
        public const int BORDERLEFT = 0;

        //game speed
        public const int UPS = 20; // Updates per second
        public const int FPS = 60; //Frames per second

        public static Art art;
        public static Grid grid;

        Vector2 ScreenSize; // ScreenSize
        Coordinates aiStart = new Coordinates(2, 2); // temporary, Prototype pathfinding leftover
        Random rnd;
        SpriteBatch spriteBatch;
       
        Rectangle mouseRect;
        SpriteFont debug;
        MouseState mouse;
        KeyboardState keyboard, old;

        Enemy enemy;

        bool pathFound;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / FPS);

            ScreenSize = new Vector2(WIDTH, HEIGHT) * SQUARESIZE;

            graphics.PreferredBackBufferWidth = (int)ScreenSize.X + BORDERRIGHT + BORDERLEFT;
            graphics.PreferredBackBufferHeight = (int)ScreenSize.Y + BORDERTOP;

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

            art = new Art();
            art.Load(Content);

            enemy = new Enemy();
            grid = new Grid(SQUARESIZE, HEIGHT, WIDTH, DEFAULYDIST);

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

            grid.Update(mouseRect, mouse, gameTime);

            enemy.Update(grid.gridStatus);

            if (keyboard.IsKeyDown(Keys.G) && old != keyboard)
            {
                grid.GenerateNewMap(rnd);
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
            enemy.Draw(spriteBatch);
            spriteBatch.End();
           
            base.Draw(gameTime);
        }
    }
}
