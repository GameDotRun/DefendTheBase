﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public int Level, Range, Health;

        public Tower(Type type, int level = 1, int range = 100, int health = 100)
        {
            TypeofTower = type;
            Level = level;
            Range = range;
            Health = health;
            switch (type)
            {
                case Type.Gun:
                    Sprite = Art.TowerGun[level - 1];
                    break;
                case Type.Rocket:
                    Sprite = Art.TowerRocket[level - 1];
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
            }
        }

        public void Update()
        {
            // Find enemy, rotate and shoot.
        }
    }
}
