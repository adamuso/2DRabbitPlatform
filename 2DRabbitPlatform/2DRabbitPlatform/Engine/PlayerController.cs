using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _2DRabbitPlatform.Engine.Entity;
using Microsoft.Xna.Framework;
using _2DRabbitPlatform.Engine.Entity.Action;

namespace _2DRabbitPlatform.Engine
{
    public class PlayerController
    {
        World world;
        ActionArgsCreator actionCreator;
        Player player;

        public PlayerController(World world)
        {
            this.world = world;
            world.InputManager.KeyboardManager.KeyDown += new EventHandler<Input.KeyboardEvent>(KeyboardManager_KeyDown);
        }

        void KeyboardManager_KeyDown(object sender, Input.KeyboardEvent e)
        {
            if (e.Key == Microsoft.Xna.Framework.Input.Keys.LeftShift)
                player.shoot(StandardEntities.SHURIKEN);
        }

        public Player createPlayer()
        {
            player = world.EntityManager.createEntity<Player>(StandardEntities.PLAYER);
            return player;
        }

        public void Update(GameTime gt)
        {
            if (world.InputManager.isDown(Input.ActionKeyType.GO_RIGHT))
            {
                if (player.CanMove)
                {
                    player.SpriteEffect = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
                    player.Animation.playOnce();
                    player.goRight();
                }
            }
            else if (world.InputManager.isDown(Input.ActionKeyType.GO_LEFT))
            {
                if (player.CanMove)
                {
                    player.SpriteEffect = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
                    player.Animation.playOnce();
                    player.goLeft();
                }
            }
            else
            {
                player.Animation.stop();
            }

            if (world.InputManager.isDown(Input.ActionKeyType.RUN))
            {
                player.sprint(true);
            }
            else
                player.sprint(false);

            if (world.InputManager.isDown(Input.ActionKeyType.JUMP))
            {
                if(player.CanMove)
                    player.jump();
            }
            else
                player.jumpReset();
        }
    }
}
