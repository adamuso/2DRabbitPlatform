using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.GFX
{
    public class TileMapInfo
    {
        GFX.TileMap map;
        float xscroll, yscroll, autox, autoy;
        bool wid, hei;
        Engine.Event.EventTile[] eventTiles;

        public TileMapInfo(GFX.TileMap map)
        {
            // TODO: Complete member initialization
            this.map = map;
        }

        public float XScrollSpeed { get { return xscroll; } set { xscroll = value; } }
        public float YScrollSpeed { get { return yscroll; } set { yscroll = value; } }
        public float AutoXSpeed { get { return autox; } set { autox = value; } }
        public float AutoYSpeed { get { return autoy; } set { autoy = value; } }
        public bool WidthWrap { get { return wid; } set { wid = value; } }
        public bool HeightWrap { get { return hei; } set { hei = value; } }
        public int Width { get { return map.Width; } }
        public int Height { get { return map.Height; } }
        public GFX.TileMap Map { get { return map; } }
        public Engine.Event.EventTile[] EventTiles { get { return eventTiles; } set { eventTiles = value; } }
    }
}
