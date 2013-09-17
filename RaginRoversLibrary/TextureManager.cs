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

namespace RaginRoversLibrary
{
    public class TextureManager
    {
        Dictionary<string, Texture2D> textures;
        ContentManager Content;

        public TextureManager(ContentManager Content)
        {
            this.Content = Content;
            textures = new Dictionary<string, Texture2D>();
        }

        public void LoadTexture(string assetname)
        {
            this.LoadTexture(assetname, assetname);
        }

        public void LoadTexture(string assetname, string assetkey)
        {
            if (!textures.ContainsKey(assetname))
            {
                textures.Add(assetkey, Content.Load<Texture2D>(assetname));
            }
        }

        public Texture2D Texture (string assetkey)
        {
            return textures.ContainsKey(assetkey) ? textures[assetkey] : null;
        }
    }
}
