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

       //dont give this random vectors. things will go wrong
        public static void AddEnemy(Enemy Enemy) 
        {
            EnemyList.Add(Enemy);
        }

        static public void RemoveEnemy(string EnemyID)
        {
            int index = EnemyList.FindIndex(item => string.Compare(item.EnemyID, EnemyID, 0) == 0);

            if (index >= 0)
                EnemyList.RemoveAt(index);
        }
    }

    public static class EnemyCreator
    {
        public static string[] TypeIDs = {"Tank"};
        static List<TankEnemy> TankEnemies = new List<TankEnemy>();

        static List<string> EnemyIDs = new List<string>();

        static void DestroyEnemy(string EnemyID, string TypeID)
        {
            EnemyListener.RemoveEnemy(EnemyID);

            if (TypeID == "Tank")
            {
                int index = TankEnemies.FindIndex(item => string.Compare(item.EnemyID, EnemyID, 0) == 0);

                if (index >= 0)
                    TankEnemies.RemoveAt(index);
            }
        }

        public static void SpawnEnemy(string TypeID)
        {
            if(TypeID == "Tank")
                TankEnemies.Add(new TankEnemy(CreateID(TypeID)));
        }

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

        public static void Draw(SpriteBatch sb)
        {
            foreach (TankEnemy Tank in TankEnemies)
            {
                Tank.Draw(sb);
            }
        }


        static string CreateID(string TypeID)
        {
            string ID = TypeID + TankEnemies.Count().ToString() + 1;

            EnemyIDs.Add(ID);

            return ID;
        }



    }


    public class Enemy : ai
    {
        internal string EnemyID;

        Texture2D sprite;

        protected float hitPoints;
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
            if (!pathFound && endPoint.HasFlag(Grid.gridFlags.endPoint))
            {
                FindPathReset();
                pathFound = FindPath(GameRoot.grid.stopPointCoord, GameRoot.grid.gridSquares, GameRoot.HEIGHT, GameRoot.WIDTH);
            }

            if (pathFound)
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
