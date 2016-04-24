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
            defDist = GameManager.DEFAULYDIST;
            tempInt = GameManager.DEFAULYDIST;
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

                            if (squares[(int)currentCoord.x - 1, (int)currentCoord.y].sqrCoord.counter == tempInt && GameManager.rnd.Next(0, 2) == 1)
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

                            if (squares[(int)currentCoord.x, (int)currentCoord.y + 1].sqrCoord.counter == tempInt && GameManager.rnd.Next(0, 2) == 1)
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

                            if (squares[(int)currentCoord.x, (int)currentCoord.y - 1].sqrCoord.counter == tempInt && GameManager.rnd.Next(0, 2) == 1)
                            {
                                tempInt = squares[(int)currentCoord.x, (int)currentCoord.y - 1].sqrCoord.counter; // set the tempint to new distance value
                                nextCoord = new Coordinates((int)currentCoord.x, (int)currentCoord.y - 1, tempInt); // set temp coord to the aipos + direction.
                            }
                        }
                    }
                }

                else if (EnemyType == "Helicopter")
                {
                    List<Coordinates> availableCoord = new List<Coordinates>();

                    foreach (Squares square in squares)
                    {
                        if (square.sqrCoord.counter != GameManager.DEFAULYDIST)
                            availableCoord.Add(square.sqrCoord);
                    }

                    int indexNext = GameManager.rnd.Next(0, availableCoord.Count);

                    nextCoord = availableCoord[indexNext];

                    if (GameManager.rnd.Next(0, 100) == 1)
                        nextCoord = GameManager.ENDPOINT;

                }

                Node = new Vector2((nextCoord.x * GameManager.SQUARESIZE) + GameManager.SQUARESIZE / 2, (int)GameManager.grid.gridBorder.Y + (nextCoord.y * GameManager.SQUARESIZE) + GameManager.SQUARESIZE / 2);

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

                    if (EnemyType == "Helicopter")
                    {

                        for (int i = 0; i < 7; i++)
                        {
                            EnemyManager.SpawnEnemy("Soldier", EnemyVect - new Vector2( 2 * i /7,  0));
                            WaveManager.WaveEnemiesUsed--;
                        }

                    }

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

    public class FriendlyAi
    {

        public Coordinates nextCoord, currentCoord;
        public Vector2 Node, previousVect, Movement;
        internal int tempInt;

        int defDist;
        bool Moving = false;

        float distance;

       

        public FriendlyAi()
        {
            defDist = GameManager.DEFAULYDIST;
            tempInt = GameManager.DEFAULYDIST;
            nextCoord = new Coordinates(0, 0);
        }

        public bool PathMove(Squares[,] squares, int height, int width, ref Vector2 EnemyVect, ref Vector2 ScreenPos, float speed, float time, Vector2 Direction)
        {
            bool wallfound = false;

            if (!Moving)
            {
                ScreenPos = new Vector2(Node.X, Node.Y);

                    if (currentCoord.x + 1 < width) // check array wont go out of bounds 
                    {
                        if (squares[(int)currentCoord.x + 1, (int)currentCoord.y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall)) //check next square is not a wall
                        {
                            tempInt = squares[(int)currentCoord.x + 1, (int)currentCoord.y].sqrCoord.counter; // set the tempint to new distance value
                            nextCoord = new Coordinates((int)currentCoord.x + 1, (int)currentCoord.y, tempInt); // set temp coord to the aipos + direction.
                            wallfound = true;
                        }
                    }


                    if (currentCoord.x - 1 >= 0) // check array wont go out of bounds 
                    {
                        if (squares[(int)currentCoord.x - 1, (int)currentCoord.y].typeOfSquare.HasFlag(Squares.SqrFlags.Wall)) //check next square is not a wall
                        {
                            if ((wallfound == true && GameManager.rnd.Next(0, 5) == 1) || !wallfound)
                            {
                                tempInt = squares[(int)currentCoord.x - 1, (int)currentCoord.y].sqrCoord.counter; // set the tempint to new distance value
                                nextCoord = new Coordinates((int)currentCoord.x - 1, (int)currentCoord.y, tempInt); // set temp coord to the aipos + direction.
                                wallfound = true;
                            }
                        }
                    }

                    if (currentCoord.y + 1 < height) // check array wont go out of bounds 
                    {
                        if (squares[(int)currentCoord.x, (int)currentCoord.y + 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall)) //check next square is not a wall
                        {
                            if ((wallfound == true && GameManager.rnd.Next(0, 5) == 1) || !wallfound)
                            {
                                tempInt = squares[(int)currentCoord.x, (int)currentCoord.y + 1].sqrCoord.counter; // set the tempint to new distance value
                                nextCoord = new Coordinates((int)currentCoord.x, (int)currentCoord.y + 1, tempInt); // set temp coord to the aipos + direction.
                                wallfound = true;
                            }
                        }
                    }

                    if (currentCoord.y - 1 >= 0 && currentCoord.x < 20) // check array wont go out of bounds 
                    {
                        if (squares[(int)currentCoord.x, (int)currentCoord.y - 1].typeOfSquare.HasFlag(Squares.SqrFlags.Wall)) //check next square is not a wall
                        {
                            if ((wallfound == true && GameManager.rnd.Next(0, 5) == 1) || !wallfound)
                            {
                                tempInt = squares[(int)currentCoord.x, (int)currentCoord.y - 1].sqrCoord.counter; // set the tempint to new distance value
                                nextCoord = new Coordinates((int)currentCoord.x, (int)currentCoord.y - 1, tempInt); // set temp coord to the aipos + direction.
                                wallfound = true;
                            }
                        }
                    }
               

                Node = new Vector2((nextCoord.x * GameManager.SQUARESIZE) + GameManager.SQUARESIZE / 2, (int)GameManager.grid.gridBorder.Y + (nextCoord.y * GameManager.SQUARESIZE) + GameManager.SQUARESIZE / 2);

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
                    if (Direction.Y >= 0 && Direction.X >= 0)
                        ScreenPos = new Vector2(Node.X, Node.Y);

                    Moving = false;

                    return false;
                }

                return true;
            }

            else return false;
        }
    
    
    
    
    }
}
