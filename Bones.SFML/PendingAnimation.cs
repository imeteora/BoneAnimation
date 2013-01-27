using System;
using SFML.Window;

namespace BoneLibrary
{
    /// <summary>
    ///     Defines ongoing animation, played on certain skeleton.
    /// </summary>
    public class PendingAnimation : IComparable<PendingAnimation>
    {
        private const float FADE_TIME = 0.4f;
        private const float FADE_RATIO = 4f;

        private readonly Skeleton skeleton;

        /// <summary>
        ///     Indicates that this animation is being replaced by another animation
        /// </summary>
        public PendingAnimation Next;

        public bool Reversed;

        public PendingAnimation(Skeleton skeleton, Animation a, bool reversed)
        {
            if (reversed) Time = a.TimeLength;
            Reversed = reversed;
            Animation = a;
            this.skeleton = skeleton;
            IsActive = true;
        }

        public Animation Animation { get; private set; }

        /// <summary>
        ///     If animation is no longer active, it should be removed from its list by skeleton
        /// </summary>
        public bool IsActive { get; private set; }

        public float Time { get; set; }

        /// <summary>
        ///     Weight determines how much that animation affects skeleton. The main purpose of this is to blend between two animations
        /// </summary>
        public float Weight { get; set; }

        public int CompareTo(PendingAnimation other)
        {
            return Animation.Priority.CompareTo(other.Animation.Priority);
        }

        public bool Apply(float[] weights, Vector2f[] angles, float time)
        {
            Time += time*(Reversed ? -1 : 1);

            if (Animation.Looped)
            {
                if (Next != null)
                {
                    Weight -= FADE_RATIO*time;
                    if (Weight <= 0) return false;
                }
                else Weight += FADE_RATIO*time;
            }
            else
            {
                if (Reversed && Time > 0 || !Reversed && Time < Animation.TimeLength)
                {
                    Weight += FADE_RATIO*time;
                    if (Weight < 0.5f) Time = Reversed ? Animation.TimeLength : 0;
                }
                else
                {
                    Weight -= FADE_RATIO*time;
                    if (Weight <= 0) 
                        return false;
                }
            }

            Weight = MathHelper.Clamp(Weight, 0, 1f);

            //Animation.ToSkeletonByTime(Time, skeleton);
            Animation.ToSkeletonByTime(Time, skeleton, weights, angles, Weight);
            return true;
        }

        public override string ToString()
        {
            return String.Format("{0} with weight {1}", Animation.Name, Weight);
        }
    }
}