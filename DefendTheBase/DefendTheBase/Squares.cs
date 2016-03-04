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

        float highlight;

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
            sqrEdited = false;

            if (rect.Contains(Input.MousePosition.ToPoint()))
            {
                if (this.HasNeighbour(BuildingType.Trench))
                {
                    // Build Trench
                    if (Input.LMBDown && Building == BuildingType.None && GameRoot.BuildState == GameRoot.BuildStates.Trench)
                    {
                        typeOfSquare |= Squares.SqrFlags.Wall;
                        Building = BuildingType.Trench;
                        sqrEdited = true;
                    }
                    // Build Gun Tower
                    else if (Input.LMBDown && Building == BuildingType.Concrete && GameRoot.BuildState == GameRoot.BuildStates.TowerGun)
                    {
                        typeOfSquare = Squares.SqrFlags.Occupied;
                        typeOfSquare |= Squares.SqrFlags.Wall;
                        typeOfSquare |= SqrFlags.Concrete;
                        Building = BuildingType.Tower;
                        TowerHere = new Tower(Tower.Type.Gun, PixelScreenPos);
                        sqrEdited = true;
                    }
                    // Build Rocket Tower
                    else if (Input.LMBDown && Building == BuildingType.Concrete && GameRoot.BuildState == GameRoot.BuildStates.TowerRocket)
                    {
                        typeOfSquare = Squares.SqrFlags.Occupied;
                        typeOfSquare |= Squares.SqrFlags.Wall;
                        typeOfSquare |= SqrFlags.Concrete;
                        Building = BuildingType.Tower;
                        TowerHere = new Tower(Tower.Type.Rocket, PixelScreenPos);
                        sqrEdited = true;
                    }
                    // Build SAM Tower
                    else if (Input.LMBDown && Building == BuildingType.Concrete && GameRoot.BuildState == GameRoot.BuildStates.TowerSAM)
                    {
                        typeOfSquare = Squares.SqrFlags.Occupied;
                        typeOfSquare |= Squares.SqrFlags.Wall;
                        typeOfSquare |= SqrFlags.Concrete;
                        Building = BuildingType.Tower;
                        TowerHere = new Tower(Tower.Type.SAM, PixelScreenPos);
                        sqrEdited = true;
                    }
                    // Build Tesla Tower
                    else if (Input.LMBDown && Building == BuildingType.Concrete && GameRoot.BuildState == GameRoot.BuildStates.TowerTesla)
                    {
                        typeOfSquare = Squares.SqrFlags.Occupied;
                        typeOfSquare |= Squares.SqrFlags.Wall;
                        typeOfSquare |= SqrFlags.Concrete;
                        Building = BuildingType.Tower;
                        TowerHere = new Tower(Tower.Type.Tesla, PixelScreenPos);
                        sqrEdited = true;
                    }

                }
                // Build Concrete
                if (Input.LMBDown && Building == BuildingType.None && GameRoot.BuildState == GameRoot.BuildStates.Concrete)
                {
                    typeOfSquare = Squares.SqrFlags.Occupied;
                    typeOfSquare = SqrFlags.Concrete;
                    Building = BuildingType.Concrete;
                    sqrEdited = true;
                }
                // Upgrade Tower
                if (Input.WasLMBClicked && Building == BuildingType.Tower && GameRoot.BuildState == GameRoot.BuildStates.Upgrade)
                {
                    TowerHere.LevelUp();
                    sqrEdited = true;
                }

                // This will likely be removed, we dont want the player freely destroying shit.
                else if (Input.WasLMBClicked && GameRoot.BuildState == GameRoot.BuildStates.Destroy)
                {
                    if (Building == BuildingType.Concrete || Building == BuildingType.Trench)
                    {
                        typeOfSquare = Squares.SqrFlags.Unoccupied;
                        Building = BuildingType.None;
                        sqrEdited = true;
                    }

                    else if (Building == BuildingType.Tower)
                    {
                        typeOfSquare = Squares.SqrFlags.Occupied;
                        typeOfSquare = Squares.SqrFlags.Concrete;
                        Building = BuildingType.Concrete;
                        TowerHere = null;
                        sqrEdited = true;
                    }
                }

                highlight = 0.5f;
            }

            else highlight = 1;
            // Update the Tower if there is one and we are active. We MUST also be next to a trench.
            if (TowerHere != null)
            {
                if (HasNeighbour(BuildingType.Trench))
                    TowerHere.IsActive = true;
                else
                    TowerHere.IsActive = false;
                TowerHere.Update();
            }
        }

        public void Draw(SpriteBatch sb, Texture2D gridSquareTex)
        {
            if (typeOfSquare.HasFlag(SqrFlags.Concrete))
                sb.Draw(Art.Concrete, rect, Color.White * highlight);
            if (Building == BuildingType.Trench)
                sb.Draw(Art.getTrenchTex(TrenchName), rect, Color.White * highlight);
            else if (Building == BuildingType.None)
                sb.Draw(gridSquareTex, rect, Color.White * highlight);
            else if (Building == BuildingType.Base)
                sb.Draw(gridSquareTex, rect, Color.Red * highlight);
        }

        public void DrawTowers(SpriteBatch sb)
        {
            if (Building == BuildingType.Tower)
            {
                sb.Draw(TowerHere.Sprite, new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2), null, Color.White, TowerHere.Rotation, new Vector2(rect.Width / 2, rect.Height / 2), 1f, SpriteEffects.None, 0f);
                TowerHere.DrawProjectiles(sb);
            }
        }

        public bool HasNeighbour(BuildingType typeOfBuilding)
        {
            // Check North
            if (this.sqrCoord.y > 0)
                if (GameRoot.grid.gridSquares[this.sqrCoord.x, this.sqrCoord.y - 1].Building == typeOfBuilding)
                    return true;
            // Check East
            if (this.sqrCoord.x < GameRoot.WIDTH - 1)
                if (GameRoot.grid.gridSquares[this.sqrCoord.x + 1, this.sqrCoord.y].Building == typeOfBuilding)
                    return true;
            // Check South
            if (this.sqrCoord.y < GameRoot.HEIGHT - 1)
                if (GameRoot.grid.gridSquares[this.sqrCoord.x, this.sqrCoord.y + 1].Building == typeOfBuilding)
                    return true;
            // Check West
            if (this.sqrCoord.x > 0)
                if (GameRoot.grid.gridSquares[this.sqrCoord.x - 1, this.sqrCoord.y].Building == typeOfBuilding)
                    return true;
            return false;
        }

        public bool getSquareEdited
        { 
            get{return sqrEdited;}
        }

    }
}
