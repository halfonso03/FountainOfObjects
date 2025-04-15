namespace TheFinalBattle.Actions;

public class NothingAction : AttackAction
{    
    public override string Name { get; set; } = "NOTHING";
    public override int DamageDealt() => 0;
}
