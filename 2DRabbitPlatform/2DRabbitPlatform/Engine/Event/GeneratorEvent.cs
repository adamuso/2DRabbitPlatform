using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.Event
{
    using Files.SerialBinary;

    public class GeneratorEvent : IndependentEvent
    {
        int id;
        Entity.Entity spawnedEntity;
        GameTimer timer;

        public GeneratorEvent(World world, int x, int y)
            : base(world, x, y)
        {
            if(!this.hasSBCompound())
                this.createSBCompound().addElements(() => id, () => Interval);

            id = 0;
            timer = createTimer();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gt)
        {
            base.Update(gt);
        }

        public override void execute()
        {
            if (spawnedEntity == null || spawnedEntity.Destroyed)
            {
                spawnedEntity = world.EntityManager.createEntity(id);
                spawnedEntity.setLocation(new Location(world, x * GFX.Tile.STANDARD_GTILE_WIDTH, y * GFX.Tile.STANDARD_GTILE_HEIGHT));
            }
        }

        public override void toStream(System.IO.BinaryWriter writer)
        {

        }

        public override bool IsOneTimeEvent { get { return true; } }
        public override string Name { get { return "Generator event"; } }
        public override string DisplayName { get { return "Generator"; } }
        public override int EventID { get { return (int)EventType.GENERATOR; } }
        public int EntityID { get { return id; } set { id = value; } }
        public int Interval { get { return timer.Delay; } set { timer.setRepeat(value, execute); } }
    }
}
