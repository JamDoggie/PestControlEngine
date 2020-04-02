using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PestControlAnimation.Objects
{
    public class Animation
    {
        // For parsing the PCAF file format.
        public const string headerName = "PestControlAnimationBinaryFile";
        public const int currentVersion = 1;

        public List<KeyFrame> KeyFrames { get; set; } = new List<KeyFrame>();

        public int EndFrame { get; set; } = 0;

        public bool Loop { get; set; } = true;

        public int LoopToFrame { get; set; } = 0;

        public string Name { get; set; } = "";

        private bool _isPlaying = false;
        private double _currentMs = 0;

        public Animation()
        {
            KeyFrames = new List<KeyFrame>();
        }

        public bool IsPlaying()
        {
            return _isPlaying;
        }

        public void Play()
        {
            _isPlaying = true;
        }

        public void Pause()
        {
            _isPlaying = false;
        }

        /// <summary>
        /// Sets the current frame of the animation based off a timeloop of 60FPS(based off delta time, so one frame every 16 Ms)
        /// </summary>
        /// <param name="frame"></param>
        public void SetFrame(int frame)
        {
            _currentMs = frame * 16;
        }

        /// <summary>
        /// Sets the current Ms of the animation. This is a more accurate version of SetFrame. Animation is based off of a 60FPS timeloop(based off delta time, so one frame every 16 Ms)
        /// </summary>
        /// <param name="ms"></param>
        public void SetCurrentMS(float ms)
        {
            _currentMs = ms;
        }

        public void Update(GameTime deltaTime)
        {
            if (_isPlaying)
            {
                if (_currentMs > EndFrame * 16)
                {
                    double msDifference = _currentMs - EndFrame * 16;
                    if (Loop)
                    {
                        _currentMs = LoopToFrame * 16 + msDifference;
                    }
                    else
                    {
                        _currentMs = EndFrame * 16;
                        _isPlaying = false;
                    }

                }
                else
                {
                    _currentMs += deltaTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
        }

        public Dictionary<string, SpriteJson> GetCurrentSprite()
        {
            Dictionary<string, SpriteJson> currentSprite = new Dictionary<string, SpriteJson>();

            int closestKeyframeTo = -1;
            int closestKeyframeDistance = -1;

            if (KeyFrames.Count > 0)
            {
                for (int i = 0; i < KeyFrames.Count; i++)
                {
                    int scrubberPos = (int)Math.Floor(_currentMs / 16);

                    KeyFrame currentFrame = KeyFrames.ElementAt(i);

                    bool isValid = true;

                    if (currentFrame.TimelineX > scrubberPos)
                    {
                        isValid = false;
                    }

                    if (isValid)
                    {
                        if (closestKeyframeDistance == -1)
                        {
                            closestKeyframeDistance = Math.Abs(scrubberPos - KeyFrames.ElementAt(i).TimelineX);
                            closestKeyframeTo = i;
                        }
                        else
                        {
                            if (Math.Abs(scrubberPos - KeyFrames.ElementAt(i).TimelineX) < closestKeyframeDistance)
                            {
                                closestKeyframeDistance = Math.Abs(scrubberPos - KeyFrames.ElementAt(i).TimelineX);
                                closestKeyframeTo = i;
                            }
                        }
                    }
                }
            }

            if (closestKeyframeTo >= 0 && KeyFrames.Count > 0)
            {
                KeyFrame originalKeyframe = KeyFrames.ElementAt(closestKeyframeTo);

                return originalKeyframe.SpriteBoxes;
            }
            else
            {
                return null;
            }
        }

        public static Animation ReadAnimationFile(string filePath)
        {
            FileInfo file = new FileInfo(filePath);

            // Null checking
            if (file == null)
                return null;

            BinaryReader binaryReader = new BinaryReader(File.Open(filePath, FileMode.Open));

            // Check if header is correct(basically check if this is even a valid PCAF file.)
            string headerString = binaryReader.ReadString();

            if (headerString == headerName)
            {
                Animation animation = new Animation();

                // Ok cool so this file is a PCAF file.
                // Load version number incase this is ever useful(and because we kinda have to push the reader forward for it to read correctly)
                int versionNumber = binaryReader.ReadInt32();

                // Load Project Info
                bool loop = binaryReader.ReadBoolean();
                int loopTo = binaryReader.ReadInt32();
                string projectName = binaryReader.ReadString();
                int endFrame = binaryReader.ReadInt32();

                animation.Loop = loop;
                animation.LoopToFrame = loopTo;
                animation.Name = projectName;
                animation.EndFrame = endFrame;

                int keyframeCount = binaryReader.ReadInt32();

                for (int i = 0; i < keyframeCount; i++)
                {
                    // Read timeline x coordinate
                    int timelineX = binaryReader.ReadInt32();

                    int spriteboxCount = binaryReader.ReadInt32();

                    KeyFrame keyframe = new KeyFrame(timelineX, 0, new Dictionary<string, SpriteJson>());

                    for (int j = 0; j < spriteboxCount; j++)
                    {
                        // Get name
                        string name = binaryReader.ReadString();

                        // General properties
                        double posX = binaryReader.ReadDouble();
                        double posY = binaryReader.ReadDouble();
                        int width = binaryReader.ReadInt32();
                        int height = binaryReader.ReadInt32();
                        string textureKey = binaryReader.ReadString();
                        float rotation = binaryReader.ReadSingle();
                        int sourceX = binaryReader.ReadInt32();
                        int sourceY = binaryReader.ReadInt32();
                        int sourceWidth = binaryReader.ReadInt32();
                        int sourceHeight = binaryReader.ReadInt32();
                        float layer = binaryReader.ReadSingle();
                        bool visible = binaryReader.ReadBoolean();

                        SpriteJson spriteBox = new SpriteJson();
                        spriteBox.posX = posX;
                        spriteBox.posY = posY;
                        spriteBox.width = width;
                        spriteBox.height = height;
                        spriteBox.textureKey = textureKey;
                        spriteBox.rotation = rotation;
                        spriteBox.sourceX = sourceX;
                        spriteBox.sourceY = sourceY;
                        spriteBox.sourceWidth = sourceWidth;
                        spriteBox.sourceHeight = sourceHeight;
                        spriteBox.layer = layer;
                        spriteBox.visible = visible;

                        keyframe.SpriteBoxes.Add(name, spriteBox);
                    }

                    animation.KeyFrames.Add(keyframe);
                }

                binaryReader.Dispose();

                return animation;
            }
            else
            {
                binaryReader.Dispose();

                return null;
            }
        }
    }
}
