using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFinalBattle.Characters;

namespace TheFinalBattle.Item
{
    public abstract class Gear
    {
        public abstract string Name { get; }
        public abstract string AttackName { get; }
        public bool Equipped { get; set; } = false;
        public abstract int DamageDealt { get; }
        public abstract void Use(Character character);
        public Character? EquippedCharacter { get; set; }
        public void SetEquippedCharacter(Character character)
        {
            EquippedCharacter = character;
        }

        internal static void UnequipCharacter(Gear? equippedGear)
        {
            if (equippedGear is not null)
            {
                equippedGear.Equipped = false;
                equippedGear.EquippedCharacter = null;                
            }            
        }
    }

    public class Dagger : Gear
    {
        public override string Name { get; } = "Dagger";

        public override int DamageDealt { get; } = 1;

        public override string AttackName { get; } = "STAB";        

        public Dagger(Character character)
        {
            EquippedCharacter = character;
            character.EquippedGear = this;
            Equipped = true;
        }

        public Dagger()
        {
        }

        public override void Use(Character character)
        {
            character.DecreaseHealthBy(DamageDealt);
        }
    }

    public class Sword : Gear
    {
        public override string Name { get; } = "Sword";

        public override int DamageDealt { get; } = 5;

        public override string AttackName { get; } = "SLASH";

        public Sword() { }

        public Sword(Character character)
        {
            EquippedCharacter = character;
            character.EquippedGear = this;
        }

        public override void Use(Character character)
        {
            character.DecreaseHealthBy(DamageDealt);
        }
    }
}
