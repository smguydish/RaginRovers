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

        public bool canStartRotation;
        int rotationDirection;

        public CannonManager()
        {
            canStartRotation = false;
            rotationDirection = 1;
        }

        public CannonState cannonState = CannonState.ROTATE;




        public void ChangeCannonState()
        {
            if (cannonState == CannonState.POWER)
            {
                cannonState = CannonState.SHOOT;
            }
            if (cannonState == CannonState.ROTATE)
                cannonState = CannonState.POWER;
        }

        public float Rotation()
        {
            return 0f;
        }
    }
}
