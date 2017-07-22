using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace _2DRabbitPlatform.Engine.Entity
{
    public class EntityManager
    {
        //--
        Dictionary<int, Entity> cloneableEntities;
        //--

        List<Entity> entities;
        Queue<Entity> creating, removing;
        World world;

        public EntityManager(World world)
        {
            this.world = world;
            this.entities = new List<Entity>();
            this.creating = new Queue<Entity>();
            this.removing = new Queue<Entity>();

            //----
            this.cloneableEntities = new Dictionary<int, Entity>();

            cloneableEntities.Add((int)StandardEntities.PLAYER, new Player(world));
            cloneableEntities.Add((int)StandardEntities.ITEM_CARROT, new ItemCarrot(world));
            cloneableEntities.Add((int)StandardEntities.WOLF, new EnemyWolf(world));
            cloneableEntities.Add((int)StandardEntities.SHURIKEN, new Shuriken(world));

            //----
        }

        public void addEntity(Entity entity)
        {
            creating.Enqueue(entity);
        }

        public void DrawEntities(SpriteBatch sb)
        {
            world.RenderManager.getEntityLayer().Begin();

            foreach (Entity ent in entities)
            {
                if (!ent.Destroyed)
                {
                    world.RenderManager.render(ent);
                }
            }

            world.RenderManager.getEntityLayer().End();
        }

        public void UpdateEntities(GameTime gt)
        {
            while (creating.Count > 0)
                entities.Insert(0, creating.Dequeue());
            while (removing.Count > 0)
                entities.Remove(removing.Dequeue());

            foreach (Entity ent in entities)
            {
                if (!ent.Destroyed)
                {
                    //if(world.WorldCamera.isOnCamera(ent.Center))
                    ent.Update(gt);

                    if (ent is IInteractable)
                    {
                        IInteractable ii = (IInteractable)ent;

                        if (world.CollisionManager.checkPerfect(world.Player, ent))
                        {
                            world.Player.interact(new Action.InteractEntityActionArgs(world.Player, ii));
                        }
                    }

                    if (ent is MoveableEntity)
                    {
                        MoveableEntity mov = (MoveableEntity)ent;

                        foreach (Entity ent2 in entities)
                        {
                            if (ent is IInteractable && ent2 is IInteractable)
                            {
                                IInteractable inter1 = (IInteractable)ent,
                                              inter2 = (IInteractable)ent2;

                                if (ent != ent2)
                                    if (world.CollisionManager.checkPerfect(ent, ent2))
                                        inter1.interact(new Action.InteractEntityActionArgs(mov, inter2));
                            }
                        }
                    }
                }
            }
        }

        public int getEntityID(Entity entity)
        {
            foreach (int key in cloneableEntities.Keys)
            {
                if (cloneableEntities[key].GetType() == entity.GetType())
                    return key;
            }

            return -1;
        }

        public void removeEntity(Entity entity)
        {
            removing.Enqueue(entity);
        }

        public void optymalizeEntity(Entity ent)
        {
            if (ent.Destroyed)
            {
                removeEntity(ent);
            }
        }

        public void clearEntities()
        {
            entities.Clear();
        }

        public Entity createEntity(int entityID)
        {
            if (cloneableEntities.ContainsKey(entityID))
            {
                Entity clone = cloneableEntities[entityID].clone();
                addEntity(clone);
                return clone;
            }

            return null;
        }

        public Entity createEntity(StandardEntities entity)
        {
            return createEntity((int)entity);
        }

        public T createEntity<T>(StandardEntities entity) where T : Entity
        {
            return (T)createEntity((int)entity);
        }

#if OLD
        public Entity createEntity(Type entity) 
        {
            Entity ent = (Entity)Activator.CreateInstance(entity, world);
            addEntity(ent);
            return ent;
        }

        public T createEntity<T>() where T : Entity
        {
            Type type = typeof(T);

            return (T)(object)createEntity(type);
        }
#endif
        public Entity getEntityAt(Location location, Entity ignore = null)
        {
            foreach (Entity ent in entities)
            {
                if (world.CollisionManager.checkLocationOver(location, ent))
                    if(ent != ignore)
                        return ent;
            }

            return null;
        }

        public List<Entity> getEntitiesAt(Location location, Entity ignore = null)
        {
            List<Entity> ret = new List<Entity>();

            foreach (Entity ent in entities)
            {
                if (world.CollisionManager.checkLocationOver(location, ent))
                    if (ent != ignore)
                        ret.Add(ent);
            }

            return ret;
        }

#if DEBUG
        public Entity[] Entities { get { return entities.ToArray(); } }
#endif
    }
}
