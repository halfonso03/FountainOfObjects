using TheFinalBattle.Actions.AttackActions;
using TheFinalBattle.Characters;
using TheFinalBattle.Item;
using TheFinalBattle.Player;


//for (var i = 0; i < 100; i++)
//{
//    var t = StandardAttackAction.GetAttackSuccessProbability();

//    Console.WriteLine(t);
//    //if (StandardAttackAction.GetAttackSuccessProbability() == 1)
//    //{
//    //    ones++;
//    //}
//    //else
//    //{
//    //    zeroes++;
//    //}

//}
//Console.WriteLine(ones);
//Console.WriteLine(zeroes);




var trueProgrammer = new HeroCharacter()
{
    Label = "The True Programmer",
    AttackModifier = new ObjectSightAttackModifier()
};
var vinFletcher = new HeroCharacter(15) { Label = "Vin Fletcher" };

var heroSword = new Sword();
var heroDagger = new Dagger();
var bow = new Bow(vinFletcher);

var mylara = new HeroCharacter(12) { Label = "Skorin" };
var cOfc= new CanonOfConsolas(mylara);




var heroParty = new Party(
        [mylara])
{
    Items = [new HealthPotion(), new HealthPotion(), new HealthPotion()]
};
heroParty.AttackGear.Add(heroSword);
heroParty.AttackGear.Add(heroDagger);



var player1 = new Player([heroParty]) { PlayerType = PlayerType.Human };

// battle 1 party setup
//var battle1Skeleton1 = new Skeleton();
//var battle1Amarok1 = new StoneAmarok();
//var dagger = new Dagger();
//var hammer = new Hammer();



//var battle1Party = new Party([battle1Amarok1, battle1Skeleton1])
//{
//    PartyType = PartyType.Villian,
//    Items = [new HealthPotion(), new Scarf(), new Scarf()]
//};
//battle1Party.AttackGear.Add(dagger);
//battle1Party.AttackGear.Add(hammer);


// battle 2 party setup
//var battle2Skeleton1 = new Skeleton();
//var battle2Skeleton1Dagger = new Dagger();
//var battle2Skeleton2 = new Skeleton();
//var battle2Skeleton2Dagger = new Dagger();

//var battle2Party = new Party(
//    [
//        battle2Skeleton1,
//        battle2Skeleton2
//    ])
//{
//    PartyType = PartyType.Villian,
//    Items = [new HealthPotion()]
//};
//battle2Party.AttackGear.Add(battle2Skeleton1Dagger);
//battle2Party.AttackGear.Add(battle2Skeleton2Dagger);


// battle 3 party setup
//var battle3Party = new Party([new UncodedOne()]) { PartyType = PartyType.Villian };
var battle3Party = new Party([new Skeleton()]) { PartyType = PartyType.Villian };


var player2 = new Player(
    [        
        battle3Party
    ]);


new BattleRunner(player1, player2, new ConsoleUserInteractor()).Run();
