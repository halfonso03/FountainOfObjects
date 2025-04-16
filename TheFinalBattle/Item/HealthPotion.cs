using TheFinalBattle.Characters;

public class HealthPotion : IItem
{
    private bool _used = false;
    private readonly int HEALTH_INCREASE = 10;
    
    public string Name { get; } = "Health Potion";
    
    public bool Used { get { return _used; } }
    
    public void UseItem(Character character)
    {
        character.IncreaseHealthBy(HEALTH_INCREASE);
        _used = true;
    }
}