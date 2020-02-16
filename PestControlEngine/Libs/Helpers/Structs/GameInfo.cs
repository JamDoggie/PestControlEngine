using Microsoft.Xna.Framework.Graphics;
using PestControlEngine.GameManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PestControlEngine.Libs.Helpers.Structs
{
    public class GameInfo
    {
        public GraphicsDevice graphicsDevice { get; set; }

        public SpriteBatch spriteBatch { get; set; }

        public ObjectManager objectManager { get; set; }

        public GameInfo(GraphicsDevice device, SpriteBatch batch, ObjectManager manager)
        {
            graphicsDevice = device;
            spriteBatch = batch;
            objectManager = manager;
        }
    }
}
