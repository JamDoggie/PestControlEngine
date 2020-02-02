using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using PestControlAnimation.Objects;
using PestControlEngine.Graphics;
using PestControlEngine.GUI;
using PestControlEngine.Libs.Helpers;
using PestControlEngine.Objects;
using PestControlEngine.Resource;
using System;
using System.IO;

namespace PestControlEngine
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteBatch guiBatch;
        ObjectManager objManager;
        Screen debugScreen;
        GUIManager guiManager;
        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;

            Window.AllowUserResizing = true;

            objManager = new ObjectManager();

            debugScreen = new Screen();

            guiManager = new GUIManager();
            guiManager.LoadScreen("debug_screen", debugScreen);

            

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            ContentLoader.LoadTexture("Ryu Intro Sheet", Texture2D.FromStream(GraphicsDevice, File.Open("Ryu Intro Sheet.png", FileMode.Open)));

            // Test, remove later.
            GameObject ryuIntro = new GameObject();
            ryuIntro.CurrentAnimation = Animation.ReadAnimationFile("intro_ryu_idle.pcaf");
            ryuIntro.CurrentAnimation.Play();

            objManager.GetObjects().Add(ryuIntro);

            base.Initialize();

            Util.CurrentResolution = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            guiBatch = new SpriteBatch(GraphicsDevice);

            ContentLoader.LoadTextures(Content);
            ContentLoader.LoadFonts(Content);
            ContentLoader.LoadShaders(Content);

            Button button = new Button(new Vector2(200, 200), 200, 10);
            button.ButtonTextBlock.Text = "Pest Control Debug Screen";

            Grid grid = new Grid();
            grid.AddCell(button, 0.25d, 0.25d, 0.5d, 0.5d);

            debugScreen.Controls.Add(grid);
            debugScreen.Controls.Add(button);

            guiManager.SetScreen("debug_screen");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            objManager.Update(gameTime);
            guiManager.Update(gameTime);

            Util.CurrentResolution = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            objManager.Draw(GraphicsDevice, spriteBatch);

            spriteBatch.End();

            guiBatch.Begin();

            guiManager.Draw(gameTime, GraphicsDevice, guiBatch);

            guiBatch.End();

            base.Draw(gameTime);
        }
    }
}
