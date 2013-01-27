using System;
using SFML.Graphics;
using SFML.Window;

namespace BoneLibrary
{
    internal static class MathHelper
    {
        public const float Pi = (float) Math.PI;
        public const float TwoPi = Pi*2;

        public static float Clamp(float v, float min, float max)
        {
            if (v < min) return min;
            if (v > max) return max;
            return v;
        }

        public static float WrapAngle(float angle)
        {
            return angle%TwoPi;
        }
    }

    internal static class Extensions
    {
        public static float GetAngle(this Vector2f v)
        {
            return (float) Math.Atan2(v.Y, v.X);
        }

        public static void DrawLine(SpriteBatch sb, Texture blank, Vector2f b, Vector2f e, float thickness, Color color)
        {
            /*
            var angle = (e - b).GetAngle();
            var length = (e - b).Length();
            sb.Draw(
                blank, b, new IntRect(0, 0, 1, 1), 
                color, angle, new Vector2f(0,0.5f), 
                new Vector2f(length, thickness), SpriteEffects.None, 0);
*/
        }

        public static Texture CreateBlank()
        {
            var tex = new Texture(1, 1);
            tex.Update(new byte[] {255, 255, 255, 255});
            return tex;
        }

        public static Vector2f Normalize(this Vector2f v)
        {
            float len = v.Length;
            if (len == 0) return new Vector2f(1, 0);
            return v/len;
        }

        /// <summary>
        ///     Rounds vector to full numbers
        /// </summary>
        /// <param name="a">Vector to round</param>
        /// <returns></returns>
        public static Vector2f Round(Vector2f a)
        {
            return new Vector2f((a.X%1 < 0.5f) ? (int) a.X : (int) a.X + 1, (a.Y%1 < 0.5f) ? (int) a.Y : (int) a.Y + 1);
        }
    }
}