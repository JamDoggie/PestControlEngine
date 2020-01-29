using System;
using System.Collections.Generic;
using System.IO;

namespace PestControlAnimation.Objects
{
    public class KeyFrame
    {
        public int TimelineX { get; set; }
        public int TimelineY { get; set; }

        // This kinda has to be public because it's for json serializing so ignore the CA2227 standard warning in visual studio here.
        public Dictionary<string, SpriteJson> SpriteBoxes { get; set; }

        public KeyFrame()
        {
            SpriteBoxes = new Dictionary<string, SpriteJson>();
        }

        public KeyFrame(int timelineX, int timelineY, Dictionary<string, SpriteJson> spriteBoxes)
        {
            TimelineX = timelineX;
            TimelineY = timelineY;
            SpriteBoxes = spriteBoxes;
        }

        

        
    }
}
