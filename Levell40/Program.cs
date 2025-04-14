




using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

new PotionMaker(new UserInterfaceLogger()).Start();


public class PotionMaker
{
    bool ruined;

    IUserInterfaceLogger _userInterfaceLogger;
    PotionType _currentPotionType = PotionType.Water;

    public PotionMaker(IUserInterfaceLogger userInterfaceLogger)
    {
        _userInterfaceLogger = userInterfaceLogger;
    }

    public void Start()
    {
        var isElixer = false;
        var completed = false;

        do
        {
            if (_currentPotionType == PotionType.Water)
            {
                _userInterfaceLogger.ShowMessageNewLine($"Your potion is now {_currentPotionType}.");
            }
            
            _userInterfaceLogger.WriteNewLine();
            _userInterfaceLogger.ShowMessageNewLine("Choose one of the ingredients to update your potion:");
            _userInterfaceLogger.ShowMessageNewLine("----------------------------------------------------");

            ShowIngredients();

            var input = _userInterfaceLogger.GetUerInput();
            var addedIngredient = (Ingredient)Convert.ToInt32(input);
            
            _userInterfaceLogger.ShowMessageNewLine($"Adding {addedIngredient}...");

            Thread.Sleep(400);

            if (_currentPotionType == PotionType.Water)
            {
                if (addedIngredient == Ingredient.Stardust)
                {
                    isElixer = true;
                    _currentPotionType = PotionType.Elixer;                    
                }
                else
                {
                    ruined = true;
                    isElixer = false;
                    _currentPotionType = PotionType.Ruined;
                }
            
            }
            else if (isElixer && !ruined)
            {
                _currentPotionType = updatePotion(addedIngredient);
            }

            ruined = _currentPotionType == PotionType.Ruined;

            if (!ruined)
            {
                _userInterfaceLogger.ShowMessageNewLine($"You now have {_currentPotionType}. Complete the potion? y/n");

                string input2 = _userInterfaceLogger.GetUerInput();

                if (input2!.ToLower() == "y")
                {
                    completed = true;
                }
                else if (input2!.ToLower() != "y" && input2!.ToLower() != "n")
                {
                    _userInterfaceLogger.ShowMessageNewLine("Invalid answer.");
                }
            }
            else
            {
                ruined = false;
                _userInterfaceLogger.ShowMessageNewLine("Mr. Potter, you have ruined the potion! :( Starting over");
                _currentPotionType = PotionType.Water;
            }

        }
        while (!completed);


        _userInterfaceLogger.ShowMessageNewLine($"Potion completed! Good bye.");


        Console.ReadKey();
    }

    private PotionType updatePotion(Ingredient addedIngredient)
    {
        return (addedIngredient, _currentPotionType) switch
        {
            (Ingredient.SnakeVenom, PotionType.Elixer) => PotionType.Poison,
            (Ingredient.DragonBreath, PotionType.Elixer) => PotionType.Flying,
            (Ingredient.ShadowGlass, PotionType.Elixer) => PotionType.Inivisibilty,
            (Ingredient.EyeshineGem, PotionType.Elixer) => PotionType.NightSight,
            (Ingredient.ShadowGlass, PotionType.NightSight) => PotionType.CloudyBrew,
            (Ingredient.EyeshineGem, PotionType.Inivisibilty) => PotionType.CloudyBrew,
            (Ingredient.Stardust, PotionType.CloudyBrew) => PotionType.Wraith,
            _ => PotionType.Ruined
        };
    }

    void ShowIngredients()
    {
        var indedients = Enum.GetValues( typeof( Ingredient ) ).Cast<Ingredient>().ToList();

        foreach (var ingredient in indedients)
        {
            _userInterfaceLogger.ShowMessageNewLine($"{(int)ingredient} = {ingredient.ToString()}");
        }
    }
}

public class UserInterfaceLogger : IUserInterfaceLogger
{
    public void ShowMessageNewLine(string message)
    {
        Console.WriteLine(message);
    }

    public string GetUerInput()
    {
        var valid = false;
        string? input;

        do
        {
            input = Console.ReadLine();

            if (!string.IsNullOrEmpty(input) && input is not null)
            {
                valid = true;
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
        }
        while (!valid);

        return input!;
    }

    public void WriteNewLine()
    {
        Console.WriteLine("\n");
    }
}




public enum PotionType
{
    Water = 1,
    Elixer,
    CloudyBrew,
    Poison,
    Flying,
    Inivisibilty,
    NightSight,
    Wraith,
    Ruined
}

public enum Ingredient
{
    Stardust = 1,
    SnakeVenom,
    DragonBreath,
    ShadowGlass,
    EyeshineGem,
}