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


namespace BaseGen.Elements.States.Enemy
{
    public class EnemyActionState : EnemyState
    {


        public override EnemyState HandleInput(Elements.Enemy enemy, EStateInput input)
        {
            throw new NotImplementedException();
        }

        public EnemyState HandleInput(Elements.Enemy enemy, EStateInput input, EnemyState state)
        {
            switch (input)
            {
                case EStateInput.Move:
                    return new EnemyRunning(direction);
                case EStateInput.SwitchDir:
                    if (direction != 0)
                    {
                        enemy.velocity.X = 0;
                        direction = (EDirection)((int)direction * -1);
                    }
                    return state;
                case EStateInput.Dead:
                    return new EnemyDead();
                case EStateInput.Hit:
                    break;
                case EStateInput.AnimEnd:

                    break;
                case EStateInput.PlayerCollision:
                    enemy.velocity = Vector2.Zero;
                    return new EnemyAttack(direction);
                case EStateInput.Stop:
                    if (state.GetType() != typeof(EnemyIdle))
                        return new EnemyIdle();
                    return this;
                default:
                    break;
            }
            return state;
        }
        public override void Update(Elements.Enemy enemy, GameTime gameTime)
        {
            //enemy.ApplyGravity();
            switch (direction)
            {
                case EDirection.Left:
                    enemy.MoveX((int)direction);
                    enemy.ClampX((int)direction);
                    enemy.flip = SpriteEffects.None;
                    break;
                case EDirection.Right:
                    enemy.MoveX((int)direction);
                    enemy.ClampX((int)direction);
                    enemy.flip = SpriteEffects.FlipHorizontally;
                    break;
                default:
                    break;
            }

        }
    }
}
