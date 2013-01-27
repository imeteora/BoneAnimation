using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using ProtoBuf;

namespace BoneLibrary
{
    /// <summary>
    /// Anything can be treated as sprite as long as it implements this interface.
    /// This allows user to utilize his own classes
    /// </summary>
    public interface ISkin
    {
        string Name { get; }
        int BoneId { get; set; }
        void Draw(SpriteBatch sb, Skeleton s, Bone b);
    }
}
