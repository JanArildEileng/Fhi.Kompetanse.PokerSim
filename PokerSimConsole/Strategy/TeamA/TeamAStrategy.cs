using PokerSimConsole.Strategy;

namespace PokerSimConsole.TeamA
{
    internal class TeamAStrategy : IBettingStrategy
    {
        public BettingChoiceRecord PlaceBet(Player player, int buyInAmount, IList<Card> communityCards, int currentPot)
        {
            /*  Is PlayFlop ? */
            if (communityCards.Any())
            {
                string hand = player.ToHand(communityCards);
            }
            else
            {
                string twoCards = player.ToHand(communityCards);
            }

             return Random.Shared.Next(0, 3) switch
            {
                0 => player.Fold(),
                1 => player.Call(),
                2 => player.Raise(Random.Shared.Next(2, Player.MaxRaise)),
                _ => throw new NotImplementedException()
            };

        }
    }
}
