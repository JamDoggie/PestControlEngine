﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PestControlAnimation.Objects
{
    public class SpriteJson
    {
        public int width { get; set; }

        public int height { get; set; }

        public double posX { get; set; }

        public double posY { get; set; }

        public float rotation { get; set; }

        public int sourceX { get; set; }

        public int sourceY { get; set; }

        public int sourceWidth { get; set; }

        public int sourceHeight { get; set; }

        public string textureKey { get; set; }

        public float layer { get; set; }

        public bool visible { get; set; } = true;
    }
}
