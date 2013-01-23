using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BoneLibrary;
using Unknown;
using System.Windows.Forms;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace BoneEditor
{
    /// <summary>
    /// Allows user to create new sprites with simple drag and drop. Can also work independly to create spritesheets.
    /// </summary>
    public class SpriteCreator : GraphicsDeviceControl
    {
        public event EventHandler SpritePicked, SelectedSpriteChanged, Initialized;
        int index;
        List<Skin> sprites = new List<Skin>();
        Texture2D Texture;
        Skin destinationSprite, _selectedSprite;
       
        public Skin Selected 
        {
            get { return _selectedSprite; }
            set
            {
                _selectedSprite = value;
                if (SelectedSpriteChanged != null) SelectedSpriteChanged(this, null);
            }

        }
        Texture2D blank;
        Texture2D arrow;

        Vector2 mouseClickPosition;
        Vector2 mousePosition;
        bool dragging;

        SpriteBatch sb;
        Camera camera;
        //Main main;
        public SpriteCreator()
        {
            //this.main = main;

        }
        public void Pick(Skin sprite, Texture2D tex)
        {
            Texture = tex;
            this.destinationSprite = sprite;
            camera.Position = new Vector2(Texture.Width, Texture.Height)/2;
            camera.Zoom = camera.ScreenSize.Y / Texture.Height * 0.95f;
            Selected = sprite;
        }
        protected override void OnMouseWheel(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (e.Delta > 0) camera.Zoom *= 1.1f;
            if (e.Delta < 0) camera.Zoom /= 1.1f;
            Refresh();

        }
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            mouseClickPosition = mousePosition;
            if (Texture == null) return;
            Skin overMouse = null;
            foreach(var s in sprites)
                if (s.Source.Contains((int)mousePosition.X, (int)mousePosition.Y))
                {
                    overMouse = s;
                }
            if (overMouse != null && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                sprites.Remove(overMouse);
                if (Selected == overMouse)
                {
                    Selected = null;
                }
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (overMouse != null)
                {
                    Selected = overMouse;
                    Selected.Origin = mousePosition - new Vector2(Selected.Source.X, Selected.Source.Y);
                    if (ModifierKeys.HasFlag(Keys.Shift))
                    {
                        var half = new Vector2(Selected.Source.Width, Selected.Source.Height) / 2;
                        Selected.Origin = Extensions.Round(Selected.Origin / half) * half;
                    }
                    return;
                }
                
                var fill = new FloodFill(Texture, (int)mousePosition.X, (int)mousePosition.Y);

                if (fill.BoundingRectangle == Rectangle.Empty)
                {

                    if (Selected != null) Selected.Source = Rectangle.Empty;
                    if (destinationSprite != null && SpritePicked != null) SpritePicked(this, null);
                    return;
                }
                if (destinationSprite != null)
                {
                    destinationSprite.Source = fill.BoundingRectangle;
                    dragging = true;

                }
                else
                {
                    foreach (var s in sprites)
                        if (s.Source.Intersects(fill.BoundingRectangle)) return;
                    Selected = new Skin() { Texture = Texture, Source = fill.BoundingRectangle, Name = "sprite"+index++};
                    Selected.Origin = mousePosition - new Vector2(Selected.Source.X, Selected.Source.Y);

                    sprites.Add(Selected);
                }
                mouseClickPosition = mousePosition;
   
            }
        }
        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (!dragging || !Enabled || e.Button != System.Windows.Forms.MouseButtons.Left || destinationSprite == null) return;
            destinationSprite.Origin = mouseClickPosition - new Vector2(destinationSprite.Source.X, destinationSprite.Source.Y);
            
            dragging = false;
            if (SpritePicked != null) SpritePicked(this, null);
        }
        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            mousePrevious = mouseCurrent;
            mouseCurrent = new Vector2(e.X, e.Y);
            if (e.Button == System.Windows.Forms.MouseButtons.Middle) camera.Position -= (mouseCurrent - mousePrevious) / camera.Zoom;
            base.OnMouseMove(e);
            mousePosition = new Vector2(e.X, e.Y);
            mousePosition = camera.Unproject(mousePosition);
            if (e.Button == System.Windows.Forms.MouseButtons.Left & Selected != null)
            {
                Selected.Angle = -(mousePosition - mouseClickPosition).GetAngle();
                if(ModifierKeys.HasFlag(Keys.Shift))
                    Selected.Angle = (float)Math.Round(Selected.Angle / MathHelper.PiOver4) * MathHelper.PiOver4;
            }
            Refresh();
        }



        protected override void Initialize()
        {
            sb = new SpriteBatch(GraphicsDevice);
            camera = new Camera(GraphicsDevice);
            blank = Extensions.CreateBlank(GraphicsDevice);
            var content = new ContentManager(Services, "Content");
            arrow = content.Load<Texture2D>("arrow");
            if (Initialized != null) Initialized(this, null);
        }

        protected override void Draw()
        {
            //GraphicsDevice.Clear(Color.Pink);
            if (Texture == null) return;
            camera.Begin(sb);
            sb.Draw(Texture, Vector2.Zero, Color.White);
            if (dragging)
            {
                var v = mouseClickPosition - mousePosition;
                sb.Draw(
                    blank, mouseClickPosition, new Rectangle(0, 0, 1, 1), Color.White,
                    -destinationSprite.Angle, new Vector2(2/100f, 0.5f), new Vector2(100, 4)/camera.Zoom, SpriteEffects.None, 0);
            }
            foreach (var s in sprites)
            {
                sb.Draw(blank, s.Source, new Rectangle(0, 0, 1, 1), s==Selected ? Color.Yellow * 0.3f : Color.White * 0.2f);
                sb.Draw(arrow, new Vector2(s.Origin.X + s.Source.X, s.Origin.Y + s.Source.Y), new Rectangle(0, 0, arrow.Width, arrow.Height), Color.White, s.Angle+MathHelper.PiOver2, new Vector2(4f,2f), 1, SpriteEffects.None, 0);
            }
            sb.End();
        }

        /// <summary>
        /// Flood fill is used to determine source rectangle of new sprite
        /// </summary>
        class FloodFill
        {
            public int bX, bY, eX, eY;
            public Color[] colors;
            public int width, height;
            public Color this[int x, int y]
            {
                get { return colors[x + y * width]; }
                set { colors[x + y * width] = value; }
            }
            public FloodFill(Texture2D tex, int sX, int sY)
            {
                if (sX <= 0 || sX >= tex.Width - 1 || sY <= 0 || sY >= tex.Height - 1) return;
                colors = new Color[tex.Width * tex.Height];
                tex.GetData(colors);
                width = tex.Width;
                height = tex.Height;
                bX = sX;
                bY = sY;
                eX = sX;
                eY = sY;

                Fill(sX, sY);
            }
            void Fill(int x, int y)
            {
                if (this[x, y].A == 0) return;
                this[x, y] = Color.Transparent;
                if (x < bX) bX = x;
                if (x > eX) eX = x;
                if (y < bY) bY = y;
                if (y > eY) eY = y;

                if (x > 0 && x < width - 1 && y > 0 && y < height - 1)
                {
                    Fill(x - 1, y);
                    Fill(x + 1, y);
                    Fill(x, y - 1);
                    Fill(x, y + 1);
                }
            }
            public Rectangle BoundingRectangle
            {
                get
                {
                    if (bX == eX || bY == eY) return Rectangle.Empty;
                    return new Rectangle(bX, bY, eX - bX + 1, eY - bY + 1);
                }
            }
        }

        public void Load(string path)
        {
            Selected = null;
            var sheet = Spritesheet.FromFile(path, GraphicsDevice);
            foreach (var sprite in sheet.GetSprites())
                sprites.Add(sprite);
            Texture = sheet.Texture;
            asset = sheet.TextureAsset;
            camera.Position = new Vector2(Texture.Width, Texture.Height) / 2;
            camera.Zoom = camera.ScreenSize.Y / Texture.Height * 0.7f;
        }

        public void Save(string path)
        {
            var sheet = new Spritesheet(asset,sprites);

            using (var stream = File.Create(path))
                sheet.ToStream(stream);
        }

        string asset;
        private Vector2 mouseCurrent;
        private Vector2 mousePrevious;
        public void FromPng(string p)
        {
            asset = Path.GetFileNameWithoutExtension(p);
            sprites = new List<Skin>();
            Texture = Extensions.FromFile(GraphicsDevice, p, true);
            camera.Position = new Vector2(Texture.Width, Texture.Height) / 2;
            camera.Zoom = camera.ScreenSize.Y / Texture.Height * 0.7f;

        }
    }
}
