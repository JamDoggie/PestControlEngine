using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PestControlEngine.GameManagers;
using PestControlEngine.GUI;
using PestControlEngine.Sound;
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

        public GUIManager guiManager { get; set; }

        public Vector2 Resolution { get; set; }

        public ContentManager Content { get; set; }

        public SoundManager SoundManager { get; set; }
        public GameInfo(GraphicsDevice device, SpriteBatch batch, ObjectManager manager, GUIManager GuiManager, Vector2 res, ContentManager conManager, SoundManager soundManager)
        {
            graphicsDevice = device;
            spriteBatch = batch;
            objectManager = manager;
            guiManager = GuiManager;
            Resolution = res;
            Content = conManager;
            SoundManager = soundManager;
        }
    }
}
