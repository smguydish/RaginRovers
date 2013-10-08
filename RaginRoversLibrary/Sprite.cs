using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework.Input;

namespace RaginRoversLibrary
{


    public class Sprite
    {
        public Texture2D Texture;

        protected List<Rectangle> frames = new List<Rectangle>();
        protected int frameWidth = 0;
        protected int frameHeight = 0;
        protected int currentFrame;
        protected float frameTime = 0.1f;
        protected float timeForCurrentFrame = 0.0f;
        protected float scale = 1.0f;

        protected Color tintColor = Color.White;
        protected float rotation = 0.0f;

        public int CollisionRadius = 0;
        public int BoundingXPadding = 0;
        public int BoundingYPadding = 0;
        public bool ReSpawn = true;
        public int HitPoints = 100;

        //cannonspecifics

        public int rotationDirection = 1;
        public float UpperRotationBounds = 0;
        public float LowerRotationBounds = 0;

        //

        public object tag;

        public Sprite target = null;

        protected Vector2 location = Vector2.Zero;
        protected Vector2 velocity = Vector2.Zero;
        protected Vector2 origin = Vector2.Zero;

        private bool inScreenSpace = false;

        protected Body body;
        protected Fixture bodyfixture;

        protected string name;

         public bool flip = false;

        public event OnCollisionEventHandler OnCollision;

        public Sprite(
            string Name,
            Vector2 location,
            Texture2D texture,
            Rectangle initialFrame,
            Vector2 velocity,
            BodyType bodytype,
            bool AddFixture
            ) // True
        {
            this.location = location;
            Texture = texture;

            this.name = Name;
            this.dead = false;

            frames.Add(initialFrame);
            frameWidth = initialFrame.Width;
            frameHeight = initialFrame.Height;
            origin = new Vector2(frameWidth / 2, frameHeight / 2);

            tag = null;

            body = BodyFactory.CreateBody(GameWorld.world);
            body.BodyType = bodytype;
            body.UserData = this;

            /*
            body.Restitution = 1f;
            body.Mass = 20;
            body.Friction = 5;
            body.LinearDamping = 2.4f;
            body.AngularDamping = 6.4f;
            body.Rotation = 1.3f;
            //            box.AngularVelocity = 0.1f;
            body.Inertia = 25.5f;
            */
            this.Location = location;
//            body.Position = ConvertUnits.ToSimUnits(this.Location);
            body.IgnoreGravity = false;

            if (AddFixture)
            {
                bodyfixture = FixtureFactory.AttachRectangle(ConvertUnits.ToSimUnits(initialFrame.Width), ConvertUnits.ToSimUnits(initialFrame.Height), 1, Vector2.Zero, body);
                bodyfixture.Restitution = 1;
                bodyfixture.Friction = 1;

                bodyfixture.OnCollision += new OnCollisionEventHandler(HandleCollision);
            }

            this.Velocity = ConvertUnits.ToSimUnits(velocity);
        }

        
        public void Destroy()
        {
            if (GameWorld.world.BodyList.Contains(body))
                GameWorld.world.RemoveBody(body);
        }

         ~Sprite()
        {
            this.Destroy();
        }

         public override string ToString()
         {
             return this.name;
         }
    
        public virtual bool HandleCollision(Fixture a, Fixture b, Contact contact)
        {
            if (OnCollision != null)
            {
                if ((string)a.Body.UserData == name || (string)b.Body.UserData == name)
                    return OnCollision(a, b, contact);
            }

            return true;
        }

        public Body PhysicsBody
        {
            get
            {
                return body;
            }
        }

        public Fixture PhysicsBodyFixture
        {
            get
            {
                return bodyfixture;
            }
        }

        public bool ScreenSpace
        {
            get
            {
                return inScreenSpace;
            }
            set
            {
                inScreenSpace = value;
            }
        }

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public Vector2 Location
        {
            get
            {
                return ConvertUnits.ToDisplayUnits(body.Position) -
                  new Vector2(((float)frameWidth * scale) / 2, ((float)frameHeight * scale) / 2);
            }
            set
            {
                body.Position = ConvertUnits.ToSimUnits(value + new Vector2(((float)frameWidth * scale) / 2, ((float)frameHeight * scale) / 2));
            }
        }

        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public Vector2 Velocity
        {
            get { return ConvertUnits.ToDisplayUnits(body.LinearVelocity); }
            set { body.LinearVelocity = value; }
        }

        public Color TintColor
        {
            get { return tintColor; }
            set { tintColor = value; }
        }

        public float Rotation
        {
            get { return body.Rotation; }
            set { body.Rotation = value % MathHelper.TwoPi; }
        }

        public virtual int Frame
        {
            get { return currentFrame; }
            set
            {
                currentFrame = (int)MathHelper.Clamp(value, 0,
                frames.Count - 1);
            }
        }

        public virtual float FrameTime
        {
            get { return frameTime; }
            set { frameTime = MathHelper.Max(0, value); }
        }

        public virtual Rectangle Source
        {
            get { return frames[currentFrame]; }
        }

        public Rectangle Destination
        {
            get
            {
                location = this.Location;

                return new Rectangle(
                    (int)location.X,
                    (int)location.Y,
                    (int)((float)frameWidth*scale),
                    (int)((float)frameHeight*scale));
            }
        }

        public Vector2 Center
        {
            get
            {
                return Location +
                    new Vector2(((float)frameWidth * scale) / 2, ((float)frameHeight * scale) / 2);
            }
        }

        public Rectangle BoundingBoxRect
        {
            get
            {
                location = this.Location;

                return new Rectangle(
                    (int)location.X + BoundingXPadding,
                    (int)location.Y + BoundingYPadding,
                    (int)((float)frameWidth * scale) - (BoundingXPadding * 2),
                    (int)((float)frameHeight * scale) - (BoundingYPadding * 2));
            }
        }

        public bool IsBoxColliding(Rectangle OtherBox)
        {
            return BoundingBoxRect.Intersects(OtherBox);
        }

        public bool IsCircleColliding(Vector2 otherCenter, float otherRadius)
        {
            if (Vector2.Distance(Center, otherCenter) <
                ((int)((float)CollisionRadius*scale) + otherRadius))
                return true;
            else
                return false;
        }
        

        public void AddFrame(Rectangle frameRectangle)
        {
            frames.Add(frameRectangle);
        }

        protected bool dead;
        public bool Dead
        {
            get { return this.dead; }
            set { dead = value; }
        }

        public virtual void Update(GameTime gameTime)
        {
            
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            timeForCurrentFrame += elapsed;

            if (timeForCurrentFrame >= FrameTime)
            {
                currentFrame = (currentFrame + 1) % (frames.Count);
                timeForCurrentFrame = 0.0f;
            }
            /*
            location += (velocity * elapsed);
            */
            //location = ConvertUnits.ToDisplayUnits( body.Position );
            //location = this.Location;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = Center;
            Rectangle dest = new Rectangle((int)position.X, (int)position.Y, (int)((float)frameWidth * scale), (int)((float)frameHeight * scale));


            // Translate position to display unit coordinates
            dest = new Rectangle((int)(ConvertUnits.ToDisplayUnits(body.Position.X)), (int)ConvertUnits.ToDisplayUnits(body.Position.Y), (int)((float)frameWidth * scale), (int)((float)frameHeight * scale));
            

//            spriteBatch.Draw(crate, new Rectangle((int)ConvertUnits.ToDisplayUnits(box.Position.X), (int)ConvertUnits.ToDisplayUnits(box.Position.Y), (int)crate.Width, (int)crate.Height), null, Color.White, box.Rotation, Vector2.Zero, SpriteEffects.None, 0f);

            spriteBatch.Draw(
                Texture,
                dest,
                //Destination,
                Source,
                tintColor,
                body.Rotation,
                origin,
               // 1.0f,
              //SpriteEffects.None,
              flip == false ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0.0f);
        }

    }
}
