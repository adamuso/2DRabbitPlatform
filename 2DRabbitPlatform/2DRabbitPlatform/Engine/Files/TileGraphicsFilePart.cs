using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using _2DRabbitPlatform.GFX;

namespace _2DRabbitPlatform.Engine.Files
{
    public class TileGraphicsFilePart
    {
        public static Texture2D fromStream(RabbitPlatform game, Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            MemoryStream mem = new MemoryStream(reader.ReadBytes(reader.ReadInt32()));

            return Texture2D.FromStream(game.GraphicsDevice, mem);
        }

        public static void toStream(Texture2D texture, Stream stream)
        {
            MemoryStream mem = new MemoryStream();
            texture.SaveAsPng(mem, texture.Width, texture.Height);
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write((int)mem.Length);
            writer.Write(mem.ToArray());
        }
    }
}
