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
    class BreakBlock : TileObject
    {
        public BreakBlock() { }
        private float dest;
        public Vector2 posIndex;
        public BreakBlock(Vector2 _position, string _name):base(_position,_name)
        {
            dest = 1f;
            posIndex = new Vector2(position.X / Tile.width, position.Y / Tile.height);
        }

        public override void Update(GameTime gameTime)
        {
            
            if(dest <= 0)
            {
                Managers.Executive.level.removeQueue.Enqueue(this);
                Managers.Executive.level.ChangeTileType(new Point((int)posIndex.X, (int)posIndex.Y), TileType.Passable);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle drawRect = new Rectangle((int)(position.X - Managers.Camera.Offset.X), (int)(position.Y - Managers.Camera.Offset.Y),
                texture.Width, texture.Height);
            spriteBatch.Draw(texture.sprite, drawRect, Color.White * dest);
        }

        public override void Attacked()
        {
            dest -= 0.5f;
        }
    }
}
