namespace TheFinalBattle.Actions; 
public class Punch : AttackAction
{
    public override string Name { get; set; } = "PUNCH";
    public override int DamageDealt() => 1;
}
