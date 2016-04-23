using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Flextensions;
namespace DefendTheBase
{
    class TroopListener
    {
        public static List<Troop> TroopList; // allows access of enemies variable pretty much anywhere

        public static void InitiliseListener()
        {
            TroopList = new List<Troop>();
        }

        /// <summary>
        /// Add enemies to listener, for towers to interact with them
        /// </summary>
        /// <param name="Enemy"></param>
        public static void AddTroop(Troop troop)
        {
            TroopList.Add(troop);
        }


        /// <summary>
        /// Removes enemy from listener. Shouldnt have to be called manually,  should be automted.
        /// </summary>
        /// <param name="EnemyID"></param>
        static public void RemoveTroop(string troopID)
        {
            int index = TroopList.FindIndex(item => string.Compare(item.troopID, troopID, 0) == 0);

            if (index >= 0)
                TroopList.RemoveAt(index);
        }
    
    
    
    }

    class Troop : FriendlyAi
    {
        public string troopID;

        public float hitPoints, speed, animElasped, time;
        public bool usingSpriteSheet;
        public string TroopType;

        public int sheetFrameTotal;
        public float targetElasped;

        Vector2 ScreenPos, TroopVect, Direction;
        Rectangle SourceRect;

        public int spriteSheetNo = 0;

        bool moving = false;

        public Troop(string TroopID)
        {
            troopID = TroopID;
            TroopVect = new Vector2(GameManager.ENDPOINT.x, GameManager.ENDPOINT.y);
            ScreenPos = new Vector2(GameManager.ENDPOINT.x * GameManager.SQUARESIZE, GameManager.ENDPOINT.y * GameManager.SQUARESIZE);
            TroopListener.AddTroop(this);
        
        }

        public void Update(GameTime gameTime)
        {
            time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            currentCoord = new Coordinates((int)TroopVect.X, (int)TroopVect.Y);

            moving = PathMove(GameManager.grid.gridSquares, GameManager.HEIGHT, GameManager.WIDTH, ref TroopVect, ref ScreenPos, speed, time, Direction);

            if (moving)
                ScreenPos = new Vector2((int)GameManager.grid.gridBorder.X + (TroopVect.X * GameManager.SQUARESIZE) + GameManager.SQUARESIZE / 2, (int)GameManager.grid.gridBorder.Y + (TroopVect.Y * GameManager.SQUARESIZE) + GameManager.SQUARESIZE / 2);

            else
            {
                TroopVect.X = (float)Math.Round(TroopVect.X);
                TroopVect.Y = (float)Math.Round(TroopVect.Y);
            }

            Vector2 NextScreenPos = new Vector2((int)GameManager.grid.gridBorder.X + (nextCoord.x * GameManager.SQUARESIZE + 0.1f), (int)GameManager.grid.gridBorder.Y + (nextCoord.y * GameManager.SQUARESIZE));
            Direction = Movement;

            if (usingSpriteSheet)
            {
                EffectManager.spriteSheetUpdate(ref spriteSheetNo, ref animElasped, targetElasped, sheetFrameTotal, gameTime);
                SourceRect = new Rectangle(0, spriteSheetNo * Art.FriendlySoldier.Height / (sheetFrameTotal + 1), Art.FriendlySoldier.Width, Art.FriendlySoldier.Height / (sheetFrameTotal + 1));
                
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if(TroopType == "Infantry")
                sb.Draw(Art.FriendlySoldier, new Vector2(ScreenPos.X, ScreenPos.Y), SourceRect, Color.White, Direction.ToAngle(), new Vector2(SourceRect.Width / 2, SourceRect.Height / 2), 1f, SpriteEffects.None, 0);
            else if (TroopType == "Mechanic")
                sb.Draw(Art.FriendlyMechanic, new Vector2(ScreenPos.X, ScreenPos.Y), SourceRect, Color.White, Direction.ToAngle(), new Vector2(SourceRect.Width / 2, SourceRect.Height / 2), 1f, SpriteEffects.None, 0);
        }

    }

    class infantry : Troop
    { 
    
        public string Type = "Infantry";

        private float frameSpeed = 100;
        private int frameTotal = 2; // total - 1

        private float m_hp = 50;
        private float m_speed = 2;
      
        private bool spriteSheet = true;


        public infantry(string TroopID) 
            : base(TroopID)
        {
            hitPoints = m_hp;
            speed = m_speed;
            TroopType = Type;
            usingSpriteSheet = spriteSheet;
            targetElasped = frameSpeed;
            sheetFrameTotal = frameTotal;
        }
   
    
    }

    class Mechanic : Troop
    { 
        public string Type = "Mechanic";

        private float frameSpeed = 100;
        private int frameTotal = 2; // total - 1

        private float m_hp = 50;
        private float m_speed = 2;
      
        private bool spriteSheet = true;


        public Mechanic(string TroopID) 
            : base(TroopID)
        {
            hitPoints = m_hp;
            speed = m_speed;
            TroopType = Type;
            usingSpriteSheet = spriteSheet;
            targetElasped = frameSpeed;
            sheetFrameTotal = frameTotal;
        }
    
    
    
    }

}
