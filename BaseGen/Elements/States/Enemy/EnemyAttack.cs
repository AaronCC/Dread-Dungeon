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
    class EnemyAttack : EnemyActionState
    {
        private float hitTime;
        private float timer;
        public EnemyAttack(EDirection _direction)
        {
            direction = _direction;
            animState = EAnimState.Attacking;
            timer = 0;
            hitTime = 500;
        }
        public override EnemyState HandleInput(Elements.Enemy enemy, EStateInput input)
        {
            switch (input)
            {
                case EStateInput.AnimEnd:
                    return new EnemyIdle();

                case EStateInput.Dead:
                    return new EnemyDead();
                default:
                    break;
            }
            return this;
        }
        public override void Update(Elements.Enemy enemy, GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds;
            if (timer >= hitTime)
            {
                if (enemy.Hitbox.Intersects(Managers.Executive.level.Player.Hitbox))
                    Managers.Executive.level.Player.SendStateInput(StateInput.Hit);
                timer = 0;
            }
        }
    }
}
