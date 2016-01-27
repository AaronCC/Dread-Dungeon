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
    class Spike : TileObject
    {
        public override Rectangle Hitbox
        {
            get
            {
                if (flipV == SpriteEffects.None)
                {
                    if (yOff == 0)
                        return new Rectangle((int)position.X, (int)position.Y + (Tile.height / 2), Tile.width, Tile.height / 2);
                    else if (yOff == 1)
                        return new Rectangle((int)position.X - Tile.width, (int)position.Y, Tile.width / 2, Tile.height);
                    else if (yOff == -1)
                        return new Rectangle((int)position.X + (Tile.width / 2), (int)position.Y - Tile.height, Tile.width / 2, Tile.height);
                    else
                        return new Rectangle((int)position.X, (int)position.Y, Tile.width, Tile.height / 2);
                }
                else if (flipV == SpriteEffects.FlipVertically)
                {
                    return new Rectangle((int)position.X, (int)position.Y, Tile.width, Tile.height / 2);
                }
                else
                    return new Rectangle((int)position.X, (int)position.Y, Tile.width, Tile.height / 2);
            }
        }
        public override Vector2 Position
        {
            get
            {
                return Managers.Extensions.Position_TopLeft(new Vector2(Hitbox.X, Hitbox.Y + (Tile.height / 2)));
            }
        }
        SpriteEffects flipV;
        float flipH;
        int yOff;
        public Spike(Vector2 _position, string _name, bool _flipV, int _flipH)
            : base(_position, _name)
        {
            flipV = _flipV == true ? SpriteEffects.FlipVertically : SpriteEffects.None;
            position = _position;
            if (_flipH == 0)
                flipH = 0;
            if (_flipH == 1)
            {
                position.X += texture.Width;
                flipH = (float)Math.PI / 2;
            }
            if (_flipH == -1)
            {
                position.Y += texture.Height;
                flipH = (float)(3.0 * Math.PI) / 2;
            }
            yOff = _flipH;
            name = _name;
            texture = Managers.AssetManager.GetTextureAsset(name);
        }
        public override void Update(GameTime gameTime)
        {
            if (IsColliding(Managers.Executive.level.Player.Hitbox))
            {
                Managers.Executive.level.Player.SendStateInput(States.StateInput.Hit);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Rectangle startTest = new Rectangle((int)(Hitbox.X - Managers.Camera.Offset.X), (int)(Hitbox.Y - Managers.Camera.Offset.Y), (int)Hitbox.Width, (int)Hitbox.Height);
            //spriteBatch.Draw(Managers.AssetManager.GetTextureAsset("Level0").sprite, startTest, Color.Black);
            Rectangle drawPos = new Rectangle((int)(position.X - Managers.Camera.Offset.X), (int)(position.Y - Managers.Camera.Offset.Y), texture.Width, texture.Height);
            //Rectangle destRect = new Rectangle(0, 0, texture.Width, texture.Height);
            spriteBatch.Draw(texture.sprite, drawPos, null, Color.White, flipH, new Vector2(0, 0), flipV, 1.0f);
        }
    }
}