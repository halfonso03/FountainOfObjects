using TheFinalBattle.Characters;
using TheFinalBattle.Item;

namespace TheFinalBattle.Actions;


public abstract class AttackAction: CharacterAction
{
    public abstract int DamageDealt();   

    public void PerformAttack(Character target, IUserInteractor userInteractor)
    {
        var damage = DamageDealt();

        target.DecreaseHealthBy(damage);

        userInteractor.WriteLine($"{Name} dealt {damage} to {target.Name}");
        userInteractor.WriteLine($"{target.Name} is now at {target.CurrentHealth}/{target.MaximumHP}", TextColor.DarkYellow);
    }
}

public abstract class StandardAttackAction : AttackAction
{       
    
}


public class GearAttackAction : StandardAttackAction
{
    private readonly Gear _gear;

    public override string Name { get; }    

    public GearAttackAction(Gear gear)
    {
        _gear = gear;
        Name = $"{_gear.Name} ({_gear.AttackName})";
    }

    public override int DamageDealt()
    {
        return _gear.DamageDealt;
    }
}