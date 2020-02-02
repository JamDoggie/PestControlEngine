using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PestControlEngine.GUI
{
    public class Screen
    {
        public List<UIElement> Controls { get; set; } = new List<UIElement>();

        public void Update(GameTime gameTime)
        {
            foreach(UIElement control in Controls)
            {
                control.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch)
        {
            foreach (UIElement control in Controls)
            {
                control.Draw(gameTime, device, spriteBatch);
            }
        }
    }
}
