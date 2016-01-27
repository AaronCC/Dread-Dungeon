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
    class TileObject : GameObject
    {
        public TileObject() { }

        public Vector2 index;
        public TileObject(Vector2 _position, string _name):base(_position,_name)
        {
            index = new Vector2(position.X / Tile.width, position.Y / Tile.height);
        }

        public override void Update(GameTime gameTime)
        {

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle drawRect = new Rectangle((int)(position.X - Managers.Camera.Offset.X), (int)(position.Y - Managers.Camera.Offset.Y),
                texture.Width, texture.Height);
            spriteBatch.Draw(texture.sprite, drawRect, Color.White);
        }

        public override void Attacked()
        {

        }

    }
}
