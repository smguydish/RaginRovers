using System;
using System.Collections.Generic;
using System.Linq;
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
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Factories;

namespace RaginRovers
{
    public enum GameObjectTypes
    {
        DOG,
        CAT,
        CLOUD,
        WOOD1,
        WOOD2,
        WOOD3,
        WOOD4,
        PLATFORM_LEFT,
        PLATFORM_MIDDLE,
        PLATFORM_RIGHT,
        CANNON,
        CANNONWHEEL,
        POWERMETERBAR,
        POWERMETERTAB,
        BOOM = 300
    }

    // Helper class to create sprites
    public static class SpriteCreators
    {
        public static Dictionary<string, Rectangle> spriteSourceRectangles;

        public static void Load(string path)
        {
            SpriteCreators.spriteSourceRectangles = new Dictionary<string, Rectangle>();

            // open a StreamReader to read the index

            using (StreamReader reader = new StreamReader(path))
            {
                // while we're not done reading...
                while (!reader.EndOfStream)
                {
                    // get a line
                    string line = reader.ReadLine();

                    // split at the equals sign
                    string[] sides = line.Split('=');

                    // trim the right side and split based on spaces
                    string[] rectParts = sides[1].Trim().Split(' ');

                    // create a rectangle from those parts
                    Rectangle r = new Rectangle(
                       int.Parse(rectParts[0]),
                       int.Parse(rectParts[1]),
                       int.Parse(rectParts[2]),
                       int.Parse(rectParts[3]));

                    // add the name and rectangle to the dictionary
                    SpriteCreators.spriteSourceRectangles.Add(sides[0].Trim(), r);
                }
            }

        }

        public static void AddFrames(Sprite sprite, string prefix, int count)
        {
            for (int i = 1; i <= count; i++)
            {
                string key = prefix + i.ToString().PadLeft(2, '0');
                sprite.AddFrame(SpriteCreators.spriteSourceRectangles[key]);
            }
        }

        public static Sprite CreateWood(
                                            int type,
                                            Vector2 location,
                                            Texture2D texture,
                                            Vector2 velocity,
                                            float rotation)
        {
            Sprite sprite = new Sprite("woodshape" + type,
                                               location,
                                               texture,
                                               SpriteCreators.spriteSourceRectangles["woodShape" + type],
                                               velocity,
                                               BodyType.Dynamic,
                                               true);

            
            sprite.PhysicsBody.AngularDamping = 0.9f;
            sprite.PhysicsBody.Restitution = 0.2f;
            sprite.PhysicsBody.Mass = 40;
            sprite.Rotation = rotation;

            return sprite;
        }

        public static Sprite CreateWood1(Vector2 location,Texture2D texture,Vector2 velocity,float rotation)
        {
            return SpriteCreators.CreateWood(1, location, texture, velocity, rotation);
        }

        public static Sprite CreateWood2(Vector2 location, Texture2D texture, Vector2 velocity, float rotation)
        {
            return SpriteCreators.CreateWood(2, location, texture, velocity, rotation);
        }

        public static Sprite CreateWood3(Vector2 location, Texture2D texture, Vector2 velocity, float rotation)
        {
            return SpriteCreators.CreateWood(3, location, texture, velocity, rotation);
        }

        public static Sprite CreateWood4(Vector2 location, Texture2D texture, Vector2 velocity, float rotation)
        {
            return SpriteCreators.CreateWood(4, location, texture, velocity, rotation);
        }

        public static Sprite CreateCat(
                                            Vector2 location,
                                            Texture2D texture,
                                            Vector2 velocity,
                                            float rotation)
        {

            Sprite sprite = new Sprite("sprite",
                                   location,
                                   texture,
                                   SpriteCreators.spriteSourceRectangles["cat00"],
                                   velocity,
                                   BodyType.Dynamic,
                                   true);

            SpriteCreators.AddFrames(sprite, "cat", 10);

            /*
            for (int i = 1; i <= 10; i++)
            {
                string key = "cat" + i.ToString().PadLeft(2, '0');
                sprite.AddFrame(SpriteCreators.spriteSourceRectangles[key]);
            }
            */

            sprite.Rotation = rotation;

            return sprite;
        }

        public static Sprite CreateDog(
                                            Vector2 location,
                                            Texture2D texture,
                                            Vector2 velocity,
                                            float rotation)
        {
            Sprite sprite = new Sprite("sprite",
                                   location,
                                   texture,
                                   SpriteCreators.spriteSourceRectangles["rover00"],
                                   velocity,
                                   BodyType.Dynamic,
                                   false);

            //FarseerPhysics.Common.Vertices verts = new FarseerPhysics.Common.Vertices();
            
            Fixture bodyfixture = FixtureFactory.AttachRectangle(ConvertUnits.ToSimUnits(sprite.BoundingBoxRect.Width), ConvertUnits.ToSimUnits(sprite.BoundingBoxRect.Height), 1, Vector2.Zero, sprite.PhysicsBody);
            bodyfixture.Restitution = 1;
            bodyfixture.Friction = 1;

            SpriteCreators.AddFrames(sprite, "rover", 12);

            sprite.Rotation = rotation;

            return sprite;
        }

        public static Sprite CreateBoom(Vector2 location,
                                            Texture2D texture,
                                            Vector2 velocity,
                                            float rotation)
        {
            Sprite sprite = new Sprite("sprite",
                                   location,
                                   texture,
                                   SpriteCreators.spriteSourceRectangles["boom00"],
                                   velocity,
                                   BodyType.Static,
                                   true);

            sprite.PhysicsBody.CollidesWith = Category.None;
            sprite.FrameTime = 0.06f;

            SpriteCreators.AddFrames(sprite, "boom", 12);

            sprite.Rotation = rotation;

            return sprite;
        }

        public static Sprite CreatePlatformLeft(Vector2 location,
                                                    Texture2D texture,
                                                    Vector2 velocity,
                                                    float rotation)
        {
            Sprite sprite = new Sprite("sprite",
                                   location,
                                   texture,
                                   SpriteCreators.spriteSourceRectangles["platform_left"],
                                   velocity,
                                   BodyType.Static,
                                   true);

            sprite.Rotation = rotation;

            return sprite;
        }

        public static Sprite CreatePlatformMiddle(Vector2 location,
                                                    Texture2D texture,
                                                    Vector2 velocity,
                                                    float rotation)
        {
            Sprite sprite = new Sprite("sprite",
                                   location,
                                   texture,
                                   SpriteCreators.spriteSourceRectangles["platform_middle"],
                                   velocity,
                                   BodyType.Static,
                                   true);

            sprite.Rotation = rotation;

            return sprite;
        }

        public static Sprite CreatePlatformRight(Vector2 location,
                                                    Texture2D texture,
                                                    Vector2 velocity,
                                                    float rotation)
        {
            Sprite sprite = new Sprite("sprite",
                                   location,
                                   texture,
                                   SpriteCreators.spriteSourceRectangles["platform_right"],
                                   velocity,
                                   BodyType.Static,
                                   true);

            sprite.Rotation = rotation;

            return sprite;
        }

        public static Sprite CreateCannon(Vector2 location,
                                                    Texture2D texture,
                                                    Vector2 velocity,
                                                    float rotation)
        {
           Sprite sprite = new Sprite("sprite",
                                   location,
                                   texture,
                                   SpriteCreators.spriteSourceRectangles["cannon_0001_Layer-3"],
                                   velocity,
                                   BodyType.Static,
                                   true);


            sprite.PhysicsBody.CollidesWith = Category.None;

            sprite.Rotation = rotation; //will change

            return sprite;
        }
        public static Sprite CreateCannonWheel(Vector2 location,
                                                    Texture2D texture,
                                                    Vector2 velocity,
                                                    float rotation)
        {
            Sprite sprite = new Sprite("sprite",
                                   location,
                                   texture,
                                   SpriteCreators.spriteSourceRectangles["cannon_0000_Layer-7"],
                                   velocity,
                                   BodyType.Static,
                                   true);

                sprite.PhysicsBody.CollidesWith = Category.None;

            sprite.Rotation = rotation;

            return sprite;
        }
        public static Sprite CreatePowerMeterBar(Vector2 location,
                                                     Texture2D texture,
                                                     Vector2 velocity,
                                                     float rotation)
        {
            Sprite sprite = new Sprite("sprite",
                                   location,
                                   texture,
                                   new Rectangle(0,0,320,50),
                                   velocity,
                                   BodyType.Static,
                                   true);

            sprite.PhysicsBody.CollidesWith = Category.None;

            sprite.Rotation = rotation;

            return sprite;
        }
        public static Sprite CreatePowerMeterTab(Vector2 location,
                                                     Texture2D texture,
                                                     Vector2 velocity,
                                                     float rotation)
        {
            Sprite sprite = new Sprite("sprite",
                                   location,
                                   texture,
                                   new Rectangle(0, 0, 32, 100),
                                   velocity,
                                   BodyType.Static,
                                   true);

            sprite.PhysicsBody.CollidesWith = Category.None;

            sprite.Rotation = rotation;

            return sprite;
        }
    }
}
