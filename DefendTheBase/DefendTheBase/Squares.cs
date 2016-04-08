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
        public Vector2 PixelScreenPos;

        public bool sqrEdited = false;

        public Texture2D ghostImage;
        Color ghostCol;
        float highlight;

        bool canClick = true;

        public Squares(int SquareSize, Vector2 Location, int x, int y, int defDist)
        {
            sqrLoc = Location;
            
            typeOfSquare = SqrFlags.Unoccupied;
            Building = BuildingType.None;
            TowerHere = null;

            rect = new Rectangle((int)Location.X, (int)Location.Y, SquareSize, SquareSize);

            sqrCoord = new Coordinates(x, y, defDist);

            PixelScreenPos = new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
        }

        public void Update()
        {
            ghostImage = Art.GroundTexs;
            ghostCol = Color.White;
            sqrEdited = false;

            if (Input.WasLMBClicked)
                canClick = true;

            if (!sqrCoord.CoordEqual(GameRoot.STARTPOINT))
            {
                if (rect.Contains(Input.MousePosition.ToPoint()))
                {
                    GameManager.mouseSqrCoords = new Coordinates(sqrCoord.x, sqrCoord.y);

                    if (this.HasNeighbour(BuildingType.Trench))
                    {
                        if (Building == BuildingType.None && GameManager.BuildState == GameManager.BuildStates.Trench)
                        {
                            ghostImage = Art.getTrenchTex(GameRoot.grid.sqrTexDecider((int)sqrCoord.x, (int)sqrCoord.y));
                        }

                        if (Input.LMBDown && canClick && Building != BuildingType.Tower && Building != BuildingType.Trench && !WaveManager.WaveStarted)
                        {
                            BuildManager.Build();
                            sqrEdited = true;
                        }
                    }

                    if (Input.WasLMBClicked && (GameManager.BuildState == GameManager.BuildStates.Upgrade || GameManager.BuildState == GameManager.BuildStates.Destroy) && !WaveManager.WaveStarted)
                        BuildManager.Build();

                    highlight = 0.5f;
                }

                else highlight = 1;
            }
            
        }

        public void Draw(SpriteBatch sb, Texture2D gridSquareTex)
        {
            if (Building == BuildingType.Concrete && GameManager.BuildState != GameManager.BuildStates.Concrete)
                sb.Draw(ghostImage, rect, ghostCol * highlight);
            if (typeOfSquare.HasFlag(SqrFlags.Concrete))
                sb.Draw(Art.Concrete, rect, ghostCol * highlight);
            if (Building == BuildingType.Trench)
                sb.Draw(Art.getTrenchTex(TrenchName), rect, ghostCol * highlight);
            else if (Building == BuildingType.None)
                sb.Draw(ghostImage, rect, ghostCol * highlight);
            else if (Building == BuildingType.Base)
                sb.Draw(gridSquareTex, rect, Color.Red * highlight);
        }

        //public void DrawTowers(SpriteBatch sb)
        //{
        //    if (Building == BuildingType.Tower)
        //    {
        //        sb.Draw(TowerHere.Sprite, new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2), null, Color.White, TowerHere.Rotation, new Vector2(rect.Width / 2, rect.Height / 2), 1f, SpriteEffects.None, 0f);
        //        TowerHere.DrawProjectiles(sb);
        //    }
        //}

        public bool HasNeighbour(BuildingType typeOfBuilding)
        {
            // Check North
            if (this.sqrCoord.y > 0)
                if (GameRoot.grid.gridSquares[(int)this.sqrCoord.x, (int)this.sqrCoord.y - 1].Building == typeOfBuilding)
                    return true;
            // Check East
            if (this.sqrCoord.x < GameRoot.WIDTH - 1)
                if (GameRoot.grid.gridSquares[(int)this.sqrCoord.x + 1, (int)this.sqrCoord.y].Building == typeOfBuilding)
                    return true;
            // Check South
            if (this.sqrCoord.y < GameRoot.HEIGHT - 1)
                if (GameRoot.grid.gridSquares[(int)this.sqrCoord.x, (int)this.sqrCoord.y + 1].Building == typeOfBuilding)
                    return true;
            // Check West
            if (this.sqrCoord.x > 0)
                if (GameRoot.grid.gridSquares[(int)this.sqrCoord.x - 1, (int)this.sqrCoord.y].Building == typeOfBuilding)
                    return true;
            return false;
        }

        public bool getSquareEdited
        { 
            get{return sqrEdited;}
        }

    }
}
