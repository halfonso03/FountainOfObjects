
using TheFinalBattle.Actions;
using TheFinalBattle.Actions.AttackActions;

namespace TheFinalBattle.Characters;

public class StoneAmarok : Character
{
    public override string Name { get; set; } = "STONE AMAROK";
    public override string Label { get; set; } = string.Empty;
    public override CharacterAction StandardAttack => Actions[1];
    public override int InitialHP => 4;
    public override int MaximumHP => 4;
    public override CharacterAction[] Actions { get; init; } = [new NothingAction(), new Bite()];
    public override IAttackModifier? AttackModifier { get; set; } =  new StoneArmorAttackModifier();
    public StoneAmarok()
    {
        CurrentHealth = InitialHP;
    }
}