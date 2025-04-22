using TheFinalBattle.Actions.AttackActions;
using TheFinalBattle.Actions.AttackModifiers;
using TheFinalBattle.Characters;
using TheFinalBattle.Item;
using TheFinalBattle.Player;


int ones = 0;
int zeroes = 0;

//for (var j = 0; j < 10; j++)
//{
//    Console.WriteLine("Test " + j.ToString());
//    Console.WriteLine("----------------------------------------");
//    for (var i = 0; i < 300; i++)
//    {
//        //var t = StandardAttackAction.GetAttackSuccessProbability(90);
//        if (StandardAttackAction.GetS2(80))
//        {
//            ones++;
//        }
//        else
//        {
//            zeroes++;
//        }

//    }
//    Console.WriteLine(ones);
//    Console.WriteLine(zeroes);
//    Console.WriteLine((double)ones / (ones + zeroes));
//    Console.WriteLine((double)zeroes / (ones + zeroes));

//    ones = 0;
//    zeroes = 0;
//}


//Console.ReadKey();




new BattleRunner(new ConsoleUserInteractor()).Run();
