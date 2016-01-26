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
    class ai
    {
        public bool[,] AIPOS;
        public bool[,] ENDPOS;
        public Coordinates aiPos;

        List<Coordinates> coords;
        List<Coordinates> tempCoords;
        Coordinates currentElement;
        Coordinates tempCoord;

        int tempInt, defDist;
        int count = 0;
        bool done = false;
        
        public ai(int Height, int Width, Coordinates aiStart, int defaultDist)
        {
            aiPos = aiStart;
            defDist = defaultDist;
            tempInt = defaultDist;

            AIPOS = new bool[Width, Height];
            ENDPOS = new bool[Width, Height];
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    AIPOS[x, y] = false;
                    ENDPOS[x, y] = false;
                }

            AIPOS[0, 0] = true;

            tempCoord = new Coordinates(0, 0, defaultDist);
            coords = new List<Coordinates>();
            tempCoords = new List<Coordinates>();
        }

        public void Update(Coordinates endPoint, Squares[,] squares, int height, int width)
        {
            if (AIPOS != ENDPOS)
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
        
        }

        public bool FindPath(Coordinates endPoint, Squares[,] squares, int height, int width)
        {
            ENDPOS[(int)endPoint.x, (int)endPoint.y] = true;

            if (!done)
            {
                coords.Add(endPoint);
                currentElement = coords[count];
                done = true;
            }

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

            return false;
        
        }

         
        //&& tempCoords[i].counter >= coords[v].counter
        void duplicateCheck(Squares[,] squares)
        { 
            for(int i = 0; i < tempCoords.Count; i++)
                for (int v = 0; v < coords.Count; v++)
                {
                    if (tempCoords[i].x == coords[v].x && tempCoords[i].y == coords[v].y )
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

        /*bool canMove(Squares[,] sqrs, Vector2 aiLoc)
        {
            if (aiLoc.X >= 0 && aiLoc.Y >= 0)
            {
                if (!sqrs[(int)aiLoc.X, (int)aiLoc.Y].selected)
                    return true;
                else return false;
            }

            else return false;
        
        }*/

        void getPos(int height, int width)
        {

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    if (AIPOS[x, y])
                    {
                        aiPos = new Coordinates(x, y);
                        break;
                    }

                }
        
        }

        public void Draw()
        { }

    }
}
