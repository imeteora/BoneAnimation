using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BoneLibrary;
using System.ComponentModel;

namespace BoneEditor
{
    /// <summary>
    /// Allows user to add or remove sprites from skeleton with checkboxes on data grid view
    /// </summary>
    class DgvSpritesItem
    {
        public DgvSpritesItem(Skeleton sk, Skin sp)
        {
            Skeleton = sk;
            Sprite = sp;
        }
        Skeleton Skeleton;
         [Browsable(false)]
        public Skin Sprite { get; private set; }
        public string Name
        {
            get { return Sprite.Name; }
            set { Sprite.Name = value; }
        }
        public int BoneId
        {
            get { return Sprite.BoneId; }
        }
        public bool Visible
        {
            get { return Skeleton.Sprites.Contains(Sprite); }
            set
            {
                if (value) Skeleton.Sprites.Add(Sprite);
                else Skeleton.Sprites.Remove(Sprite);
            }
        }
    }
}
