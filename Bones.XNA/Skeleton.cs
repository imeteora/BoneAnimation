using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Timers;
using System.Diagnostics;

using ProtoBuf;

namespace BoneLibrary
{
    /// <summary>
    /// Skeleton is organized collection of bones and sprites
    /// </summary>
    [ProtoContract]
    public class Skeleton
    {

        List<PendingAnimation> pAnimations = new List<PendingAnimation>();

        public SkeletonAsset asset;

        public Vector2 Position;

        /// <summary>
        /// Indicates how big the skeleton is. Scale below 0 means that skeleton is flipped horizontally.
        /// </summary>
        public float Scale=1;
        public float Rotation;
        

        [ProtoMember(1)]
        public List<Bone> Bones { get; private set; }


        
        public List<ISkin> Sprites = new List<ISkin>();

        [ProtoMember(2)]
        List<string> SpriteNames;

        [ProtoBeforeSerialization]
        void Before()
        {
            SpriteNames = Sprites.Select(x => x.Name).ToList();
        }
        [ProtoAfterDeserialization]
        void After()
        {
            if (SpriteNames == null) return;
            foreach (var spr in SpriteNames)
                Sprites.Add(asset.GetSprite(spr));
        }

        public int Count { get { return Bones.Count; } }

        public Skeleton Clone()
        {
            var result = new Skeleton() { Position = Position, Rotation = Rotation, Scale = Scale, asset = asset };
            for(int i=0; i< Count; i++)
            {
                result.Bones.Add(Bones[i].Clone());
            }
            result.Initialize(asset);
            result.Sprites = Sprites.ToList();
            return result;
        }

        /// <summary>
        /// Applies pending animations on skeleton
        /// </summary>
        public void ApplyAnimations()
        {
            /*
             *Animations are ordered by priority. Important animations goes first. Every animation fills weight array.
             *Weight array indicates space left for other animation. If we have small animation that affects 2 bones,
             *it will add weight only for these two bones. That means other animations would be able to "control" other bones.
             *
             *Additional thing is weight of certain animations. Weight is used to smoothly start and stop animations:
             *Animation starts with weight 0, which means it doesnt affect skeleton at all, and then weight is being
             *increased each frame up to 1, which means that animation affect skeleton fully.
             *So if skeleton changes animation from walking to jumping - the transition will be smooth.
             */
            var weights = new float[Bones.Count];
            var angles = new Vector2[Bones.Count];
            for(int i=0; i< pAnimations.Count; i++)
            {
                if (!pAnimations[i].Apply(weights, angles, 1 / 60f))
                {
                    pAnimations.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < angles.Length; i++)
                if(angles[i] != Vector2.Zero)
                    Bones[i].Rotation = angles[i].GetAngle();
        }

        public void ApplyTransformations()
        {
            //root position is determined by skeleton
            Bones[0].Apply(Position, Rotation + (Scale > 0 ? 0 : MathHelper.Pi), Scale);

            //absolute position of each bone is  a sum of 
            for (int i = 1; i < Bones.Count; i++)
            {
                var b = Bones[i];
                var parent = Bones[b.ParentId];
                //each bone(except root) is transformed by position and rotation of its parent
                b.Apply(parent.EndPosition, parent.AbsoluteRotation, Scale);
            }
        }

        public void Apply()
        {
            ApplyAnimations();
            ApplyTransformations();
        }

        public Skeleton()
        {
            Bones = new List<Bone>();
            Sprites = new List<ISkin>();
            //Bones.Add(bone);
            //drawList.Add(bone);
        }
        public static Skeleton CreateNew()
        {
            var skeleton = new Skeleton();
            skeleton.Bones.Add(new Bone());
            //skeleton.drawList.AddLast(skeleton.Bones[0]);
            return skeleton;
        }

        public Rectangle GetBoundingRectangle()
        {
            float bx=Position.X, by= Position.Y, ex= Position.X, ey = Position.Y;
            foreach (var b in Bones)
            {
                if (bx > b.EndPosition.X) bx = b.EndPosition.X;
                if (by > b.EndPosition.Y) by = b.EndPosition.Y;
                if (ex < b.EndPosition.X) ex = b.EndPosition.X;
                if (ey < b.EndPosition.Y) ey = b.EndPosition.Y;
            }
            return new Rectangle((int)bx, (int)by, (int)(ex-bx), (int)(ey-by));
        }

        public void Draw(SpriteBatch sb)
        {
            //if (Scale > 0)
                foreach (var b in Sprites)
                    b.Draw(sb, this, Bones[b.BoneId]);
            //else
            //{
            //    foreach (var b in Sprites.Reverse<IBoneSprite>())
            //        b.Draw(sb, this, Bones[b.BoneId]);
            //}
        }

        public Skeleton(IEnumerable<Bone> bones)
        {
            this.Bones = bones.ToList();
            ApplyTransformations();
            //Initialize();
        }
        internal void Initialize(SkeletonAsset asset)
        {
            this.asset = asset;
            pAnimations = new List<PendingAnimation>();
            Scale = 1;
            if (Sprites == null) Sprites = new List<ISkin>();
            for (int i = 0; i < Bones.Count; i++)
                Bones[i].Id = i;
        }
        public void PlayAnimation(string p)
        {
            PlayAnimation(p, false);
        }
        public void PlayAnimation(string p, bool reversed)
        {

            /*Pending animations are ordered by priority
             *If two animations have the same priority, the older will start to fade (by reducing its weight)
             */
            var a = asset.GetAnimation(p);
            foreach (var pa2 in pAnimations)
                if (pa2.Animation == a) return;

            var pa = new PendingAnimation(this, asset.GetAnimation(p),reversed);
            bool done = false;
            for (int i = 0; i < pAnimations.Count; i++)
            {
                if (pAnimations[i].Animation.Priority <= pa.Animation.Priority)
                {
                    if (pAnimations[i].Animation.Priority == pa.Animation.Priority)
                        pAnimations[i].Next = pa;
                    pAnimations.Insert(i, pa);
                    done = true;
                    break;
                }
            }
            if (!done) pAnimations.Add(pa);
        }

        public void Visit(Action<Bone> action)
        {
            foreach (var b in Bones)
                action(b);
        }

        public void AttachSprite(ISkin s)
        {
            if (Sprites.Contains(s)) return;
            Sprites.Add(s);
        }
        public void DetachSprite(ISkin s)
        {

            Sprites.Remove(s);
        }

        public Bone GetParent(ISkin s)
        {
            return Bones[s.BoneId];
        }
        public Bone GetParent(Bone b)
        {
            if (b.ParentId < 0) return null;
            return Bones[b.ParentId];
        }
        public void RemoveSprite(string toRemove)
        {
            var spr = asset.GetSprite(toRemove);
            Sprites.Remove(spr);
        }
        public void ReplaceSprite(string toRemove, string toAdd)
        {
            var spr1 = asset.GetSprite(toRemove);
            var spr2 = asset.GetSprite(toAdd);
            var index = Sprites.IndexOf(spr1);
            if (index == -1) return;
            Sprites[index] = spr2;
        }
        public void AddSpriteAbove(string toAdd, string after)
        {
            var sAdd = asset.GetSprite(toAdd);
            for (int i = 0; i < Sprites.Count; i++)
            {
                if (Sprites[i].Name == after)
                    Sprites.Insert(i + 1, sAdd);
            }
        }

    }
}
