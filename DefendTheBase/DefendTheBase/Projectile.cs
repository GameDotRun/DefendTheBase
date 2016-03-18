using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flextensions;

namespace DefendTheBase
{
    public class Projectile
    {
        public enum Type
        {
            Gun,
            Rocket,
            SAM,
            Tesla
        }

        public Type TypeofProj;
        public int Radius;                          // The collision radius in pixels.
        public Vector2 Position;                    // The pixel position on screen.
        public float Lifetime, TimeSinceSpawn;      // Used to destroy projectiles after a while.

        private int m_damage;
        private float m_speed, m_rotation;
        private Texture2D m_sprite;
        private Vector2 m_position, m_velocity, m_center;

        private Enemy m_enemy;
        private Tower m_tower;

        public Projectile(Type type, Enemy enemy, Vector2 position, Vector2 direction, float lifetime, int damage, float speed = 10)
        {
            direction.Normalize();
            m_enemy = enemy;
            Lifetime = lifetime;
            m_position = Position = position;
            m_speed = speed;
            m_velocity = direction * m_speed;
            m_damage = damage;
            switch(type)
            {
                case Type.Gun:
                    TypeofProj = type;
                    m_sprite = Art.ProjectileGun;
                    m_center = new Vector2(m_sprite.Width / 2, m_sprite.Height / 2);
                    Radius = m_sprite.Width / 2;
                    break;
                case Type.Rocket:
                    TypeofProj = type;
                    m_sprite = Art.ProjectileRocket;
                    m_center = new Vector2(m_sprite.Width / 2, m_sprite.Height / 2);
                    Radius = m_sprite.Width / 2;
                    break;
                case Type.SAM:
                    TypeofProj = type;
                    m_sprite = Art.ProjectileSAM;
                    m_center = new Vector2(m_sprite.Width / 2, m_sprite.Height / 2);
                    Radius = m_sprite.Width / 2;
                    break;
                case Type.Tesla:
                    TypeofProj = type;
                    m_sprite = Art.ProjectileTesla;
                    m_center = new Vector2(m_sprite.Width / 2, m_sprite.Height / 2);
                    Radius = m_sprite.Width / 2;
                    break;
            }
        }

        public Projectile(Type type, Tower tower, Vector2 position, Vector2 direction, float lifetime, int damage, float speed = 10)
        {
            direction.Normalize();
            m_tower = tower;
            Lifetime = lifetime;
            m_position = Position = position;
            m_speed = speed;
            m_velocity = direction * m_speed;
            m_damage = damage;
            switch (type)
            {
                case Type.Gun:
                    TypeofProj = type;
                    m_sprite = Art.ProjectileGun;
                    m_center = new Vector2(m_sprite.Width / 2, m_sprite.Height / 2);
                    Radius = m_sprite.Width / 2;
                    break;
            }
        }

        public void Update()
        {
            if (m_enemy != null)
            {
                m_velocity = m_enemy.ScreenPos - m_position;
                m_velocity.Normalize();
                m_velocity *= m_speed;
                if (Vector2.Distance(m_enemy.ScreenPos, m_position) < 10)
                {
                    Lifetime = 0;
                    EnemyManager.EnemyDamaged(m_damage, m_enemy, TypeofProj);
                }
            }

            else if (m_tower != null)
            {
                m_velocity = m_tower.Position - m_position;
                m_velocity.Normalize();
                m_velocity *= m_speed;
                if (Vector2.Distance(m_tower.Position, m_position) < 10)
                {
                    Lifetime = 0;
                }
            }

            TimeSinceSpawn += 1 / 60f;
            m_position += m_velocity;
            Position = m_position;
            m_rotation = m_velocity.ToAngle();
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(m_sprite, m_position, null, Color.White, m_rotation, m_center, 1, SpriteEffects.None, 0f);
        }
    }
}
