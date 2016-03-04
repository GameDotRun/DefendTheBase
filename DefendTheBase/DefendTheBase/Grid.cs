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

            GenerateNewMap();

        }

        public void Update(Rectangle mouseRect, GameTime gameTime)
        {
            foreach (Squares square in gridSquares)
                square.Update();

            // In case we delete everything, always have one bit of Trench.
            if (gridSquares[width - 1, height - 1].Building != Squares.BuildingType.Trench)
            {
                gridSquares[width - 1, height - 1].typeOfSquare |= Squares.SqrFlags.Wall;
                gridSquares[width - 1, height - 1].Building = Squares.BuildingType.Trench;
            }

            updateTimer += gameTime.ElapsedGameTime;

            if (updateTimer.TotalMilliseconds > 1000f / GameRoot.UPS)
            {
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                    {
                        if (mouseRect.Intersects(gridSquares[x, y].rect) && Input.RMBDown && Input.IsKeyDown(Keys.LeftShift) && !gridStatus.HasFlag(gridFlags.endPoint)) // temporary.
                        {
                            gridStatus = gridFlags.endPoint;
                            stopPointCoord = new Coordinates(x, y, 0);
                            gridSquares[x, y].typeOfSquare |= Squares.SqrFlags.StopPoint;
                            gridSquares[x, y].Building = Squares.BuildingType.Base;

                        }
                    }

                for (int y = 0; y < height - 0; y++)
                    for (int x = 0; x < width - 0; x++)
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

        // Generate a Starter Map. Couple of trenches and base in the bottom right.
        public void GenerateNewMap()
        {
            // manually set the coords of base and a few trenches to begin with.
            resetGrid();
            // X X X    Create 3 Trenches in the bot-right corner,
            // X X 3    in the order the numbers are shown,
            // X 1 2    to the left here.
            gridSquares[width - 2, height - 1].typeOfSquare |= Squares.SqrFlags.Wall;   // 1
            gridSquares[width - 2, height - 1].Building = Squares.BuildingType.Trench;  // 1
            gridSquares[width - 1, height - 1].typeOfSquare |= Squares.SqrFlags.Wall;   // 2
            gridSquares[width - 1, height - 1].Building = Squares.BuildingType.Trench;  // 2
            gridSquares[width - 1, height - 2].typeOfSquare |= Squares.SqrFlags.Wall;   // 3
            gridSquares[width - 1, height - 2].Building = Squares.BuildingType.Trench;  // 3        
        }

        void sqrTexDecider(int x, int y)
        {
            // Create a name to select a texture based on the present neighbours. For example:
            // Result will be "Trench_NEW" if it has neighbours to the North, East and West.
            gridSquares[x, y].TrenchName = "Trench_";

            if (y > 0)
                if (gridSquares[x, y - 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    gridSquares[x, y].TrenchName += "N";

            if (x < GameRoot.WIDTH - 1)
                if (gridSquares[x + 1, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    gridSquares[x, y].TrenchName += "E";

            if (y < GameRoot.HEIGHT - 1)
                if (gridSquares[x, y + 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    gridSquares[x, y].TrenchName += "S";

            if (x > 0)
                if (gridSquares[x - 1, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    gridSquares[x, y].TrenchName += "W";
        }

        public void resetGrid()
        {
            foreach (Squares square in gridSquares)
            {
                square.typeOfSquare &= ~Squares.SqrFlags.Wall;
                square.typeOfSquare |= Squares.SqrFlags.Unoccupied;
                square.Building = Squares.BuildingType.None;
            }

        }
    }
}
