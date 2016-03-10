using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flextensions;

namespace DefendTheBase
{
    public static class EnemyListener
    {
       public static List<Enemy> EnemyList; // allows access of enemies variable pretty much anywhere

       public static void InitiliseListener()
       {
           EnemyList = new List<Enemy>();
       }

       /// <summary>
       /// Add enemies to listener, for towers to interact with them
       /// </summary>
       /// <param name="Enemy"></param>
        public static void AddEnemy(Enemy Enemy) 
        {
            EnemyList.Add(Enemy);
        }


        /// <summary>
        /// Removes enemy from listener. Shouldnt have to be called manually,  should be automted.
        /// </summary>
        /// <param name="EnemyID"></param>
        static public void RemoveEnemy(string EnemyID)
        {
            int index = EnemyList.FindIndex(item => string.Compare(item.EnemyID, EnemyID, 0) == 0);

            if (index >= 0)
                EnemyList.RemoveAt(index);
        }
    }

    /// <summary>
    /// Spawn and destroy enemies
    /// manages all the enemies 
    /// 
    /// </summary>
    public static class EnemyManager
    {
        public static string[] TypeIDs = {"Tank"};
        static List<TankEnemy> TankEnemies = new List<TankEnemy>();

        static List<string> EnemyIDs = new List<string>();

        /// <summary>
        /// Destroys enemies and cleans up references in other lists of said enemy
        /// </summary>
        /// <param name="EnemyID"></param>
        /// <param name="TypeID"></param>
        static void DestroyEnemy(string EnemyID, string TypeID)
        {
            EnemyListener.RemoveEnemy(EnemyID);

            if (TypeID == "Tank")
            {
                int index = TankEnemies.FindIndex(item => string.Compare(item.EnemyID, EnemyID, 0) == 0);

                if (index >= 0)
                    TankEnemies.RemoveAt(index);
            }

            int index2 = EnemyIDs.FindIndex(item => string.Compare(item, EnemyID, 0) == 0);

            if (index2 >= 0)
                EnemyIDs.RemoveAt(index2);


        }

        /// <summary>
        /// spawns enemy of given TypeID
        /// </summary>
        /// <param name="TypeID"></param>
        public static void SpawnEnemy(string TypeID)
        {
            if(TypeID == "Tank")
                TankEnemies.Add(new TankEnemy(CreateID(TypeID)));
        }


        /// <summary>
        /// Updates the enemies and checkes for destroyed enemies
        /// </summary>
        public static void Update()
        {
            foreach (TankEnemy Tank in TankEnemies)
            {
                if (Tank.IsDestroyed)
                {
                    DestroyEnemy(Tank.EnemyID, Tank.Type);
                    break;
                }

                else
                    Tank.Update(GameRoot.grid.gridStatus);
            }
        }

        /// <summary>
        /// draws the enemies
        /// </summary>
        /// <param name="sb"></param>
        public static void Draw(SpriteBatch sb)
        {
            foreach (TankEnemy Tank in TankEnemies)
            {
                Tank.Draw(sb);
            }
        }

        /// <summary>
        /// creates a unique ID for the enemy, if the random ID is not unique it will retry. Chances of this happening more than once are 1/1,000,000
        /// </summary>
        /// <param name="TypeID"></param>
        /// <returns></returns>
        static string CreateID(string TypeID)
        {
            bool IsUnique = false;
            string ID = "";
            while (!IsUnique)
            {
                ID = TypeID + GameRoot.rnd.Next(0, 10).ToString() + GameRoot.rnd.Next(0, 100000).ToString();

                foreach (string id in EnemyIDs)
                    if (id == ID)
                    {
                        IsUnique = false;
                        break;
                    }
                    else IsUnique = true;

                if (EnemyIDs.Count() == 0)
                    IsUnique = true;
                        

            }
                
            EnemyIDs.Add(ID);

            return ID;
        }



    }


    public class Enemy : ai
    {
        internal string EnemyID;

        Texture2D sprite;

        public float hitPoints;
        protected float speed;

        public Coordinates enemyPos;
        public Vector2 enemyVect, ScreenPos, Direction;

        public bool pathFound = false;

        public bool IsDestroyed = false;

        public Enemy(string enemyID) : base(new Coordinates(0,0))
        {
            enemyVect = ScreenPos = new Vector2(0, 0);
            enemyPos = new Coordinates(0, 0);
            sprite = Art.EnemyTex;
            EnemyID = enemyID;

        }

        public void Update(Grid.gridFlags endPoint)
        {
            if (GameRoot.grid.pathFound)
            {
               PathMove(GameRoot.grid.stopPointCoord, GameRoot.grid.gridSquares, GameRoot.HEIGHT, GameRoot.WIDTH, ref enemyVect, speed);
               enemyPos = aiPos;
            }

            if (GameRoot.grid.stopPointCoord != null)
            {
                if (enemyVect.X == GameRoot.grid.stopPointCoord.x && enemyVect.Y == GameRoot.grid.stopPointCoord.y)
                {
                    LevelWaves.WaveEnemiesUsed++;
                    IsDestroyed = true;

                    /*enemyPos = new Coordinates(0, 0);
                    enemyVect = new Vector2(0, 0);
                    PathMoveReset();
                    pathFound = false;
                    aiPos = enemyPos;*/
                }
            }

            if (hitPoints <= 0)
            {
                LevelWaves.WaveEnemiesUsed++;
                // Change this!
                GameManager.ModifyResources(100);
                IsDestroyed = true;
            }

            // Get screen pixel position from Grid Coordinates (enemyVect).
            ScreenPos = new Vector2((int)GameRoot.grid.gridBorder.X + (enemyVect.X * GameRoot.SQUARESIZE), (int)GameRoot.grid.gridBorder.Y + (enemyVect.Y * GameRoot.SQUARESIZE));
            Vector2 NextScreenPos = new Vector2((int)GameRoot.grid.gridBorder.X + (aiPos.x * GameRoot.SQUARESIZE + 0.1f), (int)GameRoot.grid.gridBorder.Y + (aiPos.y * GameRoot.SQUARESIZE));
            Direction = NextScreenPos - ScreenPos;

            EnemyListener.RemoveEnemy(EnemyID);
            EnemyListener.AddEnemy(this);
            
        }
    }


    class TankEnemy : Enemy
    {
        public string Type = "Tank";

        private float m_hp = 20;
        private float m_speed = 10f; // i have no clue how this works, it just does. it was bugged until i divided everything by 100 now it works. wut even. mfw cynical.jpg
        private float m_BottomRotation = 0f;
        private float m_TopRotation = 0f;

        public TankEnemy(string enemyID)
            : base(enemyID)
        {
            hitPoints = m_hp;
            speed = m_speed;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Art.TankBottom, new Vector2(ScreenPos.X + Art.TankBottom.Width / 2, ScreenPos.Y + Art.TankBottom.Height / 2), null, Color.White, Direction.ToAngle() , new Vector2(Art.TankBottom.Width / 2, Art.TankBottom.Height / 2), 1f, SpriteEffects.None, 0);
            sb.Draw(Art.TankTop, new Vector2(ScreenPos.X + Art.TankBottom.Width / 2, ScreenPos.Y + Art.TankBottom.Height / 2), null, Color.White, Direction.ToAngle(), new Vector2(Art.TankTop.Width / 3, Art.TankTop.Height / 2), 1f, SpriteEffects.None, 0);

        }
    
    
    }

}
