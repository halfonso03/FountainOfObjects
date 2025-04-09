
public enum PlayerChoice
{
    None,
    Rock,
    Paper,
    Scissors
}


public class RockPaperScissorsTester()
{

}

public class RockPaperScissorsGame
{
    private string[] _playerNames = new string[2];    
    private int _roundNumber = 1;

    private List<(int, string, PlayerChoice)> _winners = new List<(int, string, PlayerChoice)>();

    public void StartGame()
    {
        GetPlayerNames();

        BeginGame();

        DisplayWinners();
    }

    private void BeginGame()
    {
        var keepPlaying = true;
        var choicesCollected = 0;
        var player1ChoiceMade = false;
        PlayerChoice currentPlayerChoice = PlayerChoice.None;
        PlayerChoice[] playerChoices = new PlayerChoice[2];


        do
        {
            do
            {
                var currentPlayername = player1ChoiceMade ? _playerNames[1] : _playerNames[0];

                Console.WriteLine($"{currentPlayername}, your turn. Choose (R)ock, (P)aper, or (S)cissors");

                var input = Console.ReadLine();

                if (!ValidChoiceMade(input))
                {
                    Console.WriteLine("Invalid choice. Try again.");
                }
                else
                {
                    var currentPlayerIndex = playerChoices[0] == PlayerChoice.None ? 0 : 1;
                    player1ChoiceMade = currentPlayerIndex == 0 && !player1ChoiceMade;

                    playerChoices[currentPlayerIndex] = input switch
                    {
                        "r" => PlayerChoice.Rock,
                        "p" => PlayerChoice.Paper,
                        "s" => PlayerChoice.Scissors,
                    };
                    choicesCollected++;

                    Console.WriteLine("Selection made.\n");
                }                                     
            }
            while (choicesCollected < 2);



            int winnerIndex = CalculateWinner(_playerNames, playerChoices);

            Console.WriteLine($"Winner: {_playerNames[winnerIndex]}");

            _winners.Add(
                (_roundNumber, _playerNames[winnerIndex], playerChoices[winnerIndex]));
            


            Console.WriteLine("Another Round Y/N?");

            var keepPlayingChoice = Console.ReadLine();

            if (!String.IsNullOrEmpty(keepPlayingChoice))
            {
                keepPlaying = keepPlayingChoice.ToLower() != "n";
            }

            if (keepPlaying)
            {
                Console.WriteLine("----------------------------------------");
                player1ChoiceMade = false;
                playerChoices[0] = PlayerChoice.None;
                playerChoices[1] = PlayerChoice.None;
                choicesCollected = 0;
                _roundNumber++;
            }
           
        }

        while (keepPlaying);       
    }

    private int CalculateWinner(string[] playerNames, PlayerChoice[] playerChoices)
    {
        int winnerIndex = checkWinner(playerChoices[0], playerChoices[1]) ? 0 : -1;

        if (winnerIndex == -1)
        {
            winnerIndex = checkWinner(playerChoices[1], playerChoices[0]) ? 1 : -1;
        }

        return winnerIndex;
    }

    private bool checkWinner(PlayerChoice playerChoice1, PlayerChoice playerChoice2)
    {
        if (playerChoice1 == PlayerChoice.Rock && playerChoice2 == PlayerChoice.Scissors)
        {
            return true;
        }

        if (playerChoice1 == PlayerChoice.Scissors && playerChoice2 == PlayerChoice.Paper)
        {
            return true;
        }

        if (playerChoice1 == PlayerChoice.Paper && playerChoice2 == PlayerChoice.Rock)
        {
            return true;
        }

        return false;
    }

    private void DisplayWinners()
    {
        foreach (var winner in _winners)
        {
            Console.WriteLine($"Round: {winner.Item1} Winner: {winner.Item2}, Choice: {winner.Item3}");
        }
    }

    private bool ValidChoiceMade(string? input)
    {
        if (!string.IsNullOrEmpty(input))
        {
            if (input.Trim().Length == 1)
            {
                if (input.ToLower() != "r" && input.ToLower() != "p" && input.ToLower() != "s")
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void GetPlayerNames()
    {

        var namesCollected = false;
        var player1NameCollected = false;

        do
        {
            var playerNameToCollect = (!player1NameCollected) ? 1 : 2;

            Console.WriteLine($"Enter player {playerNameToCollect} name:");

            var name = Console.ReadLine();

            if (!String.IsNullOrEmpty(name))
            {
                if (!player1NameCollected)
                {
                    _playerNames[0] = name;
                    player1NameCollected= true;
                }
                else
                {
                    _playerNames[1] = name;
                    namesCollected = true;
                }                               
            }
        }
        while (!namesCollected);
    }
}
//do
//{
//    Console.WriteLine("Enter a password");

//    var input = Console.ReadLine();

//    var result = PasswordValidator.ValidatePassword(input);

//    Console.WriteLine(result);
//}
//while (true);



//var passcode = "";

//Door? d = null;

//Console.WriteLine("Enter a passcode to create a new door:");

//while (passcode == "")
//{
//    passcode = Console.ReadLine();

//    if (passcode != string.Empty && passcode != null)
//    {
//        d  = new Door(passcode);
//    }
//}

//Console.WriteLine($"new door created with passcode: {passcode}.");
//Console.WriteLine("");



//if (d != null)
//{
//    do
//    {
//        Console.WriteLine("Enter a command (O = open, C = close, L = lock, U = unlock, cp = Change Passcode):");
//        string? input = Console.ReadLine();


//        if (input == "cp")
//        {
//            string command = "";

//            Console.WriteLine("Enter old passcode (enter cancel to exit changing passcode):");

//            do
//            {
//                string? oldPasscode = Console.ReadLine();

//                if (oldPasscode == "cancel")
//                {
//                    command = "";
//                }
//                else
//                {
//                    if (!string.IsNullOrEmpty(oldPasscode))
//                    {
//                        Console.WriteLine("Enter a new passcode (enter cancel to exit changing passcode):");

//                        string? newPasscode = Console.ReadLine();

//                        do
//                        {
//                            if (newPasscode == "cancel")
//                            {
//                                command = "";
//                            }
//                            else if (!String.IsNullOrEmpty(newPasscode))
//                            {
//                                if (d.ChangePasscode(newPasscode, oldPasscode))
//                                {
//                                    Console.WriteLine($"Passcode changed to {newPasscode}");
//                                    command = "";
//                                }
//                                else
//                                {
//                                    Console.WriteLine("Invalid old passcode. ");
//                                    command = "";                                    
//                                }
//                            }

//                        } while (command == "cp");
//                    }
//                }             
//            } while (command == "cp");

//            Console.WriteLine("Exiting change passcode.");
//        }
//        else if (input.ToLower() == "o")
//        {
//            if (d.Open())
//            {
//                Console.WriteLine("Door has been opened!");
//            }
//            else
//            {
//                Console.WriteLine("Opening door failed!");
//            }
//        }
//        else if (input == "c")
//        {
//            if (d.Close())
//            {
//                Console.WriteLine("Door has been closed!");
//            }
//            else
//            {
//                Console.WriteLine("Closing door failed!");
//            }
//        }
//        else if (input == "l")
//        {
//            if (d.Lock())
//            {
//                Console.WriteLine("Door has been locked!");
//            }
//            else
//            {
//                Console.WriteLine("Locking door failed!");
//            }
//        }
//        else if (input == "u")
//        {
//            if (d.Unlock())
//            {
//                Console.WriteLine("Door has been unlocked!");
//            }
//            else
//            {
//                Console.WriteLine("Unlocking door failed!");
//            }
//        }
//        else
//        {
//            Console.WriteLine("Invalid command! Try again.");
//        }
//    } while (true);


//}

//public class Door
//{
//    public bool IsLocked { get; internal set; } = false;
//    public bool IsOpen { get; internal set; } = true;
//    public bool IsClosed { get; internal set; } = false;
//    public string Passcode { get; internal set; }

//    public Door(string passcode)
//    {
//        Passcode = passcode;
//    }

//    public bool Close()
//    {
//        if (IsOpen && !IsClosed)
//        {
//            IsClosed = true;
//            IsOpen = false;
//            return true;
//        }
//        return false;
//    }

//    public bool Open()
//    {
//        if (!IsOpen && IsClosed && !IsLocked)
//        {
//            IsOpen = true;
//            IsClosed = false;
//            return true;
//        }

//        return false;
//    }

//    public bool Lock()
//    {
//        if (IsClosed && !IsLocked)
//        {
//            IsLocked = true;
//            return true;
//        }
//        return false;
//    }

//    public bool Unlock()
//    {
//        if (IsLocked)
//        {
//            IsLocked = false;
//            return true;    
//        }
//        return false;   
//    }

//    public bool ChangePasscode(string newPasscode, string oldPasscode)
//    {
//        if (oldPasscode.Trim() == Passcode)
//        {
//            Passcode = newPasscode;
//            return true;
//        }

//        return false;
//    }

//}

//var deck = new CardDeck();

//Console.ReadKey();



//public class CardDeck
//{

//    public Card[] Cards { get; } = new Card[56];

//    public CardDeck()
//    {
//        int counter = 0;

//        for (var i = 1; i <= 4; i++)
//        {
//            for (var j = 1; j <= 14; j++)
//            {
//                Cards[counter] = new Card((CardColor)i, (CardRank)j);
//                counter++;
//            }
//        }


//        foreach(var card in Cards)
//        {
//            Console.WriteLine($"{card.Color} {card.Rank}");
//        }
//    }
//}

//public class Card
//{
//    public CardColor Color { get; }
//    public CardRank Rank { get; }

//    public Card(CardColor cardColor, CardRank cardRank)
//    {
//        Color = cardColor;
//        Rank = cardRank;
//    }

//    public bool IsNumber()
//    {
//        if ((int)Rank <= 10 ) return true;
//        return false;
//    }

//    public bool IsSymbol()
//    {
//        if ((int)Rank > 10) return true;
//        return false;
//    }
//}

//public enum CardColor
//{
//    red = 1,
//    green,
//    blue,
//    yellow
//}

//public enum CardRank
//{
//    one = 1,
//    two,
//    three,
//    four,
//    five,
//    six,
//    seven,
//    eight,
//    nine,
//    ten,
//    dollar,
//    percent,
//    pow,
//    ampersand
//}



// color class


//var color1 = new Color(1,1,1);
//var color2 = Color.CreateGreen();


//Console.WriteLine($"Red: {color1.Red} Blue: {color1.Blue} Green: {color1.Green}");
//Console.WriteLine($"Red: {color2.Red} Blue: {color2.Blue} Green: {color2.Green}");


//public class Color
//{
//    public short Red { get; set; }
//    public short Green { get; set; }
//    public short Blue { get; set; }

//    public Color(short red, short green, short blue)
//    {
//        Red = red;
//        Green = green;
//        Blue = blue;
//    }

//    public static Color CreateWhite() => new(255,255, 255);
//    public static Color CreateBlack() => new(0, 0, 0);
//    public static Color CreateRed() => new(255, 0, 0);
//    public static Color CreateOrange() => new(255, 165, 0);
//    public static Color CreateYellow() => new(255, 255, 0);
//    public static Color CreateGreen() => new(0, 128, 0);
//    public static Color CreateBlue() => new(0, 0, 255);
//    public static Color CreatePurple() => new(128, 0, 128);    
//}