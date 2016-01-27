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
    public class EnemyIdle : EnemyActionState
    {
        public EnemyIdle()
        {
            animState = EAnimState.Idle;
            direction = EDirection.Left;
        }
        public override EnemyState HandleInput(Elements.Enemy enemy, EStateInput input)
        {
            switch(input)
            {
                case EStateInput.AnimEnd:
                    animState = EAnimState.Idle;
                    break;
                default:
                    break;
            }
            return base.HandleInput(enemy, input, this);
        }
        public override void Update(Elements.Enemy enemy, GameTime gameTime)
        {

        }
    }
}
