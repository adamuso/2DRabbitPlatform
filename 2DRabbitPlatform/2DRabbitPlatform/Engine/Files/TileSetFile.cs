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
    public class TileSetFile : BinaryReader
    {
        string path;

        private TileSetFile(string setFileName)
            : base(new FileStream(TileSet_Directory + setFileName, FileMode.Open))
        {
            path = TileSet_Directory + setFileName;
        }

        public static TileSet loadTileSet(RabbitPlatform game, string setFileName)
        {
            TileSetInfo info;
            TileSet ts;
            TileSetFile file = new TileSetFile(setFileName);
            TileMask[] masks;

            if (file.ReadMagic() != "TS")
                throw new FileLoadException("Wrong file format!", file.path);

            info = new TileSetInfo(file.ReadUInt32(), file.ReadUInt32(), file.ReadByte());
            masks = new TileMask[info.Width * info.Height];
            Texture2D texture = TileGraphicsFilePart.fromStream(game, file.BaseStream);

            for (int y = 0; y < info.Height; y++)
                for (int x = 0; x < info.Width; x++)
                    masks[x + y * info.Width] = TileMaskFilePart.fromStream(file.BaseStream);

            ts = TileSet.loadFromTexture(texture, info, masks);
            return ts;
        }

        public static void generate(RabbitPlatform game, byte tileSize, string tileSetPath, string tileMasksPath, string output)
        {
            FileStream stream = new FileStream(TileSet_Directory + output, FileMode.Create);
            Texture2D texture = game.TextureLoader.FromFile(TileSet_Directory + tileSetPath);
            Texture2D masks = game.TextureLoader.FromFile(TileSet_Directory + tileMasksPath);
            BinaryWriter writer = new BinaryWriter(stream);
            uint width = (uint)texture.Width / tileSize, height = (uint)texture.Height / tileSize;

            writer.Write(ASCIIEncoding.ASCII.GetBytes("TS"));
            writer.Write(width);
            writer.Write(height);
            writer.Write(tileSize);
            TileGraphicsFilePart.toStream(texture, stream);
            TileMaskFilePart.toStream(game, stream, masks);
            writer.Close();
        }

        public string ReadMagic()
        {
            return ASCIIEncoding.ASCII.GetString(ReadBytes(2));
        }

        public static readonly string TileSet_Directory = "TileSets\\";
    }
}
