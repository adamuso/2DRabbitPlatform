using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.Engine
{
    public class Resolution
    {
        RabbitPlatform game;
        Vector2 size;
        Matrix matrix;
        bool stretch;
        bool enabled;

        public Resolution(RabbitPlatform instance)
        {
            this.game = instance;
            matrix = Matrix.Identity;
            stretch = false;
            enabled = false;
        }

        public void setResolution(int width, int height, bool fullscreen = false)
        {
            this.game.GraphicsManager.PreferredBackBufferWidth = width;
            this.game.GraphicsManager.PreferredBackBufferHeight = height;
            this.game.GraphicsManager.IsFullScreen = fullscreen;
            this.game.GraphicsManager.ApplyChanges();
        }

        public void setSceneSize(int width, int height)
        {
            size = new Vector2(width, height);
            updateTransform();
        }

        private void updateTransform()
        {
            float xscale = game.GraphicsDevice.Viewport.Width / size.X,
                  yscale = stretch ? game.GraphicsDevice.Viewport.Height / size.Y : xscale;

            matrix = Matrix.CreateScale(xscale, yscale, 1);
        }

        public Matrix Projection { get { return enabled ? matrix : Matrix.Identity; } }
        public Vector2 Scene { get { return enabled ? size : new Vector2(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height); } }
    }
}
