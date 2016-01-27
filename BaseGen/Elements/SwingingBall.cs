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
    class SwingingBall : GameObject
    {
        float delay;

        float radians;
        float chainrads;
        private float angle;
        public Circle hitCircle;
        Rectangle destRect;
        Assets.Texture chainTexture;
        public SwingingBall(Vector2 _position, string _name, float _delay) : base(_position, _name)
        {
            delay = _delay;
            angle = 0f;
            hitCircle = new Circle(new Vector2(position.X + ((Tile.width / 2)), position.Y + ((Tile.width / 2))), (Tile.width / 2));
            chainTexture = Managers.AssetManager.GetTextureAsset("Chain");
            radians = (angle / 180f) * (float)Math.PI;
            chainrads = ((angle - 90) / 180f) * (float)Math.PI;
            Vector2 aposition = new Vector2((int)(position.X) + Tile.width / 2, (int)(position.Y) + Tile.height / 2);
            Vector2 rot = new Vector2((Tile.width * 1.1f) * (float)Math.Cos(radians), (Tile.height * 1.1f) * (float)Math.Sin(radians));
            Vector2 origin = new Vector2(0, 0);
            aposition.X += (int)rot.X;
            aposition.Y += (int)rot.Y;
            hitCircle = new Circle(new Vector2(
                aposition.X
                , aposition.Y)
                , (Tile.width / 2) * 0.8f);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (delay > 0 && Main.GetState() != Main.GameState.Editor)
                return;
            destRect = new Rectangle((int)(hitCircle.Center.X - hitCircle.Radius - Managers.Camera.Offset.X),
                (int)(hitCircle.Center.Y - hitCircle.Radius - Managers.Camera.Offset.Y),
                (int)hitCircle.Radius * 2, (int)hitCircle.Radius * 2);
            Rectangle chainRect = new Rectangle((int)(position.X - Managers.Camera.Offset.X), (int)(position.Y - Managers.Camera.Offset.Y), Tile.width, Tile.height);
            //spriteBatch.Draw(Managers.AssetManager.GetTextureAsset("Level0").sprite, destRect, Color.Black);
            //spriteBatch.Draw(texture.sprite, destRect, null, Color.White);
            //destRect = new Rectangle((int)(position.X - Managers.Camera.Offset.X),(int)(position.X - Managers.Camera.Offset.X), Tile.width, Tile.height * 2);
            Vector2 origin = new Vector2(texture.sprite.Width / 2, texture.sprite.Height / 2);
            Vector2 chainOrigin = new Vector2(chainTexture.sprite.Width / 2, 0);
            destRect.X += destRect.Width / 2;
            destRect.Y += destRect.Height / 2;
            chainRect.X += chainRect.Width / 2;
            chainRect.Y += chainRect.Height / 2;
            spriteBatch.Draw(chainTexture.sprite, chainRect, null, Color.White, chainrads, chainOrigin, SpriteEffects.None, 0);
            spriteBatch.Draw(texture.sprite, destRect, null, Color.White, radians, origin, SpriteEffects.None, 0);
        }
        public override void Update(GameTime gameTime)
        {
            if (delay > 0)
            {
                delay -= gameTime.ElapsedGameTime.Milliseconds;
                return;
            }
            angle += 3f;
            if (angle >= 360)
                angle = 0;
            radians = (angle / 180f) * (float)Math.PI;
            chainrads = ((angle - 90) / 180f) * (float)Math.PI;
            Vector2 _position = new Vector2((int)(position.X) + Tile.width / 2, (int)(position.Y) + Tile.height / 2);
            Vector2 rot = new Vector2((Tile.width * 1.1f) * (float)Math.Cos(radians), (Tile.height * 1.1f) * (float)Math.Sin(radians));
            Vector2 origin = new Vector2(0, 0);
            _position.X += (int)rot.X;
            _position.Y += (int)rot.Y;
            hitCircle = new Circle(new Vector2(
                _position.X
                , _position.Y)
                , (Tile.width / 2) * 0.8f);
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
