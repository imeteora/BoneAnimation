using SFML.Graphics;

namespace BoneLibrary
{
    /// <summary>
    ///     Anything can be treated as sprite as long as it implements this interface.
    ///     This allows user to utilize his own classes
    /// </summary>
    public interface ISkin
    {
        string Name { get; }
        int BoneId { get; set; }
        void Draw(SpriteBatch sb, Skeleton s, Bone b);
    }
}