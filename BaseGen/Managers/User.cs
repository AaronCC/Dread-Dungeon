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

namespace BaseGen.Managers
{
    public static class User
    {
        public static KeyboardState kState;
        public static MouseState mState;
        public static KeyboardState old_kState;
        public static MouseState old_mState;
        public static Point mousePos;
        public static Rectangle mHitBox;

        public static void Initialize()
        {
            kState = new KeyboardState();
            old_kState = new KeyboardState();
            mState = new MouseState();
            old_mState = new MouseState();
            mState = Mouse.GetState();
            kState = Keyboard.GetState();
            mousePos.X = (int)(mState.Position.X / Managers.ScreenManager.scalingFactor.X);
            mousePos.Y = (int)(mState.Position.Y / Managers.ScreenManager.scalingFactor.Y);
            mHitBox = new Rectangle(mousePos.X, mousePos.Y, 2, 2);
        }

        public static void Update()
        {
            GetStates();
            if (kState.IsKeyDown(Keys.Escape) && old_kState.IsKeyUp(Keys.Escape) && Main.GetState() != Main.GameState.Menu)
            {
                if (Main.GetState() == Main.GameState.Playing)
                    Main.ChangeState(Main.GameState.Paused);
                else if (Main.GetState() == Main.GameState.Paused)
                    Main.ChangeState(Main.GameState.Playing);
            }
            if (kState.IsKeyDown(Keys.F2) && old_kState.IsKeyUp(Keys.F2))
                Main.debug = !Main.debug;
        }
        private static void GetStates()
        {
            old_mState = mState;
            old_kState = kState;
            mState = Mouse.GetState();
            kState = Keyboard.GetState();
            mousePos.X = (int)(mState.Position.X / Managers.ScreenManager.scalingFactor.X);
            mousePos.Y = (int)(mState.Position.Y / Managers.ScreenManager.scalingFactor.Y);
            mHitBox = new Rectangle(mousePos.X, mousePos.Y, 1, 1);
        }
        public static string Typed()
        {
            Keys[] pressedKeys = kState.GetPressedKeys();

            string input = string.Empty;
            foreach (Keys key in pressedKeys)
            {
                if (old_kState.IsKeyDown(key))
                    break;
                switch (key)
                {
                    case Keys.LeftShift:
                        break;
                    case Keys.RightShift:
                        break;
                    case Keys.OemQuotes:
                        return "'";
                    case Keys.Space:
                        return " ";

                    case Keys.Back:
                        return "Back";
                    case Keys.Enter:
                        return string.Empty;
                    default:
                        if (kState.IsKeyDown(Keys.LeftShift))
                            input += key.ToString()[0];
                        else
                            input += key.ToString().ToLower()[0];
                        break;
                }
            }
            return input;
        }
    }

}
