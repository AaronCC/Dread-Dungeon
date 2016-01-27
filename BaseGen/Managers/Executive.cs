#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
#endregion
namespace BaseGen.Managers
{
    public static class Executive
    {
        public static Dictionary<string, Menus.Menu> menuDict;
        public static Dictionary<string, Elements.GameObject> objectDict;
        public static Stack<Menus.Menu> menuStack;
        public static List<List<char[,]>> levels;
        public static Elements.Level level;
        public static List<List<string>> signData;
        public static Editor editor;
        public static Dictionary<char, string> tileData;
        public static void Initialize()
        {
            tileData = new Dictionary<char, string>();
            signData = new List<List<string>>();
            menuDict = new Dictionary<string, Menus.Menu>();
            objectDict = new Dictionary<string, Elements.GameObject>();
            menuStack = new Stack<Menus.Menu>();
            levels = new List<List<char[,]>>();
        }
        public static Elements.Level LoadLevel(int index)
        {
            int hearts = 0;
            if (index != 0)
               hearts = level.Player.hearts;
            level = new Elements.Level(index);
            level.LoadLevel();
            if (index == 0)
                level.Player.hearts = level.Player.startHearts;
            else
                level.Player.hearts = hearts;
            return level;
        }
        public static void Update(GameTime gameTime)
        {
            switch (Main.GetState())
            {
                case Main.GameState.Menu:
                    if (menuStack.Count > 0)
                        menuStack.Peek().Update();
                    break;
                case Main.GameState.Playing:
                    if (User.kState.IsKeyDown(Keys.Back))
                    {
                        Main.ChangeState(Main.GameState.Menu);
                        break;
                    }
                    level.Update(gameTime);
                    Camera.Update();

                    break;
                case Main.GameState.Editor:
                    editor.UpdateEdit(gameTime);
                    Camera.Update();
                    break;
                case Main.GameState.Paused:
                    break;
                default:
                    throw new NotSupportedException("Invalid gameState exception in Executive.Update");
            }
        }
        public static void Draw(SpriteBatch spriteBatch, SpriteBatch lightBatch)
        {
            switch (Main.GetState())
            {
                case Main.GameState.Menu:
                    if (menuStack.Count > 0)
                        menuStack.Peek().Draw(spriteBatch);
                    break;
                case Main.GameState.Playing:
                    level.Draw(spriteBatch, lightBatch);
                    break;
                case Main.GameState.Paused:
                    break;
                case Main.GameState.Editor:
                    editor.DrawEdit(spriteBatch);
                    break;
                default:
                    throw new NotSupportedException("Invalid gameState exception in Executive.Draw");
            }

        }
        public static void NewEditor(int changeIndex, int index)
        {
            List<char[,]> tempLevel = new List<char[,]>();
            Main.ChangeState(Main.GameState.Editor);
            Menus.EditorMenu menu = (Menus.EditorMenu)menuStack.Peek();
            if (changeIndex == 0 || index > levels.Count - 1) // New
            {
                changeIndex = 0;
                tempLevel.Add(new char[menu.rows, menu.cols]);
                tempLevel.Add(new char[menu.rows, menu.cols]);
                levels.Add(tempLevel);
                level = new Elements.Level(levels.Count - 1);
            }
            else if (changeIndex == 1) // Edit
            {
                level = new Elements.Level(index);
            }
            else if (changeIndex == 2) // Replace
            {
                tempLevel.Add(new char[menu.rows, menu.cols]);
                tempLevel.Add(new char[menu.rows, menu.cols]);
                levels[index] = tempLevel;
                level = new Elements.Level(index);
            }
            editor = new Editor(level, changeIndex);
        }
    }
}
