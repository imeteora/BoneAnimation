using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SFML.Graphics;
using SFML.Window;
using SFML.Utils;
namespace BoneLibrary
{
    /// <summary>
    ///     Provides useful functions to build skeleton in editor.
    /// </summary>
    public class AssetBuilder
    {
        private Texture texture;

        public AssetBuilder()
        {
            Skeleton = Skeleton.CreateNew();
            Animations = new BindingList<Animation>();
            Sprites = new BindingList<Skin>();
            Sprites.ListChanged += AllSprites_ListChanged;
        }

        public Skeleton Skeleton { get; private set; }
        public BindingList<Animation> Animations { get; private set; }
        public BindingList<Skin> Sprites { get; private set; }

        private List<Bone> Bones
        {
            get { return Skeleton.Bones; }
        }

        private List<ISkin> SkeletonSprites
        {
            get { return Skeleton.Sprites; }
        }

        public Texture Texture
        {
            get { return texture; }
            set
            {
                texture = value;
                foreach (Skin s in Sprites)
                    s.Texture = texture;
            }
        }

        private void AllSprites_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
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
        ///     Removes bone from skeleton and animations.
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

            foreach (Animation a in Animations)
                a.RemoveBones(removedBones, movedBones);

            for (int i = 0; i < SkeletonSprites.Count; i++)
            {
                ISkin kv = SkeletonSprites[i];
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

        public Skin FindSprite(Vector2f pos)
        {
            for (int i = SkeletonSprites.Count - 1; i >= 0; i--)
            {
                var sprite = SkeletonSprites[i] as Skin;
                if (sprite == null) continue;
                Bone bone = Bones[sprite.BoneId];
                if (sprite.CollisionTest(pos, Skeleton, bone)) return sprite;
            }
            return null;
        }

        public Bone FindByEndings(Vector2f Vector2f, float p)
        {
            foreach (Bone b in Bones.Reverse<Bone>())
            {
                if ((b.EndPosition - Vector2f).Length() < p)
                    return b;
            }
            return null;
        }

        public void PushTop(ISkin sprite)
        {
            int kv = SkeletonSprites.IndexOf(sprite);
            SkeletonSprites.RemoveAt(kv);
            SkeletonSprites.Add(sprite);
        }

        public void PushBottom(ISkin sprite)
        {
            int kv = SkeletonSprites.IndexOf(sprite);
            SkeletonSprites.RemoveAt(kv);
            SkeletonSprites.Insert(0, sprite);
        }


        public void MoveUp(ISkin sprite)
        {
            int index = SkeletonSprites.IndexOf(sprite);
            if (index >= SkeletonSprites.Count - 1) return;
            SkeletonSprites[index] = SkeletonSprites[index + 1];
            SkeletonSprites[index + 1] = sprite;
        }

        public void MoveDown(ISkin sprite)
        {
            int index = SkeletonSprites.IndexOf(sprite);
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
            if (asset.Texture != null) Texture = asset.Texture;
            Skeleton = asset.CreateSkeleton();
            foreach (Animation a in asset.Animations)
                Animations.Add(a);
            foreach (Skin s in asset.Sprites)
                Sprites.Add(s);
        }

        public SkeletonAsset ToAsset()
        {
            return new SkeletonAsset(Texture, Skeleton, Animations, Sprites);
        }
    }
}