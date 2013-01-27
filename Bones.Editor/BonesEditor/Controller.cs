using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BoneLibrary;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework;
using System.Windows.Forms;
using System.ComponentModel;

namespace BoneEditor
{
    /// <summary>
    /// Handles state of skeleton: current animation, frame, etc.
    /// </summary>
    public class Controller
    {
        public AssetBuilder Builder { get; private set; }

        public Skeleton Skeleton
        {
            get { return Builder.Skeleton; }
        }


        object selectedItem;
        public object SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                Fire(SelectedChanged);
            }
        }
        public Bone SelectedBone
        {
            get { return selectedItem as Bone; }
            set
            {
                SelectedItem = value;
            }
        }
        public Skin SelectedSprite
        {
            get { return selectedItem as Skin; }
            set { SelectedItem = value; }
        }

        //public SkeletonAsset Asset;
        public string texturePath, assetPath;
        Stopwatch watch = new Stopwatch();
        GraphicsDevice gd;


        int aIndex=-1, fIndex=-1;

        public event EventHandler PendingAnimationChanged, PendingFrameChanged, AnimationsChanged, PlayChanged, AssetLoaded, BoneSelected, SelectedChanged;

        public Controller()
        {
            Builder = new AssetBuilder();
        }

        void SavePose()
        {
            if (IsPlaying) return;
            if (aIndex == -1 || fIndex == -1 || fIndex >= Animation.Length) return;
            Animation.FromSkeleton(fIndex, Skeleton);
        }
        void LoadPose()
        {
            if (aIndex == -1 || fIndex == -1) return;
            Animation.ToSkeleton(fIndex, Skeleton);
        }

        public void BoneSelect(Bone bone)
        {
            SelectedBone = bone;
            Fire(BoneSelected);
        }
        public void BoneRemoveSelected()
        {
            Builder.RemoveBone(SelectedBone);
            SelectedBone = null;
        }

        public void FrameInsert(int index)
        {
            if (Animation == null) return;
            SavePose();
            Animation.InsertFrame(index, Builder.Skeleton);
            Fire(PendingAnimationChanged);
            //Fire(PendingFrameChanged);
            if(index <0) index = 0;
            if(index >= Animation.Length) index = Animation.Length-1;
            FrameIndex = index;
        }
        public int FrameIndex
        {
            get { return fIndex; }
            set
            {
                SavePose();
                fIndex = value;
                if (Animation == null || Animation.Length == 0) return;
                if (fIndex < 0) fIndex = Animation.Length - 1;
                if (fIndex >= Animation.Length) fIndex = 0;
                LoadPose();
                Fire(PendingFrameChanged);
            }
        }
        internal void FrameRemoveSelected()
        {
            Animation.RemoveFrame(fIndex);
            if (fIndex >= Animation.Length) FrameIndex = Animation.Length - 1;
            Fire(PendingAnimationChanged);
            Fire(PendingFrameChanged);
        }

        public void AssetNew()
        {
            Builder.Clear();
            Builder.Texture = Extensions.CreateBlank(gd);
            //Builder.LoadTexture(gd, "blank");
            //AssetChanged();
            //Root = new SkeletonAsset();
            //Root.LoadTexture(Blank);
            texturePath = null;
            assetPath = null;
            fIndex = -1;
            aIndex = -1;
            SelectedBone = null;
            Fire(AssetLoaded);
        }
        public void AssetLoad(string path)
        {
            var asset = SkeletonAsset.FromFile(path, gd);
            Builder.FromAsset(asset);
            assetPath = path;
            path = Path.ChangeExtension(path, ".png");
            if (File.Exists(path)) texturePath = path;
            aIndex = Builder.Animations.Count - 1;
            fIndex = Animation != null ? Animation.Length - 1 : -1;
            SelectedBone = null;
            Fire(AssetLoaded);
        }
        public void AssetSave(string path)
        {
            assetPath = path;
            AssetSave();
        }
        public void AssetSave()
        {
            try
            {
                if (assetPath == null) return;
                var asset = Builder.ToAsset();
                SavePose();

                asset.Save(assetPath);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Failed to save asset");
            }
        }
        public void AssetLoadTexture()
        {
            var tex = Extensions.FromFile(gd, texturePath, true);
            Builder.Texture = tex;
            Texture = tex;
            //Builder.LoadTexture(gd, texturePath);
        }
        public void AssetLoadTexture(string path)
        {
            texturePath = path;
            AssetLoadTexture();
        }

        public bool IsPlaying
        {

            get;
            private set;

        }
        public Animation Animation
        {
            get
            {
                return aIndex == -1 ? null : Builder.Animations[aIndex];
            }
        }
        public void AnimationAdd()
        {
            SavePose();
            Builder.Animations.Add(new Animation());
            aIndex = Builder.Animations.Count - 1;
            fIndex = -1;
            Fire(AnimationsChanged);
            Fire(PendingAnimationChanged);
        }
        public void AnimationRemove()
        {
            var index = aIndex;
            if (index == -1) return;
            Builder.Animations.RemoveAt(index);
            aIndex--;
            fIndex = Animation != null ? Animation.Length - 1 : -1;
            LoadPose();
            Fire(AnimationsChanged);
            Fire(PendingAnimationChanged);
        }
        public void AnimationToggle()
        {
            if (Animation == null) return;
            if (!IsPlaying) SavePose();
            IsPlaying = !IsPlaying;
            
            SelectedBone = null;

            if (IsPlaying)
            {
                watch.Restart();
                
                FrameIndex = 0;
            }
            else LoadPose();

            Fire(PlayChanged);
        }
        public void AnimationSelect(int id)
        {
            SavePose();
            aIndex = id;
            Fire(PendingAnimationChanged);
            fIndex = Animation.Length - 1;
            LoadPose();
            FrameIndex = FrameIndex;

            
        }
        public void AnimationAddRemoveBone()
        {
            var a = Animation;
            var bone = SelectedBone;
            var index = Builder.Skeleton.Bones.IndexOf(bone);
            if (index < 0 || bone == null) throw new Exception();
            if (a.IsAnimated(index))
                a.RemoveBone(index);
            else
                a.AddBone(index, bone.Rotation);
        }
        public void AnimationAddBones()
        {
            if (Animation == null) return;
            for (int i = 0; i < Builder.Skeleton.Count; i++)
                Animation.AddBone(i, Builder.Skeleton.Bones[i].Rotation);
        }
        public float AnimationTime
        {
            get
            {
                if (!IsPlaying) return 0;
                return (float)watch.Elapsed.TotalSeconds +
                    Animation.Fps * fIndex;
            }
        }

        void Fire(EventHandler e)
        {
            if(e != null) e(this, null);
        }

        internal void SetGraphicsDevice(GraphicsDevice gd)
        {
            this.gd = gd;
        }

        public Texture2D Texture
        {
            get { return Builder.Texture; }
            set { Builder.Texture = value; }
        }
    }
}
