using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BaseGen.Elements.States
{
    class Dead : State
    {
        public Dead()
        {
            animState = AnimState.Dead;
        }
        public override State HandleInput(Player player, StateInput input)
        {
            switch (input)
            {
                case StateInput.AnimEnd:
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
