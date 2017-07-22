using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.GFX
{
    public class TileMask : Mask
    {
        public TileMask() 
            : base(Tile.STANDARD_GTILE_WIDTH, Tile.STANDARD_GTILE_HEIGHT)
        {

        }

        public static TileMask fromTexture(Texture2D texture)
        {
            TileMask mask = new TileMask();
            Color[] colors = new Color[mask.width * mask.height];
            texture.GetData<Color>(colors);
            
            for (int y = 0; y < mask.height; y++)
                for (int x = 0; x < mask.width; x++)
                {
                    if (colors[x + y * mask.width] == Color.Black)
                    {
                        mask.mask[x + y * mask.width] = 1;
                    }
                }

            return mask;
        }

        public static TileMask fromData(byte[] data)
        {
            TileMask mask = new TileMask();
            mask.mask = data;
            return mask;
        }
    }
}
