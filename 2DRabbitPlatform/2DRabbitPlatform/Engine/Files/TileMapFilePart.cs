using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using _2DRabbitPlatform.GFX;

namespace _2DRabbitPlatform.Engine.Files
{
    using Engine.Files.SerialBinary;

    public class TileMapFilePart
    {
        public static TileMapInfo fromStream(Level level, Stream stream, TileSet ts)
        {
            BinaryReader reader = new BinaryReader(stream);
            int w = reader.ReadInt32(), h = reader.ReadInt32();
            List<Event.EventTile> eventTiles = new List<Event.EventTile>();
            TileMap map = new TileMap(w, h, ts);
 
            for(int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    int data = reader.ReadInt32();

                    if ((data & 1 << 31) == 1 << 31)
                    {
                        TileData buffer = new TileData(data);

                        bool flipped = false;
                        data = data ^ 1 << 31;

                        // additional data
                        byte flags = reader.ReadByte();

                        if ((flags & 1 << 0) == 1 << 0)
                        {
                            flipped = true;
                            buffer = new ExtendedTileData(data, flipped);
                        }

                        if((flags & 1 << 6) == 1 << 6)
                        {
                            Event.TEvent[] events = null;

                            if ((flags & 1 << 7) == 1 << 7)
                            {
                                int count = reader.ReadInt32();
                                events = new Event.TEvent[count];       

                                for (int i = 0; i < count; i++)
                                {
                                    events[i] = level.TileManager.createEvent(reader.ReadInt32(), level.World, x, y);
                                    events[i].getSBCompound().readCompound(reader);
                                }
                            }
                            
                            int idscount = reader.ReadInt32();

                            List<int> ids = new List<int>();

                            for (int i = 0; i < idscount; i++)
                                ids.Add(reader.ReadInt32());

                            int interval = reader.ReadInt32();
                            bool pingpong = reader.ReadBoolean();

                            TileAnimation anim = new TileAnimation(ids.ToArray(), interval, pingpong, flipped, x, y, events);
                            buffer = anim;
                            eventTiles.Add(anim);
                        }
                        else
                            if ((flags & 1 << 7) == 1 << 7)
                            {
                                int count = reader.ReadInt32();
                                Event.TEvent[] events = new Event.TEvent[count];

                                for (int i = 0; i < count; i++)
                                {
                                    events[i] = level.TileManager.createEvent(reader.ReadInt32(), level.World, x, y);
                                    events[i].getSBCompound().readCompound(reader);
                                }

                                Event.EventTile evT = new Event.EventTile(data, flipped, x, y, events /*events...*/);
                                buffer = evT;
                                eventTiles.Add(evT);
                            }

                        map.setTile(x, y, buffer);
                    }
                    else
                        map.setTile(x, y, new TileData(data));
                }

            if (h == 0 || w == 0)
                map = new TileMap(1, 1, ts);

            TileMapInfo info = new TileMapInfo(map) 
            {
                XScrollSpeed = reader.ReadSingle(),
                YScrollSpeed = reader.ReadSingle(),
                AutoXSpeed = reader.ReadSingle(),
                AutoYSpeed = reader.ReadSingle(),
                WidthWrap = reader.ReadBoolean(),
                HeightWrap = reader.ReadBoolean(),
                EventTiles = eventTiles.ToArray()
            };

            return info;
        }

        public static void toStream(Stream stream, MapLayer layer)
        {
            if (layer.HasMap)
            {
                MapLayer map = (MapLayer)layer;
                BinaryWriter writer = new BinaryWriter(stream);
                TileMap tiles = map.Map;

                writer.Write(tiles.Width);
                writer.Write(tiles.Height);

                for (int y = 0; y < tiles.Height; y++)
                    for (int x = 0; x < tiles.Width; x++)
                    {
                        tiles.getTile(x, y).toStream(writer);
                    }

                writer.Write(map.XSpeed);
                writer.Write(map.YSpeed);
                writer.Write(map.AutoXScroll);
                writer.Write(map.AutoYScroll);
                writer.Write(map.WidthWrap);
                writer.Write(map.HeightWrap);
            }
            else
            {
                BinaryWriter writer = new BinaryWriter(stream);

                writer.Write(0);
                writer.Write(0);

                writer.Write(layer.XSpeed);
                writer.Write(layer.YSpeed);
                writer.Write(0f);
                writer.Write(0f);
                writer.Write(layer.WidthWrap);
                writer.Write(layer.HeightWrap);
            }
        }
    }
}
