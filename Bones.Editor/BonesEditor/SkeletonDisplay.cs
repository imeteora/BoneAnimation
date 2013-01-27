#region File Description
//-----------------------------------------------------------------------------
// SpriteFontControl.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System;
using System.Diagnostics;
using Unknown;

using WForms = System.Windows.Forms;
using BoneLibrary;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Input;
#endregion

namespace BoneEditor
{

    /// <summary>
    /// Draws skeleton and allows user to modify it
    /// </summary>
    public class SkeletonDisplay : GraphicsDeviceControl
    {
        const string info = @"
        Mouse - rotate bones\n
        Ctrl - add bones\n
        Shift - resize bones\n
        Right click - context menu\n
        Delete - delete bones
        Home - push sprite to top
        End - push sprite to end
        Page up - move sprite up
        Page down - move sprite down
        ";
        
        
        public event EventHandler Initialized;
        Texture2D circle;
        public Camera camera;
        ContentManager content;
        SpriteBatch spriteBatch;
        SpriteFont font;
        System.Threading.Timer timer;

        Skeleton skeleton { get { return controller.Skeleton; } }

        Vector2 mousePrevious, mouseCurrent;
        Main main;
        Controller controller { get { return main.controller; } }

        Bone CurrentBone
        { get { return main.controller.SelectedBone; } }
        Bone boneUnder;
        Skin spriteUnder;

        public bool DrawWires = true;
        public bool ShowSkelet { get; set; }
        public Bone Selected
        {
            get { return controller.SelectedBone as Bone; }
            set
            {
                controller.BoneSelect(value);
            }
        }

        public SkeletonDisplay(Main editor)
        {
            this.main = editor;
            ShowSkelet = true;
            DrawSprites = true;

        }
        protected override void Initialize()
        {

            content = new ContentManager(Services, "Content");
            camera = new Camera(GraphicsDevice);
            blank = new Texture2D(GraphicsDevice, 1, 1);
            blank.SetData(new Color[] { Color.White });
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = content.Load<SpriteFont>("hudFont");
            timer = new System.Threading.Timer(OnTick, null, 0, 20);
            circle = content.Load<Texture2D>("circle");
            //circle = Extensions.FromFile(GraphicsDevice, "circle.png", true);
            if (Initialized != null) Initialized(this, new EventArgs());
        }
        public void PickTexture()
        {
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            offset = 0;
        }
        float offset;
        float HandleSize = 15;
        private Vector2 mouseClickPosition;

        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            if (controller.IsPlaying) return;
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case WForms.Keys.Delete:
                    var bone = controller.SelectedBone;
                    if (bone != null)
                        controller.Builder.RemoveBone(bone);
                    var sprite = controller.SelectedSprite;
                    if (sprite != null)
                    {
                        controller.Builder.Skeleton.DetachSprite(sprite);
                        controller.Builder.Sprites.Remove(sprite);
                    }
                    controller.SelectedItem = null;
                    break;
                case WForms.Keys.Left:
                    controller.FrameIndex--;
                    break;
                case WForms.Keys.Right:
                    controller.FrameIndex++;
                    break;
                case WForms.Keys.Home: controller.Builder.PushTop(controller.SelectedSprite);
                    break;
                case WForms.Keys.End: controller.Builder.PushBottom(controller.SelectedSprite);
                    break;
                case WForms.Keys.PageUp: controller.Builder.MoveUp(controller.SelectedSprite);
                    break;
                case WForms.Keys.PageDown: controller.Builder.MoveDown(controller.SelectedSprite);
                    break;
                case WForms.Keys.Q:
                    skeleton.Scale *= -1;
                    break;

                //case WForms.Keys.D1: Mode = EditMode.BoneRotate; break;
                //case WForms.Keys.D2: Mode = EditMode.BoneRotateAndResize; break;
                //case WForms.Keys.D3: Mode = EditMode.BoneAdd; break;
                //case WForms.Keys.D4: Mode = EditMode.SpriteMove; break;
                //case WForms.Keys.D5: Mode = EditMode.SpriteAdding; break;

            }


        }
        protected override void OnMouseWheel(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (e.Delta > 0) camera.Zoom *= 1.1f;
            if (e.Delta < 0) camera.Zoom /= 1.1f;
            Refresh();

        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.Focus();
        }
        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            mousePrevious = mouseCurrent;
            mouseCurrent = new Vector2(e.X, e.Y);
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                camera.Position -= (mouseCurrent - mousePrevious) / camera.Zoom;
                Refresh();
            }
            if (controller.IsPlaying) return;
            var left = e.Button == WForms.MouseButtons.Left;
            var right = e.Button == WForms.MouseButtons.Right;
            var shift = ModifierKeys.HasFlag(System.Windows.Forms.Keys.Shift);
            var ctrl = ModifierKeys.HasFlag(System.Windows.Forms.Keys.Control);

            if (!left)
            {
                var oldBone = boneUnder;
                var oldSprite = spriteUnder;
                if (DrawWires)
                    boneUnder = controller.Builder.FindByEndings(camera.Unproject(mouseCurrent), HandleSize / 2 / camera.Zoom);
                if (DrawSprites)
                    spriteUnder = controller.Builder.FindSprite(camera.Unproject(mouseCurrent));
                if (oldBone != boneUnder || oldSprite != spriteUnder) Refresh();

                return;
            }
            if((mouseCurrent - mouseClickPosition).Length() < 2) return;
            var bone = controller.SelectedBone;
            if (bone != null)
            {
                bone.SetEnd(camera.Unproject(mouseCurrent), !ctrl && !shift);
                Refresh();
            }
            else if (controller.SelectedSprite != null)
            {
                bone = skeleton.GetParent(controller.SelectedSprite);
                if (shift)
                {
                    //controller.SelectedSprite.Offset = (camera.Unproject(mouseCurrent) - bone.BeginPosition).Rotate(-bone.AbsoluteRotation);
                }
                else if (ctrl)
                {
                    controller.SelectedSprite.Angle = (camera.Unproject(mouseCurrent) - bone.BeginPosition).GetAngle();
                    Refresh();
                }
                else
                {

                    bone.SetEnd(camera.Unproject(mouseCurrent), true);
                    bone.Rotation -= offset;
                    skeleton.ApplyTransformations();
                    Refresh();
                }
            }
            
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            mouseClickPosition = new Vector2(e.X, e.Y);
            if (controller.IsPlaying) return;
            var ctrl = ModifierKeys.HasFlag(System.Windows.Forms.Keys.Control);


            if (e.Button == WForms.MouseButtons.Left || e.Button == WForms.MouseButtons.Right) 
            {
                if (boneUnder != null)
                    controller.SelectedItem = boneUnder;
                else
                    controller.SelectedItem = spriteUnder;
                if (spriteUnder != null)
                {
                    var bone = skeleton.GetParent(spriteUnder);
                    offset = (camera.Unproject(mouseClickPosition) - bone.BeginPosition).GetAngle() - bone.AbsoluteRotation ;
                }

                if (e.Button == WForms.MouseButtons.Right && (controller.SelectedBone != null || controller.SelectedSprite != null)) 
                    main.BoneMenu.Show(this, e.X, e.Y);
            }
            if (ctrl && e.Button == WForms.MouseButtons.Left && controller.SelectedBone != null)
            {
                var bone = new Bone();
                controller.Builder.AddBone(controller.SelectedBone, bone);
                bone.Apply(controller.SelectedBone.EndPosition, boneUnder.AbsoluteRotation, 1);
                controller.BoneSelect(bone);
            }
            
        }

        int skip;
        void OnTick(object state)
        {
            if (controller.IsPlaying || skip > 10)
            {
                if (InvokeRequired)
                    Invoke(new Action(Refresh));
                else Refresh();
                skip = 0;
            }
            skip++;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //content.Unload();
            }
            base.Dispose(disposing);
        }
        protected override bool IsInputKey(WForms.Keys keyData)
        {
            switch (keyData)
            {
                case WForms.Keys.Right:
                case WForms.Keys.Left:
                case WForms.Keys.Up:
                case WForms.Keys.Down:
                    return true;
                case WForms.Keys.Shift | WForms.Keys.Right:
                case WForms.Keys.Shift | WForms.Keys.Left:
                case WForms.Keys.Shift | WForms.Keys.Up:
                case WForms.Keys.Shift | WForms.Keys.Down:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override void Draw()
        {
            GraphicsDevice.Clear(new Color(0.1f, 0.1f, 0.1f));
            spriteBatch.Begin();
            //spriteBatch.DrawString(font, info, new Vector2(10, 10), Color.White * 0.4f);
            spriteBatch.End();
            Bone yellow =null;
            if (yellow == null) yellow = boneUnder;

            var ctr = main.controller;

            camera.Begin(spriteBatch);
            //main.Draw(spriteBatch, camera.Zoom);
            if (ctr.IsPlaying)
            {
                if (!ctr.Animation.Looped && ctr.AnimationTime > ctr.Animation.TimeLength)
                    ctr.AnimationToggle();
                else
                    ctr.Animation.ToSkeletonByTime(ctr.AnimationTime, skeleton);
            }
            skeleton.ApplyTransformations();
            if(DrawSprites) skeleton.Draw(spriteBatch);
            if (DrawWires)
            {
                skeleton.Visit(b =>
                {
                    DrawBone(b, ctr.Animation != null && ctr.Animation.IsAnimated(skeleton.Bones.IndexOf(b)) ? Color.LightBlue : Color.Orange);
                });

                if(yellow != null) 
                    DrawPoint(yellow.EndPosition, Color.Blue*0.3f,1);
                //if (main.Animation != null)
                //    DrawPoint(main.Animation.Bone.EndPosition, Color.Blue * 0.5f ,0.75f);
                if (ctr.SelectedBone != null)
                    DrawPoint(ctr.SelectedBone.EndPosition, Color.Red, 0.5f);
            }
            if (spriteUnder != null)
            {
                var bone = skeleton.GetParent(spriteUnder);
                spriteUnder.Draw(spriteBatch, skeleton, bone, Color.Yellow * 0.2f);
            }
            if (controller.SelectedSprite != null && controller.Skeleton.Sprites.Contains(controller.SelectedSprite))
            {
                var bone = skeleton.GetParent(controller.SelectedSprite);
                controller.SelectedSprite.Draw(spriteBatch, skeleton, bone, Color.Blue * 0.2f);
            }

            spriteBatch.End();
            //spriteBatch.Begin();
            //spriteBatch.DrawString(font,GetModeString(Mode), Vector2.One * 10, Color.White);
            //spriteBatch.End();
        }
        void DrawBone(Bone node, Color tint)
        {
            DrawPoint(node.EndPosition, tint,1);
            //spriteBatch.Draw(main.Blank, new Rectangle(0, 0, 1, 1), tint);
            spriteBatch.Draw(blank, node.BeginPosition, new Rectangle(0, 0, 1, 1), 
                tint, node.AbsoluteRotation, new Vector2(0f, 0.5f), new Vector2(node.Length * Math.Abs(skeleton.Scale), 2 / camera.Zoom), SpriteEffects.None, 0);
        }
        void DrawPoint(Vector2 pos, Color tint, float size)
        {
            spriteBatch.Draw(circle, pos, new Rectangle(0, 0, circle.Width, circle.Height),
                tint, 0, new Vector2(circle.Width / 2, circle.Height / 2), size*HandleSize / 64f / camera.Zoom, SpriteEffects.None, 0);
        }


        public bool DrawSprites { get; set; }

        public Texture2D blank { get; set; }
    }
}
