using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DefendTheBase
{
    /// <summary>
    /// Spawn and destroy enemies
    /// manages all the enemies 
    /// 
    /// </summary>
    public static class EnemyManager
    {
        public static string[] TypeIDs = { "Tank", "Soldier", "Helicopter", "Jeep", "Transport" };
        static List<Enemy> Enemies = new List<Enemy>();
        static List<string> EnemyIDs = new List<string>();

        /// <summary>
        /// Destroys enemies and cleans up references in other lists of said enemy
        /// </summary>
        static void DestroyEnemy(string EnemyID, string TypeID)
        {
            EnemyListener.RemoveEnemy(EnemyID);

            int index = Enemies.FindIndex(item => string.Compare(item.EnemyID, EnemyID, 0) == 0);

            if (index >= 0)
                Enemies.RemoveAt(index);

            int index2 = EnemyIDs.FindIndex(item => string.Compare(item, EnemyID, 0) == 0);

            if (index2 >= 0)
                EnemyIDs.RemoveAt(index2);
        }

        /// <summary>
        /// spawns enemy of given TypeID
        /// </summary>
        public static void SpawnEnemy(string TypeID, Vector2 enemyVector)
        {
            if (TypeID == "Tank")
                Enemies.Add(new TankEnemy(CreateID(TypeID), enemyVector));
            else if (TypeID == "Soldier")
                Enemies.Add(new SoldierEnemy(CreateID(TypeID), enemyVector));
            else if (TypeID == "Helicopter")
                Enemies.Add(new HelicopterEnemy(CreateID(TypeID), enemyVector));
            else if (TypeID == "Jeep")
                Enemies.Add(new JeepEnemy(CreateID(TypeID), enemyVector));
            else if (TypeID == "Transport")
                Enemies.Add(new TransportEnemy(CreateID(TypeID), enemyVector));
        }

        /// <summary>
        /// Updates the enemies and checks for destroyed enemies
        /// </summary>
        public static void Update(GameTime gt)
        {
            foreach (Enemy Enemy in Enemies)
            {
                if (Enemy.IsDestroyed)
                {
                    WaveManager.WaveEnemiesUsed++;

                    if (Enemy.hitPoints <= 0) // check if it was destroyed by means of towers
                    {
                        GameManager.EnemyWasDestroyed(Enemy.EnemyType); // resource acquisition 

                        if (Enemy.EnemyType == "Transport")
                        {
                            for (float i = 0; i < 4; i++)
                            {
                                WaveManager.WaveEnemiesUsed--;
                                SpawnEnemy("Soldier", Enemy.enemyVect - new Vector2(Enemy.Direction.X * -i / 4, Enemy.Direction.Y * -i / 4));
                            }
                        }

                        else if (Enemy.EnemyType == "Tank")
                        {
                            for (float i = 0; i < 2; i++)
                            {
                                WaveManager.WaveEnemiesUsed--;
                                SpawnEnemy("Soldier", Enemy.enemyVect - new Vector2(Enemy.Direction.X * -i / 4, Enemy.Direction.Y * -i / 4));
                            }
                        }

                        else if (Enemy.EnemyType == "Jeep")
                        {
                            for (float i = 0; i < 1; i++)
                            {
                                WaveManager.WaveEnemiesUsed--;
                                SpawnEnemy("Soldier", Enemy.enemyVect - new Vector2(Enemy.Direction.X * -i / 4, Enemy.Direction.Y * -i / 4));
                            }
                        }

                        else if (Enemy.EnemyType == "Soldier")
                        {
                            EffectManager.EffectCall(EffectManager.EffectEnums.Blood, Enemy.ScreenPos, false);
                        
                        }
                    }

                    DestroyEnemy(Enemy.EnemyID, Enemy.EnemyType); 
                    break;
                }

                else
                    Enemy.Update(GameManager.grid.gridStatus, gt); //updates the enemy
            }

            foreach (Projectile proj in TankTurret.EnemyProjectiles) //updates the projectiles of enemy
                proj.Update();
        }



        /// <summary>
        /// draws the enemies
        /// </summary>
        /// <param name="sb"></param>
        public static void Draw(SpriteBatch sb)
        {
            foreach (Enemy Enemy in Enemies)
            {
                Enemy.Draw(sb);
            }
        }

        public static void ResetEnemyAI()
        {
            foreach (Enemy Enemy in Enemies)
            {
                Enemy.tempInt = GameManager.DEFAULYDIST;
            }

        }

        public static void EnemyDamaged(int Damage, Enemy enemy, Projectile.Type projectile)
        {
            //Damage calcualtions should be put here. Not sure on the best way to handle this, could get very large with if's
            enemy.hitPoints -= Damage;
        }
        /// <summary>
        /// creates a unique ID for the enemy, if the random ID is not unique it will retry. Chances of this happening more than once are 1/1,000,000
        /// </summary>
        static string CreateID(string TypeID)
        {
            bool IsUnique = false;
            string ID = "";
            while (!IsUnique)
            {
                ID = TypeID + GameManager.rnd.Next(0, 10).ToString() + GameManager.rnd.Next(0, 100000).ToString();

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

}
