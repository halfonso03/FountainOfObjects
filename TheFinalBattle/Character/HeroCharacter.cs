
using TheFinalBattle.Actions;
using TheFinalBattle.Actions.AttackActions;
using TheFinalBattle.Actions.AttackModifiers;

namespace TheFinalBattle.Characters;

public class HeroCharacter : Character
{
    
    public override string Name { get; set; } = string.Empty;
    public override string Label { get; set; } = string.Empty;
    public override CharacterAction[] Actions { get; init; } = 
        [new NothingAction(), new Punch()];
    public override CharacterAction StandardAttack => Actions[1];
    public override int InitialHP { get; } = 25;
    public override int MaximumHP { get; } = 25;
    public override IDefenseModifier? DefenseModifier { get; set; } = null;
    public HeroCharacter(int HP = 0)
    {
        if (HP == 0)
        {
            CurrentHealth = InitialHP;
        }
        else
        {
            CurrentHealth = HP;
            MaximumHP = HP;
            InitialHP = HP;
        }
    }

    public override string ToString()
    {
        return this.Name + " " + this.Label;
    }

}
