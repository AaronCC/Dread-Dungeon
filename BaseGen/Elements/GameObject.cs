#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace BaseGen.Elements
{
    public class GameObject
    {
        public virtual Rectangle Hitbox
        {
            get { return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); }
        }
        public virtual Vector2 Position
        {
            get { return position; }
        }
        protected Vector2 position;
        public string Name
        {
            get { return name; }
        }
        protected string name;
        public Assets.Texture Texture
        {
            get { return texture; }
        }
        protected Assets.Texture texture;
        public GameObject()
        {

        }
        public GameObject(Vector2 _position, string _name)
        {
            position = _position;
            name = _name;
            texture = Managers.AssetManager.GetTextureAsset(name);
        }
        public virtual void Update(GameTime gameTime)
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 _position)
        {

        }
        public bool IsColliding(Rectangle Hitbox)
        {
            return Hitbox.Intersects(this.Hitbox);
        }
        public virtual void Attacked()
        {

        }
        public virtual Items.Item ToItem()
        {
            return null;
        }
    }
}
