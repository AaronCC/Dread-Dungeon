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
namespace BaseGen.Elements
{
    class Minotaur : Enemy
    {
        private int oldFollow;
        private float followDistance;
        public Minotaur(Vector2 _position, string _name) : base(_position, _name)
        {
            followDistance = 86f;
            gravity = 0.2f;
            velocity = Vector2.Zero;
            maxVelocity = 5f;
            moveAcceleration = 0.25f;
            maxSpeed = 0.8f;
            SendStateInput(States.Enemy.EStateInput.Move);
            hearts = 2;
        }
        public override void Update(GameTime gameTime)
        {
            player = Managers.Executive.level.Player;
            Behavior(gameTime);
            state.Update(this, gameTime);
            position += velocity;
            base.Update(gameTime);
        }
        private void Behavior(GameTime gameTime)
        {
            HandleCollisions();
        }
        private void HandleCollisions()
        {
            Vector2 next_topLeft = Managers.Extensions.Position_TopLeft(position + velocity);
            Vector2 topLeft = Managers.Extensions.Position_TopLeft(position);
            next_topLeft = new Vector2(next_topLeft.X / Tile.height, next_topLeft.Y / Tile.width);
            topLeft = new Vector2(topLeft.X / Tile.height, topLeft.Y / Tile.width);
            topLeft.Y++;
            //next_topLeft.X += (int)state.direction;
            TileType next_below_collision, forward_collision, below_collision;
            below_collision = Managers.Executive.level.CheckCollision((int)topLeft.X, (int)topLeft.Y);
            next_below_collision = Managers.Executive.level.CheckCollision((int)next_topLeft.X, (int)next_topLeft.Y + 1);
            forward_collision = Managers.Executive.level.CheckCollision((int)next_topLeft.X, (int)next_topLeft.Y);
            int d = 0;
            Vector2 groundDepth = OnGround(new Point((int)topLeft.X, (int)topLeft.Y));
            Vector2 wallDepth = CheckWall(next_topLeft);
            if (below_collision == TileType.Passable)
                ApplyGravity();
            else if ((groundDepth) != Vector2.Zero)
            {
                position.Y += groundDepth.Y;
                velocity.Y = 0;
            }
            if (CheckAttack())
            {
                SendStateInput(States.Enemy.EStateInput.PlayerCollision);
            }
            //else if (wallDepth != Vector2.Zero)
            //{
            //    position += wallDepth;
            //    SendStateInput(States.Enemy.EStateInput.Move);
            //    SendStateInput(States.Enemy.EStateInput.SwitchDir);
            //}
            else if (CheckFalling(next_below_collision) || CheckWall(forward_collision))
            {
                if (CheckFollow() == 0)
                {
                    SendStateInput(States.Enemy.EStateInput.Move);
                    SendStateInput(States.Enemy.EStateInput.SwitchDir);
                }
                else
                    SendStateInput(States.Enemy.EStateInput.Stop);
            }
            else if ((d = CheckFollow()) != 0)
            {
                SendStateInput(States.Enemy.EStateInput.Move);
                ChangeDirection((States.Enemy.EDirection)d);
                if (oldFollow == 0)
                    NewAnimation(States.Enemy.EAnimState.Roar);
            }
            else if (CheckNear() == 1)
                SendStateInput(States.Enemy.EStateInput.Move);
            oldFollow = CheckFollow();
        }
        private void ChangeDirection(States.Enemy.EDirection dir)
        {
            state.direction = dir;
        }
        private bool CheckAttack()
        {

            return Hitbox.Intersects(Managers.Executive.level.Player.Hitbox);
        }
        private bool CheckFalling(TileType below_collision)
        {
            if (!(below_collision == TileType.Impassable || below_collision == TileType.Platform))
                return true;
            return false;
        }
        private Vector2 CheckWall(Vector2 next)
        {
            Vector2 depth;
            if ((depth = Managers.Extensions.GetIntersectionDepth(Hitbox, new Rectangle((int)next.X, (int)next.Y, Tile.width, Tile.height))) != Vector2.Zero)
                return depth;
            return Vector2.Zero;
        }
        private bool CheckWall(TileType forward_collision)
        {
            if (forward_collision == TileType.Impassable)
                return true;
            return false;
        }
        private Vector2 OnGround(Point tile)
        {
            Vector2 depth = Managers.Extensions.GetIntersectionDepth
                (Hitbox, new Rectangle(tile.X * Tile.width, tile.Y * Tile.height, Tile.width, Tile.height));
            return depth;
        }
        private int CheckNear()
        {
            if (Math.Abs(player.Position.X - position.X) < Hitbox.Width / 2 && !CheckAttack())
            {
                SendStateInput(States.Enemy.EStateInput.Stop);
                return 0;
            }
            return 1;
        }
        private int CheckFollow()
        {
            if (CheckNear() == 0)
                return 0;
            if (Vector2.Distance(Managers.Executive.level.Player.Position, Position) < followDistance)
            {
                if (Managers.Executive.level.Player.Position.X < Position.X)
                    return -1;
                else
                    return 1;
            }
            return 0;
        }

    }
}
