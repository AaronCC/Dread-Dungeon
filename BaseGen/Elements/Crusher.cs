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
    class Crusher : GameObject
    {
        public enum CrusherType
        {
            vertical = 0,
            horizontalLR = 1,
            horizontalRL = 2,
        }
        private CrusherType cType;

        public bool crushing;
        
        Vector2 startPos;
        private Assets.Texture railTexture;
        private int crushSpeed;
        private int railLength;
        private const float WAIT = 500f;
        private float timer;
        public override Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, Tile.width, Tile.height);
            }
        }

        public Crusher(Vector2 _position, string _name, int _railLength, int type, bool wait) : base(_position, _name)
        {
            cType = (CrusherType)type;
            crushing = wait;
            startPos = _position;
            // railTexture = Managers.AssetManager.GetTextureAsset("Rail");
            railLength = _railLength;
            crushSpeed = 2 * (railLength - 1);
            timer = 0f;
        }
        public override void Update(GameTime gameTime)
        {
            switch (cType)
            {
                case CrusherType.vertical:
                    if (crushing)
                    {
                        position.Y += crushSpeed;
                        if (position.Y - startPos.Y + Tile.height > Tile.height * railLength)
                        {
                            crushing = false;
                        }
                        if (IsColliding(Managers.Executive.level.Player.Hitbox))
                            Managers.Executive.level.Player.SendStateInput(States.StateInput.Hit);
                    }
                    else
                    {
                        if (position.Y <= startPos.Y)
                        {
                            timer += gameTime.ElapsedGameTime.Milliseconds;
                            if (timer >= WAIT)
                            {
                                crushing = true;
                                timer = 0;
                            }
                        }
                        else
                            position.Y -= crushSpeed / 2;
                    }
                    break;
                case CrusherType.horizontalLR:
                    if (crushing)
                    {
                        position.X += crushSpeed;
                        if (position.X - startPos.X + Tile.width > Tile.width * railLength)
                            crushing = false;
                        if (IsColliding(Managers.Executive.level.Player.Hitbox))
                            Managers.Executive.level.Player.SendStateInput(States.StateInput.Hit);
                    }
                    else
                    {
                        if (position.X <= startPos.X)
                        {
                            timer += gameTime.ElapsedGameTime.Milliseconds;
                            if (timer >= WAIT)
                            {
                                crushing = true;
                                timer = 0;
                            }
                        }
                        else
                            position.X -= crushSpeed / 2;
                    }
                    break;
                case CrusherType.horizontalRL:
                    if (crushing)
                    {
                        position.X -= crushSpeed;
                        if (startPos.X - position.X + Tile.width > Tile.width * railLength)
                            crushing = false;
                        if (IsColliding(Managers.Executive.level.Player.Hitbox))
                            Managers.Executive.level.Player.SendStateInput(States.StateInput.Hit);
                    }
                    else
                    {
                        if (position.X >= startPos.X)
                        {
                            timer += gameTime.ElapsedGameTime.Milliseconds;
                            if (timer >= WAIT)
                            {
                                crushing = true;
                                timer = 0;
                            }
                        }
                        else
                            position.X += crushSpeed / 2;
                    }
                    break;
                default:
                    break;
            }


        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Rectangle railRect = new Rectangle((int)(startPos.X - Managers.Camera.Offset.X), (int)(startPos.Y - Managers.Camera.Offset.Y)
            //, Tile.width, Tile.height * railLength);
            Rectangle weightRect = new Rectangle((int)(position.X - Managers.Camera.Offset.X), (int)(position.Y - Managers.Camera.Offset.Y)
                , Tile.width, Tile.height);
            // spriteBatch.Draw(railTexture.sprite, railRect, null, Color.White);
            if (crushing)
                spriteBatch.Draw(texture.sprite, weightRect, null, Color.White);
            else
                spriteBatch.Draw(texture.sprite, weightRect, null, Color.White * 0.5f);
        }
    }
}
