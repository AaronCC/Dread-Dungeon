#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace BaseGen.Managers
{
    public static class ScreenManager
    {
        public static Vector2 virtualScreen { get; set; }
        public static Vector2 actualScreen { get; set; }
        public static Vector3 scalingFactor { get; set; }
        public static Matrix scale { get; set; }
        public static void Initialize(Vector2 _virtual, Vector2 _actual)
        {
            virtualScreen = _virtual;
            actualScreen = _actual;
            CalcScaleFactor();
        }
        private static void CalcScaleFactor()
        {
            float widthScale = (float)actualScreen.X / virtualScreen.X;
            float heightScale = (float)actualScreen.Y / virtualScreen.Y;
            scalingFactor = new Vector3(widthScale, heightScale, 1);
            scale = Matrix.CreateScale(scalingFactor);
        }
    }
}
