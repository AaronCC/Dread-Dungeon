#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
#endregion
namespace BaseGen.Menus.Buttons
{
    public class Button
    {
        public Point size;
        public bool enabled = true;
        public bool flagged = false;
        public Vector2 position;
        public Rectangle HitBox
        {
            get { return new Rectangle((int)position.X, (int)position.Y, size.X, size.Y); }
        }
        private Rectangle hitBox;
        public string Name
        {
            get { return name; }
        }
        private string name;
        public Assets.Texture Texture
        {
            get { return texture; }
        }
        private Assets.Texture texture;

        public Button(Vector2 _position, string _name)
        {
            size = new Point(240, 50);
            //enabled = true;
            name = _name;
            position = _position;
            texture = Managers.AssetManager.GetTextureAsset(name);
            hitBox = new Rectangle((int)position.X, (int)position.Y, texture.sprite.Width, texture.sprite.Height);
        }
        public virtual void Update() { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
        public virtual void ClickEvent()
        {
            Managers.Executive.LoadLevel(0);
        }
    }
}
