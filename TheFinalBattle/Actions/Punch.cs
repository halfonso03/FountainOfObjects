namespace TheFinalBattle.Actions; 
public class Punch : StandardAttackAction
{
    public override string Name { get; } = "PUNCH";
    public override int DamageDealt() => 1;
}
