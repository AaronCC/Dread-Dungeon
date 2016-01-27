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

namespace BaseGen.Elements.Items
{
    class TorchItem : Item
    {
        public Light light;
        public Vector2 position;
        private float timer;
        private float intTick;
        LightSource source;

        public TorchItem(Vector2 _position, string _name, Light _light):base(_position, _name)
        {
            name = "Torch";
            position = _position;
            light = new Light(_light.color, _light.intensity, _light.type, _light.position);
            source = new LightSource(position, name, light);
            Managers.Executive.level.AddLightSource(ref source);
            intTick = 300f;
            texture = Managers.AssetManager.GetTextureAsset(name);
        }

        public override void Update(GameTime gameTime, Player player)
        {
            if(source.light.intensity >=0.2f)
            timer += gameTime.ElapsedGameTime.Milliseconds;
            if(timer >= intTick)
            {
                timer = 0;
                source.light.intensity -= 0.01f;
                if (source.light.intensity <= 0.2f)
                    display = false;
            }
            position.X = player.Hitbox.Y / Tile.height;
            position.Y = player.Hitbox.X / Tile.width;
            source.light.position = new Point((int)position.X, (int)position.Y);
        }

        public override void Dequipped()
        {
            Managers.Executive.level.RemoveLightSource(source);
        }
    }
}
