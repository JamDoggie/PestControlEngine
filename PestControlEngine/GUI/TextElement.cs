using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using PestControlEngine.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PestControlEngine.GUI
{
    public class TextElement : UIElement
    {
        public string Text { get; set; }

        public Color TextColor { get; set; } = Color.White;

        private RasterizerState _RasterizerState = new RasterizerState() { ScissorTestEnable = true };

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch)
        {
            // Little explanation, this draws the text so that monogame doesn't draw it outside it's parent element. 
            //This unfortunately does restart the spritebatch which(i believe) can effect performance even if it's small.

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, _RasterizerState);

            // Save copy of scissor rectangle for later
            Rectangle currentRect = spriteBatch.GraphicsDevice.ScissorRectangle;

            // Set scissor rectangle to the bounding box of the parent object. This makes it so the text is only rendered within the parent element and will cut off outside of it.
            if (Parent != null)
                spriteBatch.GraphicsDevice.ScissorRectangle = Parent.GetBoundingBox();

            spriteBatch.DrawString(ContentLoader.GetFont("engine_font"), Text, RenderPosition, TextColor);

            // Restore scissor rectangle
            spriteBatch.GraphicsDevice.ScissorRectangle = currentRect;

            spriteBatch.End();
            spriteBatch.Begin();

            base.Draw(gameTime, device, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            Width = (int)ContentLoader.GetFont("engine_font").MeasureString(Text).Width;
            Height = (int)ContentLoader.GetFont("engine_font").MeasureString(Text).Height;

            base.Update(gameTime);
        }
    }
}
