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
    class Climbing : ActionState
    {
        public Climbing()
        {
            animState = AnimState.Walking;
        }
        public enum HorizDir
        {
            Null = 0,
            Left = -1,
            Right = 1,
        }
        public enum VertDir
        {
            Null = 0,
            Up = -2,
            Down = 2,
        }
        private HorizDir hDir;
        private VertDir vDir;
        public override State HandleInput(Player player, StateInput input)
        {
            if (input == StateInput.KeyUp)
            {
                hDir = HorizDir.Null;
                vDir = VertDir.Null;
            }
            switch (input)
            {
                case StateInput.LeftKeyPress:
                    hDir = HorizDir.Left;
                    break;
                case StateInput.RightKeyPress:
                    hDir = HorizDir.Right;
                    break;
                case StateInput.UpKeyPress:
                    vDir = VertDir.Up;
                    break;
                case StateInput.DownKeyPress:
                    vDir = VertDir.Down;
                    break;
                case StateInput.Hit:
                    player.TakeHit(1);
                    break;
                case StateInput.Null:
                    return new Idle();
                default:
                    break;
            }
            if (player.velocity == Vector2.Zero)
                animState = AnimState.Idle;
            else
                animState = AnimState.Walking;
            return base.HandleInput(player, input, this);
        }

        public override void Update(Player player, GameTime gameTime)
        {
            switch (hDir)
            {
                case HorizDir.Null:
                    player.velocity.X = 0;
                    break;
                case HorizDir.Left:
                    player.MoveX(-1);
                    player.flip = SpriteEffects.FlipHorizontally;
                    player.ClampX(player.maxSpeed / 2, -1);
                    break;
                case HorizDir.Right:
                    player.MoveX(1);
                    player.flip = SpriteEffects.None;
                    player.ClampX(player.maxSpeed / 2, 1);
                    break;
                default:
                    break;
            }
            switch (vDir)
            {
                case VertDir.Null:
                    player.velocity.Y = 0;
                    break;
                case VertDir.Up:
                    player.MoveY(-1);
                    player.ClampY(player.maxSpeed / 2, -1);
                    break;
                case VertDir.Down:
                    player.MoveY(1);
                    player.ClampY(player.maxSpeed / 2, 1);
                    break;
                default:
                    break;
            }

        }
    }
}
