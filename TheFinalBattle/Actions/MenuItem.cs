
namespace TheFinalBattle.Actions;

public record MenuItem (int Id, string Description, bool IsEnabled, CharacterAction Action);

public record MenuItemMultipleAction(int Id, string Description, bool IsEnabled, CharacterAction[] Action);
