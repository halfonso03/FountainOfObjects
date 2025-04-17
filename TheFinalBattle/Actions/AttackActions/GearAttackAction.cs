using TheFinalBattle.Item;

namespace TheFinalBattle.Actions.AttackActions;

public class GearAttackAction : StandardAttackAction
{
    private readonly Gear _gear;

    public override string Name { get; }

    public GearAttackAction(Gear gear) : base()
    {
        _gear = gear;
        Name = $"{_gear.Name} ({_gear.AttackName})";
    }

    public override int DamageDealt()
    {
        if (GetAttackSuccessProbability() == 1)
        {
            return _gear.DamageDealt;
        }
        else
        {
            return 0;
        }

    }
}