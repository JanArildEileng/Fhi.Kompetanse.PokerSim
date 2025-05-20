using PokerSimConsole.Strategy;

namespace PokerSimConsole.SimpleRandom.RandomStrategy;

internal class CallOrRaiseStrategy : IBettingStrategy
{
    public BettingChoiceRecord PlaceBet(Player player, int buyInAmount, IList<Card>  communityCards, int currentPot)
    {
        if (buyInAmount - player.CurrentPotInvest > 0)
        {
            return player.Call();
        }
        return player.Raise(Random.Shared.Next(2, 10));
    }
}
