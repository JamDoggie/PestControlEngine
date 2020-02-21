using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using PestControlAnimation.Objects;
using PestControlEngine.GameManagers;
using PestControlEngine.GUI;
using PestControlEngine.GUI.Enum;
using PestControlEngine.Libs.Helpers;
using PestControlEngine.Libs.Helpers.Structs;
using PestControlEngine.Objects;
using PestControlEngine.Resource;
using PestControlEngine.Sound;
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
        Screen fadeTest;
        public GUIManager guiManager;
        public SoundManager soundManager;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = false;

            IsMouseVisible = true;

            Window.AllowUserResizing = true;

            objManager = new ObjectManager();

            debugScreen = new Screen();
            fadeTest = new Screen();

            guiManager = new GUIManager();
            guiManager.LoadScreen("debug_screen", debugScreen);
            guiManager.LoadScreen("fade_screen", fadeTest);

            GameInfo gameInfo = new GameInfo(GraphicsDevice, spriteBatch, objManager, guiManager, GetResolution(), Content, soundManager);

            soundManager = new SoundManager(gameInfo);
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

            GameCamera MainCamera = new GameCamera(new Rectangle(0, 0, 256, 224))
            {
                IsEnabled = true
            };
            objManager.GetObjects().Add(MainCamera);

            GraphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;

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
            guiBatch = new SpriteBatch(GraphicsDevice);

            

            ContentLoader.LoadTextures(Content);
            ContentLoader.LoadFonts(Content);
            ContentLoader.LoadShaders(Content);
            ContentLoader.LoadSounds(Content);


            Grid grid = new Grid();

            Button button = new Button(new Vector2(200, 200), 200, 200);
            button.ButtonTextBlock.Text = "Scale Test";
            button.ScaleToText = false;

            
            grid.AddCell(button, 0.25d, 0.25d, 0.5d, 0.5d);

            debugScreen.AddControl(grid);

            UIFPSCounter fpsCounter = new UIFPSCounter();
            fpsCounter.HorizontalAlignment = EnumHorizontalAlignment.RIGHT;

            debugScreen.AddControl(fpsCounter);

            // TEST SCREEN FOR FADING
            Grid grid2 = new Grid();

            Button button2 = new Button(new Vector2(200, 200), 200, 200);
            button2.ButtonTextBlock.Text = "Faded";
            button2.ScaleToText = false;

            grid2.AddCell(button2, 0.1d, 0.1d, 0.5d, 0.5d);

            fadeTest.AddControl(grid2);

            // Set default screen
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
            GameInfo gameInfo = new GameInfo(GraphicsDevice, spriteBatch, objManager, guiManager, GetResolution(), Content, soundManager);

            objManager.Update(gameTime, gameInfo);
            guiManager.Update(gameTime, gameInfo);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GameInfo gameInfo = new GameInfo(GraphicsDevice, spriteBatch, objManager, guiManager, GetResolution(), Content, soundManager);

            GraphicsDevice.Clear(Color.Black);

            // Draw objects in current map.
            objManager.Draw(GraphicsDevice, spriteBatch, gameInfo);

            guiManager.Draw(gameTime, GraphicsDevice, guiBatch, gameInfo);

            base.Draw(gameTime);
        }

        public Vector2 GetResolution()
        {
            if (objManager.UseVirtualSize)
                return new Vector2(objManager.VirtualViewWidth, objManager.VirtualViewHeight);

            return new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        }
    }
}
