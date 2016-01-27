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
    class Heart : Item
    {
        public Light light;
        public override Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)(position.X + (Tile.width / 3)), (int)(position.Y + (Tile.height / 3)), (int)Tile.width / 3, (int)Tile.height / 3);
            }
        }

        public Heart(Vector2 _position, string _name):base(_position, _name)
        {

        }

        public override void Update(GameTime gameTime, Player player)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle drawRect = new Rectangle(Hitbox.X - (int)Managers.Camera.Offset.X, Hitbox.Y - (int)Managers.Camera.Offset.Y, Hitbox.Width, Hitbox.Height);
            spriteBatch.Draw(texture.sprite, drawRect, Color.White);
        }
        public override void Dequipped()
        {

        }
    }
}
