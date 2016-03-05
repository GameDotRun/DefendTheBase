using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        private float m_speed;
        private Texture2D m_sprite;
        private Vector2 m_position, m_velocity, m_center;

        private Enemy m_enemy;

        public Projectile(Type type, Enemy enemy, Vector2 position, Vector2 direction, float lifetime, float speed = 10)
        {
            direction.Normalize();
            m_enemy = enemy;
            Lifetime = lifetime;
            m_position = Position = position;
            m_speed = speed;
            m_velocity = direction * m_speed;
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
                    m_sprite = Art.ProjectileGun;
                    m_center = new Vector2(m_sprite.Width / 2, m_sprite.Height / 2);
                    Radius = m_sprite.Width / 2;
                    break;
                case Type.SAM:
                    TypeofProj = type;
                    m_sprite = Art.ProjectileGun;
                    m_center = new Vector2(m_sprite.Width / 2, m_sprite.Height / 2);
                    Radius = m_sprite.Width / 2;
                    break;
                case Type.Tesla:
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
                    // Change this to enemy.Hit or something...
                    m_enemy.IsDestroyed = true;
                }
            }
            TimeSinceSpawn += 1 / 60f;
            m_position += m_velocity;
            Position = m_position;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(m_sprite, m_position, null, Color.Black, 0, m_center, 1, SpriteEffects.None, 0f);
        }
    }
}
