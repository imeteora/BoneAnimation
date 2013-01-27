using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace BoneLibrary
{
    public static class Extensions
    {
        public static float GetAngle(this Vector2 v)
        {
            return (float)Math.Atan2(v.Y, v.X);
        }
        public static Vector2 Rotate(this Vector2 v, float angle)
        {
            return new Vector2(
                v.X * (float)Math.Cos(angle) - v.Y * (float)Math.Sin(angle),
                v.X * (float)Math.Sin(angle) + v.Y * (float)Math.Cos(angle)
                );
        }

        public static void DrawLine(SpriteBatch sb, Texture2D blank, Vector2 b,  Vector2 e, float thickness, Color color)
        {
            var angle = (e - b).GetAngle();
            var length = (e - b).Length();
            sb.Draw(
                blank, b, new Rectangle(0, 0, 1, 1), 
                color, angle, new Vector2(0,0.5f), 
                new Vector2(length, thickness), SpriteEffects.None, 0);

        }

        public static Texture2D CreateBlank(GraphicsDevice gd)
        {
            var tex = new Texture2D(gd, 1, 1);
            tex.SetData(new Color[] { Color.White });
            return tex;
        }

        /// <summary>
        /// Loads texture with optional alpha premultiplication
        /// More info: http://blogs.msdn.com/b/shawnhar/archive/2009/11/06/premultiplied-alpha.aspx
        /// </summary>
        public static Texture2D FromFile(GraphicsDevice gd, string path, bool premultiply)
        {
            Texture2D result;
            using (var stream = File.OpenRead(path))
                result = Texture2D.FromStream(gd, stream);

            if (premultiply)
            {
                Color[] data = new Color[result.Width * result.Height];
                result.GetData(data);
                for (int i = 0; i < data.Length; i++)
                {
                    var c = data[i];
                    data[i] = new Color(c.R * c.A/255f/255f, c.G * c.A/255f/255f, c.B * c.A/255f/255f, c.A/255f);
                }
                result.SetData(data);
                //var rt = new RenderTarget2D(gd, result.Width, result.Height);
                //gd.SetRenderTarget(rt);
                //gd.Clear(Color.Transparent);
                //var batch = new SpriteBatch(gd);
                //batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
                //batch.Draw(result, Vector2.Zero, Color.White);
                //batch.End();
                //gd.SetRenderTarget(null);
                //result.Dispose();
                //result = rt;
                //batch.Dispose();
            }

            return result;
        }

        /// <summary>
        /// Rounds vector to full numbers
        /// </summary>
        /// <param name="a">Vector to round</param>
        /// <returns></returns>
        public static Vector2 Round(Vector2 a)
        {
            return new Vector2( (a.X % 1 < 0.5f) ? (int)a.X : (int)a.X+1, (a.Y % 1 < 0.5f) ? (int)a.Y : (int)a.Y+1);
        }
    }
}
