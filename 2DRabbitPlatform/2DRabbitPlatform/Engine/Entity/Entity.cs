using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.Engine.Entity 
{
    public abstract class Entity : HandleTimerBase, ICollisionable
    {
        protected Location position;
        protected Vector2 offset;
        protected Texture2D current;
        protected World world;
        protected bool destroyed;
        protected Rectangle collisionArea;
        protected Rectangle drawingArea;
        protected EntityMask entityMask;
        protected bool draw;
        protected Color color;
        private TileData lastTile;
        private bool flipped;
        private Entity attach;
        private Vector2 attachorigin;
        protected Vector2 rotationCenter;
        protected float rotation;

        public Entity(World world)
        {
            this.world = world;
            position = new Location(world, 0, 0);
            color = Color.White;
            flipped = false;
            offset = Vector2.Zero;
            rotationCenter = Vector2.Zero;
            rotation = 0f;
            drawingArea = Rectangle.Empty;
        }

        /// <summary>
        /// Oblicza odległość od podanego Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Odległość</returns>
        public float getDistance(Entity entity)
        {
            return this.Location.getDistance(entity.Location);
        }

        /// <summary>
        /// Ustawia aktualną teksture Entity
        /// </summary>
        /// <param name="texture">Tekstura</param>
        protected void setTexture(Texture2D texture)
        {
            current = texture;
        }

        protected void init(Rectangle drawingArea)
        {
            this.drawingArea = drawingArea;
        }

        /// <summary>
        /// Ustawia flagę zniszczenia
        /// </summary>
        public void destroy()
        {
            destroyed = true;

            world.EntityManager.removeEntity(this);
        }

        public void attachToEntity(Entity ent)
        {
            this.attach = ent;
            this.attachorigin = (Vector2)Location - (Vector2)ent.Location;
        }

        public void unattach()
        {
            this.attach = null;
            this.attachorigin = Vector2.Zero;
        }

#if OLD
        /// <summary>
        /// Sprawdza czy dany Entity może oddziaływać na innego.
        /// </summary>
        /// <param name="entity">Entity na które oddziałujemy</param>
        /// <returns></returns>
        public bool canInteract(Entity entity)
        {
            //return isInRange(Center.getDistance(entity.Center), 1f);
            return world.CollisionManager.checkRoundCenter(this, entity, 0.8f);
            //return world.CollisionManager.checkRoundArea(this, entity);
        }

        /// <summary>
        /// Sprawdza czy dany Entity może oddziaływać na innego.
        /// </summary>
        /// <param name="location">Lokacja, na którą odziałujemy</param>
        /// <returns></returns>
        public bool canInteract(Location location)
        {
            //return isInRange(Center.getDistance(entity.Center), 1f);
            return world.CollisionManager.checkRoundCenter(this, location, 0.8f);
            //return world.CollisionManager.checkRoundArea(this, entity);
        }
#endif
        /// <summary>
        /// Funkcja rysująca.
        /// </summary>
        /// <param name="sb">SpriteBatch</param>
        /// <param name="di">Informacje o głębokości</param>
        public virtual void Draw(GFX.MapLayer layer)
        {
            layer.Draw(current, new Rectangle(DrawingArea.X + (int)rotationCenter.X, DrawingArea.Y + (int)rotationCenter.Y,  DrawingArea.Width, DrawingArea.Height), new Rectangle(0, 0, current.Width, current.Height), color, rotation, rotationCenter, SpriteEffects.None, layer.Depth);
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);

            if (IsAttached)
                setLocation(new Location(this.world, (Vector2)attach.Location + attachorigin));

            if(Center.getTile() != lastTile)
            if (Center.getTile().HasEvents)
            {
                Event.EventTile events = (Event.EventTile)Center.getTile();
                events.execute(this);
            }

            lastTile = Center.getTile();
        }

        /// <summary>
        /// Wskazówka po najechaniu na Entity kursorem
        /// </summary>
        public virtual string CursorTip { get { return ""; } }

        public void setLocation(Location l) { position = l; }

        /// <summary>
        /// Pozycja wyznaczana z lewego górnego rogu.
        /// </summary>
        public virtual Location Location { get { return new Location(world, position + offset); } set { position = value; } }
        /// <summary>
        /// Pozycja wyznaczna z prawego dolnego rogu.
        /// </summary>
        //public virtual Location Location { get { return new Location(world, Position + new Vector2(drawingArea.Width, drawingArea.Height)); } }
        ///// <summary>
        ///// Środek Entity
        ///// </summary>
        public virtual Location Center { get { return Location.offset(drawingArea.Width / 2, drawingArea.Height / 2); }}
        /// <summary>
        /// Obiekt zniszczony
        /// </summary>
        public bool Destroyed { get { return destroyed; } }
        /// <summary>
        /// Obszar kolizji obiektu
        /// </summary>
        public virtual Rectangle CollisionArea { get { return new Rectangle(collisionArea.X + Location.iX, collisionArea.Y + Location.iY, collisionArea.Width, collisionArea.Height); } }
        /// <summary>
        /// Obszar rysowania obiektu
        /// </summary>
        public virtual Rectangle DrawingArea { get { return new Rectangle(Location.iX, Location.iY, drawingArea.Width, drawingArea.Height); } }

        public bool IsFlipped { get { return flipped; } set { flipped = value; } }

        public World World { get { return world; } }

        public GFX.Mask Mask { get { return entityMask; } }

        public Texture2D RawTexture { get { return current; } }

        public abstract Entity clone();

        public bool IsAttached { get { return attach != null; } }
    }
}
