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
    public class ActionState : State
    {
        public override State HandleInput(Player player, StateInput input)
        {
            throw new NotImplementedException();
        }
        protected Direction direction;
        public State HandleInput(Player player, StateInput input, ActionState state)
        {
            if (input == StateInput.KeyUp)
                state.direction = Direction.Null;
            switch (input)
            {
                case StateInput.LeftKeyPress:
                    state.direction = Direction.Left;
                    break;
                case StateInput.RightKeyPress:
                    state.direction = Direction.Right;
                    break;
                case StateInput.Hit:
                    return player.TakeHit(1);
                case StateInput.GroundCollision:
                    player.velocity.Y = 0;
                    break;
                case StateInput.WallCollision:
                    player.velocity.X = 0;
                    break;
                case StateInput.Victory:
                    return new Victory();
                case StateInput.LadderCollision:
                    if (Managers.User.kState.IsKeyDown(Keys.Up) || Math.Abs(player.velocity.Y) > (player.gravityAcceleration * 2))
                        return new Climbing();
                    break;
                default:
                    break;
            }
            return state;
        }

        public override void Update(Player player, GameTime gameTime)
        {
            switch (direction)
            {
                case Direction.Left:
                    if (player.velocity.X > 0)
                        player.MoveX(-4);
                    else
                    {
                        player.MoveX(-1);
                        player.ClampX(player.maxSpeed, -1);
                    }
                    player.flip = SpriteEffects.FlipHorizontally;
                    break;
                case Direction.Right:
                    if (player.velocity.X < 0)
                        player.MoveX(4);
                    else
                    {
                        player.MoveX(1);
                        player.ClampX(player.maxSpeed, 1);
                    }
                    player.flip = SpriteEffects.None;
                    break;
                case Direction.Null:
                    player.ApplyFriction(3);
                    break;
                default:
                    break;
            }
            player.ApplyGravity(1);
        }
    }
}
