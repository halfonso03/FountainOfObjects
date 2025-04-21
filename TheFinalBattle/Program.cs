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


var trueProgrammer = new HeroCharacter()
{
    Label = "The True Programmer",
    DefenseModifier = new ObjectSightAttackModifier()
};
var vinFletcher = new HeroCharacter(15) { Label = "Vin Fletcher" };

var heroSword = new BroadSword(trueProgrammer);
var flamingSword = new FlamingSword();
var heroDagger1 = new Dagger();
var heroDagger2 = new Dagger();
var bow = new Bow(vinFletcher);

var mylara = new HeroCharacter(12) { Label = "Skorin" };
var cOfc= new CanonOfConsolas(mylara);




var heroParty = new Party(
        [trueProgrammer])
{
    Items = [new HealthPotion(), new HealthPotion(), new HealthPotion()]
};
//heroParty.AttackGear.Add(heroSword);
heroParty.AttackGear.Add(heroDagger1);
heroParty.AttackGear.Add(heroDagger2);



var player1 = new Player([heroParty]) { PlayerType = PlayerType.Human };



//battle 1 party setup
var battle1Skeleton1 = new Skeleton();
var battle1Skeleton2 = new Skeleton();
//var battle1Amarok1 = new StoneAmarok();
var dagger1 = new Dagger();
var dagger2 = new Dagger();
var dagger3 = new Dagger(battle1Skeleton2);
var hammer = new Hammer();



var battle1Party = new Party([battle1Skeleton1])
{
    PartyType = PartyType.Villian,
    Items = [new HealthPotion()]
};
battle1Party.AttackGear.Add(dagger1);
battle1Party.AttackGear.Add(dagger2);
//battle1Party.AttackGear.Add(hammer);


// battle 2 party setup
var battle2Skeleton1 = new Skeleton();
var battle2Skeleton1Dagger = new Dagger(battle2Skeleton1);
var battle2Skeleton2 = new Skeleton();
var battle2Skeleton2Dagger = new Dagger();

var battle2Party = new Party(
    [
        battle2Skeleton1,
        battle2Skeleton2
    ])
{
    PartyType = PartyType.Villian,
    Items = [new HealthPotion()]
};



// battle 3 party setup
var battle3Party = new Party([new UncodedOne()]) { PartyType = PartyType.Villian
, Items = [new HealthPotion()]};
//var battle3Party = new Party([new StoneAmarok()]) { PartyType = PartyType.Villian };


var player2 = new Player(
    [
    battle1Party,
    battle2Party,
        battle3Party
    ]);


new BattleRunner(player1, player2, new ConsoleUserInteractor()).Run();
