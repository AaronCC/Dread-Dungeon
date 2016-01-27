#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
#endregion


namespace BaseGen.Elements
{

    class Torch : LightSource
    {
        public Torch(Vector2 _position, string _name)
            : base(_position, _name)
        {
            display = true;
            light = new Light(Color.White, 0, LightType.Null, Point.Zero);
        }
        public Torch()
        {
            display = true;
            light = new Light(Color.White, 0, LightType.Null, Point.Zero);
        }
        public Torch(Vector2 _position, string _name, Light _light)
            : base(_position, _name)
        {
            display = true;
            light = _light;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!display)
                return;

            Rectangle drawRect = new Rectangle((int)(position.X - Managers.Camera.Offset.X), (int)(position.Y - Managers.Camera.Offset.Y),
                    Tile.width, Tile.height);
            spriteBatch.Draw(texture.sprite, drawRect, Color.White);
        }
        public override void Update(GameTime gameTime)
        {
           
        }
        
        public override void Draw(SpriteBatch spriteBatch, Vector2 _position)
        {
        }
        public override Items.Item ToItem()
        {
            light.intensity = 1f;
            return new Items.TorchItem(position, name, light);
        }
    }
}
