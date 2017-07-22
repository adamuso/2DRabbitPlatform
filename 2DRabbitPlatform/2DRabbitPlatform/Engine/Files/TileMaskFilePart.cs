using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _2DRabbitPlatform.GFX;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.Engine.Files
{
    public class TileMaskFilePart
    {
        public static TileMask fromStream(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            byte[] compressed = reader.ReadBytes((int)Math.Ceiling(Tile.STANDARD_GTILE_WIDTH / 8d) * Tile.STANDARD_GTILE_HEIGHT),
                   data = new byte[Tile.STANDARD_GTILE_WIDTH * Tile.STANDARD_GTILE_WIDTH];

            for (int y = 0; y < Tile.STANDARD_GTILE_HEIGHT; y++)
            {
                for (int x = 0; x < Math.Ceiling(Tile.STANDARD_GTILE_WIDTH / 8d); x++)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if ((byte)(compressed[(int)(x + y * Math.Ceiling(Tile.STANDARD_GTILE_WIDTH / 8d))] & (1 << i)) == (1 << i))
                        {
                            data[x * 8 + y * Tile.STANDARD_GTILE_WIDTH + i] = 1;
                        }
                    }
                }
            }

            return TileMask.fromData(data);
        }

        public static byte[] toStream(Stream stream, Mask mask)
        {
            BinaryWriter writer = new BinaryWriter(stream);
            byte[] data = mask.GetData(),
                   compressed = new byte[(int)(Math.Ceiling(Tile.STANDARD_GTILE_WIDTH / 8d) * Tile.STANDARD_GTILE_HEIGHT)];
            int current = 0;

            for (int y = 0; y < Tile.STANDARD_GTILE_HEIGHT; y++)
            {
                for (int x = 0; x < Tile.STANDARD_GTILE_WIDTH; x++)
                {
                    current = (x + y * Tile.STANDARD_GTILE_WIDTH) / 8;

                    if (data[x + y * Tile.STANDARD_GTILE_WIDTH] == 1)
                    {
                        compressed[current] = (byte)(compressed[current] | (1 << (x + y * Tile.STANDARD_GTILE_WIDTH) % 8));
                    }

                    if ((x + y * Tile.STANDARD_GTILE_WIDTH + 1) % 8 == 0)
                    {
                        writer.Write(compressed[current]);
                    }
                }
            }

            return compressed;
        }

        public static void toStream(RabbitPlatform game, Stream stream, Texture2D texture)
        {
            RenderTarget2D target = new RenderTarget2D(game.GraphicsDevice, Tile.STANDARD_GTILE_WIDTH, Tile.STANDARD_GTILE_HEIGHT);
            SpriteBatch sb = new SpriteBatch(game.GraphicsDevice);
            int w = texture.Width / Tile.STANDARD_GTILE_WIDTH, h = texture.Height / Tile.STANDARD_GTILE_HEIGHT;

            for(int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    game.GraphicsDevice.SetRenderTarget(target);
                    game.GraphicsDevice.Clear(Color.White);
                    sb.Begin();
                    sb.Draw(texture, new Rectangle(0, 0, Tile.STANDARD_GTILE_WIDTH, Tile.STANDARD_GTILE_HEIGHT), new Rectangle(x * Tile.STANDARD_GTILE_WIDTH, y * Tile.STANDARD_GTILE_HEIGHT, Tile.STANDARD_GTILE_WIDTH, Tile.STANDARD_GTILE_HEIGHT), Color.White);
                    sb.End();
                    game.GraphicsDevice.SetRenderTarget(null);

                    toStream(stream, TileMask.fromTexture(target));
                }


        }
    }
}
