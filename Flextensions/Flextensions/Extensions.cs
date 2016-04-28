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
        /// Gets a vector given an angle in radians.
        /// </summary>
        /// <param name="angle">The angle to get the vector of.</param>
        /// <returns>Float angle in radians of a Vector2.</returns>
        public static Vector2 ToVector(this float angle)
        {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
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

        /// <summary>
        /// Code from: http://circlessuck.blogspot.co.uk/2012/07/xna-smooth-sprite-rotation.html
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static float CurveAngle(float from, float to, float step)
        {
            if (step == 0) return from;
            if (from == to || step == 1) return to;

            Vector2 fromVector = new Vector2((float)Math.Cos(from), (float)Math.Sin(from));
            Vector2 toVector = new Vector2((float)Math.Cos(to), (float)Math.Sin(to));

            Vector2 currentVector = Slerp(fromVector, toVector, step);

            return (float)Math.Atan2(currentVector.Y, currentVector.X);
        }

        /// <summary>
        /// Code from: http://circlessuck.blogspot.co.uk/2012/07/xna-smooth-sprite-rotation.html
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        private static Vector2 Slerp(Vector2 from, Vector2 to, float step)
        {
            if (step == 0) return from;
            if (from == to || step == 1) return to;

            double theta = Math.Acos(Vector2.Dot(from, to));
            if (theta == 0) return to;

            double sinTheta = Math.Sin(theta);
            return (float)(Math.Sin((1 - step) * theta) / sinTheta) * from + (float)(Math.Sin(step * theta) / sinTheta) * to;
        }
    }
}
