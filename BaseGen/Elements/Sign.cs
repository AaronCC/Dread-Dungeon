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
    class Sign : GameObject
    {
        string text;

        public Sign(Vector2 _position, string _name, string _text)
            : base(_position, _name)
        {
            text = _text;
        }
        public override void Update(GameTime gameTime)
        {

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle drawRect = new Rectangle((int)(position.X - Managers.Camera.Offset.X), (int)(position.Y - Managers.Camera.Offset.Y),
                Tile.width,Tile.height);
            spriteBatch.Draw(texture.sprite, drawRect, Color.White);
            if (Main.GetState() != Main.GameState.Editor && Managers.Executive.level.Player != null)
                if (IsColliding(Managers.Executive.level.Player.Hitbox))
                {
                    spriteBatch.DrawString(Managers.Executive.level.Font, text, new Vector2(drawRect.X - (3 * text.Length), drawRect.Y + texture.Height), Color.White);
                }
        }
    }
}
