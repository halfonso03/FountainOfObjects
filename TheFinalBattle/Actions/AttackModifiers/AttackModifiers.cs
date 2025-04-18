using TheFinalBattle.Enums;

namespace TheFinalBattle.Actions.AttackModifiers;

public class StoneArmorAttackModifier : IDefenseModifier
{
    public int ModifyBy { get; } = -1;
    public string Name { get; set; } = "stone armor";
    public DamageType DefendsFromDamageType { get; } = DamageType.Normal;
}

public class ObjectSightAttackModifier : IDefenseModifier
{
    public int ModifyBy { get; set; } = -2;
    public string Name { get; set; } = "object sight";
    public DamageType DefendsFromDamageType { get; } = DamageType.Decoding;
}

public interface IDefenseModifier
{
    int ModifyBy { get; }
    string Name { get; set; }
    DamageType DefendsFromDamageType { get; }
}


public class FireSwordAttackModifier
{

}