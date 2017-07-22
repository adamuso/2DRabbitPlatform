using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.GFX
{
    public abstract class Mask
    {
        protected byte[] mask;
        protected int width, height;

        public Mask(int width, int height)
        {
            mask = new byte[width * height];
            this.width = width;
            this.height = height;
        }

        public byte[] GetData()
        {
            return mask;
        }

        public virtual byte getByte(int index, bool flipped = false)
        {
            if (!flipped)
                return mask[index];

            int y = index / width;
            int x = width - (index - y * width) - 1;

            return mask[y * width + x];
        }

        public static bool Intersects(Engine.ICollisionable collisionAble1, Engine.ICollisionable collisionAble2, bool coll1flipped = false, bool coll2flipped = false)
        {
            return IsIntersectPixels(collisionAble1.CollisionArea, collisionAble1.Mask, collisionAble2.CollisionArea, collisionAble2.Mask, coll1flipped, coll2flipped);
        }

        public static bool IntersectsWithTile(Engine.ICollisionable collisionAble1, Engine.Location location)
        {
            Engine.TileData data = location.getTile();
            Tile tile = location.World.Level.TileSet[data.ID];
            Rectangle rect = tile.CollisionArea, coll = collisionAble1.CollisionArea;
            rect.Offset(location.getTileX() * Tile.STANDARD_GTILE_WIDTH, location.getTileY() * Tile.STANDARD_GTILE_HEIGHT);

            return IsIntersectPixels(coll, collisionAble1.Mask, rect, tile.Mask, false, data.Flipped);
        }

        public static bool IntersectsWithTile(Engine.ICollisionable collisionAble1, Engine.Location location, out int maxY)
        {
            Engine.TileData data = location.getTile();
            Tile tile = location.World.Level.TileSet[data.ID];
            Rectangle rect = tile.CollisionArea, coll = collisionAble1.CollisionArea;
            rect.Offset(location.getTileX() * Tile.STANDARD_GTILE_WIDTH, location.getTileY() * Tile.STANDARD_GTILE_HEIGHT);

            return IntersectPixels(coll, collisionAble1.Mask, rect, tile.Mask, false, data.Flipped, out maxY);
        }

        public static bool IntersectsWithTile(Engine.ICollisionable collisionAble1, Engine.World world, int tileX, int tileY, out int maxY)
        {
            Engine.TileData data = world.Level.getTile(tileX, tileY);
            Tile tile = world.Level.TileSet[data.ID];
            Rectangle rect = tile.CollisionArea, coll = collisionAble1.CollisionArea;
            rect.Offset(tileX * Tile.STANDARD_GTILE_WIDTH, tileY * Tile.STANDARD_GTILE_HEIGHT);

            return IntersectPixels(coll, collisionAble1.Mask, rect, tile.Mask, false, data.Flipped, out maxY);
        }

        public static bool Intersects(Vector2 position1, Mask mask1, Vector2 position2, Mask mask2, bool mask1flipped, bool mask2flipepd)
        {
            return IntersectPixels(new Rectangle((int)position1.X, (int)position1.Y, mask1.width, mask1.height), mask1, new Rectangle((int)position2.X, (int)position2.Y, mask2.width, mask2.height), mask2, mask1flipped, mask2flipepd);
        }

        public static bool Intersects(Vector2 position1, Mask mask1, Vector2 position2, Mask mask2, bool mask1flipped, bool mask2flipped, out int maxY)
        {
            return IntersectPixels(new Rectangle((int)position1.X, (int)position1.Y, mask1.width, mask1.height), mask1, new Rectangle((int)position2.X, (int)position2.Y, mask2.width, mask2.height), mask2, mask1flipped, mask2flipped, out maxY);
        }

        public static bool IntersectPixels(Rectangle rectangleA, Mask dataA, Rectangle rectangleB, Mask dataB, bool dataAflipped, bool dataBflipped)
        {
            int buf;
            return IntersectPixels(rectangleA, dataA, rectangleB, dataB, dataAflipped, dataBflipped, out buf);
        }

        public static bool IntersectPixels(Rectangle rectangleA, Mask dataA, Rectangle rectangleB, Mask dataB, bool dataAflipped, bool dataBflipped, out int maxY)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);
            bool result = false;
            maxY = -rectangleB.Height;
            int miny = rectangleB.Height;

            // Check every point within the intersection bound
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    byte byteA = dataA.getByte((x - rectangleA.Left) +
                                 (y - rectangleA.Top) * rectangleA.Width, dataAflipped);
                    byte byteB = dataB.getByte((x - rectangleB.Left) +
                                 (y - rectangleB.Top) * rectangleB.Width, dataBflipped);

                    if (byteA != 0 && byteB != 0)
                    {
                        int rely = y - rectangleA.Top;
                        maxY = Math.Max(maxY, rectangleA.Height - rely);
                    }

                    // If both pixels are not completely transparent,
                    if (byteA != 0 && byteB != 0)
                    {
                        // then an intersection has been found
                        result = true;
                    }
                }
            }

            //maxY = maxY - miny;

            return result;
        }

        public static bool IsIntersectPixels(Rectangle rectangleA, Mask dataA, Rectangle rectangleB, Mask dataB, bool dataAflipped, bool dataBflipped)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    byte byteA = dataA.getByte((x - rectangleA.Left) +
                     (y - rectangleA.Top) * rectangleA.Width, dataAflipped);
                    byte byteB = dataB.getByte((x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width, dataBflipped);

                    // If both pixels are not completely transparent,
                    if (byteA != 0 && byteB != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
