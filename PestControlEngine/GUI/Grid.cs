using Microsoft.Xna.Framework;
using PestControlEngine.Libs.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PestControlEngine.GUI
{
    public class Grid : UIElement
    {
        public List<GridCell> Cells = new List<GridCell>();

        public void AddCell(UIElement element, double xPos, double yPos, double Width, double Height)
        {
            Cells.Add(new GridCell()
            {
                posX = xPos,
                posY = yPos,
                width = Width,
                height = Height,
                Child = element
            });
        }

        public void RemoveCell(UIElement element)
        {
            GridCell toRemove = null;

            foreach(GridCell cell in Cells)
            {
                if (cell.Child == element)
                {
                    toRemove = cell;
                }
            }

            Cells.Remove(toRemove);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach(GridCell cell in Cells)
            {
                if (cell.Child != null)
                {
                    cell.Child.Position = new Vector2((int)(Util.CurrentResolution.X * (float)cell.posX), (int)(Util.CurrentResolution.Y * (float)cell.posY));
                    cell.Child.Width = (int)(Util.CurrentResolution.X * cell.width);
                    cell.Child.Height = (int)(Util.CurrentResolution.Y * cell.height);
                }
            }
        }
    }

    public class GridCell
    {
        // These numbers go from 0 to 1 because they are percents
        public double posX = 0;
        public double posY = 0;
        public double width = 0;
        public double height = 0;

        public UIElement Child = null;
    }
}
