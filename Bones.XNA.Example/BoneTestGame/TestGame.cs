using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using BoneLibrary;
using System.IO;

namespace BoneTestGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TestGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Skeleton s;
        SpriteFont font;
        List<Animation> animations;
        int index;
        public TestGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //var asset2 = SkeletonAsset.FromFile("c:/human.skt", true);
            //asset2.Save("c:/asset.skt");
            var asset = SkeletonAsset.FromFile("hero.skt", GraphicsDevice);
            
            s = asset.CreateSkeleton();


            s.Position = Vector2.One * 200;
            //s.SetPose("idle");
            s.PlayAnimation("idle");
            font = Content.Load<SpriteFont>("font");
            animations = asset.Animations.ToList();
            //s.PlayAnimation("punch");
            //s.SetIdleAnimation("idle");
            //s.PlayAnimation("walk");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        KeyboardState ksP, ksC;

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            ksP = ksC;
            ksC = Keyboard.GetState();
            var ks = Keyboard.GetState();

            if(animations.Count > 0)
            {

            if (ksC.IsKeyDown(Keys.Q) && ksP.IsKeyUp(Keys.Q)) s.PlayAnimation(animations[index].Name);
            if (ksC.IsKeyDown(Keys.A) && ksP.IsKeyUp(Keys.A)) index--;
            if (ksC.IsKeyDown(Keys.D) && ksP.IsKeyUp(Keys.D)) index++;
            if (index < 0) index = animations.Count - 1;
            if (index >= animations.Count) index = 0;
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if(animations.Count> 0)
                spriteBatch.DrawString(font, animations[index].Name, new Vector2(10, 10), Color.White);
            s.Apply();
            s.Draw(spriteBatch);
            spriteBatch.DrawString(font, "A and D to change selected animation. Q to play it", new Vector2(10,30), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
