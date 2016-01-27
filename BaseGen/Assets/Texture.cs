#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace BaseGen.Assets
{
    public class Texture
    {
        public Texture2D sprite { get; set; }
        public int rows { get; set; }
        public int cols { get; set; }
        public int mpf { get; set; }

        public string name;

        public int Width
        {
            get { return sprite.Width / cols; }
        }
        public int Height
        {
            get { return sprite.Height / rows; }
        }

        public Texture(string _name, Texture2D _sound, int _rows, int _cols, int _mpf)
        {
            name = _name;
            sprite = _sound;
            rows = _rows;
            cols = _cols;
            mpf = _mpf;
        }
    }
}
