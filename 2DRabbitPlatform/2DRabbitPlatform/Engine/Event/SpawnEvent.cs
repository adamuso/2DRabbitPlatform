using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.Event
{
    using Files.SerialBinary;

    public class SpawnEvent : DependentEvent
    {
        int id;

        public SpawnEvent() 
            : base()
        {
            if (!this.hasSBCompound())
                this.createSBCompound().addElements(() => id);

            id = 0;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gt)
        {

        }

        public override void execute(World world, int x, int y)
        {
            if (enabled)
            {
                world.EntityManager.createEntity(id).setLocation(new Location(world, x * GFX.Tile.STANDARD_GTILE_WIDTH, y * GFX.Tile.STANDARD_GTILE_HEIGHT));
                enabled = false;
            }
        }

        public override void toStream(System.IO.BinaryWriter writer)
        {

        }

        public override void execute(Entity.Entity executer, int x, int y) { return; }
        public override bool IsOneTimeEvent { get { return true; } }
        public override string Name { get { return "Spawn object event"; } }
        public override string DisplayName { get { return "Spawn"; } }
        public override int EventID { get { return (int)EventType.SPAWN; } }
        public int EntityID { get { return id; } set { id = value; } }
    }
}
