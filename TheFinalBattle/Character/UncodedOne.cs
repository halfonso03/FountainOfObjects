using TheFinalBattle.Actions;
using TheFinalBattle.Actions.AttackActions;

namespace TheFinalBattle.Characters;

public class UncodedOne : Character
{
    public override CharacterAction[] Actions { get; init; } = [new NothingAction(), new Unravel()];
    public override string Name { get; set; } = "The Uncoded One";
    public override string Label { get; set; } = string.Empty;
    public override CharacterAction StandardAttack => Actions[1];
    public override int InitialHP => 15;
    public override int MaximumHP => 15;
    public UncodedOne()
    {
        CurrentHealth = InitialHP;
    }      
}
