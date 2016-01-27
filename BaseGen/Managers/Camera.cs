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
    static class Camera
    {
        public static Rectangle LevelBounds
        {
            get { return levelBounds; }
        }
        private static Rectangle levelBounds;
        public static Vector2 Offset
        {
            get { return offset; }
        }
        private static Vector2 offset;
        public static Rectangle Visible
        {
            get { return visible; }
        }
        private static Rectangle visible;
        public static void Initialize()
        {
            offset = new Vector2(0, 0);
            visible = new Rectangle(0, 0, (int)Main.virtualResolution.X, (int)Main.virtualResolution.Y);
        }
        public static void NewLevel()
        {
            levelBounds = new Rectangle(0, 0, Executive.level.Width, Executive.level.Height);
            Update();
        }
        public static void Update()
        {
            switch (Main.GetState())
            {
                case Main.GameState.Playing:
                    visible.X = (int)Executive.level.Player.Position.X - (visible.Width / 2) > 0 ? (int)Executive.level.Player.Position.X - (visible.Width / 2) : 0;
                    visible.Y = (int)Executive.level.Player.Position.Y - (visible.Height / 2) > 0 ? (int)Executive.level.Player.Position.Y - (visible.Height / 2) : 0;
                    if (visible.X < 0)
                        visible.X = 0;
                    if (visible.Y < 0)
                        visible.Y = 0;
                    offset.X = (int)Executive.level.Player.Position.X - (visible.Width / 2);
                    offset.Y = (int)Executive.level.Player.Position.Y - (visible.Height / 2);
                    break;
                case Main.GameState.Editor:
                    if (visible.X < 0)
                        visible.X = 0;
                    if (visible.Y < 0)
                        visible.Y = 0;
                    if (offset.X < 0)
                        offset.X = 0;
                    if (offset.Y < 0)
                        offset.Y = 0;
                    break;
                default:
                    break;
            }
        }
        public static void Move(Point movement)
        {
            visible = new Rectangle(visible.X + movement.X, visible.Y + movement.Y, visible.Width, visible.Height);
            if (visible.X < 0)
                visible.X = 0;
            if (visible.Y < 0)
                visible.Y = 0;
            offset.X = visible.X;
            offset.Y = visible.Y;
        }
    }
}
