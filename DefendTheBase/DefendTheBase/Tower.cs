using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flextensions;
using Microsoft.Xna.Framework;
using RPGEx;

namespace DefendTheBase
{
    public static class TowerListener
    {
        public static List<Tower> TowersList = new List<Tower>();

        public static void InitiliseListener()
        {
            TowersList = new List<Tower>();
        }

        public static void Add(Tower tower)
        {
            TowersList.Add(tower);
        }

        static public void Remove(string TowerID)
        {
            int index = TowersList.FindIndex(item => string.Compare(item.TowerID, TowerID, 0) == 0);

            if (index >= 0)
                TowersList.RemoveAt(index);
        }
    }


    public class Tower
    {
        // This will contain the type of tower, range, level, health and fireRate.
        // It will also select an appropriate enemy to shoot at within range. Perhaps Closest enemy?

        public int HEALTHDEF = 100;

        internal string TowerID;
        internal string TowerType;

        public enum Type
        {
            Gun,
            Rocket,
            SAM,
            Tesla
        }

        public Texture2D Sprite;
        public Type TypeofTower;
        public List<Projectile> TowerProjectiles;
        public Vector2 Position;
        public float Rotation, FireRate;
        public bool IsActive = true;
        bool rotClock = true;
        public int Level, Range, Health;
        public float Damage;

        public Coordinates towerCoords;

        public UiStatusBars healthBar;

        private float  shootTimer;

        public Tower(string towerID, Type type, Vector2 position, Coordinates coords, int level = 1, int range = 200, int health = 100, int damage = 500, float fireRate = 2f)
        {
            TowerID = towerID;
            TowerListener.Add(this);

            TypeofTower = type;
            TowerProjectiles = new List<Projectile>();
            Rotation = 0;
            Position = position;
            Level = level;
            Range = range;
            Health = health;
            Damage = damage;
            FireRate = fireRate;
            shootTimer = fireRate;

            switch (type)
            {
                case Type.Gun:
                    Sprite = Art.TowerGun[level - 1];
                    Damage = 500;
                    TowerType = "Gun";
                    break;
                case Type.Rocket:
                    Sprite = Art.TowerRocket[level - 1];
                    Damage = 1700;
                    FireRate = 0.5f;
                    TowerType = "Rocket";
                    break;
                case Type.SAM:
                    Sprite = Art.TowerSAM[level - 1];
                    TowerType = "SAM";
                    break;
                case Type.Tesla:
                    Sprite = Art.TowerTesla[level - 1];
                    Damage = 40;
                    FireRate = 100;
                    TowerType = "Tesla";
                    break;
            }

            towerCoords = new Coordinates(coords.x, coords.y);
            healthBar = new UiStatusBars(health, new Vector2(32, 12), Position - new Vector2(Art.TowerGun[0].Width / 2, 20), Art.HpBar[0], Art.HpBar[1]);
        }

        public void LevelUp()
        {
            if (Level < 4)
            {
                Level++;
                switch (TypeofTower)
                {
                    case Type.Gun:
                        Sprite = Art.TowerGun[Level - 1];
                        Range += 25;
                        break;
                    case Type.Rocket:
                        Sprite = Art.TowerRocket[Level - 1];
                        Range += 30;
                        break;
                    case Type.SAM:
                        Sprite = Art.TowerSAM[Level - 1];
                        Range += 75;
                        break;
                    case Type.Tesla:
                        Range += 75;
                        Damage += 40;
                        Sprite = Art.TowerTesla[Level - 1];
                        break;
                }
            }
        }

        public void Shoot(Enemy targetEnemy)
        {
            Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, Rotation);
            Vector2 offset = Vector2.Transform(new Vector2(25, -16), aimQuat);
            Sound.GunShot.Play(GameManager.MASTER_VOL * GameManager.SOUNDFX_VOL * 0.5f, (float)GameManager.rnd.NextDouble(), 0f);
            if (GameManager.rnd.Next(1, 50) == 1) // misfire
            {
                if (TowerType != "Tesla")
                    Health -= 5;
                else Health -= 1;
                PopUpTextManager.Add(new PopUpText(PopUpTextManager.Misfire, Position, Color.Red));
            }

            else
            {
                switch (TypeofTower)
                {
                    case Type.Gun:
                        // Quaternion FTW. Used to Transform offset so bullets spawn correctly even as tower rotates.

                        // Aesthetically pleasing, but fuuuu...
                        if (Level == 1)
                            TowerProjectiles.Add(new Projectile(Projectile.Type.Gun, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                        else if (Level == 2)
                        {
                            TowerProjectiles.Add(new Projectile(Projectile.Type.Gun, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, 16), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.Gun, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                        }
                        else if (Level == 3)
                        {
                            TowerProjectiles.Add(new Projectile(Projectile.Type.Gun, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, 16), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.Gun, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, 0), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.Gun, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                        }
                        else
                        {
                            TowerProjectiles.Add(new Projectile(Projectile.Type.Gun, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, -8), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.Gun, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, 8), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.Gun, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, 16), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.Gun, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                        }
                        break;
                    case Type.Rocket:
                        if (Level == 1)
                            TowerProjectiles.Add(new Projectile(Projectile.Type.Rocket, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                        else if (Level == 2)
                        {
                            TowerProjectiles.Add(new Projectile(Projectile.Type.Rocket, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, 16), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.Rocket, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                        }
                        else if (Level == 3)
                        {
                            TowerProjectiles.Add(new Projectile(Projectile.Type.Rocket, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, 16), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.Rocket, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, 0), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.Rocket, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                        }
                        else
                        {
                            TowerProjectiles.Add(new Projectile(Projectile.Type.Rocket, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, -8), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.Rocket, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, 8), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.Rocket, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, 16), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.Rocket, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                        }
                        break;
                    case Type.SAM:
                        if (Level == 1)
                        {
                            offset = Vector2.Transform(new Vector2(25, -8), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.SAM, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, -4), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.SAM, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                        }
                        else if (Level == 2)
                        {
                            offset = Vector2.Transform(new Vector2(25, -8), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.SAM, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, -4), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.SAM, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, 8), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.SAM, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, 4), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.SAM, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                        }
                        else if (Level == 3)
                        {
                            offset = Vector2.Transform(new Vector2(25, -10), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.SAM, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, -8), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.SAM, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, -2), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.SAM, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, 2), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.SAM, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, 8), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.SAM, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, 10), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.SAM, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                        }
                        else
                        {
                            offset = Vector2.Transform(new Vector2(25, -10), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.SAM, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, -8), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.SAM, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, -6), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.SAM, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, -4), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.SAM, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, 4), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.SAM, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, 6), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.SAM, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, 8), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.SAM, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                            offset = Vector2.Transform(new Vector2(25, 10), aimQuat);
                            TowerProjectiles.Add(new Projectile(Projectile.Type.SAM, targetEnemy, Position + offset, Rotation.ToVector(), 1f, Damage));
                        }
                        break;
                    case Type.Tesla:
                        TowerProjectiles.Add(new Projectile(Projectile.Type.Tesla, targetEnemy, Position, Rotation.ToVector(), 1f, Damage));
                        break;
                }
            }
        }

        public void Update()
        {
            if (IsActive)
            {
                shootTimer += 1 / 60f;
                // Find closest enemy, rotate and shoot.
                List<Enemy> enemyList = EnemyListener.EnemyList;
                Enemy targetEnemy = null;
                Enemy tempEnemy = null;
                float dist = Range;
                int index = 0;
                for (int i = 0; i < enemyList.Count; i++)
                {
                    if ((enemyList[i].EnemyType != "Helicopter" && TypeofTower != Type.SAM) || (TypeofTower == Type.SAM && enemyList[i].EnemyType == "Helicopter"))
                    {
                        tempEnemy = enemyList[i];
                        if (dist > Vector2.Distance(tempEnemy.ScreenPos, this.Position))
                        {

                            dist = Vector2.Distance(tempEnemy.ScreenPos, this.Position);

                            if (TypeofTower == Type.Gun)
                            {
                                if (targetEnemy == null || i < index || targetEnemy.EnemyType != "Soldier")
                                {
                                    index = i;
                                    targetEnemy = enemyList[i];
                                }
                            }

                            if (TypeofTower == Type.Rocket)
                            {
                                if (targetEnemy == null || i < index || (targetEnemy.EnemyType != "Tank" && targetEnemy.EnemyType != "Transport" && targetEnemy.EnemyType != "Helicopter"))
                                {
                                    index = i;
                                    targetEnemy = enemyList[i];
                                }
                            }


                            if (TypeofTower == Type.SAM || TypeofTower == Type.Tesla)
                            {
                                if (targetEnemy == null || i < index)
                                {
                                    index = i;
                                    targetEnemy = enemyList[i];
                                }
                            }
                        }
                    }
                }
                if (targetEnemy != null)
                {
                    // LERPING HERE
                    float nextRotation = Extensions.ToAngle(targetEnemy.ScreenPos - Position);
                    Rotation = Extensions.CurveAngle(Rotation, nextRotation, 0.6f);
                    // Shoot
                    if (shootTimer >= (1f/FireRate))
                    {
                        shootTimer = 0;
                        Shoot(targetEnemy);
                    }
                }
                // If no enemy, rotate back and forth.
                if (Rotation < 0)
                    rotClock = true;
                if (Rotation > 6.2f)
                    rotClock = false;

                if (rotClock)
                    Rotation += 0.02f;
                else
                    Rotation -= 0.02f;
            }
            // Update Projectiles before deleting any.
            foreach (Projectile proj in TowerProjectiles)
                proj.Update();
            // Remove Projectiles after lifetime.
            for (int i = 0; i < TowerProjectiles.Count(); i++)
                if (TowerProjectiles[i].TimeSinceSpawn > TowerProjectiles[i].Lifetime)
                    TowerProjectiles.RemoveAt(i);

            healthBar.Update(Health);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Sprite, Position, null, Color.White, Rotation, new Vector2(GameManager.SQUARESIZE / 2, GameManager.SQUARESIZE / 2), 1f, SpriteEffects.None, 0f);
            DrawProjectiles(sb);
        }

        public void DrawHpBar(SpriteBatch sb)
        {
            healthBar.Draw(sb);
        }

        public void DrawProjectiles(SpriteBatch sb)
        {
            foreach (Projectile proj in TowerProjectiles)
                proj.Draw(sb);

            if(TankTurret.EnemyProjectiles != null)
                foreach (Projectile proj in TankTurret.EnemyProjectiles)
                    proj.Draw(sb);
        }
    }
}
