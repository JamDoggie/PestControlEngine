using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PestControlEngine.GUI.Enum;
using PestControlEngine.Libs.Helpers;
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

        public void LoadScreen(string key, Screen screen)
        {
            _LoadedScreens.Add(key, screen);
        }

        public void SetScreen(string screen)
        {
            ScreenToLoad = screen;
        }

        public void Update(GameTime gameTime)
        {
            if (ScreenToLoad != Util.GetEngineNull())
            {
                _CurrentScreen = ScreenToLoad;
                ScreenToLoad = Util.GetEngineNull();
            }

            Screen currentScreen = GetScreen(_CurrentScreen);

            if (currentScreen != null)
                currentScreen.Update(gameTime);

        }

        public void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch)
        {
            Screen currentScreen = GetScreen(_CurrentScreen);

            if (currentScreen != null)
                currentScreen.Draw(gameTime, device, spriteBatch);
        }

        public Screen GetScreen(string screen)
        {
            Screen retScreen;

            _LoadedScreens.TryGetValue(_CurrentScreen, out retScreen);

            return retScreen;
        }
    }
}
