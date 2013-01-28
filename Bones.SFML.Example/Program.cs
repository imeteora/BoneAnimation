using BoneLibrary;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Utils;
namespace Test
{
    class Program
    {
        SkeletonAsset asset;
        Skeleton skeleton;
        SpriteBatch sb;


        void Run()
        {
            var x = Texture.MaximumSize/2;
            var tex1 = new Texture(x, x);
            var img = tex1.CopyToImage();

            var shape = new ConvexShape();
            shape.SetPointCount(4);
            shape.SetPoint(0, new Vector2f(0, 0));
            shape.SetPoint(1, new Vector2f(100, 0));
            shape.SetPoint(2, new Vector2f(100, 100));
            shape.SetPoint(3, new Vector2f(0, 100));

            shape.Position = new Vector2f(300, 300);
            shape.FillColor = Color.Yellow;

            var settings = new ContextSettings();
            settings.AntialiasingLevel = 16;

            var rt = new RenderWindow(new VideoMode(800, 600), "Game", Styles.Default, settings);
            rt.SetFramerateLimit(60);
            rt.Closed += rt_Closed;
            asset = SkeletonAsset.FromFile("hero.skt");
            skeleton = asset.CreateSkeleton();
            skeleton.Position = new Vector2f(400, 400);
            skeleton.Scale = 1f;

            var view = rt.GetView();

            rt.SetView(view);
            skeleton.PlayAnimation("idle");
            skeleton.PlayAnimation("walk");

            sb = new SpriteBatch();

            while (rt.IsOpen())
            {
                rt.DispatchEvents();
                rt.Clear();
                sb.Begin();

                skeleton.ApplyAnimations(1/60f);
                skeleton.ApplyTransformations();
                skeleton.Draw(sb);


                rt.Draw(shape);
                sb.End();
                rt.Draw(sb);
                rt.Display();
            }
        }
        float angle, scale;
        float vel = 0.1f;

        static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }

        void rt_Closed(object sender, EventArgs e)
        {
            var rt = sender as RenderWindow;
            rt.Close();
        }
    }
}
