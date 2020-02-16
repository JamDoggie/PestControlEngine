using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PestControlEngine.Event.Structs;
using PestControlEngine.GameManagers;
using PestControlEngine.GUI.Enum;
using PestControlEngine.Libs.Helpers;
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

        public UIRectangle ButtonRectangle = new UIRectangle();

        public Color OutlineColor { get; set; } = Color.White;

        public Color OutlineHoverColor { get; set; } = new Color(170, 170, 170);

        public Color OutlinePressColor { get; set; } = new Color(110, 110, 110);

        public Color TextColor { get; set; } = Color.White;

        public int LineThickness { get; set; } = 2;

        private Color _CurrentOutlineColor = Color.White;

        public bool IsHovered { get; set; } = false;

        public bool IsPressed { get; set; } = false;

        public bool ScaleToText { get; set; } = true;

        public Button(Vector2 position, int width, int height)
        {
            Position = position;
            Width = width;
            Height = height;

            // Text element
            AddChild(ButtonTextBlock);

            ButtonTextBlock.HorizontalAlignment = EnumHorizontalAlignment.CENTER;
            ButtonTextBlock.VerticalAlignment = EnumVerticalAlignment.CENTER;

            // Rectangle element
            AddChild(ButtonRectangle);

            ButtonRectangle.Filled = false;
            ButtonRectangle.StrokeSize = LineThickness;
            ButtonRectangle.FillParent = true;

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
            if (ScaleToText)
            {
                Vector2 textBoxSize = ContentLoader.GetFont("engine_font").MeasureString(ButtonTextBlock.Text);

                Width = (int)textBoxSize.X;
                Height = (int)textBoxSize.Y;
            }

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

            ButtonRectangle.RectangleColor = _CurrentOutlineColor;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, device, spriteBatch);
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

            if (ButtonTextBlock.Text == "Faded")
            {
                if (Game.GetGame().guiManager != null)
                {
                    Game.GetGame().guiManager.SetScreen("debug_screen");
                }
            }
            else
            {
                if (Game.GetGame().guiManager != null)
                {
                    Game.GetGame().guiManager.SetScreen("fade_screen");
                }
            }
        }

        private void Button_MouseReleasedEvent(MouseEventArgs e)
        {
            IsPressed = false;
        }
    }
}
