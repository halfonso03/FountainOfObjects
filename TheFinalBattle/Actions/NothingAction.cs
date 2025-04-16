namespace TheFinalBattle.Actions;

public class NothingAction : StandardAttackAction
{    
    public override string Name { get; } = "NOTHING";    
    public override int DamageDealt() => 0;
}
