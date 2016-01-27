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
    public class LightSource : GameObject
    {
        public bool display;
        public Light light;
        public LightSource()
        {
            display = true;
            light = new Light(Color.White, 0, LightType.Null, Point.Zero);
        }
        public LightSource(Vector2 _position, string _name)
            : base(_position, _name)
        {
            display = true;
            light = new Light(Color.White, 0, LightType.Null, Point.Zero);
        }
        public LightSource(Vector2 _position, string _name, Light _light)
            : base(_position, _name)
        {
            display = true;
            light = _light;

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle drawRect = new Rectangle((int)(position.X - Managers.Camera.Offset.X), (int)(position.Y - Managers.Camera.Offset.Y),
               Tile.width, Tile.height);
            spriteBatch.Draw(texture.sprite, drawRect, Color.White);
        }
      
      
      
    }
}
