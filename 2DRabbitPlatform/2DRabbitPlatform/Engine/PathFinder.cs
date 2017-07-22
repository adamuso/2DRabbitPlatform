#if OLD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _2DRabbitPlatform.Engine.Entity;

namespace _2DRabbitPlatform.Engine
{
    public class PathFinder
    {
        Location location;
        Entity.Entity entity, findEntity;
        bool findpath;

        public PathFinder(Entity.Entity entity)
        {
            this.entity = entity;
            this.findEntity = null;
            this.findpath = false;
        }

        public void goTo(Location location)
        {
            this.location = location;
            findEntity = null;
            this.findpath = true;
        }

        public void goTo(Entity.Entity entity)
        {
            findEntity = entity;
            findpath = true;
        }

        public float getDiretion()
        {
            if (findEntity == null)
                return entity.Center.getDirectionTo(location);
            else
                return entity.Center.getDirectionTo(findEntity.Center);
        }

        public void Update()
        {
            if (findEntity == null)
            {
                if (entity.Center.getDistance(location) < 3)
                    findpath = false;
            }
            else
            {
                if (findEntity.World.CollisionManager.checkRoundCenter(entity, findEntity, 0.6f))
                {
                    findpath = false;
                    findEntity = null;
                }
            }
        }

        public bool IsFindingPath { get { return findpath; } }
    }
}
#endif