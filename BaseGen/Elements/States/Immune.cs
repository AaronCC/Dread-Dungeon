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
namespace BaseGen.Elements.States
{
    class Immune : EffectState
    {
        private bool blink;
        public float blinkTimer;
        public const float BLINK_CLOCK = 100f;
        public float immuneTimer;
        public float IMMUNE_CLOCK = 2000f;
        public Immune()
        {
            blink = false;
            blinkTimer = 0;
            immuneTimer = 0;
        }
        public override State HandleInput(Player player, StateInput input)
        {
            throw new NotImplementedException();
        }

        public override void Update(Player player, GameTime gameTime)
        {
            if (immuneTimer == 0)
            {
                player.Jump(0.4f);
                player.velocity.X = 0;
            }
                
            immuneTimer += gameTime.ElapsedGameTime.Milliseconds;
            blinkTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (blinkTimer >= BLINK_CLOCK)
            {
                blink = !blink;
                blinkTimer = 0f;
            }
            if (immuneTimer >= IMMUNE_CLOCK)
            {
                player.effectState = null;
            }
        }

        public override void DrawPlayer(Player player, SpriteBatch spriteBatch, Rectangle drawRect, Rectangle destRect, SpriteEffects flip)
        {
            switch (blink)
            {
                case false:
                    spriteBatch.Draw(player.texture.sprite, drawRect, destRect, Color.White * 1f, 0.0f, new Vector2(0, 0), flip, 1.0f);
                    break;
                case true:

                    spriteBatch.Draw(player.texture.sprite, drawRect, destRect, Color.White * 0.5f, 0.0f, new Vector2(0, 0), flip, 1.0f);
                    break;
                default:
                    break;
            }

        }
    }
}
