using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BoneLibrary
{
    /// <summary>
    /// Provides useful functions to build skeleton in editor.
    /// </summary>
    public class AssetBuilder
    {
        public Skeleton Skeleton { get; private set; }
        public BindingList<Animation> Animations { get; private set; }
        public BindingList<Skin> Sprites { get; private set; }

        List<Bone> Bones
        {
            get { return Skeleton.Bones; }
        }
        List<ISkin> SkeletonSprites
        {
            get { return Skeleton.Sprites; }
        }
        public AssetBuilder()
        {
            Skeleton = Skeleton.CreateNew();
            Animations = new BindingList<Animation>();
            Sprites = new BindingList<Skin>();
            Sprites.ListChanged += new ListChangedEventHandler(AllSprites_ListChanged);
        }

        void AllSprites_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch(e.ListChangedType)
            {
                case ListChangedType.ItemAdded: 
                    Sprites[e.NewIndex].Texture = Texture;
                    break;
            }
        }
        public void AddBone(Bone parent, Bone child)
        {
            child.ParentId = Bones.IndexOf(parent);
            child.Id = Bones.Count;
            Bones.Add(child);
        }

        /// <summary>
        /// Removes bone from skeleton and animations.
        /// </summary>
        /// <param name="bone"></param>
        public void RemoveBone(Bone bone)
        {
            if (bone == Bones[0]) return;
            var removedBones = new List<int>();
            var emptySpaces = new LinkedList<int>();
            var movedBones = new Dictionary<int, int>();

            //var index = Bones.IndexOf(bone);
            //removedBones.Add(index);
            for (int i = 0; i < Bones.Count; i++)
            {

                if (removedBones.Contains(Bones[i].ParentId) || Bones[i] == bone)
                {
                    removedBones.Add(i);
                    emptySpaces.AddLast(i);
                }
                else
                {
                    int moved;
                    if (movedBones.TryGetValue(Bones[i].ParentId, out moved))
                        Bones[i].ParentId = moved;
                    if (emptySpaces.Count > 0)
                    {
                        Bones[emptySpaces.First.Value] = Bones[i];
                        Bones[i].Id = emptySpaces.First.Value;
                        movedBones.Add(i, emptySpaces.First.Value);
                        emptySpaces.RemoveFirst();
                        emptySpaces.AddLast(i);
                    }
                }
            }
            Bones.RemoveRange(Bones.Count - removedBones.Count, removedBones.Count);

            foreach (var a in Animations)
                a.RemoveBones(removedBones, movedBones);

            for (int i = 0; i < SkeletonSprites.Count; i++)
            {
                var kv = SkeletonSprites[i];
                if (removedBones.Contains(kv.BoneId))
                {
                    SkeletonSprites.RemoveAt(i);
                    i--;
                    continue;
                }
                int index;
                if (movedBones.TryGetValue(SkeletonSprites[i].BoneId, out index))
                    SkeletonSprites[i].BoneId = index;

            }

        }

        public Skin FindSprite(Vector2 pos)
        {
            for (int i = SkeletonSprites.Count - 1; i >= 0; i--)
            {
                var sprite = SkeletonSprites[i] as Skin;
                if (sprite == null) continue;
                var bone = Bones[sprite.BoneId];
                if (sprite.CollisionTest(pos, Skeleton, bone)) return sprite;
            }
            return null;
        }
        public Bone FindByEndings(Vector2 vector2, float p)
        {
            foreach (var b in Bones.Reverse<Bone>())
            {
                if ((b.EndPosition - vector2).Length() < p)
                    return b;
            }
            return null;
        }
        public void PushTop(ISkin sprite)
        {
            var kv = SkeletonSprites.IndexOf(sprite);
            SkeletonSprites.RemoveAt(kv);
            SkeletonSprites.Add(sprite);
        }
        public void PushBottom(ISkin sprite)
        {
            var kv = SkeletonSprites.IndexOf(sprite);
            SkeletonSprites.RemoveAt(kv);
            SkeletonSprites.Insert(0, sprite);
        }


        public void MoveUp(ISkin sprite)
        {
            var index = SkeletonSprites.IndexOf(sprite);
            if (index >= SkeletonSprites.Count - 1) return;
            SkeletonSprites[index] = SkeletonSprites[index + 1];
            SkeletonSprites[index + 1] = sprite;
        }

        public void MoveDown(ISkin sprite)
        {
            var index = SkeletonSprites.IndexOf(sprite);
            if (index < 1) return;
            SkeletonSprites[index] = SkeletonSprites[index - 1];
            SkeletonSprites[index - 1] = sprite;
        }

        public void Clear()
        {
            Skeleton = Skeleton.CreateNew();
            Animations.Clear();
            Sprites.Clear();
        }

        public void FromAsset(SkeletonAsset asset)
        {
            Clear();
            if (asset.Texture != null) this.Texture = asset.Texture;
            Skeleton = asset.CreateSkeleton();
            foreach (var a in asset.Animations)
                Animations.Add(a);
            foreach (var s in asset.Sprites)
                Sprites.Add(s);
        }
        public SkeletonAsset ToAsset()
        {
            return new SkeletonAsset(Texture, Skeleton, Animations, Sprites);
        }

        Texture2D texture;
        public Texture2D Texture
        {
            get { return texture; }
            set
            {
                texture = value;
                foreach (var s in Sprites)
                    s.Texture = texture;
            }
        }
    }
}
