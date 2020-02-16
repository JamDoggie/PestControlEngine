using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PestControlAnimation.Objects;
using PestControlEngine.Event.Structs;
using PestControlEngine.Libs.Helpers.Structs;
using PestControlEngine.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PestControlEngine.Objects
{
    public class GameObject
    {
        // Variables
        private Vector2 _Position = new Vector2();

        private string _Name = "";

        private List<GameObject> _children = new List<GameObject>();

        private GameObject _parent = null;

        public Animation CurrentAnimation { get; set; } = new Animation();

        public Rectangle BoundingBox { get; set; } = new Rectangle();

        private MouseState previousMouse;

        // Events
        public delegate void MouseClick(MouseEventArgs e);
        public event MouseClick MouseClickedEvent;

        public delegate void MouseMove(MouseEventArgs e);
        public event MouseMove MouseMovedEvent;

        public GameObject()
        {
            // Event init
            MouseClickedEvent += MouseClicked;
            MouseMovedEvent += MouseMoved;
        }

        /// <summary>
        /// Overridable, allows us to control how to draw this object directly.
        /// Not recommended if you can achieve your goal with more higher level methods.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(GraphicsDevice device, SpriteBatch spriteBatch)
        {
            if (CurrentAnimation != null && CurrentAnimation.GetCurrentSprite() != null)
            {
                var orderedSprites = FromJsonSprites(CurrentAnimation.GetCurrentSprite()).OrderBy(f => f.Value.GetLayer()).ToList();

                foreach (KeyValuePair<string, Sprite> pair in orderedSprites)
                {
                    Sprite spriteBox = pair.Value;

                    if (spriteBatch != null && ContentLoader.GetTexture(spriteBox.GetTextureKey()) != null && spriteBox.Visible())
                    {
                        spriteBatch.Draw(ContentLoader.GetTexture(spriteBox.GetTextureKey()), new Rectangle((int)(spriteBox.GetPosition().X + GetPosition().X), (int)(spriteBox.GetPosition().Y + GetPosition().Y), spriteBox.GetWidth(), spriteBox.GetHeight()), spriteBox.GetSourceRectangle(), Color.White);
                    }
                }
            }

            // Draw children
            foreach (GameObject d in _children)
            {
                d.Draw(device, spriteBatch);
            }
        }

        public virtual void Update(GameTime gameTime, GameInfo gameInfo)
        {
            // Update children
            foreach (GameObject d in _children)
            {
                d.Update(gameTime, gameInfo);
            }

            // Update animation
            CurrentAnimation.Update(gameTime);

            // Events
            if (new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1).Intersects(GetBoundingBox()) && Mouse.GetState().LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released)
            {
                MouseClickedEvent.Invoke(new MouseEventArgs()
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

            previousMouse = Mouse.GetState();
        }

        public static Dictionary<string, Sprite> FromJsonSprites(Dictionary<string, SpriteJson> spriteDictionary)
        {
            Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

            foreach(KeyValuePair<string, SpriteJson> pair in spriteDictionary)
            {
                sprites.Add(pair.Key, Sprite.FromJsonElement(pair.Value));
            }

            return sprites;
        }

        public virtual Rectangle GetBoundingBox()
        {
            if (CurrentAnimation == null || CurrentAnimation.GetCurrentSprite() == null)
            {
                return BoundingBox;
            }
            else
            {
                int lowestX = 0;
                int lowestY = 0;
                int highestX = 0;
                int highestY = 0;

                foreach (KeyValuePair<string, Sprite> pair in FromJsonSprites(CurrentAnimation.GetCurrentSprite()))
                {
                    if (pair.Value.GetPosition().X < lowestX)
                        lowestX = (int)pair.Value.GetPosition().X;

                    if (pair.Value.GetPosition().Y < lowestY)
                        lowestY = (int)pair.Value.GetPosition().Y;

                    if (pair.Value.GetRectangle().Right > highestX)
                        highestX = pair.Value.GetRectangle().Right;

                    if (pair.Value.GetRectangle().Bottom > highestY)
                        highestY = pair.Value.GetRectangle().Bottom;
                }

                return new Rectangle(lowestX + (int)GetPosition().X, lowestY + (int)GetPosition().Y, highestX - lowestX, highestY - lowestY);
            }

            
        }

        public virtual Vector2 GetPosition()
        {
            if (_parent != null)
            {
                return new Vector2(_Position.X + GetParent().GetPosition().X, _Position.Y + GetParent().GetPosition().Y);
            }
            else
            {
                return _Position;
            }

        }

        public virtual Vector2 GetRawPosition()
        {
            return _Position;
        }

        public virtual void SetPosition(Vector2 position)
        {
            _Position = position;
        }

        public virtual Dictionary<string, Sprite> GetSpriteBoxes()
        {
            if (CurrentAnimation == null || CurrentAnimation.GetCurrentSprite() == null)
                return new Dictionary<string, Sprite>();

            return FromJsonSprites(CurrentAnimation.GetCurrentSprite());
        }

        public virtual void AddChild(GameObject drawable)
        {
            if (drawable == null)
                return;

            drawable._parent = this;
            _children.Add(drawable);
        }

        public virtual void RemoveChild(GameObject drawable)
        {
            if (drawable == null)
                return;

            if (drawable._parent == this)
            {
                _children.Remove(drawable);
                drawable._parent = null;
            }

        }

        public virtual List<GameObject> GetChildren()
        {
            return _children;
        }

        /// <summary>
        /// Sets the list of children in this drawable. NOTE: if you wish to add or remove children, it's recommended
        /// to use RemoveChild and AddChild.
        /// </summary>
        /// <param name="list"></param>
        public virtual void SetChildren(List<GameObject> list)
        {
            _children = list;
        }

        public virtual GameObject GetParent()
        {
            return _parent;
        }

        public virtual void SetParent(GameObject parent)
        {
            _parent = parent;
        }

        // Base event methods
        private void MouseClicked(MouseEventArgs e)
        {
        }

        private void MouseMoved(MouseEventArgs e)
        {
        }
    }
}
