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
        public Coordinates stopPointCoord;
        Texture2D gridSquareTex, trenchTex;
        int height, width;

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

                for (int y = 1; y < height - 1; y++)
                    for (int x = 1; x < width - 1; x++)
                    {
                        if (gridSquares[x, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                            sqrTexDecider(x, y);
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
                    if (gridSquares[x, y].sqrCoord.counter < 2000)
                        sb.DrawString(deb, gridSquares[x, y].sqrCoord.counter.ToString(), new Vector2(gridSquares[x, y].rect.X + 10, gridSquares[x, y].rect.Y), Color.Black);
                }
        }

        // temporary maze generation, maybe map generation in the future? DEM IFS THO
        public void GenerateNewMap(ai ai, Random rnd)
        {
            int disparity = 10; // how populated the maze is, lower = less max = 12
            int counter = 12;
            int sideCount = 2;
            resetGrid();

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    if (!gridSquares[x, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    {
                        if ((gridSquares[x, y].sqrCoord.x != ai.aiPos.x && gridSquares[x, y].sqrCoord.y != ai.aiPos.y))
                        {
                            if (rnd.Next(0, counter) < disparity)
                            {
                                if (x != 0 && x != width - 1 && y != 0 && y != height - 1)
                                {
                                    if (!gridSquares[x + 1, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall) && !gridSquares[x - 1, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall) && !gridSquares[x, y + 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall) && !gridSquares[x, y - 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                                    {
                                        if (!gridSquares[x + 1, y - 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall) && !gridSquares[x - 1, y - 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall) && !gridSquares[x - 1, y + 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall) && !gridSquares[x + 1, y + 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                                        {
                                            for (int i = 0; i < rnd.Next(1, width - x); i++)
                                            {
                                                try
                                                {
                                                    if (!gridSquares[x + i + 1, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                                                    {
                                                        gridSquares[x + i, y].typeOfSquare = Squares.SqrFlags.Wall;
                                                    }
                                                    else break;
                                                }
                                                catch
                                                {
                                                    gridSquares[x + i, y].typeOfSquare = Squares.SqrFlags.Wall;
                                                }
                                            }
                                            for (int i = 0; i < rnd.Next(1, height - y); i++)
                                            {
                                                try
                                                {
                                                    if (!gridSquares[x, y + i + 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                                                        gridSquares[x, y + i].typeOfSquare = Squares.SqrFlags.Wall;
                                                    else break;
                                                }

                                                catch
                                                {
                                                    gridSquares[x, y + i].typeOfSquare = Squares.SqrFlags.Wall;
                                                }
                                            }
                                        }

                                    }
                                }

                                else
                                {
                                    if (rnd.Next(1, 50) > sideCount)
                                    {
                                        if (x == 0)
                                            gridSquares[rnd.Next(1, width), y].typeOfSquare = Squares.SqrFlags.Wall;
                                        if (y == 0)
                                            gridSquares[x, rnd.Next(1, height)].typeOfSquare = Squares.SqrFlags.Wall;

                                        sideCount += 2;
                                    }
                                
                                }

                            }
                        }
                    }


                }
        
        }

        void sqrTexDecider(int x, int y)
        {

                if (gridSquares[x + 1, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    gridSquares[x, y].texEnum = Art.TrenchEnum.ELeft;

                if (gridSquares[x - 1, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    gridSquares[x, y].texEnum = Art.TrenchEnum.ERight;

                if (gridSquares[x, y + 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    gridSquares[x, y].texEnum = Art.TrenchEnum.EUp;

                if (gridSquares[x, y - 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    gridSquares[x, y].texEnum = Art.TrenchEnum.EDown;

                if (gridSquares[x + 1, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall) && gridSquares[x - 1, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    gridSquares[x, y].texEnum = Art.TrenchEnum.Horizontal;

                if (gridSquares[x, y + 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall) && gridSquares[x, y - 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    gridSquares[x, y].texEnum = Art.TrenchEnum.Vertical;

                if (gridSquares[x + 1, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall) && gridSquares[x, y + 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    gridSquares[x, y].texEnum = Art.TrenchEnum.TRight;

                if (gridSquares[x + 1, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall) && gridSquares[x, y - 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    gridSquares[x, y].texEnum = Art.TrenchEnum.TRight;

                if (gridSquares[x - 1, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall) && gridSquares[x, y + 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    gridSquares[x, y].texEnum = Art.TrenchEnum.TLeft;

                if (gridSquares[x - 1, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall) && gridSquares[x, y - 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    gridSquares[x, y].texEnum = Art.TrenchEnum.TLeft;

                if (gridSquares[x, y + 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall) && gridSquares[x, y - 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall) && gridSquares[x + 1, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    gridSquares[x, y].texEnum = Art.TrenchEnum.TRight;
   
                if (gridSquares[x, y + 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall) && gridSquares[x, y - 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall) && gridSquares[x - 1, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    gridSquares[x, y].texEnum = Art.TrenchEnum.TLeft;

                if (gridSquares[x + 1, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall) && gridSquares[x - 1, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall) && gridSquares[x, y + 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    gridSquares[x, y].texEnum = Art.TrenchEnum.TDown;

                if (gridSquares[x + 1, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall) && gridSquares[x - 1, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall) && gridSquares[x, y - 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    gridSquares[x, y].texEnum = Art.TrenchEnum.TUp;

                if (gridSquares[x + 1, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall) && gridSquares[x - 1, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall) && gridSquares[x, y - 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall) && gridSquares[x, y + 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    gridSquares[x, y].texEnum = Art.TrenchEnum.trenchX;
        }

        void resetGrid()
        { 
            foreach (Squares square in gridSquares)
                square.typeOfSquare &= ~Squares.SqrFlags.Wall;
     
        }

    }

   


}
