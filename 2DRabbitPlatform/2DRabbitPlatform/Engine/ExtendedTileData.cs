using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IO = System.IO;

namespace _2DRabbitPlatform.Engine
{
    public class ExtendedTileData : TileData
    {
        bool flipped;

        public ExtendedTileData(int id, bool flipped)
            : base(id)
        {
            this.flipped = flipped;
        }

        public override void toStream(IO.BinaryWriter writer)
        {
            int extended = base.ID | 1 << 31;
            writer.Write(extended);
            writer.Write(getExtendedFlags());
        }

        protected virtual byte getExtendedFlags()
        {
            byte ret = 0;
            ret = flipped ? (byte)(ret | (1 << 0)) : ret;

            return ret;
        }

        public override bool Flipped { get { return flipped; } set { flipped = value; } }
    }
}
