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
    class Running : ActionState
    {
        public Running(int _direction)
        {
            animState = AnimState.Running;
            direction = (Direction)_direction;
        }

        public override State HandleInput(Player player, StateInput input)
        {
            switch (input)
            {
                case StateInput.UpKeyPress:
                    player.Jump(1);
                    return new Jumping((int)direction);
                case StateInput.AnimEnd:
                    break;
                case StateInput.AttackKeyPress:
                    return new Attacking(this);
                default:
                    break;
            }
            if (direction == Direction.Null && player.velocity.X == 0)
            {
                return new Idle();
            }

            if (player.velocity.Y > player.gravityAcceleration * 4)
                return new Falling(direction);

            return base.HandleInput(player, input, this);
        }

    }
}
