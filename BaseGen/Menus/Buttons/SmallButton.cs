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
    class SmallButton:Button
    {
        public SmallButton(Vector2 _position, string name) : base(_position, name)
        {
            size = new Point(50, 50);
            enabled = false;
            flagged = true;
        }
    }
}
