using PokerSimConsole.Strategy;


namespace PokerSimConsole.Jan;

internal class JanStrategy : IBettingStrategy
{
    public BettingChoiceRecord PlaceBet(Player player, int buyInAmount, IList<Card> communityCards,  int currentPot)
    {
        string hand = player.ToHand(communityCards);
        List<int> sortedCardValues = E054PokerHands.GetSortedCardValues(hand);
     
        int delta = (buyInAmount - player.CurrentPotInvest);

        /*  Is PlayFlop ? */
        if (communityCards.Any())
        {
            PokerHandRanking pokerHandRanking = E054PokerHands.RankPokerHand(sortedCardValues, hand);

            if (pokerHandRanking > PokerHandRanking.TwoPairs)
            {
                return player.Raise(Player.MaxRaise);
            }

            if (pokerHandRanking >= PokerHandRanking.TwoPairs)
            {
                return player.Raise(5);
            }

            if (pokerHandRanking >= PokerHandRanking.OnePair)
            {
                return player.Call();
            }
      
            if (delta > 0.15 * currentPot)
            {
                return player.Fold();
            }

        }
        else //Flop
        {
   
            if (sortedCardValues[0] >= 12)
            {
                return player.Raise(Player.MaxRaise);
            }

            if (sortedCardValues[0] >= 10 || sortedCardValues[1]> 12 )
            {
                return player.Raise(5);
            }

            if (sortedCardValues[0] == sortedCardValues[1])
            {
                return player.Raise( Player.MaxRaise);
            }

            if ( delta>6)
            {
                return player.Fold();
            }   

        }

      return player.Call();
    }
}
