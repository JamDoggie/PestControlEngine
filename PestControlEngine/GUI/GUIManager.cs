using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private Dictionary<string, Screen> _LoadedScreens { get; set; } = new Dictionary<string, Screen>();

        private string _CurrentScreen { get; set; } = Util.GetEngineNull();

        public void LoadScreen(string key, Screen screen)
        {
            _LoadedScreens.Add(key, screen);
        }

        public void SetScreen(string screen)
        {
            _CurrentScreen = screen;
        }

        public void Update(GameTime gameTime)
        {
            Screen currentScreen = null;

            _LoadedScreens.TryGetValue(_CurrentScreen, out currentScreen);

            if (currentScreen != null)
                currentScreen.Update(gameTime);
        }

        public void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch)
        {
            Screen currentScreen = null;

            _LoadedScreens.TryGetValue(_CurrentScreen, out currentScreen);

            if (currentScreen != null)
                currentScreen.Draw(gameTime, device, spriteBatch);
        }
    }
}
