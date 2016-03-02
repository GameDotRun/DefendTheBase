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

        public string TrenchName;
        public SqrFlags typeOfSquare;
        public Coordinates sqrCoord;
        public Rectangle rect;
        public Vector2 sqrLoc;

        public bool sqrEdited = false;

        float highlight;

        public Squares(int SquareSize, Vector2 Location, int x, int y, int defDist)
        {
            sqrLoc = Location;

            typeOfSquare = SqrFlags.Unoccupied;

            rect = new Rectangle((int)Location.X, (int)Location.Y, SquareSize, SquareSize);

            sqrCoord = new Coordinates(x, y, defDist);
        }

        public void Update()
        {
            sqrEdited = false;

            if (rect.Contains(Input.MousePosition.ToPoint()))
            {
                if (Input.LMBDown && typeOfSquare == SqrFlags.Unoccupied && GameRoot.BuildState == GameRoot.BuildStates.Trench)
                {
                    typeOfSquare |= Squares.SqrFlags.Wall;
                    sqrEdited = true;
                }
                else if (Input.LMBDown && typeOfSquare == SqrFlags.Unoccupied && GameRoot.BuildState == GameRoot.BuildStates.Concrete)
                {
                    typeOfSquare = Squares.SqrFlags.Occupied;
                    sqrEdited = true;
                }

                // This will likely be removed, we dont want the player freely destroying shit.
                else if (Input.LMBDown && GameRoot.BuildState == GameRoot.BuildStates.Destroy)
                {
                    typeOfSquare = Squares.SqrFlags.Unoccupied;
                    sqrEdited = true;
                }

                highlight = 0.5f;
            }

            else highlight = 1;
        }

        public void Draw(SpriteBatch sb, Texture2D gridSquareTex)
        {
            if (typeOfSquare.HasFlag(Squares.SqrFlags.Wall))
                sb.Draw(Art.getTrenchTex(TrenchName), rect, Color.White * highlight);
            else if (typeOfSquare.HasFlag(Squares.SqrFlags.Occupied))
                sb.Draw(gridSquareTex, rect, Color.Blue * highlight);
            else if (typeOfSquare.HasFlag(Squares.SqrFlags.Unoccupied))
                sb.Draw(gridSquareTex, rect, Color.White * highlight);
            if (typeOfSquare.HasFlag(Squares.SqrFlags.StopPoint))
                sb.Draw(gridSquareTex, rect, Color.Red * highlight);
        }

        public bool getSquareEdited
        { 
            get{return sqrEdited;}
        }

    }
}
