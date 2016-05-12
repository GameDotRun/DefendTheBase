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
        public static string[] TypeIDs = { "Tank", "Soldier", "Helicopter", "Jeep", "Transport", "TankBlue", "TankRed", "SoldierBlue", "SoldierRed", "TransportBlue", "TransportRed", "JeepBlue", "JeepRed" };

        public static string SpawnSoldierString = "Soldier";

        static HashSet<Enemy> Enemies = new HashSet<Enemy>();
        static HashSet<Enemy> EnemiesToAdd = new HashSet<Enemy>();
        static HashSet<Enemy> EnemiesToRemove = new HashSet<Enemy>();


        static List<string> EnemyIDs = new List<string>();

        public static void Init()
        {
             Enemies.Clear();
             EnemiesToAdd.Clear();
             EnemiesToRemove.Clear();

             /*HashSet<Enemy> Enemies = new HashSet<Enemy>();
             HashSet<Enemy> EnemiesToAdd = new HashSet<Enemy>();
             HashSet<Enemy> EnemiesToRemove = new HashSet<Enemy>();*/

            
        }

        /// <summary>
        /// Destroys enemies and cleans up references in other lists of said enemy
        /// </summary>
        static void DestroyEnemy(Enemy enemy, string TypeID, string EnemyID)
        {
            EnemyListener.RemoveEnemy(enemy);

            Enemies.Remove(enemy);

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
                EnemiesToAdd.Add(new TankEnemy(CreateID(TypeID), enemyVector));
            else if (TypeID == "Soldier")
                EnemiesToAdd.Add(new SoldierEnemy(CreateID(TypeID), enemyVector));
            else if (TypeID == "Helicopter")
                EnemiesToAdd.Add(new HelicopterEnemy(CreateID(TypeID), enemyVector));
            else if (TypeID == "Jeep")
                EnemiesToAdd.Add(new JeepEnemy(CreateID(TypeID), enemyVector));
            else if (TypeID == "Transport")
                EnemiesToAdd.Add(new TransportEnemy(CreateID(TypeID), enemyVector));
            else if (TypeID == "TankBlue")
                EnemiesToAdd.Add(new TankEnemyBlue(CreateID(TypeID), enemyVector));
            else if (TypeID == "SoldierBlue")
                EnemiesToAdd.Add(new SoldierEnemyBlue(CreateID(TypeID), enemyVector));
            else if (TypeID == "JeepBlue")
                EnemiesToAdd.Add(new JeepEnemyBlue(CreateID(TypeID), enemyVector));
            else if (TypeID == "TransportBlue")
                EnemiesToAdd.Add(new TransportEnemyBlue(CreateID(TypeID), enemyVector));
            else if (TypeID == "TankRed")
                EnemiesToAdd.Add(new TankEnemyRed(CreateID(TypeID), enemyVector));
            else if (TypeID == "SoldierRed")
                EnemiesToAdd.Add(new SoldierEnemyRed(CreateID(TypeID), enemyVector));
            else if (TypeID == "JeepRed")
                EnemiesToAdd.Add(new JeepEnemyRed(CreateID(TypeID), enemyVector));
            else if (TypeID == "TransportRed")
                EnemiesToAdd.Add(new TransportEnemyRed(CreateID(TypeID), enemyVector));
        }

        /// <summary>
        /// Updates the enemies and checks for destroyed enemies
        /// </summary>
        public static void Update(GameTime gt)
        {
            if (WaveManager.WaveNumber > 10)
                SpawnSoldierString = "SoldierBlue";


            if (WaveManager.WaveNumber > 20)
                SpawnSoldierString = "SoldierRed";

            foreach (Enemy Enemy in Enemies)
            {
                foreach (Projectile proj in TankTurret.EnemyProjectiles) //updates the projectiles of enemy
                    proj.Update();
                // Remove Projectiles after lifetime.
                for (int i = 0; i < TankTurret.EnemyProjectiles.Count(); i++)
                    if (TankTurret.EnemyProjectiles[i].TimeSinceSpawn > TankTurret.EnemyProjectiles[i].Lifetime)
                        TankTurret.EnemyProjectiles.RemoveAt(i);
                if (Enemy.IsDestroyed)
                {
                    if (Enemy.hitPoints <= 0) // check if it was destroyed by means of towers
                    {
                        WaveManager.EnemiesKilled++;

                        GameManager.EnemyWasDestroyed(Enemy.EnemyType); // resource acquisition 
                        if (Enemy.EnemyType == "Helicopter")
                        {
                            EffectManager.EffectCall(EffectManager.EffectEnums.Explosion, Enemy.ScreenPos - new Vector2((Art.Helicopter.Width / 4) / 2 , Art.Helicopter.Height / 2), true);
                            Sound.Explosion.Play(GameManager.MASTER_VOL * GameManager.SOUNDFX_VOL * 0.5f, 0f, 0f);
                        }

                        if (Enemy.EnemyType == "Transport")
                        {
                            for (float i = 0; i < 4; i++)
                            {

                                SpawnEnemy(SpawnSoldierString, Enemy.enemyVect - new Vector2(Enemy.Direction.X * -i / 4, Enemy.Direction.Y * -i / 4));
                                EffectManager.EffectCall(EffectManager.EffectEnums.Explosion, Enemy.ScreenPos - new Vector2(Art.Transport.Width / 2, Art.Transport.Height / 2), true);
                                Sound.Explosion.Play(GameManager.MASTER_VOL * GameManager.SOUNDFX_VOL * 0.5f, 0f, 0f);
                            }
                        }

                        else if (Enemy.EnemyType == "Tank")
                        {
                            for (float i = 0; i < 2; i++)
                            {

                                SpawnEnemy(SpawnSoldierString, Enemy.enemyVect - new Vector2(Enemy.Direction.X * -i / 4, Enemy.Direction.Y * -i / 4));
                                EffectManager.EffectCall(EffectManager.EffectEnums.Explosion, Enemy.ScreenPos - new Vector2(Art.TankBottom.Width / 2, Art.TankBottom.Height / 2), true);
                                Sound.Explosion.Play(GameManager.MASTER_VOL * GameManager.SOUNDFX_VOL * 0.5f, 0f, 0f);
                            }
                        }

                        else if (Enemy.EnemyType == "Jeep")
                        {
                            for (float i = 0; i < 1; i++)
                            {

                                SpawnEnemy(SpawnSoldierString, Enemy.enemyVect - new Vector2(Enemy.Direction.X * -i / 4, Enemy.Direction.Y * -i / 4));
                                EffectManager.EffectCall(EffectManager.EffectEnums.Explosion, Enemy.ScreenPos - new Vector2(Art.JeepBottom.Width / 2, Art.JeepBottom.Height / 2), true);
                                Sound.Explosion.Play(GameManager.MASTER_VOL * GameManager.SOUNDFX_VOL * 0.5f, 0f, 0f);
                            }
                        }

                        else if (Enemy.EnemyType == "Soldier")
                        {
                            EffectManager.EffectCall(EffectManager.EffectEnums.Blood, Enemy.ScreenPos + new Vector2(GameManager.rnd.Next(-5, 6), GameManager.rnd.Next(-5, 6)), false);

                            if(GameManager.rnd.Next(1, 100) < 25)
                                Sound.WilhelmScream.Play(GameManager.MASTER_VOL * GameManager.SOUNDFX_VOL * 0.5f, 0f, 0f);
                        }
                    }

                    //DestroyEnemy(Enemy, Enemy.EnemyType, Enemy.EnemyID); 
                    EnemiesToRemove.Add(Enemy);
                }

                if (float.IsNaN(Enemy.enemyVect.X) || float.IsNaN(Enemy.enemyVect.Y)) // This needs testing...
                {
                    //DestroyEnemy(Enemy, Enemy.EnemyType, Enemy.EnemyID);
                    EnemiesToRemove.Add(Enemy);
                    //WaveManager.WaveEnemiesUsed++;
                }

                else
                    Enemy.Update(GameManager.grid.gridStatus, gt); //updates the enemy

            }

            foreach (Enemy enemy in EnemiesToAdd)
            {
                Enemies.Add(enemy);

            }

            foreach (Enemy enemy in EnemiesToRemove)
            {
                DestroyEnemy(enemy, enemy.EnemyType, enemy.EnemyID);

            }

            EnemiesToAdd.Clear();
            EnemiesToRemove.Clear();

            
        }



        /// <summary>
        /// draws the enemies
        /// </summary>
        /// <param name="sb"></param>
        public static void Draw(SpriteBatch sb)
        {
            foreach (Enemy Enemy in Enemies)
            {
                if(Enemy.EnemyType != "Helicopter")
                    Enemy.Draw(sb);
            }

            foreach (Enemy Enemy in Enemies)
            {
                if (Enemy.EnemyType == "Helicopter")
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

        public static void EnemyDamaged(float Damage, Enemy enemy, Projectile.Type projectile)
        {
            enemy.hitPoints -= GameManager.DamageCalculator(Damage, enemy, projectile);
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
