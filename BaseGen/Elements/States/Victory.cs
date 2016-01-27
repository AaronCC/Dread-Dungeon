#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
#endregion


namespace BaseGen.Elements.States
{
    class Victory : EffectState
    {
        public Victory()
        {
            animState = AnimState.Victory;
        }
        public override State HandleInput(Player player, StateInput input)
        {
            switch (input)
            {
                case StateInput.AnimEnd:
                    if (Managers.Executive.levels.Count > player.Level.Index + 1)
                    {
                        Managers.Executive.LoadLevel(Managers.Executive.level.Index + 1);
                    }
                    else
                        Main.ChangeState(Main.GameState.Menu);
                    break;
                default:
                    break;
            }
            return this;
        }

        public override void Update(Player player, GameTime gameTime)
        {
            player.velocity.X = 0;
            player.velocity.Y = 0;
        }
    }
}
