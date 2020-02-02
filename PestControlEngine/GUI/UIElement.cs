using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PestControlEngine.Event.Structs;
using PestControlEngine.GUI.Enum;
using PestControlEngine.Libs.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PestControlEngine.GUI
{
    public class UIElement
    {
        // Variables
        public Vector2 Position;

        public int Width { get; set; }
        
        public int Height { get; set; }

        private MouseState _previousMouse = new MouseState();

        public Vector2 RenderPosition { get; set; } = new Vector2();

        public UIElement Parent { get; set; }

        public bool DynamicallyScale { get; set; }

        public EnumHorizontalAlignment HorizontalAlignment;

        public EnumVerticalAlignment VerticalAlignment;

        private List<UIElement> _Children { get; set; } = new List<UIElement>();

        // Events
        public delegate void MouseClick(MouseEventArgs e);
        public event MouseClick MouseClickedEvent;

        public delegate void MouseReleased(MouseEventArgs e);
        public event MouseReleased MouseReleasedEvent;

        public delegate void MouseMove(MouseEventArgs e);
        public event MouseMove MouseMovedEvent;

        public delegate void MouseEnter(MouseEventArgs e);
        public event MouseEnter MouseEnterEvent;

        public delegate void MouseLeave(MouseEventArgs e);
        public event MouseLeave MouseLeaveEvent;

        public UIElement()
        {
            // Event init
            MouseClickedEvent += MouseClicked_event;
            MouseReleasedEvent += MouseReleased_event;
            MouseMovedEvent += MouseMoved_event;
            MouseEnterEvent += MouseEnter_event;
            MouseLeaveEvent += MouseLeave_event;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Parent != null && Parent.Position != null)
            {
                RenderPosition = Position + Parent.Position;
            }

            // Alignment
            if (Parent != null)
            {
                switch (HorizontalAlignment)
                {
                    case EnumHorizontalAlignment.CENTER:
                        Position.X = (Parent.Width / 2) - Width / 2;
                        break;
                    case EnumHorizontalAlignment.LEFT:
                        Position.X = 0;
                        break;
                    case EnumHorizontalAlignment.RIGHT:
                        Position.X = (Parent.Width) - Width;
                        break;
                }

                switch (VerticalAlignment)
                {
                    case EnumVerticalAlignment.CENTER:
                        Position.Y = (Parent.Height / 2) - Height / 2;
                        break;
                    case EnumVerticalAlignment.TOP:
                        Position.Y = 0;
                        break;
                    case EnumVerticalAlignment.BOTTOM:
                        Position.Y = Parent.Height - Height;
                        break;
                }
            }

            // Events
            if (new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1).Intersects(GetBoundingBox()) && Mouse.GetState().LeftButton == ButtonState.Pressed && _previousMouse.LeftButton == ButtonState.Released)
            {
                MouseClickedEvent.Invoke(new MouseEventArgs()
                {
                    MouseState = Mouse.GetState(),
                    Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y)
                });
            }

            if (new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1).Intersects(GetBoundingBox()) && Mouse.GetState().LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
            {
                MouseReleasedEvent.Invoke(new MouseEventArgs()
                {
                    MouseState = Mouse.GetState(),
                    Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y)
                });
            }

            if (new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1).Intersects(GetBoundingBox()))
            {
                MouseMovedEvent.Invoke(new MouseEventArgs()
                {
                    MouseState = Mouse.GetState(),
                    Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y)
                });
            }

            if (new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1).Intersects(GetBoundingBox()) && !new Rectangle(_previousMouse.X, _previousMouse.Y, 1, 1).Intersects(GetBoundingBox()))
            {
                MouseEnterEvent.Invoke(new MouseEventArgs()
                {
                    MouseState = Mouse.GetState(),
                    Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y)
                });
            }

            if (!new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1).Intersects(GetBoundingBox()) && new Rectangle(_previousMouse.X, _previousMouse.Y, 1, 1).Intersects(GetBoundingBox()))
            {
                MouseLeaveEvent.Invoke(new MouseEventArgs()
                {
                    MouseState = Mouse.GetState(),
                    Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y)
                });
            }

            foreach(UIElement element in _Children)
            {
                element.Update(gameTime);
            }

            _previousMouse = Mouse.GetState();
        }

        public virtual void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch)
        {
            foreach (UIElement element in _Children)
            {
                element.Draw(gameTime, device, spriteBatch);
            }
        }

        public virtual Rectangle GetBoundingBox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        public virtual List<UIElement> GetChildren()
        {
            return _Children;
        }

        public virtual void RemoveChild(UIElement child)
        {
            _Children.Remove(child);
            child.Parent = null;
        }

        public virtual void AddChild(UIElement child)
        {
            _Children.Add(child);
            child.Parent = this;
        }

        // Base event methods
        private void MouseClicked_event(MouseEventArgs e)
        {
        }

        private void MouseReleased_event(MouseEventArgs e)
        {
        }

        private void MouseMoved_event(MouseEventArgs e)
        {
        }

        private void MouseEnter_event(MouseEventArgs e)
        {

        }

        private void MouseLeave_event(MouseEventArgs e)
        {

        }
    }
}
