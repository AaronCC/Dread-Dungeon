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
    class Falling : ActionState
    {
        public Falling(Direction _direction)
        {
            animState = AnimState.Falling;
            direction = _direction;
            
        }
        public override State HandleInput(Player player, StateInput input)
        {
            switch(input)
            {
                case StateInput.LeftKeyPress:
                    direction = Direction.Left;
                    break;
                case StateInput.RightKeyPress:
                    direction = Direction.Right;
                    break;
                case StateInput.GroundCollision:

                    return new Landing();
                default:
                    break;
            }
            return base.HandleInput(player, input, this);
        }
    }
}
