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
    public class EffectState : ActionState
    {
        public virtual void DrawPlayer(Player player, SpriteBatch spriteBatch, Rectangle drawRect, Rectangle destRect, SpriteEffects flip)
        {
                
        }

        public override State HandleInput(Player player, StateInput input)
        {
            throw new NotImplementedException();
        }

        public override void Update(Player player, GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
