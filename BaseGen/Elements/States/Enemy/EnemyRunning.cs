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
    class EnemyRunning : EnemyActionState
    {
        public EnemyRunning(EDirection _direction)
        {
            direction = _direction;
            animState = EAnimState.Moving;
        }
        public override EnemyState HandleInput(Elements.Enemy enemy, EStateInput input)
        {
            switch(input)
            {
                case EStateInput.AnimEnd:
                    animState = EAnimState.Moving;
                    break;
                default:

                    break;
            }
            return base.HandleInput(enemy, input, this);
        }
       
    }
}
