using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Content;
using System.ComponentModel;

using ProtoBuf;

namespace BoneLibrary
{
    [ProtoContract]
    public class Skin : ISkin
    {
        public bool IsFrozen { get; private set; }
        public void Freeze()
        {
            IsFrozen = true;
        }

        [ProtoMember(1)]
        public string Name { get; set; }

        [Browsable(false)]
        public Texture2D Texture { get; set; }

        [ProtoMember(2)]
        public Rectangle Source {get; set;}
        [ProtoMember(3)]
        public Vector2 Origin { get; set; }
        [ProtoMember(4)]
        public Vector2 Scale { get; set; }
        [ProtoMember(5)]
        public Color Color { get; set; }
        [ProtoMember(6)]
        public float Angle { get; set; }

        [ProtoMember(7)]
        public int BoneId { get; set; }

        public Skin()
        {
            Scale = Vector2.One;
            //Angle = -MathHelper.PiOver2;
            Color = Color.White;
            Name = "sprite";
        }
        public override string ToString()
        {
            return "Sprite: " + Name;
        }
        /// <summary>
        /// Calculates is given point is inside the sprite
        /// </summary>
        /// <param name="v">Point to test</param>
        /// <param name="position">Position of origin of sprite</param>
        /// <param name="angle">Additional angle of sprite</param>
        /// <param name="scale">Scale of sprite</param>
        /// <returns></returns>
        public virtual bool CollisionTest(Vector2 v, Vector2 position, float angle, float scale)
        {
            var origin = Origin;
            //var origin = new Vector2(Source.Width / 2, Source.Height / 2);
            if (scale < 0) origin.X = Source.Width - origin.X;
            v -= position;
            v = v.Rotate(-angle - Angle);
            v += origin;
            var p = new Point((int)v.X+Source.X, (int)v.Y+Source.Y);
            if (Source.Contains(p.X, p.Y))
            {
                var data = new Color[1];//p.X+ p.Y*Texture.Width
                Texture.GetData<Color>(0,new Rectangle(p.X,p.Y,1,1), data,0, data.Length);
                if (data[0].A != 0) return true;
            }
            return false;
        }

        public void Draw(SpriteBatch sb, Vector2 position, float angle, float scale)
        {
            Draw(sb, position, angle, scale, Color);
        }
        public void Draw(SpriteBatch sb, Vector2 position, float angle, float scale, Color tint)
        {
            //var origin = new Vector2(Source.Width / 2, Source.Height / 2);
            var origin = Origin;
            if (scale < 0) origin.X = Source.Width - origin.X;
            //sb.Draw(Texture, new Rectangle(0, 0, 100, 100), Microsoft.Xna.Framework.Color.White);
            sb.Draw(Texture, position, Source, tint, angle+(scale > 0 ? Angle: -Angle-MathHelper.Pi), origin, Math.Abs(scale)*Scale, scale > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,0);
        }

        public void Draw(SpriteBatch sb, Skeleton s, Bone b)
        {
            Draw(sb, b.BeginPosition, b.AbsoluteRotation, s.Scale, Color);
        }
        public void Draw(SpriteBatch sb, Skeleton s, Bone b, Color color)
        {
            Draw(sb, b.BeginPosition, b.AbsoluteRotation, s.Scale, color);
        }
        public bool CollisionTest(Vector2 v, Skeleton s, Bone b)
        {
            return CollisionTest(v, b.BeginPosition, b.AbsoluteRotation, s.Scale);
        }

        public Skin Clone()
        {
            var clone = this.MemberwiseClone() as Skin;
            clone.IsFrozen = false;
            return clone;
        }
    }
}
