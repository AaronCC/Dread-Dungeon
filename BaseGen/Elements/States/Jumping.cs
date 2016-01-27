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
    class Jumping : ActionState
    {
        public Jumping(int _direction)
        {
            animState = AnimState.Jumping;
            direction = (Direction)_direction;
        }
        public override State HandleInput(Player player, StateInput input)
        {
            switch (input)
            {
                case StateInput.KeyUp:
                    if (Managers.User.kState.IsKeyUp(Keys.Up))
                        return new Falling(direction);
                    break;
                case StateInput.UpKeyPress:
                    return this;
                case StateInput.AnimEnd:
                    return new Falling(direction);
                case StateInput.WallCollision:
                    return base.HandleInput(player, StateInput.Null, this);
                case StateInput.GroundCollision:
                    return new Landing();
                default:
                    break;
            }
            return base.HandleInput(player, input, this);
        }

        public override void Update(Player player, GameTime gameTime)
        {

            base.Update(player, gameTime);
            if (Managers.User.kState.IsKeyUp(Keys.Up))
            {
                if (player.velocity.Y < 0)
                    player.ApplyGravity(1);
                else
                    player.SendStateInput(StateInput.AnimEnd);
            }
        }
    }
}
