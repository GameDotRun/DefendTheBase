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
        public float Rotation;
        public bool IsActive = true;
        bool rotClock = true;
        public int Level, Range, Health, Damage, fireRate;

        private float timeToShoot, shootTimer;

        public Tower(Type type, Vector2 position, int level = 1, int range = 100, int health = 100, int damage = 10, int fireRate = 1)
        {
            TypeofTower = type;
            TowerProjectiles = new List<Projectile>();
            Rotation = 0;
            Position = position;
            Level = level;
            Range = range;
            Health = health;
            Damage = damage;
            timeToShoot = fireRate;
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
                Level++;
            switch (TypeofTower)
            {
                case Type.Gun:
                    Sprite = Art.TowerGun[Level - 1];
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

        public void Update()
        {
            if (IsActive)
            {
                // Find enemy, rotate and shoot.
                //Rotation = GameRoot.enemy.ScreenPos.ToAngle();

                // Shoot
                shootTimer += 1 / 60f;
                if (shootTimer >= timeToShoot)
                {
                    shootTimer = 0;
                    TowerProjectiles.Add(new Projectile(Projectile.Type.Gun, (int)Position.X, (int)Position.Y, Rotation.ToVector(), 5f));
                }

                // Remove Projectiles after lifetime.
                for (int i = 0; i < TowerProjectiles.Count(); i++)
                {
                    TowerProjectiles[i].Update();
                    if (TowerProjectiles[i].TimeSinceSpawn > TowerProjectiles[i].Lifetime)
                        TowerProjectiles.RemoveAt(i);
                }

                // If no enemy, rotate back and forth.
                if (Rotation > 6.2f || Rotation < 0)
                    rotClock = !rotClock;
                if (rotClock)
                    Rotation += 0.02f;
                else
                    Rotation -= 0.02f;
            }
        }

        public void DrawProjectiles(SpriteBatch sb)
        {
            foreach (Projectile proj in TowerProjectiles)
                proj.Draw(sb);
        }
    }
}
