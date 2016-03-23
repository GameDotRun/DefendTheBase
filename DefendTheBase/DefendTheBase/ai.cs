using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefendTheBase
{
    public class Ai
    {
        public Coordinates nextCoord, currentCoord;
        public Vector2 Node, previousVect, Movement;
        internal int tempInt;

        int defDist;
        bool Moving = false;

        float distance;

        public Ai()
        {
            defDist = GameRoot.DEFAULYDIST;
            tempInt = GameRoot.DEFAULYDIST;
            nextCoord = new Coordinates(0, 0);
        }

        public bool PathMove(Squares[,] squares, int height, int width, ref Vector2 EnemyVect, ref Vector2 ScreenPos, float speed, float time, Vector2 Direction, string EnemyType)
        {
            if (!Moving)
            {
                ScreenPos = new Vector2(Node.X, Node.Y);


                if (EnemyType != "Helicopter")
                {
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
                }

                else if (EnemyType == "Helicopter")
                    nextCoord = GameRoot.ENDPOINT;

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
                    if(Direction.Y >= 0 && Direction.X >= 0)
                        ScreenPos = new Vector2(Node.X, Node.Y);

                    Moving = false;

                    return false;
                }

                return true;
            }

            else return false;
            
        }

        public void PathMoveReset()
        {
            tempInt = defDist;
            nextCoord = new Coordinates(0, 0);
        }
    }
}
