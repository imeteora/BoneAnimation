using System;
using System.ComponentModel;
using ProtoBuf;
using SFML.Window;
using SFML.Utils;
namespace BoneLibrary
{
    /// <summary>
    ///     Part of skeleton. Used for correct positioning of sprites.
    ///     Position of each bone is affected by they ancestors.
    /// </summary>
    [ProtoContract]
    public class Bone
    {
        [ProtoMember(1)] public int ParentId = 0;

        [ReadOnly(true)]
        public int Id { get; set; }


        [ProtoMember(2)]
        public float Length { get; set; }

        [ProtoMember(3)]
        public float Rotation { get; set; }

        public Vector2f BeginPosition { get; private set; }
        public Vector2f EndPosition { get; private set; }
        public float AbsoluteRotation { get; private set; }

        public override string ToString()
        {
            return "Bone " + Id;
        }

        public void Apply(Vector2f pos, float r, float scale)
        {
            ///
            if (scale > 0)
                AbsoluteRotation = (r + Rotation)%MathHelper.TwoPi;
            else AbsoluteRotation = (r - Rotation)%MathHelper.TwoPi;

            BeginPosition = pos;
            var v = new Vector2f(Length*Math.Abs(scale), 0);

            EndPosition = pos + v.Rotate(AbsoluteRotation, true);
        }

        public Bone Clone()
        {
            return MemberwiseClone() as Bone;
        }

        /// <summary>
        ///     Sets rotation and length of the bone, so that the end of it is the same as given position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="lockLength">Indicates if length of the bone should stay unchanged</param>
        public void SetEnd(Vector2f position, bool lockLength)
        {
            float rot = AbsoluteRotation - Rotation;
            position -= BeginPosition;
            if (1 > 0)
                Rotation = position.GetAngle() - AbsoluteRotation + Rotation;
            AbsoluteRotation = rot + Rotation;
            if (!lockLength) Length = (position).Length();
        }
    }
}