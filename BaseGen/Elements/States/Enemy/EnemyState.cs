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
    public enum EDirection
    {
        Null = 0,
        Left = -1,
        Right = 1,
    }
    public enum EAnimState
    {
        Idle = 0,
        Moving = 1,
        Attacking = 2,
        Dead = 3,
        Roar = 4,
    }
    public enum EStateInput
    {
        Move = 0,
        Stop = 1,
        AnimEnd = 2,
        SwitchDir = 3,
        PlayerCollision = 4,
        Dead = 5,
        Hit = 6,
    }
    public interface EIState
    {
        EnemyState HandleInput(Elements.Enemy enemy, EStateInput input);
        void Update(Elements.Enemy enemy, GameTime gameTime);
    }

    public abstract class EnemyState : EIState
    {
        public EDirection direction;
        public EAnimState animState;
        public abstract EnemyState HandleInput(Elements.Enemy enemy, EStateInput input);
        public abstract void Update(Elements.Enemy enemy, GameTime gameTime);
    }

}
