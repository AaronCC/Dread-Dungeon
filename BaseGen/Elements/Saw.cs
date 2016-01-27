#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace BaseGen.Elements
{
    class Saw : GameObject
    {
        private float angle;
        public Circle hitCircle;
        Rectangle destRect;
        int size;
        public Saw(Vector2 _position, string _name, int _size) : base(_position, _name)
        {
            angle = 0f;
            size = _size;
            hitCircle = new Circle(new Vector2(position.X + ((Tile.width/2) * size), position.Y + ((Tile.width / 2) * size)), (Tile.width/2) * size);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            destRect = new Rectangle((int)(position.X - Managers.Camera.Offset.X), (int)(position.Y - Managers.Camera.Offset.Y),
                  Tile.width * size, Tile.height * size);
            
            //spriteBatch.Draw(Managers.AssetManager.GetTextureAsset("Level0").sprite, destRect, Color.Black);
             
            Vector2 origin = new Vector2(texture.sprite.Width / 2, texture.sprite.Height / 2);
            destRect.X += destRect.Width / 2;
            destRect.Y += destRect.Height / 2;
            spriteBatch.Draw(texture.sprite, destRect, null, Color.White, angle, origin, SpriteEffects.None, 0);
        }
        public override void Update(GameTime gameTime)
        {
            angle += 0.1f;
            if (angle > 100f)
                angle = 0;
            if (CheckHit())
                Managers.Executive.level.Player.SendStateInput(States.StateInput.Hit);
            
        }
        public bool CheckHit()
        {
            foreach (Vector2 point in Managers.Executive.level.Player.circCollPoints)
            {
                if (hitCircle.Contains(point))
                    return true;
            }
            return false;
        }

    }
}
