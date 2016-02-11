using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefendTheBase
{
    class ai
    {
        public Coordinates aiPos;

        bool loopStop = true;

        List<Coordinates> coords;
        List<Coordinates> tempCoords;
        Coordinates currentElement;
        Coordinates tempCoord;

        int tempInt, defDist;
        int count = 0;
        bool done = false;

        public ai(Coordinates aiStart, int defaultDist)
        {
            aiPos = aiStart;
            defDist = defaultDist;
            tempInt = defaultDist;

            tempCoord = new Coordinates(0, 0, defaultDist);
            coords = new List<Coordinates>();
            tempCoords = new List<Coordinates>();
        }

        public void PathMove(Coordinates endPoint, Squares[,] squares, int height, int width, ref Vector2 enemyVect)
        {
            if (aiPos.x == (int)enemyVect.X && aiPos.y == (int)enemyVect.Y)
            {
                if (aiPos.x + 1 < width)
                {
                    if (!squares[(int)aiPos.x + 1, (int)aiPos.y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    {
                        if (squares[(int)aiPos.x + 1, (int)aiPos.y].sqrCoord.counter < tempInt)
                        {
                            tempInt = squares[(int)aiPos.x + 1, (int)aiPos.y].sqrCoord.counter;
                            tempCoord = new Coordinates((int)aiPos.x + 1, (int)aiPos.y, tempInt);
                        }
                    }
                }

                if (aiPos.x - 1 >= 0)
                {
                    if (!squares[(int)aiPos.x - 1, (int)aiPos.y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    {
                        if (squares[(int)aiPos.x - 1, (int)aiPos.y].sqrCoord.counter < tempInt)
                        {
                            tempInt = squares[(int)aiPos.x - 1, (int)aiPos.y].sqrCoord.counter;
                            tempCoord = new Coordinates((int)aiPos.x - 1, (int)aiPos.y, tempInt);
                        }

                    }
                }

                if (aiPos.y + 1 < height)
                {
                    if (!squares[(int)aiPos.x, (int)aiPos.y + 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    {
                        if (squares[(int)aiPos.x, (int)aiPos.y + 1].sqrCoord.counter < tempInt)
                        {
                            tempInt = squares[(int)aiPos.x, (int)aiPos.y + 1].sqrCoord.counter;
                            tempCoord = new Coordinates((int)aiPos.x, (int)aiPos.y + 1, tempInt);
                        }
                    }
                }

                if (aiPos.y - 1 >= 0)
                {
                    if (!squares[(int)aiPos.x, (int)aiPos.y - 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    {
                        if (squares[(int)aiPos.x, (int)aiPos.y - 1].sqrCoord.counter < tempInt)
                        {
                            tempInt = squares[(int)aiPos.x, (int)aiPos.y - 1].sqrCoord.counter;
                            tempCoord = new Coordinates((int)aiPos.x, (int)aiPos.y - 1, tempInt);
                        }
                    }
                }



                aiPos = new Coordinates(tempCoord.x, tempCoord.y);
            }

            
            if (enemyVect.X < aiPos.x)
            {
                enemyVect.X += 1f / 10;
                enemyVect.X = (float)Math.Round(enemyVect.X, 2);
            }
             if (enemyVect.Y < aiPos.y)
            {
                enemyVect.Y += 1f / 10;
                enemyVect.Y = (float)Math.Round(enemyVect.Y, 2);
            }
            if (enemyVect.X > aiPos.x)
            {
                enemyVect.X -= 1f / 10;
                enemyVect.X = (float)Math.Round(enemyVect.X, 2);
            }
             if (enemyVect.Y > aiPos.y)
            {
                enemyVect.Y -= 1f / 10;
                enemyVect.Y = (float)Math.Round(enemyVect.Y, 2);
            }
            
        }

        public bool FindPath(Coordinates endPoint, Squares[,] squares, int height, int width)
        {
            if (!done)
            {
                coords.Add(endPoint);
                currentElement = coords[count];
                done = true;
            }

            while (loopStop)
            {
                //Check right square
                if (currentElement.x + 1 < width)
                    if (!squares[currentElement.x + 1, currentElement.y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                        tempCoords.Add(new Coordinates(currentElement.x + 1, currentElement.y, currentElement.counter + 1));

                //check left square
                if (currentElement.x - 1 >= 0)
                    if (!squares[currentElement.x - 1, currentElement.y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                        tempCoords.Add(new Coordinates(currentElement.x - 1, currentElement.y, currentElement.counter + 1));

                //check lower square
                if (currentElement.y + 1 < height)
                    if (!squares[currentElement.x, currentElement.y + 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                        tempCoords.Add(new Coordinates(currentElement.x, currentElement.y + 1, currentElement.counter + 1));

                //check upper square
                if (currentElement.y - 1 >= 0)
                    if (!squares[currentElement.x, currentElement.y - 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                        tempCoords.Add(new Coordinates(currentElement.x, currentElement.y - 1, currentElement.counter + 1));

                duplicateCheck(squares);

                squaresCounter(squares, currentElement.counter + 1);

                for (int i = 0; i < tempCoords.Count; i++)
                    if (aiPos.x == tempCoords[i].x && aiPos.y == tempCoords[i].y)
                    {
                        squares[(int)endPoint.x, (int)endPoint.y].sqrCoord.counter = 0;
                        return true;
                    }

                for (int i = 0; i < tempCoords.Count; i++)
                    coords.Add(tempCoords[i]);

                count++;
                currentElement = coords[count];

                tempCoords.Clear();
            }
            return false;

        }

        void duplicateCheck(Squares[,] squares)
        {
            for (int i = 0; i < tempCoords.Count; i++)
                for (int v = 0; v < coords.Count; v++)
                {
                    if (tempCoords[i].x == coords[v].x && tempCoords[i].y == coords[v].y)
                    {
                        tempCoords.RemoveAt(i);

                        i = 0;

                        if (tempCoords.Count == 0)
                            break;

                    }
                }

            for (int i = 0; i < tempCoords.Count; i++)
                for (int v = 0; v < coords.Count; v++)
                {
                    if (squares[tempCoords[i].x, tempCoords[i].y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    {
                        tempCoords.RemoveAt(i);

                        break;
                    }
                }


        }

        void squaresCounter(Squares[,] squares, int counter)
        {
            for (int v = 0; v < tempCoords.Count; v++)
            {
                if (squares[tempCoords[v].x, tempCoords[v].y].sqrCoord.counter == 0 || squares[tempCoords[v].x, tempCoords[v].y].sqrCoord.counter == defDist)
                    squares[tempCoords[v].x, tempCoords[v].y].sqrCoord.counter = counter;
            }
        }

        public void PathMoveReset()
        {
            tempInt = defDist;
            tempCoord = new Coordinates(0, 0, defDist);
        }

        public void FindPathReset()
        {
            count = 0;
            coords.Clear();
            tempCoords.Clear();
            done = false;

            for (int y = 0; y < GameRoot.HEIGHT; y++)
                for (int x = 0; x < GameRoot.WIDTH; x++)
                {
                    GameRoot.grid.gridSquares[x, y].sqrCoord.counter = defDist;
                }

            PathMoveReset();
        }

        public void Draw()
        { }
    }
}
