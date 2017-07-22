using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.Event
{
    public class PlayerSpawnEvent : DependentEvent
    {
        public PlayerSpawnEvent()
            : base()
        {
            
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gt) { }
        public override void execute(World world, int x, int y)
        {
            world.Level.addSpawnPoint(new Location(world, x * GFX.Tile.STANDARD_GTILE_WIDTH, y * GFX.Tile.STANDARD_GTILE_HEIGHT));
        }

        public override void execute(Entity.Entity executer, int x, int y) { }
        public override void toStream(System.IO.BinaryWriter writer) { }
        public override bool IsOneTimeEvent
        {
            get { return true; }
        }
        public override string Name { get { return "Player spawn event"; } }
        public override string DisplayName { get { return "Player\nspawn"; } }
        public override int EventID { get { return (int)EventType.PLAYER_SPAWN; } }    
    }
}
