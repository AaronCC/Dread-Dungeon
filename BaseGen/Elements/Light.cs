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
    public enum LightType
    {
        Point = 0,
        Directional = 1,
        Null = 2,
    }
    public struct Light
    {
        public Color color;
        public float intensity;
        public LightType type;
        public Point position;
        public Light(Color _color, float _intensity, LightType _type, Point _position)
        {
            color = _color;
            intensity = _intensity;
            type = _type;
            position = _position;
        }
    }
}
