using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using _2DRabbitPlatform.GFX;

namespace _2DRabbitPlatform.Engine.Files
{
    public class LevelFile : BinaryReader
    {
        string path;

        public LevelFile(string tileMapName)
            : base(new FileStream(TileMap_Directory + tileMapName, FileMode.Open))
        {
            path = TileMap_Directory + tileMapName;
        }

        public static Level loadLevel(RabbitPlatform game, string levelName, bool editor = false)
        {
            if (game.CurrentWorld == null)
                throw new NullReferenceException("You are unable to load level when without any world.");

            LevelFile file = new LevelFile(levelName);
            
            if (file.ReadMagic() != "LV")
                throw new FormatException();            

            //if (!editor)
            //    world = new World(game);
            //else
            //    world = new Editor.EditorWorld(game);

            string tileset = file.ReadString();
            TileSet ts = TileSetFile.loadTileSet(game, tileset);

            ushort layers = file.ReadUInt16();
            ushort entLayer = file.ReadUInt16();

            Level level = new Level(game.CurrentWorld, layers, entLayer);

            //world.prepareWorld(layers, entLayer);
            TileMapInfo info;

            for (int i = 0; i < layers; i++)
            {
                info = TileMapFilePart.fromStream(level, file.BaseStream, ts);
                level.initializeLayer(info, i);
            }

            //world.createWorld();

            file.Close();

            return level;
        }

        public string ReadMagic()
        {
            return ASCIIEncoding.ASCII.GetString(ReadBytes(2));
        }

        public static void saveLevel(Level level, string levelName, string tileset)
        {
            BinaryWriter writer = new BinaryWriter(new FileStream(TileMap_Directory + levelName, FileMode.Create));

            writer.Write(ASCIIEncoding.ASCII.GetBytes("LV"));
            writer.Write(tileset);
            writer.Write((ushort)level.RenderManager.Layers.Count);
            writer.Write(level.RenderManager.EntityLayerID);

            for (int i = 0; i < level.RenderManager.Layers.Count; i++)
            {
                TileMapFilePart.toStream(writer.BaseStream, level.RenderManager.Layers[i]);
            }

            writer.Close();
        }

        public static readonly string TileMap_Directory = "Levels\\";
    }
}
