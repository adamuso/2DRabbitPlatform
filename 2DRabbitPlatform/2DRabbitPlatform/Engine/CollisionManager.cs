using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using _2DRabbitPlatform.GFX;

namespace _2DRabbitPlatform.Engine
{
    public class CollisionManager
    {
        World world;

        public CollisionManager(World world)
        {
            this.world = world;
        }

        public int checkHorizontalMove(Entity.MoveableEntity entity, TileMap tileMap)
        {
            Rectangle rect = entity.CollisionArea;
            rect.Offset(0, 0);

            int endy = 0;
            float xVel = entity.Velocity.X, yVel = entity.Velocity.Y;

            //if (xVel > 0)
            //{
                int maxy = 0;

                int left = (int)rect.Left / Tile.STANDARD_GTILE_WIDTH,
                    right = (int)rect.Right / Tile.STANDARD_GTILE_WIDTH,
                    top = (int)rect.Top / Tile.STANDARD_GTILE_WIDTH,
                    bottom = (int)rect.Bottom / Tile.STANDARD_GTILE_WIDTH;

                for(int y = top; y <= bottom; y++)
                    for (int x = left; x <= right; x++)
                    {
                        if (!entity.World.Level.getTile(x, y).isEmpty)
                        {
                            if (Mask.IntersectsWithTile(entity, entity.World, x, y, out maxy))
                            {
                                //if (y == top)
                                //    endy += maxy;
                                //if (y == bottom)
                                    endy += maxy;
                            }
                        }
                    }

            return endy;
        }

        public int checkVerticalMove(Entity.MoveableEntity entity, TileMap tileMap)
        {
            Rectangle rect = entity.CollisionArea;
            int startx, endx,
                starty, endy;
            int result = 0;
            float yVel = entity.Velocity.Y;

            startx = rect.Left / Tile.STANDARD_GTILE_WIDTH;
            endx = rect.Right / Tile.STANDARD_GTILE_WIDTH;
            starty = rect.Top / Tile.STANDARD_GTILE_HEIGHT;
            endy = rect.Bottom / Tile.STANDARD_GTILE_HEIGHT;

            bool ad = false, sb = false;

            for (int x = startx; x <= endx; x++)
            {
                if (!entity.World.Level.getTile(x, starty).isEmpty)
                {
                    int buffer = 0;

                    if (entity.World.Level.getTile(x, starty).HasEvents)
                        if (((Event.EventTile)entity.World.Level.getTile(x, starty)).OneWayModifier)
                            if (yVel < 0)
                                continue;

                    if (Mask.IntersectsWithTile(entity, entity.World, x, starty, out buffer) && !ad)
                    {
                        result += buffer;
                        ad = true;
                    }
                }

                if (!entity.World.Level.getTile(x, endy).isEmpty)
                {
                    int buffer = 0;

                    if (entity.World.Level.getTile(x, endy).HasEvents)
                        if (((Event.EventTile)entity.World.Level.getTile(x, endy)).OneWayModifier)
                            if (yVel < 0)
                                continue;

                    if (Mask.IntersectsWithTile(entity, entity.World, x, endy, out buffer) && !sb)
                    {
                        result += buffer;
                        sb = true;
                    }
                }
            }

            return result;
        }

#if OLD
        public int checkVerticalMove(Entity.MoveableEntity entity, TileMap tileMap, bool test)
        {
            Rectangle rect = entity.CollisionArea;
            Location current;
            int endy = 0;
            float yVel = entity.Velocity.Y;

            if (yVel < 0)
            {
                int maxy = 0;

                current = new Location(entity.World, rect.Right, rect.Top);
                if (!current.getTile().isEmpty)
                {
                    if(current.getTile().HasEvents)
                        if(((Event.EventTile)current.getTile()).OneWayModifier)
                            goto escape1;

                    if (Mask.IntersectsWithTile(entity, current, out maxy))
                    {
                        endy += maxy;
                        return endy;
                    }

                escape1: ;
                }

                current = new Location(entity.World, rect.Left, rect.Top);
                if (!current.getTile().isEmpty)
                {
                    if (current.getTile().HasEvents)
                        if (((Event.EventTile)current.getTile()).OneWayModifier)
                            goto escape1;

                    if (Mask.IntersectsWithTile(entity, current, out maxy))
                    {
                        endy += maxy;
                        return endy;
                    }

                escape1: ;
                }


            }
            else if (yVel > 0)
            {
                int maxy = 0;

                current = new Location(entity.World, rect.Right, rect.Bottom);
                if (!current.getTile().isEmpty)
                    if (Mask.IntersectsWithTile(entity, current, out maxy))
                    {
                        endy -= maxy;
                        return endy;
                    }

                current = new Location(entity.World, rect.Left, rect.Bottom);
                if (!current.getTile().isEmpty)
                    if (Mask.IntersectsWithTile(entity, current, out maxy))
                    {
                        endy -= maxy;
                        return endy;
                    }
            }

            return endy;
        }
#endif
        public bool checkPerfect(Entity.Entity entity1, Entity.Entity entity2)
        {
            if(entity2.CollisionArea.Intersects(entity2.CollisionArea))
                return Mask.Intersects(entity1.Location, entity1.Mask, entity2.Location, entity2.Mask, entity1.IsFlipped, entity2.IsFlipped);
            
            return false;
        }
#if OLD
        public bool check(Entity.Entity entity1, Entity.Entity entity2)
        {
            return checkRoundArea(entity1, entity2);
        }

        public bool checkRoundArea(Entity.Entity entity1, Entity.Entity entity2)
        {
            return evalRoundArea(entity1, entity2) < 0;
        }

        public float evalRoundArea(Entity.Entity entity1, Entity.Entity entity2)
        {
            Location vertice1 = new Location(world, entity1.CollisionArea.X, entity1.CollisionArea.Y),
                     vertice2 = new Location(world, entity2.CollisionArea.X, entity2.CollisionArea.Y);
            Location center1 = new Location(world, entity1.CollisionArea.X + entity1.CollisionArea.Width / 2, entity1.CollisionArea.Y + entity1.CollisionArea.Height / 2),
                     center2 = new Location(world, entity2.CollisionArea.X + entity2.CollisionArea.Width / 2, entity2.CollisionArea.Y + entity2.CollisionArea.Height / 2);
            float radius1 = vertice1.getDistance(center1),
                  radius2 = vertice2.getDistance(center2),
                  distance = center1.getDistance(center2);

            return distance - radius1 - radius2;
        }

        public bool checkRoundArea(Location location, Entity.Entity entity)
        {
            Location vertice = new Location(world, entity.CollisionArea.X, entity.CollisionArea.Y);
            Location center = new Location(world, entity.CollisionArea.X + entity.CollisionArea.Width, entity.CollisionArea.Y + entity.CollisionArea.Height);
            float radius = vertice.getDistance(center),
                  distance = center.getDistance(location);

            return distance - radius < 0;
        }

        public bool checkRoundArea(int x, int y, Entity.Entity entity)
        {
            return checkRoundArea(new Location(world, x, y), entity);
        }

        public bool checkRectArea(Entity.Entity entity1, Entity.Entity entity2)
        {
            return entity1.CollisionArea.Intersects(entity1.CollisionArea);
        }

        public bool checkRectArea(int x, int y, Entity.Entity entity)
        {
            return checkRectArea(new Location(world, x, y), entity);
        }

        public bool checkRectArea(Location location, Entity.Entity entity)
        {
            return entity.CollisionArea.Intersects(new Microsoft.Xna.Framework.Rectangle(location.iX, location.iY, 1, 1));
        }

        public bool checkCollisionWithGround(Entity.Entity entity)
        {
            TileData first = world.getTile(new Location(world, entity.DrawingArea.Left, entity.DrawingArea.Bottom));
            TileData second = world.getTile(new Location(world, entity.DrawingArea.Right, entity.DrawingArea.Bottom));

            return !first.isEmpty || !second.isEmpty;
        }

        public bool checkRoundCenter(Entity.Entity entity1, Entity.Entity entity2, float factor)
        {
            float radius1 = entity1.Location.getDistance(entity1.Center) * factor,
                  radius2 = entity2.Location.getDistance(entity2.Center) * factor,
                  distance = entity1.Center.getDistance(entity2.Center);

            return distance - radius1 - radius2 < 0;
        }

        public bool checkRoundCenter(Entity.Entity entity1, Location location, float factor)
        {
            float radius1 = entity1.Location.getDistance(entity1.Center) * factor,
                  distance = entity1.Center.getDistance(location);

            return distance - radius1 < 0;
        }
#endif

        public bool checkLocationOver(Location location, Entity.Entity entity)
        {
            return entity.DrawingArea.Contains(location.iX, location.iY);
        }

        public bool checkMouseOver(Entity.Entity entity)
        {
            return checkLocationOver(world.InputManager.MouseLocation, entity);
        }

        public bool isGoingThrough(Rectangle rect, Vector2 v1, Vector2 v2)
        {
            float min_x = ((v2.Y - v1.Y) * (rect.X - v1.X)) / (v2.X - v1.X) + v1.Y;
            float max_x = ((v2.Y - v1.Y) * ((rect.X + rect.Width) - v1.X)) / (v2.X - v1.X) + v1.Y + rect.Height;

            return !((rect.Y + rect.Height < max_x) ^ (rect.Y > min_x));
        }

        public bool isGoingThrough(Rectangle rect, Vector2 v1, float xvel, float yvel)
        {
            Vector2 v2 = v1 + new Vector2(xvel, yvel);

            float min_x = ((v2.Y - v1.Y) * (rect.X - v1.X)) / (v2.X - v1.X) + v1.Y;
            float max_x = ((v2.Y - v1.Y) * ((rect.X + rect.Width) - v1.X)) / (v2.X - v1.X) + v1.Y + rect.Height;

            return !((rect.Y + rect.Height < max_x) ^ (rect.Y > min_x));
        }

        public bool isGoingThrough(Rectangle rect, Location v1, Location v2)
        {
            float min_x = ((v2.Y - v1.Y) * (rect.X - v1.X)) / (v2.X - v1.X) + v1.Y;
            float max_x = ((v2.Y - v1.Y) * ((rect.X + rect.Width) - v1.X)) / (v2.X - v1.X) + v1.Y + rect.Height;

            return !((rect.Y + rect.Height < max_x) ^ (rect.Y > min_x));
        }

        public bool isGoingThrough(Rectangle rect, Location v1, float xvel, float yvel)
        {
            Vector2 v2 = v1 + new Vector2(xvel, yvel);

            float min_x = ((v2.Y - v1.Y) * (rect.X - v1.X)) / (v2.X - v1.X) + v1.Y;
            float max_x = ((v2.Y - v1.Y) * ((rect.X + rect.Width) - v1.X)) / (v2.X - v1.X) + v1.Y + rect.Height;

            return !((rect.Y + rect.Height < max_x) ^ (rect.Y > min_x));
        }
    }
}
