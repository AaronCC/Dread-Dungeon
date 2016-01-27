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
    class Fire : LightSource
    {
        private float frameTime;
        private Point animCurrent;
        private Point animEnd;
        private Point animStart;
        private List<Point> animStarts;
        private List<Point> animEnds;
        public Fire(Vector2 _position, string _name, Light _light)
            : base(_position, _name, _light)
        {
            animStarts = new List<Point>();
            animEnds = new List<Point>();
            animStarts = Managers.AssetManager.GetAnimIndexes(name)[0];
            animEnds = Managers.AssetManager.GetAnimIndexes(name)[1];
            NewAnimation();
        }
        public void NewAnimation()
        {
            animStart = animStarts[0];
            animEnd = animEnds[0];
            animCurrent = animStart;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle drawRect = new Rectangle((int)(position.X - Managers.Camera.Offset.X), (int)(position.Y - Managers.Camera.Offset.Y), Tile.width, Tile.height);
            Rectangle destRect = new Rectangle(animCurrent.X * texture.Width, animCurrent.Y * texture.Height, texture.Width, texture.Height);
            spriteBatch.Draw(texture.sprite, drawRect, destRect, Color.White);
        }
        private void Animate(GameTime gameTime)
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
                    animCurrent = animStart;
                }
                frameTime = 0;
            }
        }
        public override void Update(GameTime gameTime)
        {
            Animate(gameTime);
        }
    }
}
