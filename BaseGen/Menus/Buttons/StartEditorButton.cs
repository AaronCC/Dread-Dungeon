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
namespace BaseGen.Menus.Buttons
{
    class StartEditorButton : Button
    {
        Menus.EditorMenu menu;
        public StartEditorButton(Vector2 _position, string _name):base(_position, _name)
        {
        }
        public override void ClickEvent()
        {
            menu = (EditorMenu)Managers.Executive.menuStack.Peek();
            try
            { 
                Menus.EditorMenu menu = (EditorMenu)Managers.Executive.menuStack.Peek();
                Managers.Executive.NewEditor(menu.changeIndex, menu.levelIndex);
            }
            catch(InvalidDataException)
            {
                if (menu.levelIndex > Managers.Executive.levels.Count + 1)
                    throw new Exception("Can't find existing level with that index");
                Managers.Executive.NewEditor(1, menu.levelIndex);
            }
        }
    }
}
