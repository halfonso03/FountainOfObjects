using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFinalBattle.Actions.AttackActions;
using TheFinalBattle.Characters;

namespace TheFinalBattle.Item
{
    public abstract class Gear 
    {
        protected string _internalId = Guid.NewGuid().ToString();
        public abstract string Name { get; }
        public abstract string AttackName { get; }
        public bool Equipped { get; set; } = false;
        public abstract int GetDamageDealt();
        public Character? EquippedCharacter { get; set; }
        public virtual bool Pairable => false;
        public Gear? PairedGear { get; internal set; }
        protected Gear() { }        
        public Gear(Character character)
        {
            EquippedCharacter = character;
            character.EquippedGear = this;
            Equipped = true;
        }
       
        internal static void UnequipFromCharacter(Gear? equippedGear)
        {
            if (equippedGear is not null)
            {
                equippedGear.Equipped = false;

                if (equippedGear.EquippedCharacter is not null)
                {
                    equippedGear.EquippedCharacter.EquippedGearItems.Remove(equippedGear);
                    equippedGear.EquippedCharacter.EquippedGear = null;
                    equippedGear.EquippedCharacter = null;
                }
            }            
        }

        public virtual DamageDealtSource DamageDealtSource { get; set; } = DamageDealtSource.DefaultProbability;
        

        protected static int DamageByPercentChanceOfSuccess(int percentChange = 92, int defaultDamage = 1)
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

            damageInfo.AttackMissed = damageInfo.InflictedDamage is null;

            return damageInfo.InflictedDamage ?? 0;
        }

        public override string ToString() => _internalId;
    }

    public class Dagger : Gear
    {
        public override string Name { get; } = "Dagger";
        
        public override int GetDamageDealt() => GetDamage();

        private int GetDamage()
        {
            int damage = 1;         

            return damage;
        }

        public override string AttackName { get; } = "STAB";

        public override bool Pairable => true;
        
        public Dagger(Character character) : base(character) { }
        
        public Dagger() { }

        


    }

    public class BroadSword : Gear
    {
        public override string Name { get; } = "Broad Sword";        

        public override string AttackName { get; } = "SLASH";

        public BroadSword() { }

        public BroadSword(Character character) : base(character) { }

        public override DamageDealtSource DamageDealtSource => DamageDealtSource.Custom;

        public override int GetDamageDealt() => Gear.DamageByPercentChanceOfSuccess(95, 4);
    }

    public class Hammer : Gear
    {
        public override string Name { get; } = "Hammer";        

        public override string AttackName { get; } = "SMASH";

        public Hammer() { }

        public Hammer(Character character) : base(character) { }

        public override int GetDamageDealt() => Gear.DamageByPercentChanceOfSuccess(80, 5);
        public override bool Pairable => true;
    }

    public class Bow : Gear
    {
        public override string Name { get; } = "Bow";

        public override int GetDamageDealt() => Gear.DamageByPercentChanceOfSuccess(80, 3);

        public override string AttackName { get; } = "QUICK SHOT";

        public Bow(Character character) : base(character) { }

    }

    public class CanonOfConsolas : Gear
    {
        public override string Name => "Canon of Consolas";

        public override string AttackName => "SHOOT CANON";

        public CanonOfConsolas(Character character) : base(character) { }

        public override DamageDealtSource DamageDealtSource => DamageDealtSource.Custom;

        public override int GetDamageDealt() => Gear.DamageByPercentChanceOfSuccess(80, 5);
    }

    public class FlamingSword : Gear
    {
        public override string Name => "Flaming Sword of Urundil";

        public override string AttackName => "Fire slash";

        public FlamingSword(Character character) : base(character) { }

        public FlamingSword() { }

        public override DamageDealtSource DamageDealtSource => DamageDealtSource.Custom;

        public override int GetDamageDealt() => DamageByPercentChanceOfSuccess(80, 6);
    }
}


public enum DamageDealtSource
{
    DefaultProbability,
    Custom
}