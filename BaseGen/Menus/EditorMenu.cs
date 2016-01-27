#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using System.IO;
#endregion


namespace BaseGen.Menus
{
    class EditorMenu : Menu
    {
        public int cols;
        public int rows;
        public int levelIndex;
        public List<string> changeMessages;
        public int changeIndex;
        public EditorMenu(string _name, List<Buttons.Button> _buttons) : base(_name, _buttons)
        {
            cols = 0;
            rows = 0;
            levelIndex = 0;
            changeMessages = new List<string>();
            changeMessages.Add("New");
            changeMessages.Add("Edit");
            changeMessages.Add("Replace");
            changeIndex = 0;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (changeIndex != 1)
            {
                //spriteBatch.DrawString(Main.font, "Columns", new Vector2(400, 315),
                //    Color.Black, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);

                spriteBatch.DrawString(Main.font, cols.ToString(), new Vector2(215, 240),
                    Color.Black, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);

                //spriteBatch.DrawString(Main.font, "Rows", new Vector2(610, 315),
                //    Color.Black, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);

                spriteBatch.DrawString(Main.font, rows.ToString(), new Vector2(315, 240),
                    Color.Black, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
            }
            if (changeIndex != 0)
            {
                //spriteBatch.DrawString(Main.font, "Level Index", new Vector2(770, 315),
                //    Color.Black, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);

                spriteBatch.DrawString(Main.font, levelIndex.ToString(), new Vector2(415, 240),
                    Color.Black, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
            }

            spriteBatch.DrawString(Main.font, changeMessages[changeIndex], new Vector2(200, 153),
                Color.Black, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
        }
        public override void ChangeBool(int index)
        {
            foreach (Buttons.Button btn in buttons)
                if (btn.GetType() != typeof(Buttons.SmallButton))
                    btn.enabled = true;
            changeIndex++;
            if (changeIndex > changeMessages.Count - 1)
                changeIndex = 0;
            Buttons.IncreaseButton inBtn;
            Buttons.DecreaseButton deBtn;
            switch (changeIndex)
            {
                case 0:

                    foreach (Buttons.Button btn in buttons)
                    {
                        if (btn.GetType() == typeof(Buttons.IncreaseButton))
                        {
                            inBtn = (Buttons.IncreaseButton)btn;
                            if (inBtn.index == 2)
                                btn.enabled = false;
                        }
                        else if (btn.GetType() == typeof(Buttons.DecreaseButton))
                        {
                            deBtn = (Buttons.DecreaseButton)btn;
                            if (deBtn.index == 2)
                                btn.enabled = false;
                        }
                    }
                    break;
                case 1:
                    foreach (Buttons.Button btn in buttons)
                    {
                        if (btn.GetType() == typeof(Buttons.IncreaseButton))
                        {
                            inBtn = (Buttons.IncreaseButton)btn;
                            if (inBtn.index == 1 || inBtn.index == 0)
                                btn.enabled = false;
                        }
                        else if (btn.GetType() == typeof(Buttons.DecreaseButton))
                        {
                            deBtn = (Buttons.DecreaseButton)btn;
                            if (deBtn.index == 1 || deBtn.index == 0)
                                btn.enabled = false;
                        }
                    }
                    break;
                case 2:

                    break;
                default:
                    break;
            }
        }
        public override void IncreaseValue(int index, int ammount)
        {
            switch (index)
            {
                case 0:

                    cols += ammount;
                    break;
                case 1:
                    rows += ammount;
                    break;
                case 2:
                    if (levelIndex + 1 < Managers.Executive.levels.Count)
                        levelIndex += ammount;
                    break;
                default:
                    break;
            }
        }
        public override void DecreaseValue(int index, int ammount)
        {
            switch (index)
            {
                case 0:
                    if (cols - 1 >= 0)
                        cols -= ammount;
                    break;
                case 1:
                    if (rows - 1 >= 0)
                        rows -= ammount;
                    break;
                case 2:
                    if (levelIndex - 1 >= 0)
                        levelIndex -= ammount;
                    break;
                default:
                    break;
            }
        }
        public override void TextInput(string text)
        {
            Main.levelPath = Main.contentPath + "Levels/" + text;
            Managers.Parser.ParseNewDirectory();
        }
        public override string GetText()
        {
            return new DirectoryInfo(Main.levelPath).Name;
        }
    }
}
