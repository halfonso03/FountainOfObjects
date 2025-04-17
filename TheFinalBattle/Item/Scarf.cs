using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFinalBattle.Characters;

namespace TheFinalBattle.Item
{
    internal class Scarf : IItem
    {
        public string Name => "Scarf";

        public bool Used { get; } = false;

        public void UseItem(Character character)
        {
            // wipe brow
        }
    }
}
