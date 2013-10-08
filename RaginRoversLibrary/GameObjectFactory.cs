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
using System.Runtime;

namespace RaginRoversLibrary
{

    public class GameObject
    {
        public int id;
        public int typeid;
        public string textureassetname;  // Asset name for the texture this object uses
        public Sprite sprite;  // You may never ever ever ever ever store a reference to this sprite directly
                               // if you want to reference this GameObject do it by id #
    }

    public delegate Sprite CreateSprite (Vector2 location,
                                            Texture2D texture,
                                            Vector2 velocity,
                                            float rotation);

    public class GameObjectFactory
    {
        private static GameObjectFactory instance;
        private int lastid;
        private Dictionary<int, GameObject> objects;
        private Dictionary<int, CreateSprite> creators;
        private TextureManager textureManager;


        // Making the object constructor private ensures nobody else can create a "NEW" gameobjectfactory
        private GameObjectFactory()
        {
            lastid = 0;
            this.objects = new Dictionary<int, GameObject>();
            this.creators = new Dictionary<int, CreateSprite>();
            
            /*
             *                     // Default creators
            this.creators.Add(int.CAT, SpriteCreators.CreateCat);
            this.creators.Add(int.DOG, SpriteCreators.CreateDog);

             * */

        }

        public void Initialize(TextureManager textureManager)
        {
            this.textureManager = textureManager;
        }

        public void AddCreator(int gotype, CreateSprite cs)
        {
            if (!this.creators.ContainsKey(gotype))
            {
                this.creators.Add(gotype, cs);
            }
        }

        public int Create(  int gotype, 
                            Vector2 location,
                            string textureassetname,
                            Vector2 velocity,
                            float rotation,
                            float upperBounds,
                            float lowerBounds)
        {
            lastid++;

            GameObject go = new GameObject();
            
            go.id = lastid;
            go.typeid = gotype;
            go.textureassetname = textureassetname;
            go.sprite = null;

            if (this.creators.ContainsKey(gotype))
            {
                go.sprite = this.creators[gotype](location, textureManager.Texture(textureassetname), velocity, rotation);
            }

            go.sprite.UpperRotationBounds = upperBounds;
            go.sprite.LowerRotationBounds = lowerBounds;

            this.objects.Add(lastid, go);
             
            return lastid;
        }

        public void Remove(int objectid)
        {
            if (this.objects.ContainsKey(objectid))
            {
                if (objects[objectid].sprite != null)
                {
                    objects[objectid].sprite.Destroy();
                    objects[objectid].sprite = null;
                    objects[objectid] = null;
                }

                this.objects.Remove(objectid);
            }
        }

        public string Serialize()
        {
            string lines = "";

            foreach (int key in objects.Keys)
            {
                lines +=    objects[key].id + "\t" +
                            objects[key].typeid + "\t" +
                            objects[key].sprite.Location.X + "\t" +
                            objects[key].sprite.Location.Y + "\t" +
                            objects[key].textureassetname + "\t" +
                            objects[key].sprite.Rotation +
                            "\n"
                         ;
            }

           
            return lines;
        }

        public Dictionary<int, GameObject> Objects
        {
            get
            {
                return this.objects;
            }
        }

        // Guarantee only one instance
        public static GameObjectFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObjectFactory();
                }
                return instance;
            }
        }
    }
}
