using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Content;

using ProtoBuf;

namespace BoneLibrary
{
    /// <summary>
    /// Collection of sprites (currently not used by the editor)
    /// </summary>
    [ProtoContract]
    public class Spritesheet
    {
        public const string EXTENSION = ".spr";
        [ProtoMember(1)]
        Dictionary<string, Skin> sprites;
        public Skin this[string name]
        {
            get
            {
                return sprites[name];
            }
        }
        public IEnumerable<Skin> GetSprites()
        {
            foreach (var s in sprites.Values)
                yield return s;
        }

        public Spritesheet()
        {
        }
        public Spritesheet(string textureAsset, IEnumerable<Skin> s)
        {
            sprites = new Dictionary<string, Skin>();
            TextureAsset = textureAsset;
            foreach (var sprite in s)
                sprites.Add(sprite.Name, sprite);
        }

        public Texture2D Texture { get; private set; }
        [ProtoMember(1)]
        public string TextureAsset { get; private set; }

        public static Spritesheet FromStream(Stream s, ContentManager content)
        {
            return null;
            //var serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(Spritesheet));
            //var result = serializer.ReadObject(s) as Spritesheet;
            //result.Texture = content.Load<Texture2D>(result.TextureAsset);
            //return result;
        }
        public static Spritesheet FromFile(string path, GraphicsDevice gd)
        {
            var result = FromFile(path);
            var texPath = Path.GetDirectoryName(path)+"\\"+result.TextureAsset+".png";
            result.Texture = Extensions.FromFile(gd, texPath, true);
            return result;
        }

        public void ToStream(Stream s)
        {
            //var serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(Spritesheet));
            //serializer.WriteObject(s, this);
        }

        public class Reader : ContentTypeReader<Spritesheet>
        {
            protected override Spritesheet Read(ContentReader input, Spritesheet existingInstance)
            {
                if (existingInstance != null) return existingInstance;
                return Spritesheet.FromStream(input.BaseStream, input.ContentManager);
            }
        }

        public static Spritesheet FromFile(string filename)
        {
            /*
            //var serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(Spritesheet));
            Spritesheet result;
            using (var stream = File.OpenRead(filename))
                result = serializer.ReadObject(stream) as Spritesheet;
            return result;*/
            return null;
        }
    }
}
