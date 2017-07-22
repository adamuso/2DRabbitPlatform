using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _2DRabbitPlatform.Engine;

namespace _2DRabbitPlatform.GFX
{
    public class TileSetInfo
    {
        uint width, height;
        byte tileWidth, tileHeight;

        public TileSetInfo(uint width, uint height)
        {
            this.width = width; 
            this.height = height;
        }

        public TileSetInfo(uint width, uint height, byte tileScalar)
        {
            this.width = width;
            this.height = height;
            this.tileWidth = tileScalar;
            this.tileHeight = tileScalar;
        }

        public uint Width { get { return width; } }

        public uint Height { get { return height; } }

        public byte TileWidth { get { return tileWidth; } }

        public byte TileHeight { get { return tileHeight; } }

        public static TileSetInfo loadFromStream(System.IO.Stream stream)
        {
            System.IO.BinaryReader reader = new System.IO.BinaryReader(stream);

            uint width = uint.Parse(Utility.readToBrakpoint(reader));
            uint height = uint.Parse(Utility.readToBrakpoint(reader));

            TileSetInfo info = new TileSetInfo(width, height);
            stream.Close();

            return info;
        }
    }
}
