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
    class EnemyDead : EnemyState
    {
        public EnemyDead()
        {
            animState = EAnimState.Dead;
        }
        public override EnemyState HandleInput(Elements.Enemy enemy, EStateInput input)
        {
            if (input == EStateInput.AnimEnd)
                Managers.Executive.level.removeQueue.Enqueue(enemy);
            return this;
        }

        public override void Update(Elements.Enemy enemy, GameTime gameTime)
        {
            enemy.velocity = Vector2.Zero;
        }
    }
}
