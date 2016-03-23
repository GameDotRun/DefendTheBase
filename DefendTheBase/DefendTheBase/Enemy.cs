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

    static class TankTurret
    {
        public static List<Projectile> EnemyProjectiles = new List<Projectile>();
        public static Vector2 Update(Enemy enemy)
        {
            enemy.shootTimer -= 1 / 60f;

            //Find closest tower
            Vector2 turretDirection = new Vector2();
            Tower targetTower = null;
            Tower tempTower = null;
            float dist = 300;
            foreach(Tower tower in TowerManager.Towers)
            {
                tempTower = tower;
                if (dist > Vector2.Distance(tempTower.Position, enemy.ScreenPos))
                {
                    dist = Vector2.Distance(tempTower.Position, enemy.ScreenPos);
                    targetTower = tower;

                    enemy.towerInRange = true;
                }
            }
            //Shoot
            if (targetTower != null)
            {
                turretDirection = new Vector2(targetTower.Position.X - enemy.ScreenPos.X, targetTower.Position.Y - enemy.ScreenPos.Y);
                // Shoot

                if (enemy.shootTimer <= 0)
                {
                    enemy.shootTimer = 1;
                    Shoot(targetTower, enemy, turretDirection);
                }
            }

            else enemy.towerInRange = false;

            for (int i = 0; i < EnemyProjectiles.Count(); i++)
                if (EnemyProjectiles[i].TimeSinceSpawn > EnemyProjectiles[i].Lifetime)
                    EnemyProjectiles.RemoveAt(i);

            turretDirection.Normalize();
            return turretDirection;
        }

        static void Shoot(Tower TargetTower, Enemy enemy, Vector2 Direction)
        {
            EnemyProjectiles.Add(new Projectile(Projectile.Type.Gun, TargetTower, enemy.ScreenPos, Direction, 1f, 2));
        }
    
    }

    public class Enemy : Ai
    {
        internal string EnemyID;
        internal string EnemyType;
        protected float speed;

        public float hitPoints;
        
        public Vector2 enemyVect, ScreenPos, Direction, TurretDirection;
        public bool pathFound = false;
        public bool IsDestroyed = false;
        public bool towerInRange = false;
        public bool usingSpriteSheet;
        public float time, shootTimer, animElasped, targetElasped;
        public int spriteSheetNo = 0;
        public int sheetFrameTotal;

        Rectangle SourceRect;

        bool moving = false;

        public Enemy(string enemyID) : base()
        {
            enemyVect = ScreenPos = new Vector2(0, 0);
            EnemyID = enemyID;
            EnemyListener.AddEnemy(this);
            shootTimer = 1;

            animElasped = 0;
        }

        public void Update(Grid.gridFlags endPoint, GameTime gameTime)
        {
            time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            currentCoord = new Coordinates((int)enemyVect.X, (int)enemyVect.Y);

            if (GameRoot.grid.pathFound) // this needs some form of trigger 
            {
               moving = PathMove(GameRoot.grid.gridSquares, GameRoot.HEIGHT, GameRoot.WIDTH, ref enemyVect, ref ScreenPos, speed, time, Direction, EnemyType);
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


            if (EnemyType == "Tank")
            {
                TurretDirection = TankTurret.Update(this);
            }

            Vector2 NextScreenPos = new Vector2((int)GameRoot.grid.gridBorder.X + (nextCoord.x * GameRoot.SQUARESIZE + 0.1f), (int)GameRoot.grid.gridBorder.Y + (nextCoord.y * GameRoot.SQUARESIZE));
            Direction = Movement;

            if (!towerInRange)
                TurretDirection = Direction;

            if (usingSpriteSheet)
            {
                EffectManager.spriteSheetUpdate(ref spriteSheetNo, ref animElasped, targetElasped, sheetFrameTotal, gameTime);

                if(EnemyType == "Soldier")
                    SourceRect = new Rectangle(0, spriteSheetNo * Art.Soldier.Height / 4, Art.Soldier.Width, Art.Soldier.Height / 4);
                else if (EnemyType == "Helicopter")
                    SourceRect = new Rectangle(spriteSheetNo * Art.Helicopter.Width / 2, 0, Art.Helicopter.Width / 2, Art.Helicopter.Height); 
            }

        }

        public void Draw(SpriteBatch sb)
        {
            //Add draw Methods for each enemy here
            if (EnemyType == "Tank")
            {
                sb.Draw(Art.TankBottom, new Vector2(ScreenPos.X, ScreenPos.Y), null, Color.White, Direction.ToAngle(), new Vector2(Art.TankBottom.Width / 2, Art.TankBottom.Height / 2), 1f, SpriteEffects.None, 0);
                sb.Draw(Art.TankTop, new Vector2(ScreenPos.X, ScreenPos.Y), null, Color.White, TurretDirection.ToAngle(), new Vector2(Art.TankTop.Width / 5, Art.TankTop.Height / 2), 1f, SpriteEffects.None, 0);
            }

            else if (EnemyType == "Soldier")
            {
                sb.Draw(Art.Soldier, new Vector2(ScreenPos.X, ScreenPos.Y), SourceRect, Color.White, Direction.ToAngle(), new Vector2(SourceRect.Width /2, SourceRect.Height/2), 1f, SpriteEffects.None, 0);
            
            }

            else if (EnemyType == "Helicopter")
            {
                sb.Draw(Art.Helicopter, new Vector2(ScreenPos.X, ScreenPos.Y), SourceRect, Color.White, Direction.ToAngle(), new Vector2(SourceRect.Width / 2, SourceRect.Height / 2), 1f, SpriteEffects.None, 0);
            }
        }
    }

    class TankEnemy : Enemy
    {
        public string Type = "Tank";

        private float m_hp = 20;
        private float m_speed = 3f;
        private bool spriteSheet = false;

        public TankEnemy(string enemyID)
            : base(enemyID)
        {
            hitPoints = m_hp;
            speed = m_speed;
            EnemyType = Type;
            usingSpriteSheet = spriteSheet;
        }
    }

    class SoldierEnemy : Enemy
    {
        public string Type = "Soldier";

        private float frameSpeed = 100;
        private int frameTotal = 3; // total - 1

        private float m_hp = 2;
        private float m_speed = 2;
      
        private bool spriteSheet = true;
        

        public SoldierEnemy(string enemyID)
            : base(enemyID)
        {
            hitPoints = m_hp;
            speed = m_speed;
            EnemyType = Type;
            usingSpriteSheet = spriteSheet;
            targetElasped = frameSpeed;
            sheetFrameTotal = frameTotal;
        }
   
    }

    class HelicopterEnemy : Enemy
    {
        public string Type = "Helicopter";

        private float frameSpeed = 1;
        private int frameTotal = 1; // total - 1

        private float m_hp = 10;
        // helicopter speed works very differently, as it only heads towards one node currently it goes a lot faster than other units which use multiple nodes. dividing by 10 seems good
        private float m_speed = 5f / 10; 

        
        private bool spriteSheet = true;

        public HelicopterEnemy(string enemyID)
            : base(enemyID)
        {
            hitPoints = m_hp;
            speed = m_speed;
            EnemyType = Type;
            usingSpriteSheet = spriteSheet;
            targetElasped = frameSpeed;
            sheetFrameTotal = frameTotal;
        }
    }

}
