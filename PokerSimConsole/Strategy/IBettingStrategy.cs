
namespace PokerSimConsole.Strategy
{
    internal interface IBettingStrategy
    {
         BettingChoiceRecord PlaceBet(Player player,  int buyInAmount, IList<Card>  communityCards, int currentPot);

    }
}
