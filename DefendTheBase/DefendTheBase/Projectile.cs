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

        private Texture2D m_sprite;
        private Vector2 m_position, m_velocity, m_center;

        public Projectile(Type type, int xPos, int yPos, Vector2 direction, float lifetime)
        {
            direction.Normalize();
            switch(type)
            {
                case Type.Gun:
                    TypeofProj = type;
                    m_sprite = Art.ProjectileGun;
                    m_position = Position = new Vector2(xPos, yPos);
                    m_velocity = direction * 5f;
                    m_center = new Vector2(m_sprite.Width / 2, m_sprite.Height / 2);
                    Lifetime = lifetime;
                    Radius = m_sprite.Width / 2;
                    break;
                case Type.Rocket:
                    TypeofProj = type;
                    m_sprite = Art.ProjectileGun;
                    m_position = Position = new Vector2(xPos, yPos);
                    m_velocity = direction * 5f;
                    m_center = new Vector2(m_sprite.Width / 2, m_sprite.Height / 2);
                    Lifetime = lifetime;
                    Radius = m_sprite.Width / 2;
                    break;
                case Type.SAM:
                    TypeofProj = type;
                    m_sprite = Art.ProjectileGun;
                    m_position = Position = new Vector2(xPos, yPos);
                    m_velocity = direction * 5f;
                    m_center = new Vector2(m_sprite.Width / 2, m_sprite.Height / 2);
                    Lifetime = lifetime;
                    Radius = m_sprite.Width / 2;
                    break;
                case Type.Tesla:
                    TypeofProj = type;
                    m_sprite = Art.ProjectileGun;
                    m_position = Position = new Vector2(xPos, yPos);
                    m_velocity = direction * 5f;
                    m_center = new Vector2(m_sprite.Width / 2, m_sprite.Height / 2);
                    Lifetime = lifetime;
                    Radius = m_sprite.Width / 2;
                    break;
            }
        }

        public void Update()
        {
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
