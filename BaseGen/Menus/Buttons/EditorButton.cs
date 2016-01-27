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
    class EditorButton : Button
    {
        public EditorButton(Vector2 _position, string _name) : base(_position, _name)
        {
        }
        public override void ClickEvent()
        {
            Managers.Executive.menuStack.Push(Managers.Executive.menuDict["EditorMenu"]);
        }
    }
}
