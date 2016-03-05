using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flextensions;
using Microsoft.Xna.Framework;

namespace DefendTheBase
{
    public class Tower
    {
        // This will contain the type of tower, range, level, health and fireRate.
        // It will also select an appropriate enemy to shoot at within range. Perhaps Closest enemy?

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
        public int Level, Range, Health, Damage;

        private float  shootTimer;

        public Tower(Type type, Vector2 position, int level = 1, int range = 400, int health = 100, int damage = 10, int fireRate = 1)
        {
            TypeofTower = type;
            TowerProjectiles = new List<Projectile>();
            Rotation = 0;
            Position = position;
            Level = level;
            Range = range;
            Health = health;
            Damage = damage;
            FireRate = fireRate;
            shootTimer = 0f;
            switch (type)
            {
                case Type.Gun:
                    Sprite = Art.TowerGun[level - 1];
                    break;
                case Type.Rocket:
                    Sprite = Art.TowerRocket[level - 1];
                    break;
                case Type.SAM:
                    Sprite = Art.TowerSAM[level - 1];
                    break;
                case Type.Tesla:
                    Sprite = Art.TowerTesla[level - 1];
                    break;
            }
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
                        FireRate -= 0.25f;
                        break;
                    case Type.Rocket:
                        Sprite = Art.TowerRocket[Level - 1];
                        break;
                    case Type.SAM:
                        Sprite = Art.TowerSAM[Level - 1];
                        break;
                    case Type.Tesla:
                        Sprite = Art.TowerTesla[Level - 1];
                        break;
                }
            }
        }

        public void Shoot()
        {
            switch (TypeofTower)
            {
                case Type.Gun:
                    // Quaternion FTW. Used to Transform offset so bullets spawn correctly even as tower rotates.
                    Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, Rotation);
                    Vector2 offset = Vector2.Transform(new Vector2(25, -16), aimQuat);
                    // Aesthetically pleasing, but fuuuu...
                    if (Level == 1)
                        TowerProjectiles.Add(new Projectile(Projectile.Type.Gun, Position + offset, Rotation.ToVector(), 1f));
                    else if (Level == 2)
                    {
                        TowerProjectiles.Add(new Projectile(Projectile.Type.Gun, Position + offset, Rotation.ToVector(), 1f));
                        offset = Vector2.Transform(new Vector2(25, 16), aimQuat);
                        TowerProjectiles.Add(new Projectile(Projectile.Type.Gun, Position + offset, Rotation.ToVector(), 1f));
                    }
                    else if (Level == 3)
                    {
                        TowerProjectiles.Add(new Projectile(Projectile.Type.Gun, Position + offset, Rotation.ToVector(), 1f));
                        offset = Vector2.Transform(new Vector2(25, 16), aimQuat);
                        TowerProjectiles.Add(new Projectile(Projectile.Type.Gun, Position + offset, Rotation.ToVector(), 1f));
                        offset = Vector2.Transform(new Vector2(25, 0), aimQuat);
                        TowerProjectiles.Add(new Projectile(Projectile.Type.Gun, Position + offset, Rotation.ToVector(), 1f));
                    }
                    else
                    {
                        TowerProjectiles.Add(new Projectile(Projectile.Type.Gun, Position + offset, Rotation.ToVector(), 1f));
                        offset = Vector2.Transform(new Vector2(25, -8), aimQuat);
                        TowerProjectiles.Add(new Projectile(Projectile.Type.Gun, Position + offset, Rotation.ToVector(), 1f));
                        offset = Vector2.Transform(new Vector2(25, 8), aimQuat);
                        TowerProjectiles.Add(new Projectile(Projectile.Type.Gun, Position + offset, Rotation.ToVector(), 1f));
                        offset = Vector2.Transform(new Vector2(25, 16), aimQuat);
                        TowerProjectiles.Add(new Projectile(Projectile.Type.Gun, Position + offset, Rotation.ToVector(), 1f));
                    }
                    break;
                case Type.Rocket:
                    TowerProjectiles.Add(new Projectile(Projectile.Type.Rocket, Position, Rotation.ToVector(), 0f));
                    break;
                case Type.SAM:
                    TowerProjectiles.Add(new Projectile(Projectile.Type.SAM, Position, Rotation.ToVector(), 0f));
                    break;
                case Type.Tesla:
                    TowerProjectiles.Add(new Projectile(Projectile.Type.Tesla, Position, Rotation.ToVector(), 0f));
                    break;
            }
        }

        public void Update()
        {
            if (IsActive)
            {
                // Find enemy, rotate and shoot.
                //Rotation = GameRoot.enemy.ScreenPos.ToAngle();

                List<Enemy> enemyList = EnemyListener.EnemyList;
                Enemy targetEnemy = null;
                for (int i = 0; i < enemyList.Count; i++)
                {
                    float dist = Range;
                    Enemy tempEnemy = enemyList[i];
                    if (dist > Vector2.Distance(tempEnemy.ScreenPos, this.Position))
                    {
                        dist = Vector2.Distance(tempEnemy.ScreenPos, this.Position);
                        targetEnemy = enemyList[i];
                    }
                    
                }
                if (targetEnemy != null)
                {
                    Rotation = Extensions.ToAngle(targetEnemy.ScreenPos - Position);
                    // Shoot
                    shootTimer += 1 / 60f;
                    if (shootTimer >= FireRate)
                    {
                        shootTimer = 0;
                        Shoot();
                    }
                }

                // If no enemy, rotate back and forth.
                if (Rotation > 6.2f || Rotation < 0)
                    rotClock = !rotClock;
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
        }

        public void DrawProjectiles(SpriteBatch sb)
        {
            foreach (Projectile proj in TowerProjectiles)
                proj.Draw(sb);
        }
    }
}
