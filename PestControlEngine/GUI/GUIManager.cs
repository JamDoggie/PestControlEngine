using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PestControlEngine.GUI.Enum;
using PestControlEngine.Libs.Helpers;
using PestControlEngine.Libs.Helpers.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PestControlEngine.GUI
{
    public class GUIManager
    {
        private Dictionary<string, Screen> _LoadedScreens = new Dictionary<string, Screen>();

        private string _CurrentScreen = Util.GetEngineNull();

        public string ScreenToLoad = Util.GetEngineNull();

        public int VirtualViewWidth { get; set; } = 256;

        public int VirtualViewHeight { get; set; } = 224;

        public StretchType Stretch { get; set; } = StretchType.FIT_PARENT_ASPECT_RATIO;

        public bool UseVirtualSize { get; set; } = true;

        private RenderTarget2D _RenderTarget = null;

        public void LoadScreen(string key, Screen screen)
        {
            _LoadedScreens.Add(key, screen);
        }

        public void SetScreen(string screen)
        {
            ScreenToLoad = screen;
        }

        public void Update(GameTime gameTime, GameInfo info)
        {
            if (UseVirtualSize)
            {
                switch (Stretch)
                {
                    case StretchType.FIT_PARENT_ASPECT_RATIO:
                        // Make aspect ratio of viewport the same as the parent window.
                        float aspectRatio = ((float)info.graphicsDevice.PresentationParameters.BackBufferWidth / (float)info.graphicsDevice.PresentationParameters.BackBufferHeight);
                        float newWidth = (VirtualViewHeight * aspectRatio);


                        Console.WriteLine($"{info.graphicsDevice.PresentationParameters.BackBufferWidth}____{info.graphicsDevice.PresentationParameters.BackBufferHeight}");
                        VirtualViewWidth = (int)newWidth;
                        break;
                }
            }

            if (_RenderTarget != null)
                _RenderTarget.Dispose();

            _RenderTarget = new RenderTarget2D(info.graphicsDevice, VirtualViewWidth, VirtualViewHeight, false, info.graphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);

            if (ScreenToLoad != Util.GetEngineNull())
            {
                _CurrentScreen = ScreenToLoad;
                ScreenToLoad = Util.GetEngineNull();
            }

            Screen currentScreen = GetScreen(_CurrentScreen);

            if (currentScreen != null)
                currentScreen.Update(gameTime, info);

        }

        public void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, GameInfo info)
        {
            if (info == null)
                return;

            Screen currentScreen = GetScreen(_CurrentScreen);

            if (_RenderTarget != null)
                info.graphicsDevice.SetRenderTarget(_RenderTarget);

            // Only do this on the GUI layer so that it doesn't overwrite the object layer.
            if (_RenderTarget != null)
                info.graphicsDevice.Clear(Color.Transparent);

            StartDefaultSpriteBatch(spriteBatch);

            if (currentScreen != null)
                currentScreen.Draw(gameTime, device, spriteBatch, info);

            SwitchToDefaultSpriteBatch(spriteBatch);

            info.graphicsDevice.SetRenderTarget(null);

            // Draw render target to screen so we can "stretch" the viewport if needed.
            Rectangle bounds = new Rectangle(0, 0, info.graphicsDevice.PresentationParameters.BackBufferWidth, info.graphicsDevice.PresentationParameters.BackBufferHeight);

            spriteBatch.Draw(_RenderTarget, bounds, Color.White);

            spriteBatch.End();
        }

        public Screen GetScreen(string screen)
        {
            Screen retScreen;

            _LoadedScreens.TryGetValue(_CurrentScreen, out retScreen);

            return retScreen;
        }

        /// <summary>
        /// Ends given sprite batch and begins it with the default parameters.
        /// WARNING: sprite batch must already be started. Otherwise it will throw an exception.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void SwitchToDefaultSpriteBatch(SpriteBatch spriteBatch)
        {
            if (spriteBatch == null)
                return;

            spriteBatch.End();
            StartDefaultSpriteBatch(spriteBatch);
        }

        /// <summary>
        /// Starts given spritebatch and beings it with the default parameters.
        /// WARNING: sprite batch must not already be started. This starts the spritebatch assuming it wasn't running.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void StartDefaultSpriteBatch(SpriteBatch spriteBatch)
        {
            if (spriteBatch == null)
                return;

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
        }
    }
}
