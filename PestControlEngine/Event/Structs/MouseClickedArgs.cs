using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PestControlEngine.Libs.Helpers.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PestControlEngine.Event.Structs
{
    public class MouseEventArgs
    {
        public Vector2 Position { get; set; } = new Vector2();

        public MouseState MouseState { get; set; }

        public GameInfo gameInfo { get; set; }
    }
}
