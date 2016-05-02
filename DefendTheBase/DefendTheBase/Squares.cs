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

        public Texture2D ghostImage = Art.GroundTexs;
        Color ghostCol;
        float highlight;

        bool canClick = true;

        string squareInfo = "A grass patch, build a trench here or concrete for tower foundations!";

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

            if (sqrCoord.counter == GameManager.DEFAULYDIST)
            {
                ghostImage = Art.BlockedSquare;
                squareInfo = "A blocked of square, solve this problem by building a trench or a tower here!";
            }

            if (Building != BuildingType.Base)
            {
                if (!QuestionPopUpManager.QuestionUp)
                {
                    if (!sqrCoord.CoordEqual(GameManager.STARTPOINT))
                    {
                        if (rect.Contains(Input.MousePosition.ToPoint()))
                        {

                            if (GameManager.HelpMode)
                                if (!HelpDialogManager.Hovering)
                                    HelpDialogManager.Hovering = true;

                            GameManager.mouseSqrCoords = new Coordinates(sqrCoord.x, sqrCoord.y);

                            if (Building == BuildingType.None && GameManager.BuildState == GameManager.BuildStates.Trench)
                            {
                                ghostImage = Art.getTrenchTex(GameManager.grid.sqrTexDecider((int)sqrCoord.x, (int)sqrCoord.y));
                                squareInfo = "A grass patch, build a trench here or concrete for tower foundations!";
                            }

                            if (Building == BuildingType.Concrete)
                            {
                                squareInfo = "Build a tower on this concrete!";
                            }

                            if (Building == BuildingType.Trench)
                            {
                                squareInfo = "A trench";
                            }

                            if (Building == BuildingType.Tower)
                            {
                                squareInfo = "A tower";
                            }

                            if (Input.WasLMBClicked && canClick && !WaveManager.WaveStarted)
                            {
                                BuildManager.Build();
                                sqrEdited = true;
                            }


                            if (GameManager.HelpMode)
                            {
                                HelpDialogManager.Add(new HelpDialog(squareInfo, Input.MousePosition));

                            }

                            highlight = 0.5f;
                        }

                        else highlight = 1;
                    }
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
            if (typeOfSquare.HasFlag(SqrFlags.Concrete) && sqrCoord.counter == GameManager.DEFAULYDIST && Building == BuildingType.Concrete)
                sb.Draw(Art.ConcreteBlocked, rect, ghostCol * highlight);
            if (Building == BuildingType.Trench)
                sb.Draw(Art.getTrenchTex(TrenchName), rect, ghostCol * highlight);
            else if (Building == BuildingType.None)
                sb.Draw(ghostImage, rect, ghostCol * highlight);
            //else if (Building == BuildingType.Base)
            //    sb.Draw(Art.Base, new Rectangle(rect.X, rect.Y, rect.Width*2, rect.Height*2), Color.White);
        }

        public void DrawBase(SpriteBatch sb)
        {
            if (Building == BuildingType.Base)
                sb.Draw(Art.Base, new Rectangle(rect.X, rect.Y, rect.Width * 2, rect.Height * 2), Color.White);
        }

        //public void DrawTowers(SpriteBatch sb)
        //{
        //    if (Building == BuildingType.Tower)
        //    {
        //        sb.Draw(TowerHere.Sprite, new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2), null, Color.White, TowerHere.Rotation, new Vector2(rect.Width / 2, rect.Height / 2), 1f, SpriteEffects.None, 0f);
        //        TowerHere.DrawProjectiles(sb);
        //    }
        //}

        
        public bool getSquareEdited
        { 
            get{return sqrEdited;}
        }

    }
}
