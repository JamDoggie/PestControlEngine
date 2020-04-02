using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PestControlEngine.Types
{
    public class RectangleJSON
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public static Rectangle ToRectangle(RectangleJSON jsonRect)
        {
            Rectangle rect = new Rectangle();

            rect.X = jsonRect.X;
            rect.Y = jsonRect.Y;
            rect.Width = jsonRect.Width;
            rect.Height = jsonRect.Height;

            return rect;
        }

        public static RectangleJSON FromRectangle(Rectangle rect)
        {
            RectangleJSON jsonRect = new RectangleJSON();

            jsonRect.X = rect.X;
            jsonRect.Y = rect.Y;
            jsonRect.Width = rect.Width;
            jsonRect.Height = rect.Height;

            return jsonRect;
        }
    }
}
