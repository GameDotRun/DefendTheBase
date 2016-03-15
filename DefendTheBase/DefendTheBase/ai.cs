﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefendTheBase
{
    public class ai
    {
        public Coordinates aiPos;

        public Coordinates nextCoord;
        public Coordinates currentCoord;
        public Vector2 Node, previousVect;
        public Vector2 Movement;

        internal int tempInt;
        int defDist;

        bool firstTime = true;
        bool Moving = false;

        float distance;

        public ai()
        {
            aiPos = GameRoot.STARTPOINT;
            defDist = GameRoot.DEFAULYDIST;
            tempInt = GameRoot.DEFAULYDIST;
            nextCoord = new Coordinates(0, 0);

        }

        public bool PathMove(Squares[,] squares, int height, int width, ref Vector2 EnemyVect, ref Vector2 ScreenPos, float speed, float time, Vector2 Direction)
        {
            currentCoord = new Coordinates((int)EnemyVect.X , (int)EnemyVect.Y );

            if (!Moving)
            {
                ScreenPos = new Vector2(Node.X, Node.Y);


                if (currentCoord.x + 1 < width) // check array wont go out of bounds 
                {
                    if (!squares[(int)currentCoord.x + 1, (int)currentCoord.y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall)) //check next square is not a wall
                    {
                        if (squares[(int)currentCoord.x + 1, (int)currentCoord.y].sqrCoord.counter < tempInt) // check the square distance from endpoint is less than the current pos. 
                        {
                            tempInt = squares[(int)currentCoord.x + 1, (int)currentCoord.y].sqrCoord.counter; // set the tempint to new distance value
                            nextCoord = new Coordinates((int)currentCoord.x + 1, (int)currentCoord.y, tempInt); // set temp coord to the aipos + direction.
                        }
                    }
                }


                if (currentCoord.x - 1 >= 0) // check array wont go out of bounds 
                {
                    if (!squares[(int)currentCoord.x - 1, (int)currentCoord.y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall)) //check next square is not a wall
                    {
                        if (squares[(int)currentCoord.x - 1, (int)currentCoord.y].sqrCoord.counter < tempInt) // check the square distance from endpoint is less than the current pos. 
                        {
                            tempInt = squares[(int)currentCoord.x - 1, (int)currentCoord.y].sqrCoord.counter; // set the tempint to new distance value
                            nextCoord = new Coordinates((int)currentCoord.x - 1, (int)currentCoord.y, tempInt); // set temp coord to the aipos + direction.
                        }
                    }
                }

                if (currentCoord.y + 1 < height) // check array wont go out of bounds 
                {
                    if (!squares[(int)currentCoord.x, (int)currentCoord.y + 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall)) //check next square is not a wall
                    {
                        if (squares[(int)currentCoord.x, (int)currentCoord.y + 1].sqrCoord.counter < tempInt) // check the square distance from endpoint is less than the current pos. 
                        {
                            tempInt = squares[(int)currentCoord.x, (int)currentCoord.y + 1].sqrCoord.counter; // set the tempint to new distance value
                            nextCoord = new Coordinates((int)currentCoord.x, (int)currentCoord.y + 1, tempInt); // set temp coord to the aipos + direction.
                        }
                    }
                }

                if (currentCoord.y - 1 >= 0) // check array wont go out of bounds 
                {
                    if (!squares[(int)currentCoord.x, (int)currentCoord.y - 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall)) //check next square is not a wall
                    {
                        if (squares[(int)currentCoord.x, (int)currentCoord.y - 1].sqrCoord.counter < tempInt) // check the square distance from endpoint is less than the current pos. 
                        {
                            tempInt = squares[(int)currentCoord.x, (int)currentCoord.y - 1].sqrCoord.counter; // set the tempint to new distance value
                            nextCoord = new Coordinates((int)currentCoord.x, (int)currentCoord.y - 1, tempInt); // set temp coord to the aipos + direction.
                        }
                    }
                }

                firstTime = false;

                Node = new Vector2((nextCoord.x * GameRoot.SQUARESIZE) + GameRoot.SQUARESIZE / 2, (int)GameRoot.grid.gridBorder.Y + (nextCoord.y * GameRoot.SQUARESIZE) + GameRoot.SQUARESIZE / 2);

                distance = Vector2.Distance(new Vector2(currentCoord.x, currentCoord.y), new Vector2(nextCoord.x, nextCoord.y));

                Direction = new Vector2(nextCoord.x - currentCoord.x, nextCoord.y - currentCoord.y);
                Movement = Direction;
                Direction.Normalize();

                previousVect = new Vector2(currentCoord.x, currentCoord.y);

                Moving = true;

            }


            if (Moving)
            {
                EnemyVect += Direction * speed * time;
                if (Vector2.Distance(previousVect, EnemyVect) >= distance)
                {
                    ScreenPos = new Vector2(Node.X, Node.Y);
                    Moving = false;

                    return false;
                }

                return true;
            }

            else return false;

           

           /* if ((aiPos.x == Math.Round(enemyVect.X, 2) && aiPos.y == Math.Round(enemyVect.Y, 2)) || (aiPos.y == Math.Round(enemyVect.Y + 1, 2) && aiPos.x == Math.Round(enemyVect.X, 2)))
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

                        else if (squares[(int)aiPos.x - 1, (int)aiPos.y].sqrCoord.counter == tempInt && GameRoot.rnd.Next(0, 2) == 0) // random choice between two paths of same length
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
            }

            if (enemyVect.X > aiPos.x)
            {
                enemyVect.X -= speed / 100;

            }

            if (enemyVect.Y < aiPos.y)
            {
                enemyVect.Y += speed / 100;

            }

            if (enemyVect.Y > aiPos.y)
            {
                enemyVect.Y -= speed / 100;

            }*/
            
        }

        public void PathMoveReset()
        {
            tempInt = defDist;
            //nextCoord = new Coordinates(0, 0);
        }
    }
}
