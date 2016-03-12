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
    public static class GridManager
    {
        public static bool GridPaths(Squares[,] Grid)
        {
            GridReset(Grid);

            bool loopStop = true;
            bool done = false;
            int count = 0;

            List<Coordinates> coords;
            List<Coordinates> tempCoords;
            Coordinates currentElement;

            coords = new List<Coordinates>();
            tempCoords = new List<Coordinates>();
            currentElement = new Coordinates(0,0);

            if (!done)
            {
                coords.Add(GameRoot.ENDPOINT);
                currentElement = coords[count];
                Grid[GameRoot.ENDPOINT.x, GameRoot.ENDPOINT.y].sqrCoord.counter = GameRoot.ENDPOINT.counter;
                done = true;
            }

            while (loopStop)
            {

                //Check right square
                if (currentElement.x + 1 < GameRoot.WIDTH)
                    if (!Grid[currentElement.x + 1, currentElement.y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                        tempCoords.Add(new Coordinates(currentElement.x + 1, currentElement.y, currentElement.counter + 1));

                //check left square
                if (currentElement.x - 1 >= 0)
                    if (!Grid[currentElement.x - 1, currentElement.y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                        tempCoords.Add(new Coordinates(currentElement.x - 1, currentElement.y, currentElement.counter + 1));

                //check lower square
                if (currentElement.y + 1 < GameRoot.HEIGHT)
                    if (!Grid[currentElement.x, currentElement.y + 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                        tempCoords.Add(new Coordinates(currentElement.x, currentElement.y + 1, currentElement.counter + 1));

                //check upper square
                if (currentElement.y - 1 >= 0)
                    if (!Grid[currentElement.x, currentElement.y - 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                        tempCoords.Add(new Coordinates(currentElement.x, currentElement.y - 1, currentElement.counter + 1));

                duplicateCheck(ref count, tempCoords, coords);

                squaresCounter(currentElement.counter + 1, Grid, tempCoords);

                for (int i = 0; i < tempCoords.Count; i++)
                    coords.Add(tempCoords[i]);

                count++;

                if (count < coords.Count())
                    currentElement = coords[count];

                tempCoords.Clear();

                if (count == (GameRoot.WIDTH * GameRoot.HEIGHT) - 1 - GetTrenchCount(Grid))
                    loopStop = false;

            }

            Grid[GameRoot.ENDPOINT.x, GameRoot.ENDPOINT.y].sqrCoord.counter = GameRoot.ENDPOINT.counter;

            foreach (Squares Square in Grid)
            {
                if (Square.sqrCoord.counter == GameRoot.DEFAULYDIST && Square.typeOfSquare == Squares.SqrFlags.Unoccupied)
                    return false;
            }

            return true;
        }

        public static bool InaccessibleSquareCheck(Squares[,] Grid, Coordinates SquareCoords)
        {
            Squares[,] TempGrid;
            TempGrid = Grid;

            TempGrid[SquareCoords.x, SquareCoords.y].typeOfSquare = Squares.SqrFlags.Wall;
            TempGrid[SquareCoords.x, SquareCoords.y].Building = Squares.BuildingType.Trench;

            if (GridPaths(TempGrid))
            {
                TempGrid[SquareCoords.x, SquareCoords.y].typeOfSquare = Squares.SqrFlags.Unoccupied;
                TempGrid[SquareCoords.x, SquareCoords.y].Building = Squares.BuildingType.None ;
                return true;
            }

            else
            {
                TempGrid[SquareCoords.x, SquareCoords.y].typeOfSquare = Squares.SqrFlags.Unoccupied;
                TempGrid[SquareCoords.x, SquareCoords.y].Building = Squares.BuildingType.None;
                return false;
            }
        }

        static void GridReset(Squares[,] Grid)
        {
            for (int y = 0; y < GameRoot.HEIGHT; y++)
                for (int x = 0; x < GameRoot.WIDTH; x++)
                {
                    Grid[x, y].sqrCoord.counter = GameRoot.DEFAULYDIST;
                }
        }

        static int GetTrenchCount(Squares[,] Grid)
        {
            int count = 0;

            foreach (Squares square in Grid)
            {
                if (square.Building.HasFlag(Squares.BuildingType.Trench))
                    count++;
            }

            return count;
        }

        static void duplicateCheck(ref int count, List<Coordinates> tempCoords, List<Coordinates> Coords)
        {
            for (int i = 0; i < tempCoords.Count; i++)
                for (int v = 0; v < Coords.Count; v++)
                {
                    if (tempCoords[i].x == Coords[v].x && tempCoords[i].y == Coords[v].y)
                    {
                        tempCoords.RemoveAt(i);

                        i = 0;
                        v = 0;

                        if (tempCoords.Count == 0)
                            break;

                    }
                }
        }

        static void squaresCounter(int counter, Squares[,] Grid, List<Coordinates> tempCoords)
        {
            for (int v = 0; v < tempCoords.Count; v++)
            {
                if (Grid[tempCoords[v].x, tempCoords[v].y].sqrCoord.counter == 0 || Grid[tempCoords[v].x, tempCoords[v].y].sqrCoord.counter == GameRoot.DEFAULYDIST)
                    Grid[tempCoords[v].x, tempCoords[v].y].sqrCoord.counter = counter;
            }
        }



    }

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
        public Vector2 gridBorder;
        public bool pathFound = false;

        public Grid(int SquareSize, int defDist)
        {
            gridSquares = new Squares[GameRoot.WIDTH, GameRoot.HEIGHT];

            for (int y = 0; y < GameRoot.HEIGHT; y++)
                for (int x = 0; x < GameRoot.WIDTH; x++)
                    gridSquares[x, y] = new Squares(SquareSize, new Vector2(x * SquareSize + GameRoot.BORDERLEFT, y * SquareSize + GameRoot.BORDERTOP), x, y, defDist);

            gridBorder = new Vector2(gridSquares[0, 0].sqrLoc.X, gridSquares[0, 0].sqrLoc.Y);
            gridStatus = gridFlags.empty;
            updateTimer = TimeSpan.Zero;

            gridSquares[GameRoot.ENDPOINT.x, GameRoot.ENDPOINT.y].typeOfSquare |= Squares.SqrFlags.StopPoint;
            gridSquares[GameRoot.ENDPOINT.x, GameRoot.ENDPOINT.y].Building = Squares.BuildingType.Base;
            

            GenerateNewMap();
            gridStatus = gridFlags.endPoint;
            pathFound = GridManager.GridPaths(gridSquares);
            
        }

        public void Update(GameTime gameTime)
        {
            foreach (Squares square in gridSquares)
                square.Update();

            // In case we delete everything, always have one bit of Trench.
            if (gridSquares[GameRoot.WIDTH - 1, GameRoot.HEIGHT - 1].Building != Squares.BuildingType.Trench)
            {
                gridSquares[GameRoot.WIDTH - 1, GameRoot.HEIGHT - 1].typeOfSquare |= Squares.SqrFlags.Wall;
                gridSquares[GameRoot.WIDTH - 1, GameRoot.HEIGHT - 1].Building = Squares.BuildingType.Trench;
            }

            updateTimer += gameTime.ElapsedGameTime;

            if (updateTimer.TotalMilliseconds > 1000f / GameRoot.UPS)
            {
                for (int y = 0; y < GameRoot.HEIGHT; y++)
                    for (int x = 0; x < GameRoot.WIDTH; x++)
                    {
                        if (Input.MouseRect.Intersects(gridSquares[x, y].rect) && Input.RMBDown && Input.IsKeyDown(Keys.LeftShift) && !gridStatus.HasFlag(gridFlags.endPoint)) // temporary.
                        {
                           /* gridStatus = gridFlags.endPoint;
                            stopPointCoord = new Coordinates(x, y, 0);
                            gridSquares[x, y].typeOfSquare |= Squares.SqrFlags.StopPoint;
                            gridSquares[x, y].Building = Squares.BuildingType.Base;
                            pathFound = FindPath();*/

                        }
                    }

                for (int y = 0; y < GameRoot.HEIGHT - 0; y++)
                    for (int x = 0; x < GameRoot.WIDTH - 0; x++)
                    {
                        if (gridSquares[x, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                            sqrTexDecider(x, y);
                    }

                if (pathFound == false)
                    pathFound = GridManager.GridPaths(gridSquares);

            }
        }

        public void Draw(SpriteBatch sb, SpriteFont deb)
        {
            foreach (Squares square in gridSquares)
                square.Draw(sb, Art.GroundTexs);
            // Go through and draw towers and their projectiles on top of everything else.
            foreach (Squares square in gridSquares)
                square.DrawTowers(sb);


            for (int y = 0; y < GameRoot.HEIGHT; y++) //Debug counter Text
                for (int x = 0; x < GameRoot.WIDTH; x++)
                {
                    if (gridSquares[x, y].sqrCoord.counter < 2000)
                        sb.DrawString(deb, gridSquares[x, y].sqrCoord.counter.ToString(), new Vector2(gridSquares[x, y].rect.X + 10, gridSquares[x, y].rect.Y), Color.Black);
                }
        }

        public void GenerateNewMap() // Generate a Starter Map. Couple of trenches and base in the bottom right.
        {
            // manually set the coords of base and a few trenches to begin with.
            resetGrid();
            // X X X    Create 3 Trenches in the bot-right corner,
            // X X 3    in the order the numbers are shown,
            // X 1 2    to the left here.
            gridSquares[GameRoot.WIDTH - 2, GameRoot.HEIGHT - 1].typeOfSquare |= Squares.SqrFlags.Wall;   // 1
            gridSquares[GameRoot.WIDTH - 2, GameRoot.HEIGHT - 1].Building = Squares.BuildingType.Trench;  // 1
            gridSquares[GameRoot.WIDTH - 1, GameRoot.HEIGHT - 1].typeOfSquare |= Squares.SqrFlags.Wall;   // 2
            gridSquares[GameRoot.WIDTH - 1, GameRoot.HEIGHT - 1].Building = Squares.BuildingType.Trench;  // 2
            gridSquares[GameRoot.WIDTH - 1, GameRoot.HEIGHT - 2].typeOfSquare |= Squares.SqrFlags.Wall;   // 3
            gridSquares[GameRoot.WIDTH - 1, GameRoot.HEIGHT - 2].Building = Squares.BuildingType.Trench;  // 3        
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

        public string sqrTexDecider(int x, int y)
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
            return gridSquares[x, y].TrenchName;
        }
    }
}
