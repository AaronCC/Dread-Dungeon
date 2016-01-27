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
    public class Equip : State
    {
        public List<Items.Item> equipped;
        public Queue<Items.Item> equipQueue;
        public Queue<Items.Item> dequipQueue;
        public override State HandleInput(Player player, StateInput input)
        {
            throw new NotImplementedException();
        }
        public Equip()
        {
            equipped = new List<Items.Item>();
            equipQueue = new Queue<Items.Item>();
            dequipQueue = new Queue<Items.Item>();
        }
        public bool DequipItem(Player player, Items.Item item)
        {
            try
            {
                dequipQueue.Enqueue(item);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EquipItem(Player player, Items.Item item)
        {
            switch (item.Name)
            {
                case "Heart":
                    player.hearts++;
                    return false;
                case "Torch":
                    foreach (Items.Item _item in equipped)
                    {
                        if (item.Name == "Torch")
                            DequipItem(player, _item);
                    }
                    break;
                default:
                    break;
            }
            equipQueue.Enqueue(item);
            return true;
        }

        public override void Update(Player player, GameTime gameTime)
        {
            while (dequipQueue.Count > 0)
            {
                dequipQueue.Peek().Dequipped();
                equipped.Remove(dequipQueue.Dequeue());
            }
            while (equipQueue.Count > 0)
            {
                equipped.Add(equipQueue.Dequeue());
 
            }
            foreach (Items.Item item in equipped)
            {
                item.Update(gameTime, player);
            }
        }
    }
}
