﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefendTheBase
{
    public class Squares
    {
        [Flags]
        public enum SqrFlags
        {
            Tower = 1,
            Wall = 2,
            Moveable = 4,
            Occupied = 8,
            Unoccupied = 16,
            StopPoint = 32
        }

        public Art.TrenchEnum texEnum;
        public SqrFlags typeOfSquare;
        public Coordinates sqrCoord;
        public Rectangle rect;
        public Vector2 sqrLoc;


        float highlight;

        public Squares(int SquareSize, Vector2 Location, int x, int y, int defDist)
        {
            sqrLoc = Location;

            typeOfSquare = SqrFlags.Unoccupied;

            rect = new Rectangle((int)Location.X, (int)Location.Y, SquareSize, SquareSize);

            sqrCoord = new Coordinates(x, y, defDist);
        }

        public void Update(MouseState mouseState)
        {
            if (rect.Contains(new Point(mouseState.X, mouseState.Y)))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                    typeOfSquare |= Squares.SqrFlags.Wall;
                else if (mouseState.RightButton == ButtonState.Pressed)
                    typeOfSquare = Squares.SqrFlags.Unoccupied;

                highlight = 0.5f;
            }

            else highlight = 1;
        }

        public void Draw(SpriteBatch sb, Texture2D gridSquareTex)
        {
            if (typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                sb.Draw(Art.getTrenchTex(texEnum), rect, Color.White * highlight);
            else if (typeOfSquare.HasFlag(Squares.SqrFlags.Occupied))
                sb.Draw(gridSquareTex, rect, Color.Blue * highlight);
            else if (typeOfSquare.HasFlag(Squares.SqrFlags.StopPoint))
                sb.Draw(gridSquareTex, rect, Color.Red * highlight);
            else if (typeOfSquare.HasFlag(Squares.SqrFlags.Unoccupied))
                sb.Draw(gridSquareTex, rect, Color.White * highlight);
        }
    }
}
