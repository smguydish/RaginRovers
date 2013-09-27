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
using FarseerPhysics.Dynamics;
using System.IO;
using Microsoft.Xna.Framework.Storage;
using RaginRoversLibrary;

namespace RaginRovers
{
    class Cannon
    {
        Body cannon;

        public Cannon(Texture2D texture)
        {
            //cannon = new Sprite("cannonbase", new Vector2(300, 300), texture, new Rectangle(0, 32, 322, 184), Vector2.Zero, BodyType.Dynamic, true);
            cannon = new Body(new World(Vector2.Zero));
            cannon.BodyType = BodyType.Static;
            //FarseerPhysics.Collision.Shapes.Shape Wheels = new FarseerPhysics.Collision.Shapes.Shape;
            //cannon.PhysicsBodyFixture = new Fixture(new Body(new World(Vector2.Zero)), FarseerPhysics.Collision.Shapes.Shape
        }

        public virtual void Draw(SpriteBatch spritebatch)
        {
            //cannon.Draw(spritebatch);
        }
    }
}
