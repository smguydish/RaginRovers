using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;

namespace RaginRoversLibrary
{
    public static class GameWorld
    {
        public static World world;  // Must be initialized
        private static int world_ofs_x = 0;
        private static int worldmin = 0;
        private static int worldmax = 5700;
        private static int screen_width = 1900;
        private static int screen_height = 1080;

        public static void Initialize(int worldmin, int worldmax, int screen_width, int screen_height, Vector2 gravity)
        {
            GameWorld.worldmin = worldmin;
            GameWorld.worldmax = worldmax;
            GameWorld.screen_height = screen_height;
            GameWorld.screen_width = screen_width;

            world = new World(gravity);
        }

        public static int ViewPortXOffset
        {
            get { return GameWorld.world_ofs_x; }
            set { GameWorld.world_ofs_x = value;  }
        }

        public static int WorldWidth
        {
            get
            {
                return worldmax;
            }
        }

        public static void Update(GameTime gameTime)
        {
            world.Step((float)(gameTime.ElapsedGameTime.TotalMilliseconds * 0.001));
        }

    }
}
