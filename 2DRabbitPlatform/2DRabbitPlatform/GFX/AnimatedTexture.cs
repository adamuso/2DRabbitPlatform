using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.GFX
{
    public class AnimatedTexture
    {
        int sX, sY, frameCount, frameWidth, frameHeight, frameSwap;
        Texture2D source;

        public AnimatedTexture(Texture2D source, int startX, int startY, int fCount, int fw, int fh, int fSwap = -1)
        {
            sX = startX;
            sY = startY;
            frameCount = fCount;
            frameWidth = fw;
            frameHeight = fh;
            frameSwap = fSwap;
            this.source = source;
        }

        public static AnimatedTexture FromContent(RabbitPlatform instance, string name, int startX, int startY, int frameWidth, int frameHeight, int frameCount, int frameSwap = -1)
        {
            Texture2D main = instance.Content.Load<Texture2D>(name);
            return new AnimatedTexture(main, startX, startY, frameCount, frameWidth, frameHeight, frameSwap);
        }

        public static AnimatedTexture FromTexture(Texture2D source, int startX, int startY, int frameWidth, int frameHeight, int frameCount, int frameSwap = -1)
        {
            return new AnimatedTexture(source, startX, startY, frameCount, frameWidth, frameHeight, frameSwap);
        }

        public int FrameCount { get { return frameCount; } }
        public Texture2D Source { get { return source; } }
        public Rectangle this[int index] 
        { 
            get 
            {
                if (index < frameCount)
                    if (frameSwap != -1)
                        return new Rectangle(sX + (index % frameSwap) * frameWidth, sY + (index / frameSwap) * frameHeight, frameWidth, frameHeight);
                    else
                        return new Rectangle(sX + index * frameWidth, sY, frameWidth, frameHeight);
                else
                    throw new IndexOutOfRangeException();
            }       
        }
    }
}
