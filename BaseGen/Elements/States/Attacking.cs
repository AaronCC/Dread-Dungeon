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
    class Attacking : ActionState
    {
        List<GameObject> attacked;
        public State oldState;
        public Attacking(State old)
        {
            oldState = old;
            animState = AnimState.Attacking;
            attacked = null;
        }

        public override State HandleInput(Player player, StateInput input)
        {
            
            switch(input)
            {
                case StateInput.AnimEnd:
                    oldState.HandleInput(player, StateInput.KeyUp);
                    return oldState;
                default:
                    break;
            }
            return base.HandleInput(player, input, this);
        }

        public override void Update(Player player, GameTime gameTime)
        {
            if (attacked == null)
            {
                attacked = Managers.Executive.level.AllCollide(player.Hitbox_ex);
                foreach(GameObject obj in attacked)
                {
                    obj.Attacked();
                }
            }
            switch (direction)
            {
                case Direction.Left:
                    if (player.velocity.X > 0)
                        player.MoveX(-3);
                    else
                    {
                        player.MoveX(-1);
                    }
                    player.ClampX(player.maxSpeed, -1);
                    player.flip = SpriteEffects.FlipHorizontally;
                    break;
                case Direction.Right:
                    if (player.velocity.X < 0)
                        player.MoveX(3);
                    else
                    {
                        player.MoveX(1);
                    }
                    player.ClampX(player.maxSpeed, 1);
                    player.flip = SpriteEffects.None;
                    break;
                case Direction.Null:
                    player.ApplyFriction(2);
                    break;
                default:
                    break;
            }
            player.ApplyGravity(1);
        }
    }
}
