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
    public enum Direction
    {
        Null = 0,
        Left = -1,
        Right = 1,
    }
    public enum AnimState
    {
        Idle = 0,
        Walking = 1,
        Running = 2,
        Jumping = 3,
        Landing = 4,
        Falling = 5,
        Dead = 6,
        Victory = 7,
        Attacking = 8,
    }
    public enum StateInput
    {
        Null = 0,
        LeftKeyPress = 1,
        RightKeyPress = 2,
        UpKeyPress = 3,
        AnimEnd = 4,
        Hit = 5,
        Victory = 6,
        WallCollision = 7,
        GroundCollision = 8,
        KeyUp = 9,
        UseKeyPress = 10,
        Dead = 11,
        AttackKeyPress = 12,
        LadderCollision = 13,
        DownKeyPress = 14,
    }

    public interface IState
    {
        State HandleInput(Player player, StateInput input);
        void Update(Player player, GameTime gameTime);
    }

    public abstract class State : IState
    {
        public AnimState animState;
        public abstract State HandleInput(Player player, StateInput input);
        public abstract void Update(Player player, GameTime gameTime);
    }
}
