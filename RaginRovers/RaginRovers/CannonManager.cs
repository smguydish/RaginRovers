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

        public CannonState cannonState = CannonState.ROTATE;

        int rotationDirection = 1;

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
                        if (factory.Objects[key].sprite.Rotation >= 0)
                            rotationDirection = -1;
                        if (factory.Objects[key].sprite.Rotation <= -(MathHelper.PiOver2))
                            rotationDirection = 1;



                        factory.Objects[key].sprite.Rotation += ((MathHelper.PiOver4 / 16) * rotationDirection);
                    }
                }

            }
            if (cannonState == CannonState.POWER)
            {
                //power go between two values incrementally
                //visually show somehow
                
            }
            //Can put delay or whatever instead of these lines to reset cannonstate
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
            foreach (int key in factory.Objects.Keys)
            {
                if (factory.Objects[key].typeid == (int)RaginRovers.GameObjectTypes.CANNON)
                {
                    temp.Add(key);
                }
            }
            for (int i = 0; i < temp.Count ; i++ )
            {
                int dog = factory.Create((int)RaginRovers.GameObjectTypes.DOG,
                    factory.Objects[temp[i]].sprite.Location, "spritesheet",
                    Vector2.Zero,
                    factory.Objects[temp[i]].sprite.Rotation);
                factory.Objects[dog].sprite.PhysicsBody.LinearVelocity = new Vector2(
                        20 * (float)Math.Cos((double)ConvertToRealRadians(factory.Objects[temp[i]].sprite.Rotation)),
                        20 * (float)Math.Sin((double)ConvertToRealRadians(factory.Objects[temp[i]].sprite.Rotation)));
                factory.Objects[dog].sprite.PhysicsBody.Mass = 30;
                factory.Objects[dog].sprite.PhysicsBody.Restitution = 0.4f;
            }

            return factory;
        }
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
    }

}
