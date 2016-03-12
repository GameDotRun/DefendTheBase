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

        Texture2D ghostImage;
        Color ghostCol;
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
            ghostImage = Art.GroundTexs;
            ghostCol = Color.White;
            sqrEdited = false;

            if (rect.Contains(Input.MousePosition.ToPoint()))
            {
                if (this.HasNeighbour(BuildingType.Trench))
                {
                    // Build Trenchh
                    if (Building == BuildingType.None && GameManager.BuildState == GameManager.BuildStates.Trench)
                    {
                        ghostImage = Art.getTrenchTex(GameRoot.grid.sqrTexDecider(sqrCoord.x, sqrCoord.y));
                        if (Input.LMBDown)
                        {
                            if (GridManager.InaccessibleSquareCheck(GameRoot.grid.gridSquares, sqrCoord))
                            {
                                typeOfSquare |= Squares.SqrFlags.Wall;
                                Building = BuildingType.Trench;
                                sqrEdited = true;
                            }
                            sqrEdited = true;
                        }
                    }
                    
                    // Build Gun Tower
                    else if (Building == BuildingType.Concrete && GameManager.BuildState == GameManager.BuildStates.TowerGun)
                    {
                        ghostImage = Art.TowerGun[0];
                        if (GameManager.Manpower > 2 && GameManager.Resources > 100)
                        {
                            if (Input.LMBDown )
                            {
                                typeOfSquare = Squares.SqrFlags.Occupied;
                                typeOfSquare |= Squares.SqrFlags.Wall;
                                typeOfSquare |= SqrFlags.Concrete;
                                Building = BuildingType.Tower;
                                TowerHere = new Tower(Tower.Type.Gun, PixelScreenPos);
                                sqrEdited = true;
                                GameManager.ModifyManpower(-2f);
                                GameManager.ModifyResources(-100);
                            }
                        }
                        else
                            ghostCol = Color.Red;
                    }
                    // Build Rocket Tower
                    else if (Building == BuildingType.Concrete && GameManager.BuildState == GameManager.BuildStates.TowerRocket)
                    {
                        ghostImage = Art.TowerRocket[0];
                        if (GameManager.Manpower > 4 && GameManager.Resources > 300)
                        {
                            if (Input.LMBDown)
                            {
                                typeOfSquare = Squares.SqrFlags.Occupied;
                                typeOfSquare |= Squares.SqrFlags.Wall;
                                typeOfSquare |= SqrFlags.Concrete;
                                Building = BuildingType.Tower;
                                TowerHere = new Tower(Tower.Type.Rocket, PixelScreenPos);
                                sqrEdited = true;
                                GameManager.ModifyManpower(-4f);
                                GameManager.ModifyResources(-300);
                            }
                        }
                        else
                            ghostCol = Color.Red;
                    }
                    // Build SAM Tower
                    else if (Building == BuildingType.Concrete && GameManager.BuildState == GameManager.BuildStates.TowerSAM)
                    {
                        ghostImage = Art.TowerSAM[0];
                        if (GameManager.Manpower > 3 && GameManager.Resources > 400)
                        {
                            if (Input.LMBDown)
                            {
                                typeOfSquare = Squares.SqrFlags.Occupied;
                                typeOfSquare |= Squares.SqrFlags.Wall;
                                typeOfSquare |= SqrFlags.Concrete;
                                Building = BuildingType.Tower;
                                TowerHere = new Tower(Tower.Type.SAM, PixelScreenPos);
                                sqrEdited = true;
                                GameManager.ModifyManpower(-3f);
                                GameManager.ModifyResources(-400);
                            }
                        }
                        else
                            ghostCol = Color.Red;
                    }
                    // Build Tesla Tower
                    else if (Building == BuildingType.Concrete && GameManager.BuildState == GameManager.BuildStates.TowerTesla)
                    {
                        ghostImage = Art.TowerTesla[0];
                        if (GameManager.Manpower > 3 && GameManager.Resources > 500)
                        {
                            if (Input.LMBDown)
                            {
                                typeOfSquare = Squares.SqrFlags.Occupied;
                                typeOfSquare |= Squares.SqrFlags.Wall;
                                typeOfSquare |= SqrFlags.Concrete;
                                Building = BuildingType.Tower;
                                TowerHere = new Tower(Tower.Type.Tesla, PixelScreenPos);
                                sqrEdited = true;
                                GameManager.ModifyManpower(-3f);
                                GameManager.ModifyResources(-500);
                            }
                        }
                        else
                            ghostCol = Color.Red;
                    }
                }
                // Build Concrete
                if (Building == BuildingType.None && GameManager.BuildState == GameManager.BuildStates.Concrete)
                {
                    ghostImage = Art.Concrete;
                    if (GameManager.Resources >= 10 && GameManager.Manpower >= 0.5f)
                    {
                        ghostCol = Color.White;
                        if (Input.LMBDown)
                        {
                            typeOfSquare = Squares.SqrFlags.Occupied;
                            typeOfSquare = SqrFlags.Concrete;
                            Building = BuildingType.Concrete;
                            sqrEdited = true;
                            GameManager.ModifyManpower(-0.5f);
                            GameManager.ModifyResources(-10);
                        }
                    }
                    else
                        ghostCol = Color.Red;
                }
                // Upgrade Tower
                if (Building == BuildingType.Tower && GameManager.BuildState == GameManager.BuildStates.Upgrade)
                {
                    if (TowerHere.Level < 4 && TowerHere.TypeofTower == Tower.Type.Gun && GameManager.Manpower >= 1f && GameManager.Resources >= 100)
                    {
                        if (Input.WasLMBClicked)
                        {
                            TowerHere.LevelUp();
                            GameManager.ModifyManpower(-1f);
                            GameManager.ModifyResources(-100);
                        }
                    }
                    else if (TowerHere.Level < 4 && TowerHere.TypeofTower == Tower.Type.Rocket && GameManager.Manpower >= 2f && GameManager.Resources >= 200)
                    {
                        if (Input.WasLMBClicked)
                        {
                            TowerHere.LevelUp();
                            GameManager.ModifyManpower(-2f);
                            GameManager.ModifyResources(-200);
                        }
                    }
                    else if (TowerHere.Level < 4 && TowerHere.TypeofTower == Tower.Type.SAM && GameManager.Manpower >= 2f && GameManager.Resources >= 400)
                    {
                        if (Input.WasLMBClicked)
                        {
                            TowerHere.LevelUp();
                            GameManager.ModifyManpower(-2f);
                            GameManager.ModifyResources(-400);
                        }
                    }
                    else if (TowerHere.Level < 4 && TowerHere.TypeofTower == Tower.Type.Tesla && GameManager.Manpower >= 1f && GameManager.Resources >= 200)
                    {
                        if (Input.WasLMBClicked)
                        {
                            TowerHere.LevelUp();
                            GameManager.ModifyManpower(-1f);
                            GameManager.ModifyResources(-200);
                        }
                    }
                    else
                        ghostCol = Color.Red;
                    sqrEdited = true;
                }

                // This will likely be removed, we dont want the player freely destroying shit.
                else if (GameManager.BuildState == GameManager.BuildStates.Destroy)
                {
                    if (Building == BuildingType.Concrete && GameManager.Manpower > 1)
                    {
                        if (Input.WasLMBClicked)
                        {
                            typeOfSquare = Squares.SqrFlags.Unoccupied;
                            Building = BuildingType.None;
                            GameManager.ModifyManpower(-1f);
                            GameManager.ModifyResources(5);
                        }
                    }
                    else if (Building == BuildingType.Trench && GameManager.Manpower > 1 && GameManager.Resources > 10)
                    {
                        if (Input.WasLMBClicked)
                        {
                            typeOfSquare = Squares.SqrFlags.Unoccupied;
                            Building = BuildingType.None;
                            GameManager.ModifyManpower(-1f);
                            GameManager.ModifyResources(-10);
                        }
                    }

                    else if (Building == BuildingType.Tower && GameManager.Manpower > 1)
                    {
                        if (Input.WasLMBClicked)
                        {
                            if (TowerHere.TypeofTower == Tower.Type.Gun)
                            {
                                GameManager.ModifyManpower(-1f);
                                GameManager.ModifyResources(50);
                            }
                            if (TowerHere.TypeofTower == Tower.Type.Rocket)
                            {
                                GameManager.ModifyManpower(-1f);
                                GameManager.ModifyResources(150);
                            }
                            if (TowerHere.TypeofTower == Tower.Type.SAM)
                            {
                                GameManager.ModifyManpower(-1f);
                                GameManager.ModifyResources(200);
                            }
                            if (TowerHere.TypeofTower == Tower.Type.Tesla)
                            {
                                GameManager.ModifyManpower(-1f);
                                GameManager.ModifyResources(250);
                            }
                            typeOfSquare = Squares.SqrFlags.Occupied;
                            typeOfSquare = Squares.SqrFlags.Concrete;
                            Building = BuildingType.Concrete;
                            TowerHere = null;
                        }
                    }
                    else
                        ghostCol = Color.Red;
                    sqrEdited = true;
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
