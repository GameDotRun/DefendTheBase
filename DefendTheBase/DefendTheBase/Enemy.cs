﻿using Microsoft.Xna.Framework;
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

    public class Enemy : ai
    {
        internal string EnemyID;
        internal string EnemyType;
        protected float speed;

        public float hitPoints;
        
        public Vector2 enemyVect, ScreenPos, Direction;
        public bool pathFound = false;
        public bool IsDestroyed = false;
        public float time;

        bool moving = false;

        public Enemy(string enemyID) : base()
        {
            enemyVect = ScreenPos = new Vector2(0, 0);
            EnemyID = enemyID;
            EnemyListener.AddEnemy(this);
        }

        public void Update(Grid.gridFlags endPoint, GameTime gameTime)
        {
            time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            currentCoord = new Coordinates((int)enemyVect.X, (int)enemyVect.Y);

            if (GameRoot.grid.pathFound) // this needs some form of trigger 
            {
               moving = PathMove(GameRoot.grid.gridSquares, GameRoot.HEIGHT, GameRoot.WIDTH, ref enemyVect, ref ScreenPos, speed, time, Direction);
            }

            if (GameRoot.ENDPOINT != null)
            {
                if (currentCoord.CoordEqual(GameRoot.ENDPOINT))
                {
                    WaveManager.WaveEnemiesUsed++;
                    IsDestroyed = true;
                }
            }

            if (hitPoints <= 0)
            {
                WaveManager.WaveEnemiesUsed++;
                IsDestroyed = true;
            }

            // Get screen pixel position from Grid Coordinates (enemyVect).
            if (moving)
                ScreenPos = new Vector2((int)GameRoot.grid.gridBorder.X + (enemyVect.X * GameRoot.SQUARESIZE) + GameRoot.SQUARESIZE / 2, (int)GameRoot.grid.gridBorder.Y + (enemyVect.Y * GameRoot.SQUARESIZE) + GameRoot.SQUARESIZE / 2);

            else
            {
                enemyVect.X = (float)Math.Round(enemyVect.X);
                enemyVect.Y = (float)Math.Round(enemyVect.Y);
            }

            Vector2 NextScreenPos = new Vector2((int)GameRoot.grid.gridBorder.X + (nextCoord.x * GameRoot.SQUARESIZE + 0.1f), (int)GameRoot.grid.gridBorder.Y + (nextCoord.y * GameRoot.SQUARESIZE));
            Direction = Movement;

        }

        public void Draw(SpriteBatch sb)
        {
            //Add draw Methods for each enemy here
            if (EnemyType == "Tank")
            {
                sb.Draw(Art.TankBottom, new Vector2(ScreenPos.X, ScreenPos.Y), null, Color.White, Direction.ToAngle(), new Vector2(Art.TankBottom.Width / 2, Art.TankBottom.Height / 2), 1f, SpriteEffects.None, 0);
                sb.Draw(Art.TankTop, new Vector2(ScreenPos.X, ScreenPos.Y), null, Color.White, Direction.ToAngle(), new Vector2(Art.TankTop.Width / 3, Art.TankTop.Height / 2), 1f, SpriteEffects.None, 0);
            }
        }
    }

    class TankEnemy : Enemy
    {
        public string Type = "Tank";

        private float m_hp = 20;
        private float m_speed = 3f; // i have no clue how this works, it just does. it was bugged until i divided everything by 100 now it works. wut even. mfw cynical.jpg
        private float m_BottomRotation = 0f;
        private float m_TopRotation = 0f;

        public TankEnemy(string enemyID)
            : base(enemyID)
        {
            hitPoints = m_hp;
            speed = m_speed;
            EnemyType = Type;
        }

        
    
    }

}
