using System;
using System.Collections.Generic;
using ProtoBuf;
using SFML.Window;
using SFML.Utils;
namespace BoneLibrary
{
    //each keyframe is just list of rotations, used by Animation class

    [ProtoContract]
    internal class KeyFrame
    {
        [ProtoMember(1)] public List<float> Rotations;

        public float this[int i]
        {
            get { return Rotations[i]; }
            set { Rotations[i] = value; }
        }
    }

    /// <summary>
    ///     Holds colletion of KeyFrames and related data
    /// </summary>
    [ProtoContract]
    public class Animation
    {
        /// <summary>
        ///     Defines what bones are handled by this animation(by Id) and on which position is stored data of each bone
        /// </summary>
        [ProtoMember(1)] private readonly List<int> boneMap = new List<int>();

        //List<KeyFrame> frames  = new List<KeyFrame>();

        [ProtoMember(2)] private readonly List<KeyFrame> frames = new List<KeyFrame>();

        public Animation()
        {
            Name = "newAnimation";
            Fps = 1;
        }

        /*
        [ProtoBeforeSerialization]
        void BeforeSerialization()
        {
            if (newFrames == null) newFrames = new List<NewKeyFrame>();
            foreach (var kv in frames)
            {
                var frame = new NewKeyFrame() { Rotations = kv };
                newFrames.Add(frame);
            }
        }
        [ProtoAfterDeserialization]
        void AfterDeserialization()
        {
            foreach (var frame in newFrames)
            {
                frames.Add(frame.Rotations);
            }
        }
        */

        [ProtoMember(3)]
        public string Name { get; set; }

        [ProtoMember(4)]
        public float Fps { get; set; }

        [ProtoMember(5)]
        public int Priority { get; set; }

        [ProtoMember(6)]
        public bool Looped { get; set; }

        public int Size
        {
            get { return boneMap.Count; }
        }

        public int Length
        {
            get { return frames.Count; }
        }


        public float TimeLength
        {
            get { return (frames.Count - 1)/Fps; }
        }


        public override string ToString()
        {
            return String.Format("{0} [{1}]", Name, Length);
        }

        /// <summary>
        ///     Allows user to modify skeleton even if animations are already created. Each keyframe will be updated
        /// </summary>
        /// <param name="index">Index of new bone</param>
        /// <param name="rotation">Default rotation of new bone</param>
        public void AddBone(int index, float rotation)
        {
            if (boneMap.Contains(index)) return;
            boneMap.Add(index);
            foreach (KeyFrame k in frames)
                k.Rotations.Add(rotation);
        }

        /// <summary>
        ///     Allows user to modify skeleton even if animations are already created.
        /// </summary>
        /// <param name="indexes">Indexes of removed bones(children should be removed with its parent)</param>
        /// <param name="moved">Contains lookup table of changed bone indexes</param>
        public void RemoveBones(IEnumerable<int> indexes, Dictionary<int, int> moved)
        {
            var arrayIndexes = new List<int>();
            foreach (int i in indexes)
            {
                int index = boneMap.IndexOf(i);
                if (index != -1) arrayIndexes.Add(index);
                boneMap.Remove(index);
            }
            for (int i = 0; i < boneMap.Count; i++)
            {
                int value;
                if (moved.TryGetValue(boneMap[i], out value))
                    boneMap[i] = value;
            }
            foreach (KeyFrame k in frames)
            {
                foreach (int i in arrayIndexes)
                    k.Rotations.Remove(i);
            }
        }


        public bool IsAnimated(int index)
        {
            return boneMap.Contains(index);
        }

        public void InsertFrame(int index, Skeleton s)
        {
            KeyFrame frame = FromSkeleton(s);
            if (Length == 0 || index > Length) frames.Add(frame);
            else frames.Insert(index >= 0 ? index : 0, frame);
        }

        public void RemoveFrame(int index)
        {
            frames.RemoveAt(index);
        }

        /// <summary>
        ///     Creates new keyframe based on existing skeleotn
        /// </summary>
        /// <param name="s">Modeled skeleton</param>
        /// <returns></returns>
        private KeyFrame FromSkeleton(Skeleton s)
        {
            var frame = new KeyFrame();
            foreach (int i in boneMap)
                frame.Rotations.Add(MathHelper.WrapAngle(s.Bones[i].Rotation));
            return frame;
        }

        public void ToSkeleton(int fId, Skeleton s)
        {
            KeyFrame frame = frames[fId];
            for (int i = 0; i < Size; i++)
            {
                s.Bones[boneMap[i]].Rotation = frame[i];
            }
        }

        private Tuple<int, int, float> TimeToFrames(float time)
        {
            if (!Looped)
            {
                time = MathHelper.Clamp(time, 0, TimeLength);
            }
            int a, b;
            float ratio;
            time = (time*Fps)%Length;
            if (time < 0) time += Length;
            a = (int) time%Length;
            b = a + 1;
            if (b >= Length) b = 0;
            ratio = time%1;
            return new Tuple<int, int, float>(a, b, ratio);
        }

        public void ToSkeletonByTime(float time, Skeleton s)
        {
            Tuple<int, int, float> t = TimeToFrames(time);
            ToSkeleton(t.Item1, t.Item2, t.Item3, s);
        }

        private void ToSkeleton(int a, int b, float ratio, Skeleton s)
        {
            KeyFrame frameA = frames[a];
            KeyFrame frameB = frames[b];
            for (int i = 0; i < Size; i++)
            {
                s.Bones[boneMap[i]].Rotation = AngleLerp(frameA[i], frameB[i], ratio);
            }
        }

        public void FromSkeleton(int index, Skeleton s)
        {
            frames[index] = FromSkeleton(s);
        }

        public void RemoveBone(int index)
        {
            int id = boneMap.IndexOf(index);
            boneMap.RemoveAt(id);
            foreach (KeyFrame k in frames)
                k.Rotations.RemoveAt(id);
        }

        private static float AngleLerp(float a, float b, float ratio)
        {
            float delta = b - a;
            if (delta < -MathHelper.Pi) delta += MathHelper.TwoPi;
            else if (delta > MathHelper.Pi) delta -= MathHelper.TwoPi;
            return MathHelper.WrapAngle(a + delta*ratio);
        }

        /// <summary>
        ///     Performs animation blending
        /// </summary>
        /// <param name="Time">Time of frame</param>
        /// <param name="skeleton">Skeleton that is animated</param>
        /// <param name="weights">Indicates how much "space" is left for each bone to fill by animation</param>
        /// <param name="angles">Used to calculate weighted average between all animations</param>
        /// <param name="weight">Indicates how much animation affects skeleton</param>
        internal void ToSkeletonByTime(float Time, Skeleton skeleton, float[] weights, Vector2f[] angles, float weight)
        {
            Tuple<int, int, float> t = TimeToFrames(Time);
            KeyFrame frameA = frames[t.Item1];
            KeyFrame frameB = frames[t.Item2];
            for (int i = 0; i < Size; i++)
            {
                int boneId = boneMap[i];
                float leftSpace = 1 - weights[boneId];
                float clampedWeight = weight > leftSpace ? leftSpace : weight;
                weights[boneId] += clampedWeight;
                Vector2f rotation = new Vector2f(1, 0).Rotate(AngleLerp(frameA[i], frameB[i], t.Item3), true);
                rotation = rotation.Normalize();

                if (clampedWeight > 0)
                    angles[boneId] += rotation*clampedWeight;
                //if(weights[boneId] == 1)
                //    skeleton.Bones[boneId].Rotation = angles[boneId].GetAngle();
            }
        }
    }
}