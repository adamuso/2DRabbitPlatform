using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace _2DRabbitPlatform.GFX
{
    public class Animation
    {
        AnimatedTexture textures;
        int current;
        bool repeat, playing;

        public Animation(AnimatedTexture textures)
        {
            this.textures = textures;
            current = 0;
            repeat = false;
            playing = false;
        }

        public void play()
        {
            repeat = true;
            playing = true;
        }

        public void playOnce()
        {
            repeat = false;
            playing = true;
        }

        public void stop()
        {
            current = 0;
            repeat = false;
            playing = false;
        }

        public void Update()
        {
            if (playing)
            {
                if (current < textures.FrameCount - 1)
                    current++;
                else
                    if (!repeat)
                        stop();
                    else
                    {
                        current = 0;
                    }
            }
        }

        public AnimatedTexture Texture { get { return textures; } }
        public int Current { get { return current; } }
        public bool IsPlaying { get { return playing; } }
        public int Width { get { return textures[current].Width; } }
        public int Height { get { return textures[current].Height; } }
    }
}
