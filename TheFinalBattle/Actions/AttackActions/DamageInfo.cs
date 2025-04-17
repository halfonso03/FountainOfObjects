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
    public DamageType? DefenderDamageTypeModifier { get; internal set; }
}
