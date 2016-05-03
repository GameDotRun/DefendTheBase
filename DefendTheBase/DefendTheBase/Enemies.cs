using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DefendTheBase
{
    class TankEnemy : Enemy
    {
        private Texture2D textureTop = Art.TankTop;
        private Texture2D textureBottom = Art.TankBottom;

        public string Type = "Tank";

        private float m_resistance = 75;
        private float m_criticalResist = 80;
        private float m_hp = 500;
        private float m_speed = 3f;
        private float m_damage = 5f;
        private bool spriteSheet = false;

        public TankEnemy(string enemyID, Vector2 enemyVector)
            : base(enemyID, enemyVector)
        {
            resistance = m_resistance;
            criticalResist = m_criticalResist;
            hitPoints = m_hp;
            speed = m_speed;
            EnemyType = Type;
            usingSpriteSheet = spriteSheet;
            damage = m_damage;
            Top = textureTop;
            Bottom = textureBottom;
        }
    }


    class TankEnemyBlue : Enemy
    {
        private Texture2D textureTop = Art.TankTopBlue;
        private Texture2D textureBottom = Art.TankBottomBlue;

        public string Type = "Tank";

        private float m_resistance = 80;
        private float m_criticalResist = 85;
        private float m_hp = 1000;
        private float m_speed = 3f;
        private float m_damage = 10f;
        private bool spriteSheet = false;

        public TankEnemyBlue(string enemyID, Vector2 enemyVector)
            : base(enemyID, enemyVector)
        {
            resistance = m_resistance;
            criticalResist = m_criticalResist;
            hitPoints = m_hp;
            speed = m_speed;
            EnemyType = Type;
            usingSpriteSheet = spriteSheet;
            damage = m_damage;
            Top = textureTop;
            Bottom = textureBottom;
        }
    }

    class TankEnemyRed : Enemy
    {
        private Texture2D textureTop = Art.TankTopRed;
        private Texture2D textureBottom = Art.TankBottomRed;

        public string Type = "Tank";

        private float m_resistance = 90;
        private float m_criticalResist = 90;
        private float m_hp = 2000;
        private float m_speed = 3f;
        private float m_damage = 5f;
        private bool spriteSheet = false;

        public TankEnemyRed(string enemyID, Vector2 enemyVector)
            : base(enemyID, enemyVector)
        {
            resistance = m_resistance;
            criticalResist = m_criticalResist;
            hitPoints = m_hp;
            speed = m_speed;
            EnemyType = Type;
            usingSpriteSheet = spriteSheet;
            damage = m_damage;
            Top = textureTop;
            Bottom = textureBottom;
        }
    }

    class JeepEnemy : Enemy
    {
        private Texture2D textureTop = Art.JeepTop;
        private Texture2D textureBottom = Art.JeepBottom;

        public string Type = "Jeep";

        private float m_resistance = 30;
        private float m_criticalResist = 40;
        private float m_hp = 150;
        private float m_speed = 5f;
        private float m_damage = 1f;
        private bool spriteSheet = false;

        public JeepEnemy(string enemyID, Vector2 enemyVector)
            : base(enemyID, enemyVector)
        {
            resistance = m_resistance;
            criticalResist = m_criticalResist;
            hitPoints = m_hp;
            speed = m_speed;
            EnemyType = Type;
            usingSpriteSheet = spriteSheet;
            damage = m_damage;
            Top = textureTop;
            Bottom = textureBottom;
        }
    }


    class JeepEnemyBlue : Enemy
    {
        private Texture2D textureTop = Art.JeepTopBlue;
        private Texture2D textureBottom = Art.JeepBottomBlue;

        public string Type = "Jeep";

        private float m_resistance = 30;
        private float m_criticalResist = 40;
        private float m_hp = 300;
        private float m_speed = 5f;
        private float m_damage = 2f;
        private bool spriteSheet = false;

        public JeepEnemyBlue(string enemyID, Vector2 enemyVector)
            : base(enemyID, enemyVector)
        {
            resistance = m_resistance;
            criticalResist = m_criticalResist;
            hitPoints = m_hp;
            speed = m_speed;
            EnemyType = Type;
            usingSpriteSheet = spriteSheet;
            damage = m_damage;
            Top = textureTop;
            Bottom = textureBottom;
        }
    }

    class JeepEnemyRed : Enemy
    {
        private Texture2D textureTop = Art.JeepTopRed;
        private Texture2D textureBottom = Art.JeepBottomRed;

        public string Type = "Jeep";

        private float m_resistance = 30;
        private float m_criticalResist = 40;
        private float m_hp = 600;
        private float m_speed = 5f;
        private float m_damage = 3f;
        private bool spriteSheet = false;

        public JeepEnemyRed(string enemyID, Vector2 enemyVector)
            : base(enemyID, enemyVector)
        {
            resistance = m_resistance;
            criticalResist = m_criticalResist;
            hitPoints = m_hp;
            speed = m_speed;
            EnemyType = Type;
            usingSpriteSheet = spriteSheet;
            damage = m_damage;
            Top = textureTop;
            Bottom = textureBottom;
        }
    }

    class TransportEnemy : Enemy
    {
        private Texture2D textureBottom = Art.Transport;

        public string Type = "Transport";

        private float m_resistance = 70;
        private float m_criticalResist = 70;
        private float m_hp = 300;
        private float m_speed = 3f;
        private float m_damage = 5f;
        private bool spriteSheet = false;

        public TransportEnemy(string enemyID, Vector2 enemyVector)
            : base(enemyID, enemyVector)
        {
            resistance = m_resistance;
            criticalResist = m_criticalResist;
            hitPoints = m_hp;
            speed = m_speed;
            EnemyType = Type;
            usingSpriteSheet = spriteSheet;
            damage = m_damage;
            Bottom = textureBottom;
        }
    }

    class TransportEnemyBlue : Enemy
    {
        private Texture2D textureBottom = Art.TransportBlue;

        public string Type = "Transport";

        private float m_resistance = 75;
        private float m_criticalResist = 75;
        private float m_hp = 600;
        private float m_speed = 3f;
        private float m_damage = 5f;
        private bool spriteSheet = false;

        public TransportEnemyBlue(string enemyID, Vector2 enemyVector)
            : base(enemyID, enemyVector)
        {
            resistance = m_resistance;
            criticalResist = m_criticalResist;
            hitPoints = m_hp;
            speed = m_speed;
            EnemyType = Type;
            usingSpriteSheet = spriteSheet;
            damage = m_damage;
            Bottom = textureBottom;
        }
    }

    class TransportEnemyRed : Enemy
    {
        private Texture2D textureBottom = Art.TransportRed;

        public string Type = "Transport";

        private float m_resistance = 80;
        private float m_criticalResist = 80;
        private float m_hp = 1200;
        private float m_speed = 3f;
        private float m_damage = 5f;
        private bool spriteSheet = false;

        public TransportEnemyRed(string enemyID, Vector2 enemyVector)
            : base(enemyID, enemyVector)
        {
            resistance = m_resistance;
            criticalResist = m_criticalResist;
            hitPoints = m_hp;
            speed = m_speed;
            EnemyType = Type;
            usingSpriteSheet = spriteSheet;
            damage = m_damage;
            Bottom = textureBottom;
        }
    }


    class SoldierEnemy : Enemy
    {
        private Texture2D textureBottom = Art.Soldier;

        public string Type = "Soldier";

        private float frameSpeed = 100;
        private int frameTotal = 3; // total - 1

        private float m_resistance = 10;
        private float m_criticalResist = 50;
        private float m_hp = 50;
        private float m_speed = 2;
        private float m_damage = 5f;

        private bool spriteSheet = true;


        public SoldierEnemy(string enemyID, Vector2 enemyVector)
            : base(enemyID, enemyVector)
        {
            resistance = m_resistance;
            criticalResist = m_criticalResist;
            hitPoints = m_hp;
            speed = m_speed;
            EnemyType = Type;
            usingSpriteSheet = spriteSheet;
            targetElasped = frameSpeed;
            sheetFrameTotal = frameTotal;
            damage = m_damage;
            Bottom = textureBottom;
        }

    }

    class SoldierEnemyBlue : Enemy
    {
        private Texture2D textureBottom = Art.SoldierBlue;

        public string Type = "Soldier";

        private float frameSpeed = 100;
        private int frameTotal = 3; // total - 1

        private float m_resistance = 20;
        private float m_criticalResist = 60;
        private float m_hp = 100;
        private float m_speed = 2;
        private float m_damage = 2f;

        private bool spriteSheet = true;


        public SoldierEnemyBlue(string enemyID, Vector2 enemyVector)
            : base(enemyID, enemyVector)
        {
            resistance = m_resistance;
            criticalResist = m_criticalResist;
            hitPoints = m_hp;
            speed = m_speed;
            EnemyType = Type;
            usingSpriteSheet = spriteSheet;
            targetElasped = frameSpeed;
            sheetFrameTotal = frameTotal;
            damage = m_damage;
            Bottom = textureBottom;
        }

    }

    class SoldierEnemyRed : Enemy
    {
        private Texture2D textureBottom = Art.SoldierRed;

        public string Type = "Soldier";

        private float frameSpeed = 100;
        private int frameTotal = 3; // total - 1

        private float m_resistance = 40;
        private float m_criticalResist = 70;
        private float m_hp = 200;
        private float m_speed = 4;
        private float m_damage = 4f;

        private bool spriteSheet = true;


        public SoldierEnemyRed(string enemyID, Vector2 enemyVector)
            : base(enemyID, enemyVector)
        {
            resistance = m_resistance;
            criticalResist = m_criticalResist;
            hitPoints = m_hp;
            speed = m_speed;
            EnemyType = Type;
            usingSpriteSheet = spriteSheet;
            targetElasped = frameSpeed;
            sheetFrameTotal = frameTotal;
            damage = m_damage;
            Bottom = textureBottom;
        }

    }

    class HelicopterEnemy : Enemy
    {
        private Texture2D textureBottom = Art.Helicopter;

        public string Type = "Helicopter";

        private float frameSpeed = 50;
        private int frameTotal = 3; // total - 1

        private float m_resistance = 50;
        private float m_criticalResist = 50;
        private float m_hp = 150;
        // helicopter speed works very differently, as it only heads towards one node currently it goes a lot faster than other units which use multiple nodes. dividing by 10 seems good
        private float m_speed = 5f / 10;
        private float m_damage = 8f;

        private bool spriteSheet = true;

        public HelicopterEnemy(string enemyID, Vector2 enemyVector)
            : base(enemyID, enemyVector)
        {
            resistance = m_resistance;
            criticalResist = m_criticalResist;
            hitPoints = m_hp;
            speed = m_speed;
            EnemyType = Type;
            usingSpriteSheet = spriteSheet;
            targetElasped = frameSpeed;
            sheetFrameTotal = frameTotal;
            damage = m_damage;
            Bottom = textureBottom;
        }
    }
}
