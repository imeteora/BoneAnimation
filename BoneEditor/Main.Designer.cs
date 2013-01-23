namespace BoneEditor
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTextureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadTextureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miDrawWires = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pushTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pushBottomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addAllToAnimationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonAddFrameBehind = new System.Windows.Forms.Button();
            this.buttonPreviousFrame = new System.Windows.Forms.Button();
            this.buttonPlayPause = new System.Windows.Forms.Button();
            this.buttonAddFrameAfter = new System.Windows.Forms.Button();
            this.trackBarFrames = new System.Windows.Forms.TrackBar();
            this.buttonNextFrame = new System.Windows.Forms.Button();
            this.buttonAddAnimation = new System.Windows.Forms.Button();
            this.buttonRemoveFrame = new System.Windows.Forms.Button();
            this.buttonRemoveAnimation = new System.Windows.Forms.Button();
            this.panelDisplay = new System.Windows.Forms.Panel();
            this.dgvSprites = new System.Windows.Forms.DataGridView();
            this.columnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnBoneId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.BoneMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeBoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setSpriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddToAnimation = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.editThisSpriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showSpritesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listBoxAnimations = new BoneEditor.RefreshableListBox();
            this.menuStrip1.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFrames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSprites)).BeginInit();
            this.BoneMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1039, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Click += new System.EventHandler(this.RefreshMenuStrip);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.loadTextureToolStripMenuItem,
            this.reloadTextureToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.Clear);
            // 
            // loadTextureToolStripMenuItem
            // 
            this.loadTextureToolStripMenuItem.Name = "loadTextureToolStripMenuItem";
            this.loadTextureToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.loadTextureToolStripMenuItem.Text = "Load texture";
            this.loadTextureToolStripMenuItem.Click += new System.EventHandler(this.LoadTexture);
            // 
            // reloadTextureToolStripMenuItem
            // 
            this.reloadTextureToolStripMenuItem.Enabled = false;
            this.reloadTextureToolStripMenuItem.Name = "reloadTextureToolStripMenuItem";
            this.reloadTextureToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.reloadTextureToolStripMenuItem.Text = "Reload texture";
            this.reloadTextureToolStripMenuItem.Click += new System.EventHandler(this.ReloadTexture);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenFile);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.saveAsToolStripMenuItem.Text = "Save as...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAs);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.Save);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.Close);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miDrawWires,
            this.showSpritesToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // miDrawWires
            // 
            this.miDrawWires.Checked = true;
            this.miDrawWires.CheckOnClick = true;
            this.miDrawWires.CheckState = System.Windows.Forms.CheckState.Checked;
            this.miDrawWires.Name = "miDrawWires";
            this.miDrawWires.Size = new System.Drawing.Size(152, 22);
            this.miDrawWires.Text = "Show skelet";
            this.miDrawWires.Click += new System.EventHandler(this.ShowWireframe);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pushTopToolStripMenuItem,
            this.pushBottomToolStripMenuItem,
            this.addAllToAnimationToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // pushTopToolStripMenuItem
            // 
            this.pushTopToolStripMenuItem.Name = "pushTopToolStripMenuItem";
            this.pushTopToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.pushTopToolStripMenuItem.Text = "Push top";
            this.pushTopToolStripMenuItem.Click += new System.EventHandler(this.PushTopBone);
            // 
            // pushBottomToolStripMenuItem
            // 
            this.pushBottomToolStripMenuItem.Name = "pushBottomToolStripMenuItem";
            this.pushBottomToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.pushBottomToolStripMenuItem.Text = "Push bottom";
            this.pushBottomToolStripMenuItem.Click += new System.EventHandler(this.PushBottomBone);
            // 
            // addAllToAnimationToolStripMenuItem
            // 
            this.addAllToAnimationToolStripMenuItem.Name = "addAllToAnimationToolStripMenuItem";
            this.addAllToAnimationToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.addAllToAnimationToolStripMenuItem.Text = "Add all to animation";
            this.addAllToAnimationToolStripMenuItem.Click += new System.EventHandler(this.addAllToAnimationToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.AutoScroll = true;
            this.toolStripContainer1.ContentPanel.Controls.Add(this.tableLayoutPanel1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1039, 457);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(1039, 503);
            this.toolStripContainer1.TabIndex = 3;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1039, 22);
            this.statusStrip1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 171F));
            this.tableLayoutPanel1.Controls.Add(this.propertyGrid1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelDisplay, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.dgvSprites, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.2604F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.73961F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1039, 457);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.Location = new System.Drawing.Point(3, 3);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGrid1.Size = new System.Drawing.Size(214, 146);
            this.propertyGrid1.TabIndex = 2;
            this.propertyGrid1.ToolbarVisible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonAddFrameBehind);
            this.panel1.Controls.Add(this.buttonPreviousFrame);
            this.panel1.Controls.Add(this.buttonPlayPause);
            this.panel1.Controls.Add(this.buttonAddFrameAfter);
            this.panel1.Controls.Add(this.trackBarFrames);
            this.panel1.Controls.Add(this.buttonNextFrame);
            this.panel1.Controls.Add(this.listBoxAnimations);
            this.panel1.Controls.Add(this.buttonAddAnimation);
            this.panel1.Controls.Add(this.buttonRemoveFrame);
            this.panel1.Controls.Add(this.buttonRemoveAnimation);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 155);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(214, 299);
            this.panel1.TabIndex = 3;
            // 
            // buttonAddFrameBehind
            // 
            this.buttonAddFrameBehind.Enabled = false;
            this.buttonAddFrameBehind.Location = new System.Drawing.Point(0, 265);
            this.buttonAddFrameBehind.Name = "buttonAddFrameBehind";
            this.buttonAddFrameBehind.Size = new System.Drawing.Size(23, 23);
            this.buttonAddFrameBehind.TabIndex = 7;
            this.buttonAddFrameBehind.Text = "+";
            this.buttonAddFrameBehind.UseVisualStyleBackColor = true;
            this.buttonAddFrameBehind.Click += new System.EventHandler(this.buttonAddFrameBehind_Click);
            // 
            // buttonPreviousFrame
            // 
            this.buttonPreviousFrame.Enabled = false;
            this.buttonPreviousFrame.Location = new System.Drawing.Point(29, 265);
            this.buttonPreviousFrame.Name = "buttonPreviousFrame";
            this.buttonPreviousFrame.Size = new System.Drawing.Size(23, 23);
            this.buttonPreviousFrame.TabIndex = 9;
            this.buttonPreviousFrame.Text = "<";
            this.buttonPreviousFrame.UseVisualStyleBackColor = true;
            this.buttonPreviousFrame.Click += new System.EventHandler(this.buttonPreviousFrame_Click);
            // 
            // buttonPlayPause
            // 
            this.buttonPlayPause.Enabled = false;
            this.buttonPlayPause.Location = new System.Drawing.Point(87, 265);
            this.buttonPlayPause.Name = "buttonPlayPause";
            this.buttonPlayPause.Size = new System.Drawing.Size(63, 23);
            this.buttonPlayPause.TabIndex = 12;
            this.buttonPlayPause.Text = "|>";
            this.buttonPlayPause.UseVisualStyleBackColor = true;
            this.buttonPlayPause.Click += new System.EventHandler(this.PlayPause);
            // 
            // buttonAddFrameAfter
            // 
            this.buttonAddFrameAfter.Enabled = false;
            this.buttonAddFrameAfter.Location = new System.Drawing.Point(185, 265);
            this.buttonAddFrameAfter.Name = "buttonAddFrameAfter";
            this.buttonAddFrameAfter.Size = new System.Drawing.Size(23, 23);
            this.buttonAddFrameAfter.TabIndex = 8;
            this.buttonAddFrameAfter.Text = "+";
            this.buttonAddFrameAfter.UseVisualStyleBackColor = true;
            this.buttonAddFrameAfter.Click += new System.EventHandler(this.buttonAddFrameAfter_Click);
            // 
            // trackBarFrames
            // 
            this.trackBarFrames.Enabled = false;
            this.trackBarFrames.Location = new System.Drawing.Point(-3, 214);
            this.trackBarFrames.Maximum = 0;
            this.trackBarFrames.Name = "trackBarFrames";
            this.trackBarFrames.Size = new System.Drawing.Size(214, 45);
            this.trackBarFrames.TabIndex = 6;
            this.trackBarFrames.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarFrames.Scroll += new System.EventHandler(this.trackBarFrames_Scroll);
            // 
            // buttonNextFrame
            // 
            this.buttonNextFrame.Enabled = false;
            this.buttonNextFrame.Location = new System.Drawing.Point(156, 265);
            this.buttonNextFrame.Name = "buttonNextFrame";
            this.buttonNextFrame.Size = new System.Drawing.Size(23, 23);
            this.buttonNextFrame.TabIndex = 11;
            this.buttonNextFrame.Text = ">";
            this.buttonNextFrame.UseVisualStyleBackColor = true;
            this.buttonNextFrame.Click += new System.EventHandler(this.buttonNextFrame_Click);
            // 
            // buttonAddAnimation
            // 
            this.buttonAddAnimation.Location = new System.Drawing.Point(191, 3);
            this.buttonAddAnimation.Name = "buttonAddAnimation";
            this.buttonAddAnimation.Size = new System.Drawing.Size(23, 23);
            this.buttonAddAnimation.TabIndex = 4;
            this.buttonAddAnimation.Text = "+";
            this.buttonAddAnimation.UseVisualStyleBackColor = true;
            this.buttonAddAnimation.Click += new System.EventHandler(this.AddAnimation);
            // 
            // buttonRemoveFrame
            // 
            this.buttonRemoveFrame.Enabled = false;
            this.buttonRemoveFrame.Location = new System.Drawing.Point(58, 265);
            this.buttonRemoveFrame.Name = "buttonRemoveFrame";
            this.buttonRemoveFrame.Size = new System.Drawing.Size(23, 23);
            this.buttonRemoveFrame.TabIndex = 10;
            this.buttonRemoveFrame.Text = "-";
            this.buttonRemoveFrame.UseVisualStyleBackColor = true;
            this.buttonRemoveFrame.Click += new System.EventHandler(this.buttonRemoveFrame_Click);
            // 
            // buttonRemoveAnimation
            // 
            this.buttonRemoveAnimation.Enabled = false;
            this.buttonRemoveAnimation.Location = new System.Drawing.Point(191, 32);
            this.buttonRemoveAnimation.Name = "buttonRemoveAnimation";
            this.buttonRemoveAnimation.Size = new System.Drawing.Size(23, 23);
            this.buttonRemoveAnimation.TabIndex = 5;
            this.buttonRemoveAnimation.Text = "-";
            this.buttonRemoveAnimation.UseVisualStyleBackColor = true;
            this.buttonRemoveAnimation.Click += new System.EventHandler(this.RemoveAnimation);
            // 
            // panelDisplay
            // 
            this.panelDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDisplay.Location = new System.Drawing.Point(223, 3);
            this.panelDisplay.Name = "panelDisplay";
            this.tableLayoutPanel1.SetRowSpan(this.panelDisplay, 2);
            this.panelDisplay.Size = new System.Drawing.Size(642, 451);
            this.panelDisplay.TabIndex = 4;
            // 
            // dgvSprites
            // 
            this.dgvSprites.AllowUserToResizeColumns = false;
            this.dgvSprites.AllowUserToResizeRows = false;
            this.dgvSprites.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSprites.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvSprites.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSprites.ColumnHeadersVisible = false;
            this.dgvSprites.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnName,
            this.columnBoneId,
            this.columnVisible});
            this.dgvSprites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSprites.Location = new System.Drawing.Point(871, 3);
            this.dgvSprites.Name = "dgvSprites";
            this.dgvSprites.RowHeadersVisible = false;
            this.tableLayoutPanel1.SetRowSpan(this.dgvSprites, 2);
            this.dgvSprites.RowTemplate.Height = 18;
            this.dgvSprites.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvSprites.Size = new System.Drawing.Size(165, 451);
            this.dgvSprites.TabIndex = 5;
            this.dgvSprites.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvSprites_CurrentCellDirtyStateChanged);
            this.dgvSprites.SelectionChanged += new System.EventHandler(this.dgvSprites_SelectionChanged);
            // 
            // columnName
            // 
            this.columnName.DataPropertyName = "Name";
            this.columnName.HeaderText = "Name";
            this.columnName.Name = "columnName";
            // 
            // columnBoneId
            // 
            this.columnBoneId.DataPropertyName = "BoneId";
            this.columnBoneId.FillWeight = 20F;
            this.columnBoneId.HeaderText = "B";
            this.columnBoneId.Name = "columnBoneId";
            // 
            // columnVisible
            // 
            this.columnVisible.DataPropertyName = "Visible";
            this.columnVisible.FillWeight = 20F;
            this.columnVisible.HeaderText = "V";
            this.columnVisible.Name = "columnVisible";
            this.columnVisible.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnVisible.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // BoneMenu
            // 
            this.BoneMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeBoneToolStripMenuItem,
            this.setSpriteToolStripMenuItem,
            this.miAddToAnimation,
            this.toolStripSeparator1,
            this.editThisSpriteToolStripMenuItem});
            this.BoneMenu.Name = "contextMenuStrip1";
            this.BoneMenu.Size = new System.Drawing.Size(220, 98);
            this.BoneMenu.Opening += new System.ComponentModel.CancelEventHandler(this.BoneMenu_Opening);
            // 
            // removeBoneToolStripMenuItem
            // 
            this.removeBoneToolStripMenuItem.Name = "removeBoneToolStripMenuItem";
            this.removeBoneToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.removeBoneToolStripMenuItem.Text = "Remove bone";
            this.removeBoneToolStripMenuItem.Click += new System.EventHandler(this.RemoveBone);
            // 
            // setSpriteToolStripMenuItem
            // 
            this.setSpriteToolStripMenuItem.Name = "setSpriteToolStripMenuItem";
            this.setSpriteToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.setSpriteToolStripMenuItem.Text = "Add sprite for this bone";
            this.setSpriteToolStripMenuItem.Click += new System.EventHandler(this.SetSprite);
            // 
            // miAddToAnimation
            // 
            this.miAddToAnimation.Name = "miAddToAnimation";
            this.miAddToAnimation.Size = new System.Drawing.Size(219, 22);
            this.miAddToAnimation.Text = "Add this bone to animation";
            this.miAddToAnimation.Click += new System.EventHandler(this.AddRemoveFromAnimation);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(216, 6);
            // 
            // editThisSpriteToolStripMenuItem
            // 
            this.editThisSpriteToolStripMenuItem.Name = "editThisSpriteToolStripMenuItem";
            this.editThisSpriteToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.editThisSpriteToolStripMenuItem.Text = "Edit this sprite";
            this.editThisSpriteToolStripMenuItem.Click += new System.EventHandler(this.editThisSpriteToolStripMenuItem_Click);
            // 
            // showSpritesToolStripMenuItem
            // 
            this.showSpritesToolStripMenuItem.CheckOnClick = true;
            this.showSpritesToolStripMenuItem.Name = "showSpritesToolStripMenuItem";
            this.showSpritesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.showSpritesToolStripMenuItem.Text = "Show sprites";
            this.showSpritesToolStripMenuItem.Click += new System.EventHandler(this.showSpritesToolStripMenuItem_Click);
            // 
            // listBoxAnimations
            // 
            this.listBoxAnimations.FormattingEnabled = true;
            this.listBoxAnimations.Location = new System.Drawing.Point(3, 3);
            this.listBoxAnimations.Name = "listBoxAnimations";
            this.listBoxAnimations.Size = new System.Drawing.Size(182, 199);
            this.listBoxAnimations.TabIndex = 0;
            this.listBoxAnimations.Click += new System.EventHandler(this.listBoxAnimations_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1039, 503);
            this.Controls.Add(this.toolStripContainer1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "Bone Animation Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFrames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSprites)).EndInit();
            this.BoneMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadTextureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miDrawWires;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadTextureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pushTopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pushBottomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeBoneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setSpriteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miAddToAnimation;
        public System.Windows.Forms.ContextMenuStrip BoneMenu;
        private System.Windows.Forms.ToolStripMenuItem addAllToAnimationToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonAddFrameBehind;
        private System.Windows.Forms.Button buttonPreviousFrame;
        private System.Windows.Forms.Button buttonPlayPause;
        private System.Windows.Forms.Button buttonAddFrameAfter;
        private System.Windows.Forms.TrackBar trackBarFrames;
        private System.Windows.Forms.Button buttonNextFrame;
        private RefreshableListBox listBoxAnimations;
        private System.Windows.Forms.Button buttonAddAnimation;
        private System.Windows.Forms.Button buttonRemoveFrame;
        private System.Windows.Forms.Button buttonRemoveAnimation;
        private System.Windows.Forms.Panel panelDisplay;
        private System.Windows.Forms.DataGridView dgvSprites;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnBoneId;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnVisible;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem editThisSpriteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showSpritesToolStripMenuItem;
    }
}