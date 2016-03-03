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
            StopPoint = 32,
            Concrete = 64
        }

        public enum BuildingType
        {
            None,
            Concrete,
            Trench,
            Tower,
            Base
        }

        public Tower TowerHere;

        public string TrenchName;
        public SqrFlags typeOfSquare;
        public BuildingType Building;
        public Coordinates sqrCoord;
        public Rectangle rect;
        public Vector2 sqrLoc;

        public bool sqrEdited = false;

        float highlight;

        public Squares(int SquareSize, Vector2 Location, int x, int y, int defDist)
        {
            sqrLoc = Location;

            typeOfSquare = SqrFlags.Unoccupied;
            Building = BuildingType.None;
            TowerHere = null;

            rect = new Rectangle((int)Location.X, (int)Location.Y, SquareSize, SquareSize);

            sqrCoord = new Coordinates(x, y, defDist);
        }

        public void Update()
        {
            sqrEdited = false;

            if (rect.Contains(Input.MousePosition.ToPoint()))
            {
                // Build Trench
                if (Input.LMBDown && Building == BuildingType.None && GameRoot.BuildState == GameRoot.BuildStates.Trench)
                {
                    typeOfSquare |= Squares.SqrFlags.Wall;
                    Building = BuildingType.Trench;
                    sqrEdited = true;
                }
                // Build Concrete
                else if (Input.LMBDown && Building == BuildingType.None && GameRoot.BuildState == GameRoot.BuildStates.Concrete)
                {
                    typeOfSquare = Squares.SqrFlags.Occupied;
                    typeOfSquare = SqrFlags.Concrete;
                    Building = BuildingType.Concrete;
                    sqrEdited = true;
                }
                // Build Gun Tower
                else if (Input.LMBDown && Building == BuildingType.Concrete && GameRoot.BuildState == GameRoot.BuildStates.TowerGun)
                {
                    typeOfSquare = Squares.SqrFlags.Occupied;
                    typeOfSquare |= Squares.SqrFlags.Wall;
                    typeOfSquare |= SqrFlags.Concrete;
                    Building = BuildingType.Tower;
                    TowerHere = new Tower(Tower.Type.Gun);
                    sqrEdited = true;
                }
                // Build Rocket Tower
                else if (Input.LMBDown && Building == BuildingType.Concrete && GameRoot.BuildState == GameRoot.BuildStates.TowerRocket)
                {
                    typeOfSquare = Squares.SqrFlags.Occupied;
                    typeOfSquare |= Squares.SqrFlags.Wall;
                    typeOfSquare |= SqrFlags.Concrete;
                    Building = BuildingType.Tower;
                    TowerHere = new Tower(Tower.Type.Rocket);
                    sqrEdited = true;
                }
                else if (Input.WasLMBClicked && Building == BuildingType.Tower && GameRoot.BuildState == GameRoot.BuildStates.Upgrade)
                {
                    TowerHere.LevelUp();
                    sqrEdited = true;
                }

                // This will likely be removed, we dont want the player freely destroying shit.
                else if (Input.LMBDown && GameRoot.BuildState == GameRoot.BuildStates.Destroy)
                {
                    typeOfSquare = Squares.SqrFlags.Unoccupied;
                    Building = BuildingType.None;
                    sqrEdited = true;
                }

                highlight = 0.5f;
            }

            else highlight = 1;
        }

        public void Draw(SpriteBatch sb, Texture2D gridSquareTex)
        {
            if (typeOfSquare.HasFlag(SqrFlags.Concrete))
                sb.Draw(Art.Concrete, rect, Color.White * highlight);
            if (Building == BuildingType.Trench)
                sb.Draw(Art.getTrenchTex(TrenchName), rect, Color.White * highlight);
            else if (Building == BuildingType.Concrete)
                sb.Draw(Art.Concrete, rect, Color.White * highlight);
            else if (Building == BuildingType.None)
                sb.Draw(gridSquareTex, rect, Color.White * highlight);
            else if (Building == BuildingType.Base)
                sb.Draw(gridSquareTex, rect, Color.Red * highlight);
            else if (Building == BuildingType.Tower)
                sb.Draw(TowerHere.Sprite, rect, Color.White * highlight);
        }

        public bool getSquareEdited
        { 
            get{return sqrEdited;}
        }

    }
}
