using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using Microsoft.Xna.Framework.Content;
using System.IO;

using System.Xml;
using ProtoBuf;

namespace BoneLibrary
{
    /// <summary>
    /// Reflects structure of the file. Contains prototype skeleton and its animations and sprites, that are shared between clones of given skeleton.
    /// Each asset is based on one texture file.
    /// </summary>
    [ProtoContract]
    public class SkeletonAsset
    {
        static SkeletonAsset()
        {
            var model = ProtoBuf.Meta.RuntimeTypeModel.Default;
            var type = model.Add(typeof(Rectangle), false);
            type.AddField(1, "X");
            type.AddField(2, "Y");
            type.AddField(3, "Width");
            type.AddField(4, "Height");

            type = model.Add(typeof(Vector2), false);
            type.AddField(1, "X");
            type.AddField(2, "Y");

            type = model.Add(typeof(Color), false);
            type.AddField(1, "R");
            type.AddField(2, "G");
            type.AddField(3, "B");
            type.AddField(4, "A");
        }

        public const string EXTENSION = ".skt";

        /// <summary>
        /// Prototype of skeleton, stays unchanged in game
        /// </summary>
        [ProtoMember(1)]
        Dictionary<string, Skin> sprites;
        
        [ProtoMember(2)]
        Skeleton Skeleton { get; set; }

        [ProtoMember(3)]
        Dictionary<string, Animation> animations;

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

        public SkeletonAsset()
        {
            Skeleton = new Skeleton();
            Skeleton.asset = this;
            sprites = new Dictionary<string, Skin>();
            animations = new Dictionary<string, Animation>();
        }
        public SkeletonAsset(Texture2D texture, Skeleton s, IEnumerable<Animation> animations, IEnumerable<Skin> sprites) : this()
        {
            if (texture != null)
            {
                Texture = texture;
                //TextureAsset = texture.Name;
            }
            Skeleton = s;
            foreach (var a in animations)
                this.animations.Add(a.Name, a);
            foreach (var sprite in sprites)
                this.sprites.Add(sprite.Name, sprite);
        }


        public Texture2D Texture { get; private set; }

        //public string TextureAsset { get; set; }

        public Skeleton CreateSkeleton()
        {
            var copy = Skeleton.Clone();
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

        public void LoadTexture(ContentManager content)
        {
            //if (TextureAsset == null) return;
            //Texture = content.Load<Texture2D>(TextureAsset);
            //LoadTexture(Texture);

        }
        public void LoadTexture(GraphicsDevice gd, string fileName)
        {
            if(File.Exists(fileName))
            {
                Texture = Extensions.FromFile(gd, fileName, true);
            }
            else
            {
                Texture = new Texture2D(gd, 1, 1);
                Texture.SetData(new Color[] { Color.White });
            }
            LoadTexture(Texture);
        }


        public void LoadTexture(Texture2D texture)
        {
            //if(TextureAsset == null) TextureAsset = texture.Name;
            Texture = texture;
            foreach (var s in Sprites)
            {
                var b = s as Skin;
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
        public static SkeletonAsset FromStream(Stream stream, ContentManager content)
        {
            var asset = FromStream(stream);
            asset.LoadTexture(content);
            return asset;
        }
        public static SkeletonAsset FromFile(string fileName, GraphicsDevice gd)
        {
            SkeletonAsset result;
            using (var stream = File.OpenRead(fileName))
                result = FromStream(stream);

            if (gd != null)
            {
                var ex = Path.ChangeExtension(fileName, ".png");
                result.LoadTexture(gd, ex);
            }
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
            using (var stream = File.Create(file))
            {
                ToStream(stream);
            }

            var tex = Path.ChangeExtension(file, ".png");
            if (Texture != null && !File.Exists(tex))
            {
                using (var stream = File.Create(tex))
                {
                    Texture.SaveAsPng(stream, Texture.Width, Texture.Height);
                }
            }
        }
    }
}
