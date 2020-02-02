using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using PestControlEngine.Libs.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PestControlEngine.Resource
{
    public static class ContentLoader
    {
        private static Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();
        private static Dictionary<string, BitmapFont> _fonts = new Dictionary<string, BitmapFont>();
        private static Dictionary<string, Effect> _effects = new Dictionary<string, Effect>();

        public static void LoadTextures(ContentManager Content)
        {

        }

        public static void LoadFonts(ContentManager Content)
        {
            BitmapFont EngineFont = Content.Load<BitmapFont>("engine_font");
            LoadFont("engine_font", EngineFont);
        }

        public static void LoadShaders(ContentManager Content)
        {
            Effect blackwhite_gradient = Content.Load<Effect>("blackwhite_gradient");
            LoadShader("blackwhite_gradient", blackwhite_gradient);
        }

        /// <summary>
        /// Returns texture from the _textures list. If the texture has not been loaded into memory, it will return null even if the texture exists in the game files.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Texture2D GetTexture(string key)
        {
            _textures.TryGetValue(key, out Texture2D tex);

            return tex;
        }

        /// <summary>
        /// Returns font from the _fonts list. If the font has not been loaded into memory, it will return null even if the font exists in the game files.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static BitmapFont GetFont(string key)
        {
            _fonts.TryGetValue(key, out BitmapFont fnt);

            if (fnt == null)
            {
                Console.WriteLine($"Content Loader Error: Could not find Font {key}");
                throw new FileNotFoundException($"Content Loader could not find the Font \"{key}\"");
            }
                
            return fnt;
        }
        /// <summary>
        /// Returns an effect from the _effect list. If the shader has not been loaded into memory, it will return null even if the shader exists in the game files.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Effect GetShader(string key)
        {
            _effects.TryGetValue(key, out Effect fx);

            if (fx == null)
            {
                Console.WriteLine($"Content Loader Error: Could not find Shader {key}");
                throw new FileNotFoundException($"Content Loader could not find the Shader \"{key}\"");
            }
            
            return fx;
        }

        /// <summary>
        /// Loads the texture into the dictionary and in turn the memory.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="texture"></param>
        public static void LoadTexture(string key, Texture2D texture)
        {
            _textures[key] = texture;
        }

        /// <summary>
        /// Loads the font into the dictionary and in turn the memory.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="font"></param>
        public static void LoadFont(string key, BitmapFont font)
        {
            _fonts[key] = font;
        }

        /// <summary>
        /// Loads the font into the dictionary and in turn the memory.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="font"></param>
        public static void LoadShader(string key, Effect shader)
        {
            _effects[key] = shader;
        }
    }

}
