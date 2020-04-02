using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PestControlEngine.Event.Structs;
using PestControlEngine.GUI.Enum;
using PestControlEngine.Libs.Helpers;
using PestControlEngine.Libs.Helpers.Structs;
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

        public int Width;


        public int Height;

        public Screen ParentScreen = null;

        public Vector2 RenderPosition = new Vector2();
        
        public UIElement Parent = null;

        public bool DynamicallyScale { get; set; }

        public EnumHorizontalAlignment HorizontalAlignment = EnumHorizontalAlignment.NONE;

        public EnumVerticalAlignment VerticalAlignment = EnumVerticalAlignment.NONE;

        public bool FillParent = false;

        private List<UIElement> _Children { get; set; } = new List<UIElement>();

        private MouseState _previousMouse = new MouseState();

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

        public virtual void Update(GameTime gameTime, GameInfo info)
        {
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
                    default:
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
                    default:
                        break;
                }
            }

            // Filling
            if (FillParent)
            {
                Position = new Vector2(0, 0);

                if (Parent != null)
                {
                    Width = Parent.Width;
                    Height = Parent.Height;
                }
                else
                {
                    Width = (int)info.Resolution.X;
                    Height = (int)info.Resolution.Y;
                }
            }

            // Events
            Rectangle RenderBox = GetBoundingBox();

            if (info.guiManager.UseVirtualSize)
            {
                float scaleX = (float)info.graphicsDevice.PresentationParameters.BackBufferWidth / (float)info.guiManager.VirtualViewWidth;
                float scaleY = (float)info.graphicsDevice.PresentationParameters.BackBufferHeight / (float)info.guiManager.VirtualViewHeight;

                RenderBox = new Rectangle((int)((float)GetBoundingBox().X * scaleX), (int)((float)GetBoundingBox().Y * scaleY), (int)((float)GetBoundingBox().Width * scaleX), (int)((float)GetBoundingBox().Height * scaleY));
            }
                

            if (new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1).Intersects(RenderBox) && Mouse.GetState().LeftButton == ButtonState.Pressed && _previousMouse.LeftButton == ButtonState.Released)
            {
                MouseClickedEvent.Invoke(new MouseEventArgs()
                {
                    MouseState = Mouse.GetState(),
                    Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y),
                    gameInfo = info
                });
            }

            if (new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1).Intersects(RenderBox) && Mouse.GetState().LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
            {
                MouseReleasedEvent.Invoke(new MouseEventArgs()
                {
                    MouseState = Mouse.GetState(),
                    Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y),
                    gameInfo = info
                });
            }

            if (Mouse.GetState().Position != _previousMouse.Position)
            {
                MouseMovedEvent.Invoke(new MouseEventArgs()
                {
                    MouseState = Mouse.GetState(),
                    Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y),
                    gameInfo = info
                });
            }

            if (new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1).Intersects(RenderBox) && !new Rectangle(_previousMouse.X, _previousMouse.Y, 1, 1).Intersects(RenderBox))
            {
                MouseEnterEvent.Invoke(new MouseEventArgs()
                {
                    MouseState = Mouse.GetState(),
                    Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y),
                    gameInfo = info
                });
            }

            if (!new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1).Intersects(RenderBox) && new Rectangle(_previousMouse.X, _previousMouse.Y, 1, 1).Intersects(RenderBox))
            {
                MouseLeaveEvent.Invoke(new MouseEventArgs()
                {
                    MouseState = Mouse.GetState(),
                    Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y),
                    gameInfo = info
                });
            }

            if (Parent != null && Parent.Position != null)
            {
                RenderPosition = Position + Parent.RenderPosition;
            }
            else
            {
                RenderPosition = Position;
            }

            foreach (UIElement element in _Children)
            {
                element.Update(gameTime, info);
            }

            _previousMouse = Mouse.GetState();
        }

        public virtual void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, GameInfo info)
        {
            foreach (UIElement element in _Children)
            {
                element.Draw(gameTime, device, spriteBatch, info);
            }
        }

        public virtual Rectangle GetBoundingBox()
        {
            return new Rectangle((int)RenderPosition.X, (int)RenderPosition.Y, Width, Height);
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
            child.ParentScreen = ParentScreen;
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
