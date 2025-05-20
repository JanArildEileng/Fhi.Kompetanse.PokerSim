using PokerSimConsole.Jan;
using PokerSimConsole.SimpleRandom.RandomStrategy;
using PokerSimConsole.TeamA;
using PokerSimConsole.TeamB;
using PokerSimConsole.TeamC;

const int numbeOfTournement = 1000;
const int maxGames = 20;
const int Chips = 100;
const int MinNumberOfPlayers = 5;


Console.WriteLine($"Hello, PokerSim  !{ DateTime.Now.ToShortTimeString() } ");

PokerSimulator pokerSimulator = new PokerSimulator(CreatePlayers,numbeOfTournement,maxGames,Chips);
pokerSimulator.RunAllTournements();
pokerSimulator.DisplayWinnerStatistics(numbeOfTournement);

Console.WriteLine("Goodbye, PokerSim!");

static List<Player> CreatePlayers()
{
    List<Player> players =
    [
        //TODO : add more strategies    
       new Player(nameof(TeamAStrategy),new TeamAStrategy()),
       new Player(nameof(TeamBStrategy),new TeamBStrategy()),
       new Player(nameof(TeamCStrategy),new TeamCStrategy()),

    //   new Player(nameof(AlwaysCallStrategy),new AlwaysCallStrategy()),
    //   new Player(nameof(CallOrRaiseStrategy), new CallOrRaiseStrategy()),
       new Player(nameof(JanStrategy),new JanStrategy()),
    ];

    //add ramdomPlayers
    int i = 1;
    while (players.Count < MinNumberOfPlayers)
    {
          players.Add(new Player($"RandomPlayer {i++}", new RandomStrategy()));
    }

    return players;
}