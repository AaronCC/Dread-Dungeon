#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
using System.IO;
#endregion

namespace BaseGen.Managers
{
    public class Editor
    {
        bool exported;
        private int camMoveSpeed;
        public Elements.Level editing;
        private char input;
        private bool help;
        private int changeIndex;
        private bool back;
        private Assets.Texture selectTexture;
        private int oldX, oldY, x, y;
        private bool filling;
        private bool endFilling;
        private Point startFill;
        private Point endFill;
        public Editor(Elements.Level level, int _changeIndex)
        {
            changeIndex = _changeIndex;
            editing = level;
            camMoveSpeed = 8;
            editing.NewEditLevel(changeIndex);
            input = '.';
            exported = false;
            help = false;
            back = false;
            endFilling = false;
            filling = false;
            startFill = Point.Zero;
            endFill = Point.Zero;
            selectTexture = AssetManager.GetTextureAsset("Select");
            Managers.Camera.NewLevel();
            editing.EditUpdate();
        }

        public void UpdateEdit(GameTime gameTime)
        {
            CheckInput();
        }
        public void CheckInput()
        {
            Keys[] pressedKeys = User.kState.GetPressedKeys();
            foreach (Keys key in pressedKeys)
            {
                switch (key)
                {
                    case Keys.F:
                        if (Managers.User.old_kState.IsKeyUp(Keys.F))
                        {
                            filling = !filling;
                            endFilling = false;
                            startFill = Point.Zero;
                            endFill = Point.Zero;
                        }
                        break;
                    case Keys.Left:
                        Camera.Move(new Point(-camMoveSpeed, 0));
                        break;
                    case Keys.Right:
                        Camera.Move(new Point(camMoveSpeed, 0));
                        break;
                    case Keys.Up:
                        Camera.Move(new Point(0, -camMoveSpeed));
                        break;
                    case Keys.Down:
                        Camera.Move(new Point(0, camMoveSpeed));
                        break;

                    case Keys.Enter:
                        if (!exported)
                        {
                            exported = true;
                        }
                        break;
                    case Keys.Back:
                        Main.ChangeState(Main.GameState.Menu);
                        break;
                    case Keys.LeftShift:
                        break;
                    case Keys.OemMinus:
                        if (User.kState.IsKeyDown(Keys.LeftShift))
                            input = '_';
                        else
                            input = '-';
                        back = false;
                        break;
                    case Keys.Space:
                        input = ' ';
                        back = true;
                        break;
                    case Keys.OemPeriod:
                        if (User.kState.IsKeyDown(Keys.LeftShift) || User.kState.IsKeyDown(Keys.RightShift))
                            input = '>';
                        else
                            input = '.';
                        back = false;
                        break;
                    case Keys.OemComma:

                        if (User.kState.IsKeyDown(Keys.LeftShift) || User.kState.IsKeyDown(Keys.RightShift))
                            input = '<';
                        else
                            input = ',';
                        back = false;
                        break;
                    case Keys.F1:
                        if (User.old_kState.IsKeyUp(Keys.F1))
                            help = !help;
                        break;
                    case Keys.D2:
                        input = '@';
                        back = false;
                        break;
                    case Keys.D9:
                        input = '(';
                        back = false;
                        break;
                    case Keys.D0:
                        input = ')';
                        back = false;
                        break;
                    case Keys.OemOpenBrackets:
                        if (User.kState.IsKeyDown(Keys.LeftShift))
                            input = '{';
                        else
                            input = '[';
                        back = false;
                        break;
                    case Keys.OemCloseBrackets:

                        if (User.kState.IsKeyDown(Keys.LeftShift))
                            input = '}';
                        else
                            input = ']';
                        back = false;
                        break;
                    case Keys.D1:
                        input = '!';
                        back = false;
                        break;
                    case Keys.OemQuestion:
                        input = '?';
                        back = false;
                        break;
                    case Keys.OemPlus:
                        input = '=';
                        back = false;
                        break;
                    case Keys.D8:
                        input = '*';
                        back = false;
                        break;
                    case Keys.OemQuotes:
                        input = '"';
                        back = false;
                        break;
                    default:
                        if (User.kState.IsKeyDown(Keys.LeftShift))
                            input = key.ToString()[0];
                        else
                            input = key.ToString().ToLower()[0];
                        back = false;
                        break;
                }
            }
            if (User.mState.LeftButton == ButtonState.Pressed)
            {
                if (x != (int)(User.mousePos.X + Camera.Offset.X) / Elements.Tile.width)
                    oldX = x;
                if (y != (int)(User.mousePos.Y + Camera.Offset.Y) / Elements.Tile.height)
                    oldY = y;
                x = (int)(User.mousePos.X + Camera.Offset.X) / Elements.Tile.width;
                y = (int)(User.mousePos.Y + Camera.Offset.Y) / Elements.Tile.height;

                if (filling && endFilling == false && User.old_mState.LeftButton == ButtonState.Released)
                {
                    startFill = new Point(x, y);
                    endFilling = true;
                }
                else if (filling && User.old_mState.LeftButton == ButtonState.Released)
                {
                    endFill = new Point(x, y);
                    if (endFill.X >= startFill.X && endFill.Y >= startFill.Y)
                        for (int x = startFill.X; x <= endFill.X; x++)
                        {
                            for (int y = startFill.Y; y <= endFill.Y; y++)
                            {
                                editing.ChangeTile(input, y, x, back);
                            }
                        }
                    else if (endFill.X <= startFill.X && endFill.Y >= startFill.Y)
                        for (int x = startFill.X; x >= endFill.X; x--)
                        {
                            for (int y = startFill.Y; y <= endFill.Y; y++)
                            {
                                editing.ChangeTile(input, y, x, back);
                            }
                        }
                    else if (endFill.X >= startFill.X && endFill.Y <= startFill.Y)
                        for (int x = startFill.X; x <= endFill.X; x++)
                        {
                            for (int y = startFill.Y; y >= endFill.Y; y--)
                            {
                                editing.ChangeTile(input, y, x, back);
                            }
                        }
                    else if (endFill.X <= startFill.X && endFill.Y <= startFill.Y)
                        for (int x = startFill.X; x >= endFill.X; x--)
                        {
                            for (int y = startFill.Y; y >= endFill.Y; y--)
                            {
                                editing.ChangeTile(input, y, x, back);
                            }
                        }
                    endFilling = false;
                    startFill = Point.Zero;
                    endFill = Point.Zero;
                }
                if ((User.old_mState.LeftButton == ButtonState.Released || (x != oldX || y != oldY)) && !filling)
                {
                    editing.ChangeTile(input, y, x, back);
                    oldX = x;
                    oldY = y;
                }
            }

        }
        public void DrawEdit(SpriteBatch spriteBatch)
        {
            if (exported)
                editing.ExportLevel(spriteBatch, changeIndex, editing.Index);
            editing.EditDraw(spriteBatch);

            int i = 0;
            //if (filling)
                //spriteBatch.DrawString(Main.font, "Filling", new Vector2(User.mousePos.X + 10, User.mousePos.Y + 30), Color.White);
            int x, y;
            
            x = (int)(User.mousePos.X + Camera.Offset.X);
            y = (int)(User.mousePos.Y + Camera.Offset.Y);
            
            int xr = x / Elements.Tile.width;
            int yc = y / Elements.Tile.height;
            if (endFilling)
            {
                #region drawFill
                if (xr >= startFill.X && yc >= startFill.Y)
                    for (int r = startFill.X; r <= xr; r++)
                    {
                        for (int c = startFill.Y; c <= yc; c++)
                        {
                            spriteBatch.Draw(selectTexture.sprite, new Rectangle(r * Elements.Tile.width - (int)Camera.Offset.X, c * Elements.Tile.height - (int)Camera.Offset.Y, Elements.Tile.width, Elements.Tile.height)
                                , Color.Blue * 0.5f);
                        }
                    }
                if (xr <= startFill.X && yc >= startFill.Y)
                    for (int r = startFill.X; r >= xr; r--)
                    {
                        for (int c = startFill.Y; c <= yc; c++)
                        {
                            spriteBatch.Draw(selectTexture.sprite, new Rectangle(r * Elements.Tile.width - (int)Camera.Offset.X, c * Elements.Tile.height - (int)Camera.Offset.Y, Elements.Tile.width, Elements.Tile.height)
                                , Color.Blue * 0.5f);
                        }
                    }

                if (xr >= startFill.X && yc <= startFill.Y)
                    for (int r = startFill.X; r <= xr; r++)
                    {
                        for (int c = startFill.Y; c >= yc; c--)
                        {
                            spriteBatch.Draw(selectTexture.sprite, new Rectangle(r * Elements.Tile.width - (int)Camera.Offset.X, c * Elements.Tile.height - (int)Camera.Offset.Y, Elements.Tile.width, Elements.Tile.height)
                                , Color.Blue * 0.5f);
                        }
                    }

                if (xr <= startFill.X && yc <= startFill.Y)
                    for (int r = startFill.X; r >= xr; r--)
                    {
                        for (int c = startFill.Y; c >= yc; c--)
                        {
                            spriteBatch.Draw(selectTexture.sprite, new Rectangle(r * Elements.Tile.width - (int)Camera.Offset.X, c * Elements.Tile.height - (int)Camera.Offset.Y, Elements.Tile.width, Elements.Tile.height)
                                , Color.Blue * 0.5f);
                        }
                    }

                spriteBatch.Draw(selectTexture.sprite, new Rectangle(startFill.X * Elements.Tile.width - (int)Camera.Offset.X, startFill.Y * Elements.Tile.height - (int)Camera.Offset.Y, Elements.Tile.width, Elements.Tile.height)
                                   , Color.Green);
                #endregion
            }
            if (!filling)
                spriteBatch.Draw(selectTexture.sprite, new Rectangle(x - (x % Elements.Tile.width) - (int)Camera.Offset.X, y - (y % Elements.Tile.height) - (int)Camera.Offset.Y, Elements.Tile.width, Elements.Tile.height), Color.White);
            else if (endFilling)
                spriteBatch.Draw(selectTexture.sprite, new Rectangle(x - (x % Elements.Tile.width) - (int)Camera.Offset.X, y - (y % Elements.Tile.height) - (int)Camera.Offset.Y, Elements.Tile.width, Elements.Tile.height), Color.Red);
            else if (!endFilling)
                spriteBatch.Draw(selectTexture.sprite, new Rectangle(x - (x % Elements.Tile.width) - (int)Camera.Offset.X, y - (y % Elements.Tile.height) - (int)Camera.Offset.Y, Elements.Tile.width, Elements.Tile.height), Color.Green);

            try
            {
                spriteBatch.DrawString(Main.font, Executive.tileData[input], new Vector2(User.mousePos.X + 10, User.mousePos.Y + 10), Color.White
                    , 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
            }
            catch
            {
                spriteBatch.DrawString(Main.font, "Null", new Vector2(User.mousePos.X + 10, User.mousePos.Y + 10), Color.White
                    , 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
            }
            if (help)
                foreach (KeyValuePair<char, string> tile in Executive.tileData)
                {
                    i++;
                    spriteBatch.DrawString(Main.font, tile.Key.ToString() + "  " + tile.Value, new Vector2(Managers.ScreenManager.virtualScreen.X - 350 + (150 * (i / 26)), 20 * i - (500 * (i / 26))), Color.White
                         , 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);

                }
        }
    }
}

