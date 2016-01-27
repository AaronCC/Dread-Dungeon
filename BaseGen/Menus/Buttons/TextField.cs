#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace BaseGen.Menus.Buttons
{
    class TextField : Button
    {
        public string text;
        public delegate string GetText();
        public delegate void SendText(string _text);
        private GetText Get;
        private SendText Send;

        private bool typing;
        public TextField(Vector2 _position, string _name) : base(_position, _name)
        {
            typing = false;
            size = new Point(150,22);
        }
        public override void Update()
        {
            Menus.Menu menu = Managers.Executive.menuStack.Peek();
            if (Get == null)
            {
                Get = menu.GetText;
                text = Get();
            }
            if (Send == null)
                Send = menu.TextInput;
            if(typing)
            {
                flagged = true;
                string input = Managers.User.Typed();
                if (input == "Back" && text.Length > 0)
                    text = text.Substring(0, text.Length - 1);
                else
                    text += input;
            }
            if (Managers.User.kState.IsKeyDown(Keys.Enter) && Managers.User.old_kState.IsKeyDown(Keys.Enter))
            {
                flagged = false;
                typing = false;
                Send(text);
                text = Get();
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (text != null)
                spriteBatch.DrawString(Main.font, text, new Vector2(position.X + 5, position.Y + 3), Color.Black, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
        }
        public override void ClickEvent()
        {
            text = string.Empty;
            typing = true;
        }
       
    }
}
