using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _2DRabbitPlatform.Engine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.GFX
{
    public class RenderManager
    {
        Engine.World world;
        ushort entityLayer;
        List<MapLayer> layers;

        public RenderManager(Engine.World world, ushort layers, ushort entityLayer)
        {
            this.world = world;
            this.layers = new List<MapLayer>();
            this.entityLayer = entityLayer;

            for (int i = 0; i < layers; i++)
            {
                this.layers.Add(new MapLayer(world, null, (float)i / (float)layers));
            }
        }

        public void render(Engine.Entity.Entity entity)
        {
            MapLayer entLay = getEntityLayer();

            entity.Draw(entLay);
        }

        public void render(TileMap map)
        {
            MapLayer entLay = getLayer(entityLayer);

            entLay.Begin();
            {
                map.Draw(entLay);
            }
            entLay.End();
        }

        public void render(TileMap map, int layer)
        {
            MapLayer lay = getLayer(layer);

            int w = map.Width * Tile.STANDARD_GTILE_WIDTH, h = map.Height * Tile.STANDARD_GTILE_HEIGHT;
            int bx = (int)(world.WorldCamera.Bounds.X * lay.XSpeed), by = (int)(world.WorldCamera.Bounds.Y * lay.YSpeed),
                rig = bx + world.WorldCamera.Bounds.Width, bot = by + world.WorldCamera.Bounds.Height;
            int startX = bx / w - 1, startY = by / h - 1,
                endX = rig / w + 1, endY = bot / h + 1;

            lay.Begin();
            {
                if (lay.WidthWrap && lay.HeightWrap)
                {
                    for (int x = startX; x < endX; x++)
                        for (int y = startY; y < endY; y++)
                            map.Draw(lay, x, y);
                }
                else if (lay.WidthWrap)
                {
                    for (int x = startX; x < endX; x++)
                        map.Draw(lay, x, 0);
                }
                else if (lay.HeightWrap)
                {
                    for (int y = startY; y < endY; y++)
                        map.Draw(lay, 0, y);
                }
                else
                {
                    map.Draw(lay);
                }
            }
            lay.End();
        }

        public void renderMap(int layer)
        {
            MapLayer lay = getLayer(layer);

            if (lay is MapLayer && lay.Map != null)
            {
                TileMap map = lay.Map;
                Vector2 scroll = new Vector2(lay.XScroll, lay.YScroll);//Vector2.Transform(new Vector2(mapLay.XScroll, mapLay.YScroll), Matrix.Invert(world.Game.Resolution.Projection));

                int w = map.Width * Tile.STANDARD_GTILE_WIDTH, h = map.Height * Tile.STANDARD_GTILE_HEIGHT;
                int bx = (int)(world.WorldCamera.Bounds.X * lay.XSpeed - scroll.X), by = (int)(world.WorldCamera.Bounds.Y * lay.YSpeed - scroll.Y),
                    rig = (int)(bx + world.WorldCamera.Bounds.Width), bot = (int)(by + world.WorldCamera.Bounds.Height);
                int startX = bx / w - 1, startY = by / h - 1,
                    endX = rig / w + 1, endY = bot / h + 1;

                lay.Begin();
                {
                    if (lay.WidthWrap && lay.HeightWrap)
                    {
                        for (int x = startX; x < endX; x++)
                            for (int y = startY; y < endY; y++)
                                map.Draw(lay, x, y);
                    }
                    else if (lay.WidthWrap)
                    {
                        for (int x = startX; x < endX; x++)
                            map.Draw(lay, x, 0);
                    }
                    else if (lay.HeightWrap)
                    {
                        for (int y = startY; y < endY; y++)
                            map.Draw(lay, 0, y);
                    }
                    else
                    {
                        map.Draw(lay);
                    }
                }
                lay.End();
            }
        }

        public void Update(GameTime gt)
        {
            foreach (MapLayer layer in layers)
            {
                layer.Update(gt);
            }
        }

        public MapLayer getEntityLayer()
        {
            return layers[entityLayer];
        }

        public MapLayer getLayer(int layer)
        {
            return layers[layer];
        }

        public List<MapLayer> Layers { get { return layers; } }
        public ushort EntityLayerID { get { return (ushort)entityLayer; } }
    }
}
