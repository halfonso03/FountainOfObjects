using TheFinalBattle.Enums;

namespace TheFinalBattle.Actions.AttackActions;

public class DamageInfo
{
    public bool InflictedDamageModified { get; private set; } = false;
    public bool AttackMissed { get; set; } = false;

    private int? inflictedDamage = null;
    public int? InflictedDamage
    {
        get => inflictedDamage;
        set
        {
            if (inflictedDamage.HasValue)
            {
                InflictedDamageModified = true;
            }
            inflictedDamage = value;            
        }
    }

    public DamageType AttackerDamageType { get; internal set; }
    public DamageType? DefenderDamageType { get; internal set; }
    public DamageDealtSource DamageDealtSource { get; internal set; }
    public int StrikeCount { get; set; } = 1;
    public bool DefenseModifierUsed { get; internal set; } = false;
}
