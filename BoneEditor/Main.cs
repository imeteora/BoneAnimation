using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Unknown;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Xna = Microsoft.Xna.Framework;
using BoneLibrary;
using System.Diagnostics;
namespace BoneEditor
{
    /// <summary>
    /// Provides interface for controller and holds SkeletonDisplay and SpriteCreator
    /// </summary>
    public partial class Main : Form
    {
        public Controller controller;
        SkeletonDisplay display;
        SpriteCreator picker;
        GraphicsDevice gd;

        BindingList<DgvSpritesItem> dgvItems = new BindingList<DgvSpritesItem>();
        string path;
        public Main(string path)
        {
            this.path = path;
            InitializeComponent();          
            display = new SkeletonDisplay(this);
            picker = new SpriteCreator();
            display.Dock = DockStyle.Fill;
            picker.Dock = DockStyle.Fill;
            picker.SpritePicked += PickTextureEnd;
            panelDisplay.Controls.Add(display);
            panelDisplay.Controls.Add(picker);
            controller = new Controller();
            controller.AnimationsChanged += new EventHandler(controller_AnimationsChanged);
            controller.AssetLoaded += new EventHandler(controller_AssetLoaded);
            controller.BoneSelected += new EventHandler(controller_BoneSelected);
            controller.PendingAnimationChanged += new EventHandler(controller_PendingAnimationChanged);
            controller.PendingFrameChanged += new EventHandler(controller_PendingFrameChanged);
            controller.PlayChanged += new EventHandler(controller_PlayChanged);
            controller.SelectedChanged += new EventHandler(controller_SelectedChanged);
            controller.Builder.Sprites.ListChanged += new ListChangedEventHandler(AllSprites_ListChanged);
            dgvSprites.DataSource = dgvItems;
            display.Initialized += Initialize;
        }

        private void Initialize(object sender, EventArgs e)
        {
            gd = display.GraphicsDevice;
            controller.SetGraphicsDevice(gd);
            if (path == null)
                controller.AssetNew();
            else controller.AssetLoad(path);
            //Clear(this, null);
        }

        void SetEnabled(bool value, params Control[] controls)
        {
            foreach (var c in controls)
                c.Enabled = value;
        }


        #region Controller Events

        void controller_SelectedChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = controller.SelectedItem;
        }

        void controller_PlayChanged(object sender, EventArgs e)
        {
            buttonPlayPause.Text = controller.IsPlaying ? "|=|" : "|>";
        }

        void controller_PendingFrameChanged(object sender, EventArgs e)
        {
            var a = controller.Animation;
            if (controller.FrameIndex != -1)
            {
                trackBarFrames.Value = controller.FrameIndex;
            }
        }

        void controller_PendingAnimationChanged(object sender, EventArgs e)
        {
            var a = controller.Animation;
            if (a != null) listBoxAnimations.RefreshItem(controller.Builder.Animations.IndexOf(a));
            SetEnabled(a != null, buttonAddFrameAfter, buttonAddFrameBehind);
            SetEnabled(a != null && a.Length >= 2, buttonNextFrame, buttonPreviousFrame, buttonPlayPause, trackBarFrames);
            SetEnabled(a != null && a.Length > 0, buttonRemoveFrame);
            if (a != null && a.Length >= 2)
            {
                trackBarFrames.Minimum = 0;
                trackBarFrames.Maximum = a.Length - 1;
            }

        }

        void controller_BoneSelected(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = controller.SelectedBone;
        }

        void controller_AssetLoaded(object sender, EventArgs e)
        {
            listBoxAnimations.DataSource = controller.Builder.Animations;
            listBoxAnimations.SelectedItem = controller.Animation;
            controller_PendingAnimationChanged(this, null);
            dgvItems.Clear();
            foreach (var s in controller.Builder.Sprites)
                dgvItems.Add(new DgvSpritesItem(controller.Skeleton, s));

        }

        void controller_AnimationsChanged(object sender, EventArgs e)
        {
            listBoxAnimations.DataSource = controller.Builder.Animations;
            SetEnabled(controller.Builder.Animations.Count > 0, buttonRemoveAnimation);
        }

        #endregion

        #region Left Panel
        void Animations_ListChanged(object sender, ListChangedEventArgs e)
        {
            //SetEnabled(root.CurrentAnimation.Count > 0, buttonRemoveAnimation);
            //listBoxAnimations.RefreshItems();
            //var n = Model.Animations.Count > 0;
            //SetEnabled(n,
            //    buttonRemoveAnimation,  buttonAddFrameAfter,
            //    buttonAddFrameBehind);
            //if (n)
            //{
            //    n = Model.Animation.Count > 0;
            //    SetEnabled(n, buttonRemoveFrame, buttonPlayPause, buttonPreviousFrame, buttonNextFrame, trackBarFrames);
            //}
        }

        void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            //if (Model.Selected is Animation) Model.Update(ChangeType.Animation);
            var a = propertyGrid1.SelectedObject as Animation;
            if (a == null)
            {
                listBoxAnimations.DataSource = null;
                listBoxAnimations.DataSource = controller.Builder.Animations;
            }

        }
        private void AddAnimation(object sender, EventArgs e)
        {

            controller.AnimationAdd();
        }
        private void RemoveAnimation(object sender, EventArgs e)
        {
            controller.AnimationRemove();
        }

        private void listBoxAnimations_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void buttonAddFrameAfter_Click(object sender, EventArgs e)
        {
            controller.FrameInsert(controller.FrameIndex + 1);
        }

        private void buttonAddFrameBehind_Click(object sender, EventArgs e)
        {
            controller.FrameInsert(controller.FrameIndex - 1);
        }

        private void buttonPreviousFrame_Click(object sender, EventArgs e)
        {
            controller.FrameIndex--;
        }

        private void buttonRemoveFrame_Click(object sender, EventArgs e)
        {
            controller.FrameRemoveSelected();
            //a.RemoveFrame(bar.Value);
        }

        private void buttonNextFrame_Click(object sender, EventArgs e)
        {
            controller.FrameIndex++;
        }

        private void listBoxAnimations_Click(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = listBoxAnimations.SelectedItem;
            controller.AnimationSelect(listBoxAnimations.SelectedIndex);
           
        }
        private void trackBarFrames_Scroll(object sender, EventArgs e)
        {
            controller.FrameIndex = trackBarFrames.Value;

        }
        private void PlayPause(object sender, EventArgs e)
        {
            controller.AnimationToggle();

        }
        #endregion

        #region Menu Strip


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by Pawel Szot");
        }
        void Clear(object sender, EventArgs e)
        {
            controller.AssetNew();
            reloadTextureToolStripMenuItem.Enabled = saveToolStripMenuItem.Enabled = false;

        }
        void OpenFile(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Asset files|*" + SkeletonAsset.EXTENSION;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                controller.AssetLoad(dialog.FileName);
                saveToolStripMenuItem.Enabled = true;
                reloadTextureToolStripMenuItem.Enabled = controller.Texture.Width > 1;
            }
        }

        void SaveAs(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "Asset files|*" + SkeletonAsset.EXTENSION;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //assetPath = dialog.FileName;
                controller.AssetSave(dialog.FileName);
                //Save(this, null);
                saveToolStripMenuItem.Enabled = true;
            }
        }

        private void Save(object sender, EventArgs e)
        {

            if (controller.assetPath == null) return;
            controller.AssetSave();

        }
        private void RefreshMenuStrip(object sender, EventArgs e)
        {
            pushBottomToolStripMenuItem.Enabled = pushTopToolStripMenuItem.Enabled = controller.SelectedBone != null;
            reloadTextureToolStripMenuItem.Enabled = controller.texturePath != null;
            //saveToolStripMenuItem.Enabled = Model.XmlPath != null;
        }
        private void ReloadTexture(object sender, EventArgs e)
        {
            controller.AssetLoadTexture();
            
        }

        private void PushTopBone(object sender, EventArgs e)
        {
            if (controller.SelectedBone != null)
                controller.Builder.PushTop(controller.SelectedSprite);
        }

        private void PushBottomBone(object sender, EventArgs e)
        {
            if (controller.SelectedBone != null)
                controller.Builder.PushBottom(controller.SelectedSprite);
        }
        private void Close(object sender, EventArgs e)
        {
            Close();
        }
        private void addAllToAnimationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.AnimationAddBones();
        }
        #endregion

        #region Context Menu
        private void BoneMenu_Opening(object sender, CancelEventArgs e)
        {
            removeBoneToolStripMenuItem.Enabled = controller.SelectedBone != null && controller.SelectedBone != controller.Skeleton.Bones[0];
            miAddToAnimation.Enabled =  controller.Animation != null && controller.SelectedBone != null;
            setSpriteToolStripMenuItem.Enabled = controller.Texture.Width != 1 && controller.SelectedBone != null;
            editThisSpriteToolStripMenuItem.Enabled = controller.SelectedSprite != null;
            if (controller.Animation != null)
            {
                var index = controller.Builder.Skeleton.Bones.IndexOf(controller.SelectedBone);
                miAddToAnimation.Text = controller.Animation.IsAnimated(index) ? "Remove from animation" : "Add to animation";
            }

        }
        private void ShowWireframe(object sender, EventArgs e)
        {
            display.DrawWires = miDrawWires.Checked;
        }

        private void LoadTexture(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Image files|*.png";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                controller.AssetLoadTexture(dialog.FileName);
                //Asset.LoadTexture(display.GraphicsDevice, dialog.FileName);
                //Texture = Extensions.FromFile(gd, dialog.FileName, true);
                //foreach(var b in Asset.Skeleton.Bones)
                //    b.Sprite.Texture = Texture;

            }
        }
        public void PickTextureBegin(object sender, EventArgs e)
        {
            PickTextureBegin();
        }

        public void PickTextureBegin()
        {
            var tex = controller.Texture;
            picker.Visible = picker.Enabled = true;
            display.Visible = display.Enabled = false;
            Skin sprite = null;
            if (controller.SelectedSprite != null)
                sprite = controller.SelectedSprite;
            else if (controller.SelectedBone != null)
                sprite = new Skin() { Texture = tex };
                
            picker.Pick(sprite, tex);

        }
        public void PickTextureEnd(object sender, EventArgs ea)
        {
            picker.Visible = picker.Enabled = false;
            display.Visible = display.Enabled = true;
            var sprite = picker.Selected as Skin;
            if (sprite.Source != Xna.Rectangle.Empty && sprite.BoneId == 0)
            {
                sprite.BoneId = controller.SelectedBone.Id;
                controller.Skeleton.AttachSprite(sprite);
                controller.Builder.Sprites.Add(sprite);
            }
        }
        private void RemoveBone(object sender, EventArgs e)
        {
            controller.BoneRemoveSelected();
        }

        private void SetSprite(object sender, EventArgs e)
        {
            PickTextureBegin();
        }
        private void editThisSpriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PickTextureBegin();
        }
        private void AddRemoveFromAnimation(object sender, EventArgs e)
        {
            //Animation.Bone = Bone;
            controller.AnimationAddRemoveBone();
        }
        #endregion

        #region Right Panel
        void AllSprites_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    dgvItems.Add(new DgvSpritesItem(controller.Skeleton, controller.Builder.Sprites[e.NewIndex]));
                    break;
                case ListChangedType.ItemDeleted:
                    dgvItems.RemoveAt(e.NewIndex);
                    break;
            }
        }

        private void dgvSprites_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSprites.SelectedCells.Count == 0) return;
            var index = dgvSprites.SelectedCells[0].RowIndex;
            controller.SelectedItem = dgvItems[index].Sprite;
        }

        private void dgvSprites_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvSprites.CurrentCell.ColumnIndex == 2) dgvSprites.EndEdit();
        }
        #endregion

        private void showSpritesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            display.DrawSprites = showSpritesToolStripMenuItem.Checked;
        }



    }
}
