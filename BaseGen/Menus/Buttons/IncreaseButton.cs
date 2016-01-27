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
    class IncreaseButton : Button
    {
        public delegate void IncreaseValue(int index, int ammount);
        IncreaseValue menuFunc;
        public int index;
        public IncreaseButton(Vector2 _position, string _name, string menuName, int _index) : base(_position, _name)
        {
            index = _index;
            size = new Point(50, 50);
        }
        public override void ClickEvent()
        {
            int ammount = 1;
            if(Managers.User.kState.IsKeyDown(Keys.LeftShift) || Managers.User.kState.IsKeyDown(Keys.RightShift))
            {
                ammount = 5;
            }
            if(menuFunc == null)
                menuFunc = Managers.Executive.menuStack.Peek().IncreaseValue;
            menuFunc(index, ammount);
        }
    }
}
