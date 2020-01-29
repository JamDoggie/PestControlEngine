﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PestControlEngine.Objects;
using PestControlEngine.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PestControlEngine.Graphics
{
    public class ObjectManager
    {
        private List<GameObject> Objects = new List<GameObject>();

        public void Draw(GraphicsDevice device, SpriteBatch spriteBatch)
        {
            if (spriteBatch != null)
            {

                foreach (GameObject drawable in Objects)
                {

                    drawable.Draw(device, spriteBatch);

                }

            }
            else
            {
                throw new ArgumentNullException(nameof(spriteBatch));
            }
        }

        public List<GameObject> GetObjects()
        {
            return Objects;
        }

        public void Update(GameTime gameTime)
        {
            foreach (GameObject drawable in Objects)
            {
                drawable.Update(gameTime);
            }
        }

        // Returns 1x1 texture that is RGB 255,255,255
        public static Texture2D GetWhitePixel(GraphicsDevice graphicsDevice)
        {
            if (ContentLoader.GetTexture("engine_onepx") != null)
            {
                return ContentLoader.GetTexture("engine_onepx");
            }

            Texture2D texture = new Texture2D(graphicsDevice, 1, 1);
            Color[] colors = new Color[1];
            colors[0] = new Color(255, 255, 255);
            texture.SetData(colors);

            // Load texture into memory so we don't have to do the expensive operation of generating it every time.
            ContentLoader.LoadTexture("engine_onepx", texture);

            return texture;
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Rectangle rectangle, Color color)
        {
            Texture2D texture = GetWhitePixel(graphicsDevice);

            if (spriteBatch == null)
            {
                return;
            }

            // Top part of rectangle
            spriteBatch.Draw(texture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, 1), new Rectangle(0, 0, 1, 1), color);
            // Left side of rectangle
            spriteBatch.Draw(texture, new Rectangle(rectangle.X, rectangle.Y + 1, 1, rectangle.Height - 1), new Rectangle(0, 0, 1, 1), color);
            // Right side of rectangle
            spriteBatch.Draw(texture, new Rectangle(rectangle.X + rectangle.Width - 1, rectangle.Y + 1, 1, rectangle.Height - 1), new Rectangle(0, 0, 1, 1), color);
            // Bottom part of rectangle
            spriteBatch.Draw(texture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - 1, rectangle.Width, 1), new Rectangle(0, 0, 1, 1), color);
        }
    }
}
