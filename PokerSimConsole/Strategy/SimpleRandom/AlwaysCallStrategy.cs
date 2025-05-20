using PokerSimConsole.Strategy;

namespace PokerSimConsole.SimpleRandom.RandomStrategy;
internal class AlwaysCallStrategy : IBettingStrategy
{
    public BettingChoiceRecord PlaceBet(Player player, int buyInAmount, IList<Card>  communityCards,  int currentPot)
    {
        return player.Call();
    }
}
