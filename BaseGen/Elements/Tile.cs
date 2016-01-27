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
        public enum TileType
        {
            Impassable = 0,

            Passable = 1,

            Platform = 2,

            Polynomial = 3,

            Victory = 4,

            LightSource = 5,

            BPlatform = 6,

            Obstacle = 7,

            Ladder = 8,
        }
        public struct Tile
        {
            public const int width = 32;
            public const int height = 32;
            public Assets.Texture texture;
            public TileType type;
            public string name;
            public Tile(Assets.Texture _texture, TileType _type, string _name)
            {
                name = _name;
                texture = _texture;
                type = _type;
            }
        }
}
