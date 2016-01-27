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
    class Idle : ActionState
    {
        public Idle()
        {
            animState = AnimState.Idle;
        }

        public override State HandleInput(Player player, StateInput input)
        {
            switch(input)
            {
                case StateInput.LeftKeyPress:
                    return new Walking(-1);
                case StateInput.RightKeyPress:
                    return new Walking(1);
                case StateInput.UpKeyPress:
                    player.Jump(1);
                    return new Jumping(0);
                case StateInput.Hit:
                    player.TakeHit(1);
                    break;
                case StateInput.AttackKeyPress:
                    return new Attacking(this);
                default:
                    break;
            }
            return base.HandleInput(player, input, this);
        }

        public override void Update(Player player, GameTime gameTime)
        {
            player.ApplyFriction(2);
            player.ApplyGravity(1);
        }
        
    }
}
