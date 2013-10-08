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
using RaginRoversLibrary;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics.Contacts;
using System.IO;

namespace RaginRoversLibrary
{
    class CannonManager
    {
        public enum CannonState
        {
            ROTATE,
            POWER,
            SHOOT
        }

        public float elapsedTime = 0;

        public CannonState cannonState = CannonState.ROTATE;

        public int Direction = 1;

        public CannonManager()
        { 
        }

        public void ChangeCannonState()
        {
            if (cannonState == CannonState.POWER)
            {
                cannonState = CannonState.SHOOT;
            }
            if (cannonState == CannonState.ROTATE)
                cannonState = CannonState.POWER;
        }

        public GameObjectFactory ManipulateCannons(GameObjectFactory factory)
        {
            if (cannonState == CannonState.ROTATE)
            {
                foreach (int key in factory.Objects.Keys)
                {
                    if (factory.Objects[key].typeid == (int)RaginRovers.GameObjectTypes.CANNON)
                    {
                        //decide whether to rotate one way or back
                        if (factory.Objects[key].sprite.Rotation >= factory.Objects[key].sprite.LowerRotationBounds)
                            factory.Objects[key].sprite.rotationDirection = -1;
                        if (factory.Objects[key].sprite.Rotation <= factory.Objects[key].sprite.UpperRotationBounds)
                            factory.Objects[key].sprite.rotationDirection = 1;



                        factory.Objects[key].sprite.Rotation += ((MathHelper.PiOver4 / 16) * factory.Objects[key].sprite.rotationDirection);
                    }
                }

            }
            if (cannonState == CannonState.POWER)
            {
                //power go between two values incrementally
                //have to link the cannon objects together with integer or something so we can use them as a unit
                
                foreach (int key in factory.Objects.Keys)
                {
                    if (factory.Objects[key].typeid == (int)RaginRovers.GameObjectTypes.POWERMETERTAB)
                    {
                        if(factory.Objects[key].sprite.Location.X < 0)
                        {
                            Direction = 1;
                        }
                        else if(factory.Objects[key].sprite.Location.X > 500)
                        {
                            Direction = -1;
                        }
                        factory.Objects[key].sprite.Location += new Vector2(10 * Direction, 0);
                    }
                }

                //visually show somehow
            }
            if (cannonState == CannonState.SHOOT)
            {
                ShootDoggy(factory);
                
                cannonState = CannonState.ROTATE;
            }

            return factory;
        }

        public GameObjectFactory ShootDoggy(GameObjectFactory factory)
        {
            List<int> temp = new List<int>();
            //figure out which cannon to shoot from
            //replace once just pass which cannon
            foreach (int key in factory.Objects.Keys)
            {
                if (factory.Objects[key].typeid == (int)RaginRovers.GameObjectTypes.CANNON)
                {
                    temp.Add(key);
                }
            }
            //endreplace
            for (int i = 0; i < temp.Count ; i++ )
            {

                //dog
                int dog = factory.Create(
                    (int)RaginRovers.GameObjectTypes.DOG,
                    factory.Objects[temp[i]].sprite.Location,
                    "spritesheet",
                    Vector2.Zero,
                    factory.Objects[temp[i]].sprite.Rotation,
                    0,
                    0);

                factory.Objects[dog].sprite.PhysicsBody.LinearVelocity = new Vector2(
                        20 * (float)Math.Cos((double)factory.Objects[temp[i]].sprite.Rotation),
                        20 * (float)Math.Sin((double)factory.Objects[temp[i]].sprite.Rotation));

                factory.Objects[dog].sprite.PhysicsBody.Mass = 30;
                factory.Objects[dog].sprite.PhysicsBody.Restitution = 0.4f;

                //boom
                int boom = factory.Create(
                    (int)RaginRovers.GameObjectTypes.BOOM,
                    factory.Objects[temp[i]].sprite.Location,
                    "boom",
                    Vector2.Zero,
                    factory.Objects[temp[i]].sprite.Rotation,
                    0,
                    0);
                //changing location so that origins equal
                factory.Objects[boom].sprite.Location += factory.Objects[temp[i]].sprite.Origin - factory.Objects[boom].sprite.Origin;

                factory.Objects[boom].sprite.Scale = 1.5f;
            }

            return factory;
        }

        public GameObjectFactory CreateCannonStuff(GameObjectFactory factory, MouseState ms, Camera camera, bool isReversed)
        {
            int icannon;
            if (!isReversed)
            {
                icannon = factory.Create(
                                        (int)RaginRovers.GameObjectTypes.CANNON,
                                        new Vector2((int)ms.X + camera.Position.X - 95, (int)ms.Y - 80),
                                        "spritesheet",
                                        new Vector2(0, 0),
                                        0,
                                        -MathHelper.PiOver2,
                                        0);
            }
            else
            {
                icannon = factory.Create((int)RaginRovers.GameObjectTypes.CANNON, new Vector2((int)ms.X + camera.Position.X - 95, (int)ms.Y - 80), "spritesheet", new Vector2(0, 0), -MathHelper.Pi, -MathHelper.Pi, -MathHelper.PiOver2);
            }
            int iwheel = factory.Create(
                (int)RaginRovers.GameObjectTypes.CANNONWHEEL,
                new Vector2((int)ms.X + camera.Position.X - 30, (int)ms.Y - 120),
                "spritesheet",
                new Vector2(0, 0),
                0,
                0f,
                0f);
            int ibar = factory.Create(
                 (int)RaginRovers.GameObjectTypes.POWERMETERBAR,
                 new Vector2(
                     factory.Objects[icannon].sprite.Location.X,
                     factory.Objects[icannon].sprite.Location.Y + factory.Objects[icannon].sprite.BoundingBoxRect.Height + 50),
                 "background",
                 new Vector2(0, 0),
                 0,
                 0f,
                 0f);
            int itab = factory.Create(
                 (int)RaginRovers.GameObjectTypes.POWERMETERTAB,
                 new Vector2(
                     factory.Objects[ibar].sprite.Location.X,
                     factory.Objects[ibar].sprite.Location.Y + factory.Objects[ibar].sprite.Origin.Y),
                 "background",
                 new Vector2(0, 0),
                 0,
                 0f,
                 0f);
            //had to put after because cant access origin before sprite is created
            factory.Objects[itab].sprite.Location -= new Vector2(0, factory.Objects[itab].sprite.Origin.Y);

            Sprite cannon = factory.Objects[icannon].sprite;
            Sprite wheel = factory.Objects[iwheel].sprite;


            cannon.Origin = new Vector2(120, 103);

            wheel.Location = cannon.Location + cannon.Origin - wheel.Origin;

            return factory;
        }

        public void Update(GameTime gameTime)
        {
            //was going to use this to limit boom duration but idk
        }
        /*
        public float ConvertToRealRadians(float Rotation)
        {
            for (; ; )
            {
                if (Rotation < 0)
                {
                    Rotation += MathHelper.TwoPi;
                }
                if (Rotation > MathHelper.TwoPi)
                {
                    Rotation -= MathHelper.TwoPi;
                }
                if (Rotation >= 0 && Rotation <= MathHelper.TwoPi) 
                    break;
            }
            return Rotation;
        }
         */
    }

}
