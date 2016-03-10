using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefendTheBase
{
    public class ai
    {
        public Coordinates aiPos;

        List<Coordinates> coords;
        List<Coordinates> tempCoords;
        Coordinates tempCoord;

        int tempInt, defDist;

        public ai(Coordinates aiStart)
        {
            aiPos = aiStart;
            defDist = GameRoot.DEFAULYDIST;
            tempInt = GameRoot.DEFAULYDIST;

            tempCoord = new Coordinates(0, 0, GameRoot.DEFAULYDIST);
            coords = new List<Coordinates>();
            tempCoords = new List<Coordinates>();
        }

        public void PathMove(Coordinates endPoint, Squares[,] squares, int height, int width, ref Vector2 enemyVect, float speed)
        {
            if ((aiPos.x == (int)enemyVect.X && aiPos.y == (int)enemyVect.Y) || (aiPos.y == (int)enemyVect.Y + 1 && aiPos.x == (int)enemyVect.X))
            {
                if (aiPos.x + 1 < width) // check array wont go out of bounds 
                {
                    if (!squares[(int)aiPos.x + 1, (int)aiPos.y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall)) //check next square is not a wall
                    {
                        if (squares[(int)aiPos.x + 1, (int)aiPos.y].sqrCoord.counter < tempInt) // check the square distance from endpoint is less than the current pos. 
                        {
                            tempInt = squares[(int)aiPos.x + 1, (int)aiPos.y].sqrCoord.counter; // set the tempint to new distance value
                            tempCoord = new Coordinates((int)aiPos.x + 1, (int)aiPos.y, tempInt); // set temp coord to the aipos + direction.
                        }


                    }
                }

                if (aiPos.x - 1 >= 0) // check array wont go out of bounds 
                {
                    if (!squares[(int)aiPos.x - 1, (int)aiPos.y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    {
                        if (squares[(int)aiPos.x - 1, (int)aiPos.y].sqrCoord.counter < tempInt)
                        {
                            tempInt = squares[(int)aiPos.x - 1, (int)aiPos.y].sqrCoord.counter;
                            tempCoord = new Coordinates((int)aiPos.x - 1, (int)aiPos.y, tempInt);
                        }

                        else if (squares[(int)aiPos.x - 1, (int)aiPos.y].sqrCoord.counter == tempInt && GameRoot.rnd.Next(0, 2) == 0)
                        {
                            tempInt = squares[(int)aiPos.x - 1, (int)aiPos.y].sqrCoord.counter;
                            tempCoord = new Coordinates((int)aiPos.x - 1, (int)aiPos.y, tempInt);
                        }

                    }
                }

                if (aiPos.y + 1 < height) // check array wont go out of bounds 
                {
                    if (!squares[(int)aiPos.x, (int)aiPos.y + 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    {
                        if (squares[(int)aiPos.x, (int)aiPos.y + 1].sqrCoord.counter < tempInt)
                        {
                            tempInt = squares[(int)aiPos.x, (int)aiPos.y + 1].sqrCoord.counter;
                            tempCoord = new Coordinates((int)aiPos.x, (int)aiPos.y + 1, tempInt);
                        }

                        else if (squares[(int)aiPos.x, (int)aiPos.y + 1].sqrCoord.counter == tempInt && GameRoot.rnd.Next(0, 2) == 0)
                        {
                            tempInt = squares[(int)aiPos.x, (int)aiPos.y + 1].sqrCoord.counter;
                            tempCoord = new Coordinates((int)aiPos.x, (int)aiPos.y + 1, tempInt);
                        }
                    }
                }

                if (aiPos.y - 1 >= 0) // check array wont go out of bounds 
                {
                    if (!squares[(int)aiPos.x, (int)aiPos.y - 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                    {
                        if (squares[(int)aiPos.x, (int)aiPos.y - 1].sqrCoord.counter < tempInt)
                        {
                            tempInt = squares[(int)aiPos.x, (int)aiPos.y - 1].sqrCoord.counter;
                            tempCoord = new Coordinates((int)aiPos.x, (int)aiPos.y - 1, tempInt);
                        }

                        else if (squares[(int)aiPos.x, (int)aiPos.y - 1].sqrCoord.counter == tempInt && GameRoot.rnd.Next(0, 2) == 0)
                        {
                            tempInt = squares[(int)aiPos.x, (int)aiPos.y - 1].sqrCoord.counter;
                            tempCoord = new Coordinates((int)aiPos.x, (int)aiPos.y - 1, tempInt);
                        }
                    }
                }



                aiPos = new Coordinates(tempCoord.x, tempCoord.y); // set ai coord to the new location found
            }

            // Somewhere in these ifs is the key to diag movement or wall nope-ing...
            // adding else to all but the last ifs, removes diag but he nopes through many a wall.
            if (enemyVect.X < aiPos.x)
            {
                enemyVect.X += speed / 100;
                enemyVect.X = (float)Math.Round(enemyVect.X, 2);
            }

            if (enemyVect.X > aiPos.x)
            {
                enemyVect.X -= speed / 100;
                enemyVect.X = (float)Math.Round(enemyVect.X, 2);
            }

            if (enemyVect.Y < aiPos.y)
            {
                enemyVect.Y += speed / 100;
                enemyVect.Y = (float)Math.Round(enemyVect.Y, 2);
            }

            if (enemyVect.Y > aiPos.y)
            {
                enemyVect.Y -= speed / 100;
                enemyVect.Y = (float)Math.Round(enemyVect.Y, 2);
            }
            
        }

        public void PathMoveReset()
        {
            tempInt = defDist;
            tempCoord = new Coordinates(0, 0, defDist);
        }

        /*public void FindPathReset()
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
        }*/
    }
}
