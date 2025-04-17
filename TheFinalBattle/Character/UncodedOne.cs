using TheFinalBattle.Actions;
using TheFinalBattle.Actions.AttackActions;

namespace TheFinalBattle.Characters;

public class UncodedOne : Character
{
    public override CharacterAction[] Actions { get; init; } = [new NothingAction(), new Unravel()];
    public override string Name { get; set; } = "The Un-coded One";
    public override string Label { get; set; } = string.Empty;
    public override CharacterAction StandardAttack => Actions[1];
    public override int InitialHP => 30;
    public override int MaximumHP => 30;
    public override IAttackModifier? AttackModifier { get; set; } = null;
    public UncodedOne()
    {
        CurrentHealth = InitialHP;
    }      
}
