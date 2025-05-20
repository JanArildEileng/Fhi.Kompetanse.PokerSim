namespace PokerSimConsole;
internal class Tournement(List<Player> players)
{
    internal string DecideWinner(int maxGames)
    {
        Dealer dealer = new Dealer();

        for (int i = 0; i < maxGames; i++)
        {
            dealer.ShuffleCards();
            players.ForEach(p => p.Reset());

            //any winners yet?
            var totalPositiveChips = players.Sum(e => e.Chips > 0 ? e.Chips : 0);
            //if any player has 75% og chips  -> wins
            if (players.Any(e => e.Chips > 0.75 *totalPositiveChips )) break;


            // only players with chips can play;
            Game game = new Game(dealer, players.Where(e => e.Chips > 0).ToList());
            var dealerPos= i % players.Count;

            game.PlayGame(dealerPos);
        }

        //winner is player with most chips...
        return players.OrderByDescending(p => p.Chips).First().Name;
    }
}