using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace _2DRabbitPlatform.GFX
{
    public class TextureManager
    {
        private RabbitPlatform game;
        private static TextureManager myInstance;

        //------------------- PRIVATE TEXTURES --------------------

        private Texture2D rabbit_Anim;
        private Texture2D shuriken;

        //------------------- PUBLIC TEXTURES --------------------

        public AnimatedTexture rabbit_walkW;
        public AnimatedTexture shuriken_anim;
        public AnimatedTexture attack1;
        public Texture2D gui;
        public Texture2D rabbitMask;
        public Texture2D i_carrot;
        public Texture2D gui_healthq;
        public Texture2D shurikenMask;

        public TextureManager(RabbitPlatform instance)
        {
            this.game = instance;
            myInstance = this;
        }

        public void loadTextures()
        {
            // private
            rabbit_Anim = game.Content.Load<Texture2D>("rabbitanim");
            shuriken = game.Content.Load<Texture2D>("shuriken");

            // public
            shurikenMask = game.Content.Load<Texture2D>("shuriken_mask");
            shuriken_anim = AnimatedTexture.FromTexture(shuriken, 0, 0, 15, 14, 1);
            gui_healthq = game.Content.Load<Texture2D>("healthq");
            rabbitMask = game.Content.Load<Texture2D>("RabbitMask");
            i_carrot = game.Content.Load<Texture2D>("i_carrot");
            rabbit_walkW = AnimatedTexture.FromTexture(rabbit_Anim, 0, 0, 32, 32, 8);
            attack1 = AnimatedTexture.FromTexture(game.Content.Load<Texture2D>("attack1"), 0, 0, 10, 20, 3);
        }

        public static TextureManager Instance { get { return myInstance; } }
    }
}
