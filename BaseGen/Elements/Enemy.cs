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
    public class Enemy : GameObject
    {

        protected float gravity;
        protected float maxSpeed;
        protected float moveAcceleration;
        public Vector2 velocity;
        protected float maxVelocity;
        protected float frameTime;
        protected Point animCurrent;
        protected Point animEnd;
        protected Point animStart;
        protected List<Point> animStarts;
        protected List<Point> animEnds;
        protected States.Enemy.EnemyState state;
        protected Player player;
        protected Assets.Texture heart;
        public SpriteEffects flip;
        protected int hearts;
        protected float knockbackSpeed;
        float colorTimer;
        float endTime;

        public override Vector2 Position
        {
            get
            {
                return new Vector2(Hitbox.X, Hitbox.Y);
            }
        }
        public override Rectangle Hitbox
        {
            get
            {
                int thinX = 0;
                int thinY = 0;
                //return new Rectangle((int)position.X - (texture.Width / 2) + (thinX / 2), (int)position.Y - (texture.Height) + thinY, texture.Width - thinX, texture.Height - thinY);
                Vector2 drawPos = new Vector2(position.X - ((texture.Width / 2) / 2), position.Y - (texture.Height / 2));
                return new Rectangle((int)(drawPos.X) + (thinX / 2), (int)(drawPos.Y) + (thinY / 2), (texture.Width / 2) - (thinX), (texture.Height / 2) - thinY);
            }
        }
        public Enemy(Vector2 _position, string _name) : base(_position, _name)
        {
            gravity = 0.2f;
            endTime = 300f;
            colorTimer = endTime + 1;
            state = new States.Enemy.EnemyIdle();
            animStarts = Managers.AssetManager.GetAnimIndexes(name)[0];
            animEnds = Managers.AssetManager.GetAnimIndexes(name)[1];
            animStarts = new List<Point>();
            animEnds = new List<Point>();
            animStarts = Managers.AssetManager.GetAnimIndexes(name)[0];
            animEnds = Managers.AssetManager.GetAnimIndexes(name)[1];
            NewAnimation(state.animState);
            flip = SpriteEffects.None;
            knockbackSpeed = 2f;
            heart = Managers.AssetManager.GetTextureAsset("Heart");
        }
        public void TakeHit(int damage)
        {
            colorTimer = 0f;
            hearts--;
            if (hearts == 0)
                SendStateInput(States.Enemy.EStateInput.Dead);
        }
        public override void Attacked()
        {
            TakeHit(1);
            SendStateInput(States.Enemy.EStateInput.Hit);
        }

        public void ApplyGravity()
        {
            velocity.Y += gravity;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Vector2 topLeft = Managers.Extensions.Position_TopLeft(position);

            //Vector2 topLeft = new Vector2(Hitbox.X, Hitbox.Y);
            //Rectangle startTest = new Rectangle((int)(topLeft.X - Managers.Camera.Offset.X), (int)(topLeft.Y - Managers.Camera.Offset.Y), Hitbox.Width, Hitbox.Height);
            //spriteBatch.Draw(Managers.AssetManager.GetTextureAsset("Level0").sprite, startTest, Color.Black);

            Vector2 drawPos = new Vector2(position.X - ((texture.Width / 2) / 2), position.Y - (texture.Height / 2));

            Rectangle destRect = new Rectangle(animCurrent.X * texture.Width, animCurrent.Y * texture.Height, texture.Width, texture.Height);
            Rectangle drawRect = new Rectangle((int)(drawPos.X - Managers.Camera.Offset.X), (int)(drawPos.Y - Managers.Camera.Offset.Y), texture.Width / 2, texture.Height / 2);

            for (int x = 0; x < hearts; x++)
            {

                Rectangle heartRect = new Rectangle(drawRect.X + 5 * x, drawRect.Y, 5, 5);
                spriteBatch.Draw(heart.sprite, heartRect, Color.White);
            }
            if (colorTimer <= endTime)
                spriteBatch.Draw(texture.sprite, drawRect, destRect, Color.IndianRed, 0.0f, new Vector2(0, 0), flip, 1.0f);
            else
                spriteBatch.Draw(texture.sprite, drawRect, destRect, Color.White, 0.0f, new Vector2(0, 0), flip, 1.0f);
        }
        public override void Update(GameTime gameTime)
        {
            if (colorTimer <= endTime)
                colorTimer += gameTime.ElapsedGameTime.Milliseconds;
            player = Managers.Executive.level.Player;
            Animate(gameTime);
            base.Update(gameTime);
        }
        public void NewAnimation(States.Enemy.EAnimState anim_state)
        {
            animStart = animStarts[(int)anim_state];
            animEnd = animEnds[(int)anim_state];
            animCurrent = animStart;
        }
        protected void Animate(GameTime gameTime)
        {
            frameTime += gameTime.ElapsedGameTime.Milliseconds;
            if (frameTime >= texture.mpf)
            {
                if (animCurrent != animEnd)
                {
                    if (animCurrent.X < texture.cols - 1)
                    {
                        animCurrent.X++;
                    }
                    else
                    {
                        animCurrent.X = 0;
                        if (animCurrent.Y < texture.rows - 1)
                        {
                            animCurrent.Y++;
                        }
                    }
                }
                else if (animCurrent == animEnd)
                {
                    if (state.GetType() != typeof(States.Enemy.EnemyDead))
                        animCurrent = animStart;
                    SendStateInput(States.Enemy.EStateInput.AnimEnd);
                }
                frameTime = 0;
            }
        }

        public void SendStateInput(States.Enemy.EStateInput input)
        {
            if (input == States.Enemy.EStateInput.Stop)
                velocity = Vector2.Zero;
            States.Enemy.EnemyState newState = state.HandleInput(this, input);
            if (state.GetType() != newState.GetType())
            {
                state = state.HandleInput(this, input);
                NewAnimation(state.animState);
            }
        }
        public void MoveX(int direction)
        {
            velocity.X += direction * moveAcceleration;
            if (Math.Abs(velocity.X) < Math.Abs(direction) * moveAcceleration)
            {
                velocity.X = 0;
            }
        }
        public void ClampX(int direction)
        {
            if (Math.Abs(velocity.X) > maxSpeed)
                velocity.X = maxSpeed * direction;
        }
    }
}
