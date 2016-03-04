using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flextensions;

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
        public float Rotation;
        public bool IsActive = true;
        bool rotClock = true;
        public int Level, Range, Health, Damage, fireRate;

        public Tower(Type type, int level = 1, int range = 100, int health = 100, int damage = 10, int fireRate = 1)
        {
            TypeofTower = type;
            Rotation = 0;
            Level = level;
            Range = range;
            Health = health;
            Damage = damage;
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



                // If no enemy, rotate back and forth.
                if (Rotation > 6.2f || Rotation < 0)
                    rotClock = !rotClock;
                if (rotClock)
                    Rotation += 0.02f;
                else
                    Rotation -= 0.02f;
            }
        }
    }
}
