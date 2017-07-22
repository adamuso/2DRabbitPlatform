using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.Engine
{
    public class Camera2D
    {
        Vector3 translation;
        //float scale;
        Entity.Entity followed;
        RabbitPlatform game;
        Rectangle? cameraLimit;

        public Camera2D(RabbitPlatform instance)
        {
            this.game = instance;
            translation = new Vector3(0, 0, 0);
            followed = null;
            cameraLimit = null;
        }

        public void moveTranslation(float x, float y)
        {
            translation -= new Vector3(x, y, 0);
        }

        public void setTranslation(float x, float y)
        {
            translation = new Vector3(x, y, 0);
        }

        public void setLimit(int x, int y, int width, int height)
        {
            cameraLimit = new Rectangle(x, y, width, height);
        }

        public void removeLimit()
        {
            cameraLimit = null;
        }

        public void followEntity(Entity.Entity entity)
        {
            this.followed = entity;
        }

        public void Update(GameTime gt)
        {
            if (followed != null)
                setTranslation(-followed.Location.X + game.Resolution.Scene.X / 2, -followed.Location.Y + game.Resolution.Scene.Y / 2);
        }

        public bool isOnCamera(Location l)
        {
            return Bounds.Contains(l.iX, l.iY);
        }

        public Matrix getProjection(Vector2 parallax)
        {
            if (cameraLimit != null)
            {
                translation.X = -MathHelper.Clamp(-translation.X, cameraLimit.Value.X, cameraLimit.Value.Width);
                translation.Y = -MathHelper.Clamp(-translation.Y, cameraLimit.Value.Y, cameraLimit.Value.Height);
            }

            Vector3 trans = (translation * new Vector3(parallax, 1f)) - new Vector3(game.Resolution.Scene.X / 2, game.Resolution.Scene.Y / 2, 0);

            Matrix projection = Matrix.CreateTranslation(trans) *
                         Matrix.CreateTranslation(new Vector3(game.Resolution.Scene.X / 2, game.Resolution.Scene.Y / 2, 0));

            return projection;
        }

        public Matrix Projection { get { return getProjection(Vector2.One); } }
        public Rectangle Bounds { get { return new Rectangle((int)-translation.X, (int)-translation.Y, (int)game.Resolution.Scene.X, (int)game.Resolution.Scene.Y); } }
        public Vector2 Translation { get { return new Vector2(translation.X * -1, translation.Y * -1); } }
    }
}
