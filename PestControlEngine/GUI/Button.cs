using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PestControlEngine.Event.Structs;
using PestControlEngine.Graphics;
using PestControlEngine.GUI.Enum;
using PestControlEngine.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PestControlEngine.GUI
{
    public class Button : UIElement
    {
        public TextElement ButtonTextBlock = new TextElement();

        public Color OutlineColor { get; set; } = Color.White;

        public Color OutlineHoverColor { get; set; } = new Color(170, 170, 170);

        public Color OutlinePressColor { get; set; } = new Color(110, 110, 110);

        public Color TextColor { get; set; } = Color.White;

        private Color _CurrentOutlineColor = Color.White;

        public bool IsHovered { get; set; } = false;

        public bool IsPressed { get; set; } = false;

        public Button(Vector2 position, int width, int height)
        {
            Position = position;
            Width = width;
            Height = height;

            AddChild(ButtonTextBlock);

            ButtonTextBlock.HorizontalAlignment = EnumHorizontalAlignment.CENTER;
            ButtonTextBlock.VerticalAlignment = EnumVerticalAlignment.CENTER;

            _CurrentOutlineColor = OutlineColor;

            // Event stuff
            MouseEnterEvent += Button_MouseEnterEvent;
            MouseLeaveEvent += Button_MouseLeaveEvent;
            MouseClickedEvent += Button_MouseClickedEvent;
            MouseReleasedEvent += Button_MouseReleasedEvent;

            DynamicallyScale = true;
        }

        public override void Update(GameTime gameTime)
        {
            // Resize to fit with text.
            //Vector2 textBoxSize = ContentLoader.GetFont("engine_font").MeasureString(ButtonTextBlock.Text);

            //Width = (int)textBoxSize.X;
            //Height = (int)textBoxSize.Y;

            if (IsHovered || IsPressed)
            {
                if (IsHovered)
                    _CurrentOutlineColor = OutlineHoverColor;

                if (IsPressed)
                    _CurrentOutlineColor = OutlinePressColor;
            }
            else
            {
                _CurrentOutlineColor = OutlineColor;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, device, spriteBatch);

            ObjectManager.DrawRectangle(spriteBatch, device, GetBoundingBox(), 2, _CurrentOutlineColor);
        }

        private void Button_MouseEnterEvent(MouseEventArgs e)
        {
            IsHovered = true;
        }

        private void Button_MouseLeaveEvent(MouseEventArgs e)
        {
            IsHovered = false;
            IsPressed = false;
        }

        private void Button_MouseClickedEvent(MouseEventArgs e)
        {
            IsPressed = true;
        }

        private void Button_MouseReleasedEvent(MouseEventArgs e)
        {
            IsPressed = false;
        }
    }
}
