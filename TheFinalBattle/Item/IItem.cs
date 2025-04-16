using TheFinalBattle.Characters;

public interface IItem
{
    string Name { get; }
    void UseItem(Character character);
    bool Used { get; }
}
