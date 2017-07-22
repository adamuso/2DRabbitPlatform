using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.Event
{
    using Files.SerialBinary;

    public class WarpEvent : DependentEvent
    {
        int x, y;

        public WarpEvent()
        {
            if (!this.hasSBCompound())
                this.createSBCompound().addElements(() => x, () => y);

            this.x = 0;
            this.y = 0;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gt)
        {

        }

        public override void execute(World world, int x, int y) { return; }

        public override void execute(Entity.Entity executer, int x, int y)
        {
            if (executer is Entity.Player)
            {
                executer.setLocation(new Location(executer.World, this.x * GFX.Tile.STANDARD_GTILE_WIDTH, this.y * GFX.Tile.STANDARD_GTILE_HEIGHT));
                executer.World.WorldCamera.Update(new Microsoft.Xna.Framework.GameTime());
            }
        }

        public override void toStream(System.IO.BinaryWriter writer)
        {

        }

        public override bool IsOneTimeEvent { get { return false; } }

        public override string Name { get { return "Warp event"; } }

        public override string DisplayName { get { return "Warp"; } }

        public override int EventID { get { return (int)EventType.WARP; } }

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
    }
}
