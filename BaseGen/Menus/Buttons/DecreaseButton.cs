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
    class DecreaseButton : Button
    {
        public delegate void DecreaseValue(int index, int ammount);
        DecreaseValue menuFunc;
        public int index;
        public DecreaseButton(Vector2 _position, string _name, string menuName, int _index) : base(_position, _name)
        {
            size = new Point(50, 50);
            index = _index;
        }
        public override void ClickEvent()
        {
            int ammount = 1;
            if (Managers.User.kState.IsKeyDown(Keys.LeftShift) || Managers.User.kState.IsKeyDown(Keys.RightShift))
            {
                ammount = 5;
            }
            if (menuFunc == null)
                menuFunc = Managers.Executive.menuStack.Peek().DecreaseValue;
            menuFunc(index, ammount);
        }
    }
}
