using TheFinalBattle.Enums;

namespace TheFinalBattle.Actions.AttackActions;

public class StoneArmorAttackModifier : IAttackModifier
{
    public int ModifyBy { get; } = -1;
    public string Name { get; set; } = "stone armor";
    public DamageType DefendsDamageType { get; set; } = DamageType.Normal;
}

public class ObjectSightAttackModifier : IAttackModifier
{
    public int ModifyBy { get; set; } = -2;
    public string Name { get; set; } = "object sight";
    public DamageType DefendsDamageType { get; set; } = DamageType.Decoding;
}

public interface IAttackModifier
{
    int ModifyBy { get; }
    string Name { get; set; }
    DamageType DefendsDamageType { get; set; }
}
