using System.ComponentModel;
using ProtoBuf;
using SFML.Graphics;
using SFML.Window;
using SFML.Utils;

namespace BoneLibrary
{
    [ProtoContract]
    public class Skin : ISkin
    {
        public Skin()
        {
            Scale = new Vector2f(1, 1);
            //Angle = -MathHelper.PiOver2;
            Color = Color.White;
            Name = "sprite";
        }

        public bool IsFrozen { get; private set; }

        [Browsable(false)]
        public Texture Texture { get; set; }

        [ProtoMember(2)]
        public IntRect Source { get; set; }

        [ProtoMember(3)]
        public Vector2f Origin { get; set; }

        [ProtoMember(4)]
        public Vector2f Scale { get; set; }

        [ProtoMember(5)]
        public Color Color { get; set; }

        [ProtoMember(6)]
        public float Angle { get; set; }

        [ProtoMember(1)]
        public string Name { get; set; }

        [ProtoMember(7)]
        public int BoneId { get; set; }

        public void Draw(SpriteBatch sb, Skeleton s, Bone b)
        {
            Draw(sb, b.BeginPosition, b.AbsoluteRotation, s.Scale, Color);
        }

        public void Freeze()
        {
            IsFrozen = true;
        }

        public override string ToString()
        {
            return "Sprite: " + Name;
        }

        /// <summary>
        ///     Calculates is given point is inside the sprite
        /// </summary>
        /// <param name="v">Point to test</param>
        /// <param name="position">Position of origin of sprite</param>
        /// <param name="angle">Additional angle of sprite</param>
        /// <param name="scale">Scale of sprite</param>
        /// <returns></returns>
        public virtual bool CollisionTest(Vector2f v, Vector2f position, float angle, float scale)
        {
            Vector2f origin = Origin;
            //var origin = new Vector2f(Source.Width / 2, Source.Height / 2);
            if (scale < 0) origin.X = Source.Width - origin.X;
            v -= position;
            v = v.Rotate(-angle - Angle, true);
            v += origin;
            var p = new Vector2i((int) v.X + Source.Left, (int) v.Y + Source.Top);
            if (Source.Contains(p.X, p.Y))
            {
                /*
                var data = new Color[1];//p.X+ p.Y*Texture.Width
                Texture.GetData<Color>(0,new IntRect(p.X,p.Y,1,1), data,0, data.Length);
                if (data[0].A != 0) return true;
                 */
                return false;
            }
            return false;
        }

        public void Draw(SpriteBatch sb, Vector2f position, float angle, float scale)
        {
            Draw(sb, position, angle, scale, Color);
        }

        public void Draw(SpriteBatch sb, Vector2f position, float angle, float scale, Color tint)
        {
            //var origin = new Vector2f(Source.Width / 2, Source.Height / 2);
            Vector2f origin = Origin;
            Vector2f scaleVec = Scale*scale;
            if (scale < 0)
            {
                //origin.X = Source.Width - origin.X;
                scaleVec.Y = -scaleVec.Y;
            }
            //sb.Draw(Texture, new IntRect(0, 0, 100, 100), Microsoft.Xna.Framework.Color.White);
            angle = angle + (scale > 0 ? Angle : -Angle - MathHelper.Pi);
            angle = angle*180/MathHelper.Pi;
            sb.Draw(Texture, position, Source, tint, scaleVec, origin, angle);
        }


        public void Draw(SpriteBatch sb, Skeleton s, Bone b, Color color)
        {
            Draw(sb, b.BeginPosition, b.AbsoluteRotation, s.Scale, color);
        }

        public bool CollisionTest(Vector2f v, Skeleton s, Bone b)
        {
            return CollisionTest(v, b.BeginPosition, b.AbsoluteRotation, s.Scale);
        }

        public Skin Clone()
        {
            var clone = MemberwiseClone() as Skin;
            clone.IsFrozen = false;
            return clone;
        }
    }
}