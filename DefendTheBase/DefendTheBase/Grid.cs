using Flextensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefendTheBase
{
    public class Grid
    {
        [Flags]
        public enum gridFlags
        {
            empty = 0,
            endPoint = 1
        }

        private TimeSpan updateTimer;

        public gridFlags gridStatus;
        public Squares[,] gridSquares;
        public Coordinates stopPointCoord;
        public Vector2 gridBorder;

        int height, width;

        public Grid(int SquareSize, int Height, int Width, int defDist)
        {
            height = Height;
            width = Width;

            gridSquares = new Squares[Width, Height];

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    gridSquares[x, y] = new Squares(SquareSize, new Vector2(x * SquareSize + GameRoot.BORDERLEFT, y * SquareSize + GameRoot.BORDERTOP), x, y, defDist);

            gridBorder = new Vector2(gridSquares[0, 0].sqrLoc.X, gridSquares[0, 0].sqrLoc.Y);

            gridStatus = gridFlags.empty;

            updateTimer = TimeSpan.Zero;

        }

        public void Update(Rectangle mouseRect, GameTime gameTime)
        {
            foreach (Squares square in gridSquares)
                square.Update();

            updateTimer += gameTime.ElapsedGameTime;

            if (updateTimer.TotalMilliseconds > 1000f / GameRoot.UPS)
            {
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                    {
                        if (mouseRect.Intersects(gridSquares[x, y].rect) && Input.RMBDown && !gridStatus.HasFlag(gridFlags.endPoint)) // temporary.
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
                square.Draw(sb, Art.GroundTexs);


            for (int y = 0; y < height; y++) //Debug counter Text
                for (int x = 0; x < width; x++)
                {
                    if (gridSquares[x, y].sqrCoord.counter < 2000)
                        sb.DrawString(deb, gridSquares[x, y].sqrCoord.counter.ToString(), new Vector2(gridSquares[x, y].rect.X + 10, gridSquares[x, y].rect.Y), Color.Black);
                }
        }

        // temporary maze generation, maybe map generation in the future? DEM IFS THO
        public void GenerateNewMap(Random rnd)
        {
            int disparity = 10; // how populated the maze is, lower = less max = 12
            int counter = 12;
            int sideCount = 2;
            resetGrid();

            for (int y = 0; y < height; y++)
                for (int x = 1; x < width; x++)
                {
                    if (!gridSquares[x, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
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

        void sqrTexDecider(int x, int y)
        {
            gridSquares[x, y].TrenchName = "Trench_";
            if (gridSquares[x, y - 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                gridSquares[x, y].TrenchName += "N";

            if (gridSquares[x + 1, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                gridSquares[x, y].TrenchName += "E";

            if (gridSquares[x, y + 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                gridSquares[x, y].TrenchName += "S";

            if (gridSquares[x - 1, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                gridSquares[x, y].TrenchName += "W";
        }

        void resetGrid()
        {
            foreach (Squares square in gridSquares)
            {
                square.typeOfSquare &= ~Squares.SqrFlags.Wall;
                square.typeOfSquare |= Squares.SqrFlags.Unoccupied;
            }

        }
    }
}
