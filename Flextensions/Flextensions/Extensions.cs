using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flextensions
{
    public static class Extensions
    {
        /// <summary>
        /// Gets an angle in radians of the given vector.
        /// </summary>
        /// <param name="vector">The vector to get the angle of.</param>
        /// <returns>The angle to point a sprite at along a given vector.</returns>
        public static float ToAngle(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        /// <summary>
        /// Converts a Vector2 into a Point. Rounds to the nearest X and Y integers.
        /// </summary>
        /// <param name="vector">The vector to convert to Point.</param>
        /// <returns></returns>
        public static Point ToPoint(this Vector2 vector)
        {
            return new Point((int)Math.Round(vector.X), (int)Math.Round(vector.Y));
        }

        public static Vector2 ToVector(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }
    }
}
