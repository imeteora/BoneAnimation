using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Unknown
{
    /// <summary>
    /// Handles current state of view(zoom, position), provides matrix for SpriteBatch.
    /// </summary>
    public class Camera
    {
        GraphicsDevice gd;
        float zoom;
        public Camera(GraphicsDevice gd)
        {
            Zoom = 1;
            //Rotation = 1;
            this.gd = gd;
        }

        public Vector2 Position
        { get; set; }
        public float Zoom
        {
            get { return zoom; }
            set
            {
                zoom = value;
                //if (zoom < 1f) zoom = 1f;
            }
        }
        public Matrix Projection
        {
            get
            {
                return Matrix.CreateOrthographicOffCenter(0, ScreenSize.X, ScreenSize.Y, 0, -1, 1);
            }
        }
        public Matrix View
        {
            get
            {
                Vector2 v = ScreenSize / 2;

                return
                    Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                    Matrix.CreateScale(Zoom, Zoom, 1f) *
                    //Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateTranslation(v.X, v.Y, 0);

            }
        }
        public Rectangle ScreenRectangle
        {
            get
            {
                return new Rectangle(0, 0, gd.Viewport.Width, gd.Viewport.Height);
            }
        }
        public Vector2 ScreenSize
        {
            get
            {
                return new Vector2(gd.Viewport.Width, gd.Viewport.Height);
            }
        }
        public Rectangle VisibleArea
        {
            get
            {
                var r = Unproject(Vector2.Zero);
                return new Rectangle((int)r.X, (int)r.Y, (int)(ScreenSize.X / Zoom), (int)(ScreenSize.Y / Zoom));
            }
        }



        public void Begin(SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, View);
        }
        public Vector2 Unproject(Vector2 screenPosition)
        {

            return Position + Vector2.Divide((screenPosition - ScreenSize / 2), Zoom);
            //return -Vector2.Transform(screenPosition, View);
        }
        public Vector2 Project(Vector2 worldPosition)
        {
            return Vector2.Multiply((worldPosition - Position), Zoom) - ScreenSize / 2;
            //return Vector2.Transform(worldPosition, Matrix.Invert(View));

        }
        public override string ToString()
        {
            return String.Format("Camera, pos: {0} area: {1}", Position, VisibleArea);
            //return String.Format("Camera2D X:{0} Y:{1} Z:{2}x", Position.X, Position.Y, Zoom);
            //return String.Format("Camera2D X:{0} Y:{1} Z:{2}x R:{3}rad",Position.X, Position.Y, Zoom, Rotation);
        }

        public Vector2 UpperLeftCorner { get { return Unproject(Vector2.Zero); } }

        public Vector2 BottomRightCorner { get { return Unproject(ScreenSize); } }
    }
}
