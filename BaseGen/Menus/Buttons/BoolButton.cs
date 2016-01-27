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
    class BoolButton : Button
    {
        private delegate void ChangeBool(int index);
        private ChangeBool Change;
        int index;
        public BoolButton(Vector2 _position, string _name, int _index) : base(_position, _name)
        {
            index = _index;
            size = new Point(70, 22);
        }
        public override void ClickEvent()
        {
            if (Change == null)
                Change = Managers.Executive.menuStack.Peek().ChangeBool;
           Change(index);
        }
    }
}
