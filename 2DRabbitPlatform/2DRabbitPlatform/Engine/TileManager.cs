using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.Engine
{
    public class TileManager
    {
        Dictionary<int, Type> cloneableEvents;
        List<Event.EventTile> dynamics;
        Level level;

        public TileManager(Level level)
        {
            cloneableEvents = new Dictionary<int, Type>();
            this.level = level;
            this.dynamics = new List<Event.EventTile>();

            cloneableEvents.Add((int)Event.EventType.GENERATOR, typeof(Event.GeneratorEvent));
            cloneableEvents.Add((int)Event.EventType.PLAYER_SPAWN, typeof(Event.PlayerSpawnEvent));
            cloneableEvents.Add((int)Event.EventType.SPAWN, typeof(Event.SpawnEvent));
            cloneableEvents.Add((int)Event.EventType.STOP_AI, typeof(Event.StopAIEvent));
            cloneableEvents.Add((int)Event.EventType.WARP, typeof(Event.WarpEvent));
            cloneableEvents.Add((int)Event.EventType.ONE_WAY, typeof(Event.OneWayEvent));
        }

        public void initializeEvents()
        {
            foreach(Event.EventTile e in dynamics)
            {
                e.execute(level.World);
            }
        }

        public void Update(GameTime gt)
        {
            for (int i = 0; i < dynamics.Count; i++)
            {
                dynamics[i].Update(gt);
            }
        }

        public void addDynamicTiles(params Event.EventTile[] events)
        {
            this.dynamics.AddRange(events);
        }

        public TileData createTile(int id, int x, int y)
        {
            TileData td = new TileData(id);
            level.Map.setTile(x, y, td);
            return td;
        }

        public ExtendedTileData createExTile(int id, int x, int y, bool flipped)
        {
            ExtendedTileData ex = new ExtendedTileData(id, flipped);
            level.Map.setTile(x, y, ex);
            return ex;
        }

        public Event.EventTile createEvTile(int id, int x, int y, bool flipped, params Event.TEvent[] events)
        {
            Event.EventTile e = new Event.EventTile(id, flipped, x, y, events);
            this.dynamics.Add(e);
            level.Map.setTile(x, y, e);
            return e;
        }

        public GFX.TileAnimation createATile(int[] ids, int x, int y, bool flipped, int interval, bool pingpong, params Event.TEvent[] events)
        {
            GFX.TileAnimation a = new GFX.TileAnimation(ids, interval, pingpong, flipped, x, y, events);
            this.dynamics.Add(a);
            level.Map.setTile(x, y, a);
            return a;
        }

        public Event.TEvent createEvent(int id, World world, int x, int y)
        {
            if (cloneableEvents.ContainsKey(id))
            {     
                Event.TEvent evt = null;
                if (cloneableEvents[id].IsSubclassOf(typeof(Event.IndependentEvent)))
                    evt = (Event.TEvent)Activator.CreateInstance(cloneableEvents[id], world, x, y);
                else
                    evt = (Event.TEvent)Activator.CreateInstance(cloneableEvents[id]);

                return evt;
            }

            return null;
        }

        //public T createEvent<T>(int id, params object[] args) where T : Event.TEvent
        //{
        //    return (T)createEvent(id, args);
        //}

        //public T createEvent<T>(params object[] args) where T : Event.TEvent
        //{
        //    Event.TEvent evt = (Event.TEvent)Activator.CreateInstance(typeof(T), args);
        //    return (T)evt;
        //}
    }
}
