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
                coords.Add(GameManager.ENDPOINT);
                currentElement = coords[count];
                Grid[(int)GameManager.ENDPOINT.x, (int)GameManager.ENDPOINT.y].sqrCoord.counter = GameManager.ENDPOINT.counter;
                done = true;
            }

            while (loopStop)
            {

                //Check right square
                if (currentElement.x + 1 < GameManager.WIDTH)
                    if (!Grid[(int)currentElement.x + 1, (int)currentElement.y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                        tempCoords.Add(new Coordinates(currentElement.x + 1, currentElement.y, currentElement.counter + 1));

                //check left square
                if (currentElement.x - 1 >= 0)
                    if (!Grid[(int)currentElement.x - 1, (int)currentElement.y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                        tempCoords.Add(new Coordinates(currentElement.x - 1, currentElement.y, currentElement.counter + 1));

                //check lower square
                if (currentElement.y + 1 < GameManager.HEIGHT)
                    if (!Grid[(int)currentElement.x, (int)currentElement.y + 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                        tempCoords.Add(new Coordinates(currentElement.x, currentElement.y + 1, currentElement.counter + 1));

                //check upper square
                if (currentElement.y - 1 >= 0)
                    if (!Grid[(int)currentElement.x, (int)currentElement.y - 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                        tempCoords.Add(new Coordinates(currentElement.x, currentElement.y - 1, currentElement.counter + 1));

                duplicateCheck(ref count, tempCoords, coords);

                squaresCounter(currentElement.counter + 1, Grid, tempCoords);

                for (int i = 0; i < tempCoords.Count; i++)
                    coords.Add(tempCoords[i]);

                count++;

                if (count < coords.Count())
                    currentElement = coords[count];

                tempCoords.Clear();

                if (count == (GameManager.WIDTH * GameManager.HEIGHT) -  GetTrenchCount(Grid))
                    loopStop = false;

            }

            Grid[(int)GameManager.ENDPOINT.x, (int)GameManager.ENDPOINT.y].sqrCoord.counter = GameManager.ENDPOINT.counter;

            if (!CheckSquareCounters(Grid))
                return false;

            return true;
        }

        public static bool InaccessibleSquareCheck(Squares[,] Grid, Coordinates SquareCoords)
        {
            Squares[,] TempGrid;
            TempGrid = Grid;

            TempGrid[(int)SquareCoords.x, (int)SquareCoords.y].typeOfSquare = Squares.SqrFlags.Wall;
            TempGrid[(int)SquareCoords.x, (int)SquareCoords.y].Building = Squares.BuildingType.Trench;

            if (GridPaths(TempGrid))
            {
                TempGrid[(int)SquareCoords.x, (int)SquareCoords.y].typeOfSquare = Squares.SqrFlags.Unoccupied;
                TempGrid[(int)SquareCoords.x, (int)SquareCoords.y].Building = Squares.BuildingType.None;
                return true;
            }

            else
            {
                TempGrid[(int)SquareCoords.x, (int)SquareCoords.y].typeOfSquare = Squares.SqrFlags.Unoccupied;
                TempGrid[(int)SquareCoords.x, (int)SquareCoords.y].Building = Squares.BuildingType.None;
                return false;
            }
        } 

        public static bool CheckSquareCounters(Squares[,] Grid)
        {
            foreach (Squares Square in Grid)
            {
                if (Square.sqrCoord.counter == GameManager.DEFAULYDIST && Square.typeOfSquare == Squares.SqrFlags.Unoccupied)
                    return false;
            }

            return true;
        }

        static void GridReset(Squares[,] Grid)
        {
            for (int y = 0; y < GameManager.HEIGHT; y++)
                for (int x = 0; x < GameManager.WIDTH; x++)
                {
                    Grid[x, y].sqrCoord.counter = GameManager.DEFAULYDIST;
                }
        }

        static int GetTrenchCount(Squares[,] Grid)
        {
            int count = 0;

            foreach (Squares square in Grid)
            {
                if (square.Building.HasFlag(Squares.BuildingType.Trench) || square.Building.HasFlag(Squares.BuildingType.Tower))
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
                if (Grid[(int)tempCoords[v].x, (int)tempCoords[v].y].sqrCoord.counter == 0 || Grid[(int)tempCoords[v].x, (int)tempCoords[v].y].sqrCoord.counter == GameManager.DEFAULYDIST)
                    Grid[(int)tempCoords[v].x, (int)tempCoords[v].y].sqrCoord.counter = counter;
            }
        }

        public static bool HasNeighbour(Squares.BuildingType typeOfBuilding, Coordinates coord)
        {
            // Check North
            if (coord.y > 0)
                if (GameManager.grid.gridSquares[(int)coord.x, (int)coord.y - 1].Building == typeOfBuilding)
                    return true;
            // Check East
            if (coord.x < GameManager.WIDTH - 1)
                if (GameManager.grid.gridSquares[(int)coord.x + 1, (int)coord.y].Building == typeOfBuilding)
                    return true;
            // Check South
            if (coord.y < GameManager.HEIGHT - 1)
                if (GameManager.grid.gridSquares[(int)coord.x, (int)coord.y + 1].Building == typeOfBuilding)
                    return true;
            // Check West
            if (coord.x > 0)
                if (GameManager.grid.gridSquares[(int)coord.x - 1, (int)coord.y].Building == typeOfBuilding)
                    return true;
            return false;
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
            gridSquares = new Squares[GameManager.WIDTH, GameManager.HEIGHT];

            for (int y = 0; y < GameManager.HEIGHT; y++)
                for (int x = 0; x < GameManager.WIDTH; x++)
                    gridSquares[x, y] = new Squares(SquareSize, new Vector2(x * SquareSize + GameManager.BORDERLEFT, y * SquareSize + GameManager.BORDERTOP), x, y, defDist);

            gridBorder = new Vector2(gridSquares[0, 0].sqrLoc.X, gridSquares[0, 0].sqrLoc.Y);
            gridStatus = gridFlags.empty;
            updateTimer = TimeSpan.Zero;

            GenerateNewMap();

            gridSquares[(int)GameManager.ENDPOINT.x, (int)GameManager.ENDPOINT.y].typeOfSquare |= Squares.SqrFlags.StopPoint;
            gridSquares[(int)GameManager.ENDPOINT.x, (int)GameManager.ENDPOINT.y].Building = Squares.BuildingType.Base;
            

            gridStatus = gridFlags.endPoint;
            pathFound = GridManager.GridPaths(gridSquares);
            
        }

        public void Update(GameTime gameTime)
        {
            foreach (Squares square in gridSquares)
                square.Update();

            // In case we delete everything, always have 3 bits of Trench.
            if (gridSquares[GameManager.WIDTH - 2, GameManager.HEIGHT - 1].Building != Squares.BuildingType.Trench)
            {
                gridSquares[GameManager.WIDTH - 2, GameManager.HEIGHT - 1].typeOfSquare |= Squares.SqrFlags.Wall;
                gridSquares[GameManager.WIDTH - 2, GameManager.HEIGHT - 1].Building = Squares.BuildingType.Trench;
            }
            if (gridSquares[GameManager.WIDTH - 1, GameManager.HEIGHT - 1].Building != Squares.BuildingType.Trench)
            {
                gridSquares[GameManager.WIDTH - 1, GameManager.HEIGHT - 1].typeOfSquare |= Squares.SqrFlags.Wall;
                gridSquares[GameManager.WIDTH - 1, GameManager.HEIGHT - 1].Building = Squares.BuildingType.Trench;
            }
            if (gridSquares[GameManager.WIDTH - 1, GameManager.HEIGHT - 2].Building != Squares.BuildingType.Trench)
            {
                gridSquares[GameManager.WIDTH - 1, GameManager.HEIGHT - 2].typeOfSquare |= Squares.SqrFlags.Wall;
                gridSquares[GameManager.WIDTH - 1, GameManager.HEIGHT - 2].Building = Squares.BuildingType.Trench;
            }

            updateTimer += gameTime.ElapsedGameTime;

            if (updateTimer.TotalMilliseconds > 1000f / GameManager.UPS)
            {

                for (int y = 0; y < GameManager.HEIGHT - 0; y++)
                    for (int x = 0; x < GameManager.WIDTH - 0; x++)
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
                square.DrawBase(sb);


            /*for (int y = 0; y < GameManager.HEIGHT; y++) //Debug counter Text
                for (int x = 0; x < GameManager.WIDTH; x++)
                {
                    if (gridSquares[x, y].sqrCoord.counter < 2000)
                        sb.DrawString(deb, gridSquares[x, y].sqrCoord.counter.ToString(), new Vector2(gridSquares[x, y].rect.X + 10, gridSquares[x, y].rect.Y), Color.Black);
                }*/
        }

        public void GenerateNewMap() // Generate a Starter Map. Couple of trenches and base in the bottom right.
        {
            // manually set the coords of base and a few trenches to begin with.
            resetGrid();
            // X X X    Create 3 Trenches in the bot-right corner,
            // X X 3    in the order the numbers are shown,
            // X 1 2    to the left here.
            //gridSquares[GameManager.WIDTH - 2, GameManager.HEIGHT - 1].typeOfSquare |= Squares.SqrFlags.Wall;   // 1
            //gridSquares[GameManager.WIDTH - 2, GameManager.HEIGHT - 1].Building = Squares.BuildingType.Trench;  // 1
            //gridSquares[GameManager.WIDTH - 1, GameManager.HEIGHT - 1].typeOfSquare |= Squares.SqrFlags.Wall;   // 2
            //gridSquares[GameManager.WIDTH - 1, GameManager.HEIGHT - 1].Building = Squares.BuildingType.Trench;  // 2
            //gridSquares[GameManager.WIDTH - 1, GameManager.HEIGHT - 2].typeOfSquare |= Squares.SqrFlags.Wall;   // 3
            //gridSquares[GameManager.WIDTH - 1, GameManager.HEIGHT - 2].Building = Squares.BuildingType.Trench;  // 3        
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

            if (x < GameManager.WIDTH - 1)
                if (gridSquares[x + 1, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    gridSquares[x, y].TrenchName += "E";

            if (y < GameManager.HEIGHT - 1)
                if (gridSquares[x, y + 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    gridSquares[x, y].TrenchName += "S";

            if (x > 0)
                if (gridSquares[x - 1, y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    gridSquares[x, y].TrenchName += "W";
            return gridSquares[x, y].TrenchName;
        }
    }

    public class Coordinates
    {
        public float x, y;
        public int counter;
        public Coordinates(float X, float Y, int COUNTER)
        {
            x = X;
            y = Y;
            counter = COUNTER;
        }

        public Coordinates(float X, float Y)
        {
            x = X;
            y = Y;
        }

        public bool Equals(Coordinates Coords)
        { 
            if(Coords.x == x && Coords.y == y && Coords.counter == counter)
                return true;
            else return false;
        }

        public bool CoordEqual(Coordinates Coord)
        {
            if (Coord.x == x && Coord.y == y)
                return true;
            else return false;
        
        }
    }
}
