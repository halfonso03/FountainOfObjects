using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFinalBattle.Actions.AttackActions;
using TheFinalBattle.Characters;

namespace TheFinalBattle.Item
{
    public abstract class Gear
    {
        public abstract string Name { get; }
        public abstract string AttackName { get; }
        public bool Equipped { get; set; } = false;
        public abstract int DamageDealt { get; }
        public Character? EquippedCharacter { get; set; }
        protected Gear() { }        
        public Gear(Character character)
        {
            EquippedCharacter = character;
            character.EquippedGear = this;
            Equipped = true;
        }
       
        internal static void UnequipCharacter(Gear? equippedGear)
        {
            if (equippedGear is not null)
            {
                equippedGear.Equipped = false;

                if (equippedGear.EquippedCharacter is not null)
                {
                    equippedGear.EquippedCharacter.EquippedGear = null;
                    equippedGear.EquippedCharacter = null;
                }
            }            
        }

        public virtual DamageDealtSource DamageDealtSource { get; set; } = DamageDealtSource.StandardProbabilityCalculation;

        protected static int CalculatedDamageByProbability(int percentChange = 92, int defaultDamage = 1)
        {
            var damageInfo = new DamageInfo();

            var random = new Random();
            var wata = random.Next(1, 101);

            if (wata % 3 == 0 && wata % 5 == 0)
            {
                damageInfo.InflictedDamage = 5;
            }
            else if (wata % 3 == 0 || wata % 5 == 0)
            {
                damageInfo.InflictedDamage = 3;
            }
            else if (AttackAction.GetSuccessProbability(percentChange) == 1)
            {
                damageInfo.InflictedDamage = defaultDamage;
            }

            return damageInfo.InflictedDamage ?? 0;
        }
    }

    public class Dagger : Gear
    {
        public override string Name { get; } = "Dagger";

        public override int DamageDealt { get; } = 1;

        public override string AttackName { get; } = "STAB";        

        public Dagger(Character character) : base(character) { }
        
        public Dagger() { }
    }

    public class Sword : Gear
    {
        public override string Name { get; } = "Sword";        

        public override string AttackName { get; } = "SLASH";

        public Sword() { }

        public Sword(Character character) : base(character) { }

        public override DamageDealtSource DamageDealtSource => DamageDealtSource.Custom;

        public override int DamageDealt => Gear.CalculatedDamageByProbability();
    }

    public class Hammer : Gear
    {
        public override string Name { get; } = "Hammer";

        public override int DamageDealt { get; } = 2;

        public override string AttackName { get; } = "SMASH";

        public Hammer() { }

        public Hammer(Character character) : base(character) { }
    }

    public class Bow : Gear
    {
        public override string Name { get; } = "Bow";

        public override int DamageDealt { get; } = 3;

        public override string AttackName { get; } = "QUICK SHOT";

        public Bow(Character character) : base(character) { }

    }

    public class CanonOfConsolas : Gear
    {
        public override string Name => "Canon of Consolas";

        public override string AttackName => "SHOOT CANON";

        public CanonOfConsolas(Character character) : base(character) { }

        public override DamageDealtSource DamageDealtSource => DamageDealtSource.Custom;

        public override int DamageDealt => Gear.CalculatedDamageByProbability();
    }

    public class FlamingSword : Gear
    {
        public override string Name => "Flaming Sword of Urundil";

        public override string AttackName => "Fire slash2";

        public FlamingSword(Character character) : base(character) { }

        public override DamageDealtSource DamageDealtSource => DamageDealtSource.Custom;

        public override int DamageDealt => Gear.CalculatedDamageByProbability(90, 4);
    }
}


public enum DamageDealtSource
{
    StandardProbabilityCalculation,
    Custom
}