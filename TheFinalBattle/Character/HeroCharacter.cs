
using TheFinalBattle.Actions;

namespace TheFinalBattle.Characters;

public class HeroCharacter : Character
{
    public override string Name { get; set; } = string.Empty;
    public override CharacterAction[] Actions { get; init; } = 
        [new NothingAction(), new Punch()];
    public override CharacterAction StandardAttack => Actions[1];
    public override int InitialHP { get; } = 25;
    public override int MaximumHP { get; } = 25;
    public HeroCharacter()
    {
        CurrentHealth = InitialHP;
    }
    
}
