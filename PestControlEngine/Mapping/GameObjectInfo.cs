using PestControlAnimation.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PestControlEngine.Mapping
{
    public class GameObjectInfo
    {
        public GameObjectInfo()
        {
            Properties = new Dictionary<string, GameObjectProperty>();
        }

        public Animation DefaultAnimation { get; set; }

        public string ClassName { get; set; }

        public Dictionary<string, GameObjectProperty> Properties { get; set; }
    }
}
