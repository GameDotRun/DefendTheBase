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
        static public void RemoveEnemy(Enemy EnemyID)
        {
            EnemyList.Remove(EnemyID);
               
        }
    }

    static class TankTurret
    {
        public static List<Projectile> EnemyProjectiles = new List<Projectile>();
        static float Rotation;
        public static Vector2 Update(Enemy enemy)
        {
            enemy.shootTimer -= 1 / 60f;

            //Find closest tower
            Vector2 turretDirection = new Vector2();
            Tower targetTower = null;
            Tower tempTower = null;
            float dist = 300;
            foreach(Tower tower in TowerListener.TowersList)
            {
                tempTower = tower;
                if (dist > Vector2.Distance(tempTower.Position, enemy.ScreenPos))
                {
                    dist = Vector2.Distance(tempTower.Position, enemy.ScreenPos);

                    if(dist > 100)
                        targetTower = tower;

                    enemy.towerInRange = true;
                }
            }
            //Shoot
            if (targetTower != null)
            {
                // LERPING HERE
                turretDirection = new Vector2(targetTower.Position.X - enemy.ScreenPos.X, targetTower.Position.Y - enemy.ScreenPos.Y);
                float nextRotation = turretDirection.ToAngle();
                Rotation = Extensions.CurveAngle(Rotation, nextRotation, 0.3f);
                turretDirection = Rotation.ToVector();
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

        public Texture2D Top;
        public Texture2D Bottom;

        //attributes
        protected float speed;
        public float hitPoints;
        public float criticalResist;
        public float resistance;

        public Vector2 enemyVect, ScreenPos, Direction, TurretDirection;
        public bool pathFound = false;
        public bool IsDestroyed = false;
        public bool towerInRange = false;
        public bool usingSpriteSheet;
        public float time, shootTimer, animElasped, targetElasped, turretRotation;
        public int spriteSheetNo = 0;
        public int sheetFrameTotal;

        Rectangle SourceRect;

        bool moving = false;

        public float damage;

        public Enemy(string enemyID, Vector2 enemyVector) : base()
        {
            ScreenPos = new Vector2(0, GameManager.grid.gridBorder.Y + GameManager.SQUARESIZE / 2);
            enemyVect = enemyVector;
            EnemyID = enemyID;
            EnemyListener.AddEnemy(this);
            shootTimer = 1;

            animElasped = 0;
        }

        public void Update(Grid.gridFlags endPoint, GameTime gameTime)
        {
            time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //check for out of bounds, only really occurs when they are spawned from a destroyed vehicle as the vehicle is turning
            if (enemyVect.X >= 20)
                enemyVect.X = 19;
            if (enemyVect.Y >= 15)
                enemyVect.Y = 14;
            if (enemyVect.X < 0)
                enemyVect.X = 0;
            if (enemyVect.Y < 0)
                enemyVect.Y = 0;

            currentCoord = new Coordinates((int)enemyVect.X, (int)enemyVect.Y);

            if(enemyVect != null)
                moving = PathMove(GameManager.grid.gridSquares, GameManager.HEIGHT, GameManager.WIDTH, ref enemyVect, ref ScreenPos, speed, time, Direction, EnemyType);

            if (GameManager.ENDPOINT != null)
            {
                if (currentCoord.CoordEqual(GameManager.ENDPOINT))
                {
                    IsDestroyed = true;
                    GameManager.BaseHealth -= damage;
                }
            }

            if (hitPoints <= 0)
            {
                IsDestroyed = true;
            }

            // Get screen pixel position from Grid Coordinates (enemyVect).
            if (moving)
                ScreenPos = new Vector2((int)GameManager.grid.gridBorder.X + (enemyVect.X * GameManager.SQUARESIZE) + GameManager.SQUARESIZE / 2, (int)GameManager.grid.gridBorder.Y + (enemyVect.Y * GameManager.SQUARESIZE) + GameManager.SQUARESIZE / 2);

            else
            {
                enemyVect.X = (float)Math.Round(enemyVect.X);
                enemyVect.Y = (float)Math.Round(enemyVect.Y);
            }


            if (EnemyType == "Tank" || EnemyType == "Jeep")
            {
                TurretDirection = TankTurret.Update(this);
            }

            Vector2 NextScreenPos = new Vector2((int)GameManager.grid.gridBorder.X + (nextCoord.x * GameManager.SQUARESIZE + 0.1f), (int)GameManager.grid.gridBorder.Y + (nextCoord.y * GameManager.SQUARESIZE));
            Direction = Movement;

            if (!towerInRange)
            {
                float nextTurretDirection = Direction.ToAngle();
                // LERPING HERE
                turretRotation = Extensions.CurveAngle(turretRotation, nextTurretDirection, 0.3f);
                TurretDirection = turretRotation.ToVector();
            }

            if (usingSpriteSheet)
            {
                EffectManager.spriteSheetUpdate(ref spriteSheetNo, ref animElasped, targetElasped, sheetFrameTotal, gameTime);

                if(EnemyType == "Soldier")
                    SourceRect = new Rectangle(0, spriteSheetNo * Art.Soldier.Height / (sheetFrameTotal + 1), Art.Soldier.Width, Art.Soldier.Height / (sheetFrameTotal + 1));
                else if (EnemyType == "Helicopter")
                    SourceRect = new Rectangle(spriteSheetNo * Art.Helicopter.Width / (sheetFrameTotal + 1) , 0, Art.Helicopter.Width / (sheetFrameTotal + 1), Art.Helicopter.Height); 
            }

        }

        public void Draw(SpriteBatch sb)
        {
            //Add draw Methods for each enemy here
            if (EnemyType == "Tank")
            {
                sb.Draw(Bottom, new Vector2(ScreenPos.X, ScreenPos.Y), null, Color.White, Direction.ToAngle(), new Vector2(Art.TankBottom.Width / 2, Art.TankBottom.Height / 2), 1f, SpriteEffects.None, 0);
                sb.Draw(Top, new Vector2(ScreenPos.X, ScreenPos.Y), null, Color.White, TurretDirection.ToAngle(), new Vector2(Art.TankTop.Width / 5, Art.TankTop.Height / 2), 1f, SpriteEffects.None, 0);
            }

            else if (EnemyType == "Transport")
            {
                sb.Draw(Bottom, new Vector2(ScreenPos.X, ScreenPos.Y), null, Color.White, Direction.ToAngle(), new Vector2(Art.Transport.Width / 2, Art.Transport.Height / 2), 1f, SpriteEffects.None, 0);
            }

            else if (EnemyType == "Jeep")
            {
                sb.Draw(Bottom, new Vector2(ScreenPos.X, ScreenPos.Y), null, Color.White, Direction.ToAngle(), new Vector2(Art.JeepBottom.Width / 2, Art.JeepBottom.Height / 2), 1f, SpriteEffects.None, 0);
                sb.Draw(Top, new Vector2(ScreenPos.X - Direction.X * Art.JeepBottom.Width / 3, ScreenPos.Y - Direction.Y * Art.JeepBottom.Width / 3), null, Color.White, TurretDirection.ToAngle(), new Vector2(Art.JeepTop.Width / 4, Art.JeepTop.Height / 2), 1f, SpriteEffects.None, 0);
            }

            else if (EnemyType == "Soldier")
            {
                sb.Draw(Bottom, new Vector2(ScreenPos.X, ScreenPos.Y), SourceRect, Color.White, Direction.ToAngle(), new Vector2(SourceRect.Width / 2, SourceRect.Height / 2), 1f, SpriteEffects.None, 0);

            }

            else if (EnemyType == "Helicopter")
            {
                sb.Draw(Bottom, new Vector2(ScreenPos.X, ScreenPos.Y), SourceRect, Color.White, Direction.ToAngle(), new Vector2(SourceRect.Width / 2, SourceRect.Height / 2), 1f, SpriteEffects.None, 0);
            }
        }
    }

    

}
