using PokerSimConsole;
using System.Globalization;
using System.Numerics;
internal class PokerSimulator(Func<List<Player>> createPlayers, int numbeOfTournement, int maxGames, int chips)
{
    Dictionary<string, int> winners = new Dictionary<string, int>();

    internal void RunAllTournements()
    {
        List<Player> players = createPlayers();

        for (int i = 0; i < numbeOfTournement; i++)
        {
            players.Shuffle();

            players.ForEach(player => player.Chips = chips);

            RunTournement(players);
        }
    }

    private void RunTournement(List<Player> players)
    {
        Tournement tournement = new Tournement(players);
        string nameWinner = tournement.DecideWinner(maxGames);

        if (winners.ContainsKey(nameWinner))
        {
            winners[nameWinner] = winners[nameWinner] + 1;
        }
        else
            winners.Add(nameWinner, 1);
    }

    internal void DisplayWinnerStatistics(int numbeOfTournement)
    {
        Console.WriteLine("Winner Statistics...");
        Console.WriteLine("---");
        foreach (var win in winners.OrderByDescending(e => e.Value))
        {
            double number = 1.0 * win.Value / numbeOfTournement;

            Console.WriteLine($"({number.ToString("P2", CultureInfo.InvariantCulture)}) {win.Key} has {win.Value} wins   ");
        }
        Console.WriteLine("----");
    }
}