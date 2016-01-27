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

namespace BaseGen.Elements.Items
{
    public class Item : GameObject
    {
        public bool display;
        public Item(Vector2 _position, string _name):base(_position, _name)
        {
            display = true;
        }
        public virtual void Update(GameTime gameTime, Player player)
        {

        }
        public override Item ToItem()
        {
            return this;
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 _position)
        {
            Rectangle drawRect = new Rectangle((int)(_position.X + Tile.width / 2 - texture.Width / 2), (int)(_position.Y + Tile.height / 2 - texture.Height / 2),
               texture.Width, texture.Height);
            spriteBatch.Draw(texture.sprite, drawRect, Color.White);
        }
        public virtual void Dequipped()
        {

        }
    }
}
