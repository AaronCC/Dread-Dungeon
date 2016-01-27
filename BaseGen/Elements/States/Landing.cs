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
    class Landing : ActionState
    {
        public Landing()
        {
            animState = AnimState.Landing;
        }
        public override State HandleInput(Player player, StateInput input)
        {
            switch (input)
            {
                case StateInput.UpKeyPress:
                    player.Jump(1);
                    return new Jumping(0);
                case StateInput.AnimEnd:
                    if (player.velocity.X > 9)
                        return new Running(0);
                    else if (player.velocity.X != 0)
                        return new Walking(0);
                    else
                        return new Idle();
                case StateInput.AttackKeyPress:
                    return new Attacking(this);
                default:
                    break;
            }
            return base.HandleInput(player, input, this);
        }
    }
}
