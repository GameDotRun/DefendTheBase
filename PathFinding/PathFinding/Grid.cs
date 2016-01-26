using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PathFinding
{
    class Grid
    {
        [Flags]
        public enum gridFlags
        { 
            empty = 0,
            endPoint = 1

        }

        public gridFlags gridStatus;
        public Squares[,] gridSquares;
        int height, width;
        public Coordinates stopPointCoord;
        Texture2D gridSquareTex, trenchTex;

        private TimeSpan updateTimer;

        public Grid(int SquareSize, int Height, int Width, Texture2D squareTex, Texture2D TrenchTex, int defDist)
        {
            trenchTex = TrenchTex;
            gridSquareTex = squareTex;
            height = Height;
            width = Width;

            gridSquares = new Squares[Width, Height];

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    gridSquares[x, y] = new Squares(SquareSize, new Vector2(x * SquareSize, y * SquareSize), x, y, defDist);

            gridStatus = gridFlags.empty;

            updateTimer = TimeSpan.Zero;

        }

        public void Update(Rectangle mouseRect, MouseState mouseState, ai Ai, GameTime gameTime)
        {


            foreach (Squares square in gridSquares)
                square.Update(mouseState);

            updateTimer += gameTime.ElapsedGameTime;

            if (updateTimer.TotalMilliseconds > 1000f / Game1.UPS)
            {
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                    {
                        if (Ai.aiPos.x == gridSquares[x, y].sqrCoord.x && Ai.aiPos.y == gridSquares[x, y].sqrCoord.y)
                            gridSquares[x, y].typeOfSquare = Squares.SqrFlags.Occupied;
                        else gridSquares[x, y].typeOfSquare |= Squares.SqrFlags.Unoccupied;

                        if (mouseRect.Intersects(gridSquares[x, y].rect) && mouseState.RightButton == ButtonState.Pressed && !gridStatus.HasFlag(gridFlags.endPoint))
                        {
                            gridStatus = gridFlags.endPoint;
                            stopPointCoord = new Coordinates(x, y, 0);
                            gridSquares[x, y].typeOfSquare = Squares.SqrFlags.StopPoint;

                        }
                    }
            }
        }

        public void Draw(SpriteBatch sb, SpriteFont deb)
        {
            foreach (Squares square in gridSquares)
                square.Draw(sb, gridSquareTex, trenchTex);


            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    sb.DrawString(deb, gridSquares[x, y].sqrCoord.counter.ToString(), new Vector2(gridSquares[x, y].rect.X + 10, gridSquares[x, y].rect.Y), Color.Black);
                }
        }
    }

   


}
