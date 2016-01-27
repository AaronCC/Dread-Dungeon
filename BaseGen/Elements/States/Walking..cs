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
    class Walking : ActionState
    {
       
        public Walking(int _direction)
        {
            animState = AnimState.Walking;
            direction = (Direction)_direction;
        }


        public override State HandleInput(Player player, StateInput input)
        {
            switch (input)
            {
                case StateInput.LeftKeyPress:
                    if (Math.Abs(player.velocity.X) >= (player.maxSpeed * 0.75f))
                        return new Running(-1);
                    direction = Direction.Left;
                    player.flip = SpriteEffects.FlipHorizontally;
                    break;
                case StateInput.RightKeyPress:
                    if (Math.Abs(player.velocity.X) >= (player.maxSpeed * 0.75f))
                        return new Running(1);
                    direction = Direction.Right;
                    player.flip = SpriteEffects.None;
                    break;
                case StateInput.AttackKeyPress:
                    return new Attacking(this);
                case StateInput.AnimEnd:
                    break;
                case StateInput.UpKeyPress:
                    player.Jump(1);
                    return new Jumping((int)direction);
                default:
                    break;
            }
            if (direction == Direction.Null && player.velocity.X == 0)
                return new Idle();
            return base.HandleInput(player, input, this);
        }
    }
}
