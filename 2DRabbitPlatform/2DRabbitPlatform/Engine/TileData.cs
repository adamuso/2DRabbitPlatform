using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using IO = System.IO;

namespace _2DRabbitPlatform.Engine
{
    public class TileData : HandleTimerBase, Files.IStoreable
    {
        int tileId;

        public TileData()
        {
            tileId = 0;
        }

        public TileData(int id)
        {
            this.tileId = id;
        }

        public virtual void toStream(IO.BinaryWriter writer)
        {
            writer.Write(tileId);
        }

        public bool isEmpty { get { return tileId <= 0; } }

        public bool isAir { get { return tileId == 0; } }

        public int ID { get { return tileId; } set { tileId = value; } }

        public virtual bool Flipped { get { return false; } set { } }

        public virtual bool HasEvents { get { return false; } }

        public static TileData Empty { get { return new TileData(); } }
    }
}
