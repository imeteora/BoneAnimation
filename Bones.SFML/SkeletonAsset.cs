using System;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;
using ProtoBuf.Meta;
using SFML.Graphics;
using SFML.Window;

namespace BoneLibrary
{
    /// <summary>
    ///     Reflects structure of the file. Contains prototype skeleton and its animations and sprites, that are shared between clones of given skeleton.
    ///     Each asset is based on one texture file.
    /// </summary>
    [ProtoContract]
    public class SkeletonAsset
    {
        public const string EXTENSION = ".skt";
        [ProtoMember(3)] private readonly Dictionary<string, Animation> animations;

        /// <summary>
        ///     Prototype of skeleton, stays unchanged in game
        /// </summary>
        [ProtoMember(1)] private readonly Dictionary<string, Skin> sprites;

        static SkeletonAsset()
        {
            RuntimeTypeModel model = RuntimeTypeModel.Default;
            MetaType meta;
            Type type;

            type = typeof (IntRect);
            if (!model.IsDefined(type))
            {
                meta = model.Add(type, false);
                meta.AddField(1, "Left");
                meta.AddField(2, "Top");
                meta.AddField(3, "Width");
                meta.AddField(4, "Height");
            }

            type = typeof (Vector2f);
            if (!model.IsDefined(type))
            {
                meta = model.Add(type, false);
                meta.AddField(1, "X");
                meta.AddField(2, "Y");
            }

            type = typeof (Color);
            if (!model.IsDefined(type))
            {
                meta = model.Add(type, false);
                meta.AddField(1, "R");
                meta.AddField(2, "G");
                meta.AddField(3, "B");
                meta.AddField(4, "A");
            }
        }

        public SkeletonAsset()
        {
            Skeleton = new Skeleton();
            Skeleton.asset = this;
            sprites = new Dictionary<string, Skin>();
            animations = new Dictionary<string, Animation>();
        }

        public SkeletonAsset(Texture texture, Skeleton s, IEnumerable<Animation> animations, IEnumerable<Skin> sprites)
            : this()
        {
            if (texture != null)
            {
                Texture = texture;
                //TextureAsset = texture.Name;
            }
            Skeleton = s;
            foreach (Animation a in animations)
                this.animations.Add(a.Name, a);
            foreach (Skin sprite in sprites)
                this.sprites.Add(sprite.Name, sprite);
        }

        [ProtoMember(2)]
        private Skeleton Skeleton { get; set; }

        public IEnumerable<Animation> Animations
        {
            get
            {
                foreach (var a in animations)
                    yield return a.Value;
            }
        }

        public IEnumerable<Skin> Sprites
        {
            get
            {
                foreach (var s in sprites)
                    yield return s.Value;
            }
        }


        public Texture Texture { get; private set; }

        //public string TextureAsset { get; set; }

        public Skeleton CreateSkeleton()
        {
            Skeleton copy = Skeleton.Clone();
            copy.Scale = 1;
            return copy;
        }

        public Animation GetAnimation(string name)
        {
            return animations[name];
        }

        public Skin GetSprite(string name)
        {
            return sprites[name];
        }

        public void LoadTexture(string fileName)
        {
            if (File.Exists(fileName))
            {
                Texture = new Texture(fileName);
                LoadTexture(Texture);
            }
        }


        public void LoadTexture(Texture texture)
        {
            //if(TextureAsset == null) TextureAsset = texture.Name;
            Texture = texture;
            foreach (Skin s in Sprites)
            {
                Skin b = s;
                b.Texture = texture;
            }
        }

        public static SkeletonAsset FromStream(Stream stream)
        {
            SkeletonAsset asset;
            asset = Serializer.Deserialize<SkeletonAsset>(stream);

            asset.Skeleton.Initialize(asset);
            asset.Skeleton.asset = asset;
            asset.Skeleton.Scale = 1;
            return asset;
        }

        public static SkeletonAsset FromFile(string fileName)
        {
            SkeletonAsset result;
            using (FileStream stream = File.OpenRead(fileName))
                result = FromStream(stream);

            string ex = Path.ChangeExtension(fileName, ".png");
            result.LoadTexture(ex);

            return result;
        }

        public void ToStream(Stream stream)
        {
            /*
            var ds = new DataContractSerializer(typeof(SkeletonAsset), new Type[] { typeof(BoneSprite) });
            var settings = new XmlWriterSettings { Indent = true };
            using (var writer = XmlWriter.Create(stream, settings))
            {
                ds.WriteObject(writer, this);
            }
            */
            Serializer.Serialize(stream, this);
        }

        public void Save(string file)
        {
            using (FileStream stream = File.Create(file))
            {
                ToStream(stream);
            }

            string tex = Path.ChangeExtension(file, ".png");
            if (Texture != null && !File.Exists(tex))
            {
                using (FileStream stream = File.Create(tex))
                {
                    Image img = Texture.CopyToImage();
                    img.SaveToFile(tex);
                    //Texture.SaveAsPng(stream, Texture.Width, Texture.Height);
                }
            }
        }
    }
}