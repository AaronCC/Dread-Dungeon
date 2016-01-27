#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using Microsoft.Xna.Framework.Media;
#endregion

namespace BaseGen.Menus
{
    public class Menu
    {
        public List<Buttons.Button> buttons;
        public Assets.Texture texture;
        public string name;

        public Menu(string _name, List<Buttons.Button> _buttons)
        {

            name = _name;
            buttons = _buttons;
            texture = Managers.AssetManager.GetTextureAsset(name);
        }
        public void Update()
        {
            foreach (Buttons.Button btn in buttons)
            {
                if (Managers.User.mHitBox.Intersects(btn.HitBox) && !btn.flagged && btn.enabled)
                {
                    btn.flagged = true;
                }
                else if (!Managers.User.mHitBox.Intersects(btn.HitBox))
                    btn.flagged = false;

                if (Managers.User.mState.LeftButton == ButtonState.Pressed
                    && Managers.User.old_mState.LeftButton == ButtonState.Released
                    && btn.flagged)
                {
                    btn.ClickEvent();
                }
                btn.Update();
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            string directory = new DirectoryInfo(Main.levelPath).Name;
            spriteBatch.Draw(texture.sprite, new Vector2(0, 0), Color.White);
            if (this.GetType() == typeof(Menu))
                spriteBatch.DrawString(Main.font, new DirectoryInfo(Main.levelPath).Name, new Vector2(480 - (5 * directory.Length), 125), Color.AliceBlue);
            foreach (Buttons.Button btn in buttons)
            {
                Rectangle drawRect = new Rectangle((int)btn.position.X, (int)btn.position.Y, btn.size.X, btn.size.Y);
                if (btn.flagged)
                    spriteBatch.Draw(btn.Texture.sprite, drawRect, Color.White);
                else
                    spriteBatch.Draw(btn.Texture.sprite, drawRect, Color.LightGray);
                btn.Draw(spriteBatch);
            }
        }
        public virtual void IncreaseValue(int index, int ammount)
        {

        }
        public virtual void DecreaseValue(int index, int ammount)
        {

        }
        public virtual void ChangeBool(int index)
        {

        }
        public virtual void TextInput(string text)
        {

        }
        public virtual string GetText()
        {
            return string.Empty;
        }
    }
}
