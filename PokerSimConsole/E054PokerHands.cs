
namespace PokerSimConsole;
 public enum PokerHandRanking
{
    HighCard, OnePair, TwoPairs, ThreeofaKind, Straight, Flush, FullHouse, FourofaKind, StraightFlush
}

public class E054PokerHands 
{
     public (bool,bool) FirstPlayerWins( string firstHand,string secondHand )
    {
        List<int> firstHandSortedValues = GetSortedCardValues(firstHand);
        List<int> secondHandSortedValues = GetSortedCardValues(secondHand);

        PokerHandRanking firstPlayerRank = RankPokerHand(firstHandSortedValues,firstHand);
        PokerHandRanking secondPlayerRank = RankPokerHand(secondHandSortedValues,secondHand);

        if (firstPlayerRank != secondPlayerRank)
            return (firstPlayerRank > secondPlayerRank,false);

        return FirstPlayerBestHandWhenEqual(firstHandSortedValues,secondHandSortedValues, firstPlayerRank);
    }

    public static  PokerHandRanking RankPokerHand( List<int> values,string hand)
    {
        var disinctCardValues = values.Distinct().Count();

        switch (disinctCardValues)
        {
            case 5:  //highcard , straight, flush
                {
                    Func<List<int>, bool> IsStraight = values => ((values[0] + 4) == values[4]) || (values[3] == 5 && values[4] == 14);
                    Func<string, bool> IsFlush = hand => (hand[1] == hand[4] && hand[1] == hand[7] && hand[1] == hand[10] && hand[1] == hand[13]);

                    var hasFlush = IsFlush(hand);

                    if (IsStraight(values))
                    {
                        if (hasFlush)
                            return PokerHandRanking.StraightFlush;
                        else
                            return PokerHandRanking.Straight;
                    }
                    if (hasFlush)
                    {
                        return PokerHandRanking.Flush;
                    }
                    return PokerHandRanking.HighCard;
                }

            case 4: //1 pair 
                {
                    return PokerHandRanking.OnePair;
                }

            case 3: //2 pair , ThreeofaKind
                {
                   // if (HasMultiKinds(values, 3))
                     if (values.GroupBy(e => e).Any(g => g.Count() == 3))

                        return PokerHandRanking.ThreeofaKind;
                    else
                        return PokerHandRanking.TwoPairs;
                }
            case 2:  //FourofaKind, FullHouse
                {
                      if (values.GroupBy(e => e).Any(g => g.Count() == 4))
                        return PokerHandRanking.FourofaKind;
                    else
                        return PokerHandRanking.FullHouse;
                }
        }

        //skal aldri komme her
        throw new NotImplementedException();
    }

    public static  List<int> GetSortedCardValues(string hand)
    {
        var l = new List<int>{
          ConvertCardValue(hand[0]),
          ConvertCardValue(hand[3]),
         };

         if (hand.Length>6)
        {
            l.Add(ConvertCardValue(hand[6]));
            l.Add(ConvertCardValue(hand[9]));
            l.Add(ConvertCardValue(hand[12]));
        }
        
        l.Sort();
        return l;
    }

    private (bool,bool isDraw) FirstPlayerBestHandWhenEqual( List<int> cardvalues1, List<int> cardvalues2, PokerHandRanking value)
    {
        switch (value)
        {
            case PokerHandRanking.HighCard:
            case PokerHandRanking.Straight:
            case PokerHandRanking.StraightFlush:
            case PokerHandRanking.Flush:
                {
                    for (int i = 4; i >= 0; i--)
                    {
                        if (cardvalues1[i] != cardvalues2[i])
                            return (cardvalues1[i] > cardvalues2[i],false);
                    }

                    return (false,true); //draw
                }
            case PokerHandRanking.FourofaKind:
                {
                    int fourValue1 = cardvalues1.GroupBy(e => e).Where(g => g.Count() == 4).First().Key;
                    int fourValue2 = cardvalues2.GroupBy(e => e).Where(g => g.Count() == 4).First().Key;

                    if (fourValue1 != fourValue2)
                        return (fourValue1 > fourValue2,false);

                     return FirstPlayerBestHandWhenEqual(cardvalues1, cardvalues2, PokerHandRanking.HighCard);
                }
            case PokerHandRanking.FullHouse:
               {
                    int treesValue1 = cardvalues1.GroupBy(e => e).Where(g => g.Count() == 3).First().Key;
                    int treesValue2 = cardvalues2.GroupBy(e => e).Where(g => g.Count() == 3).First().Key;

                    if (treesValue1 != treesValue2)
                        return (treesValue1 > treesValue2,false);

                    int pairValue1 = cardvalues1.GroupBy(e => e).Where(g => g.Count() == 2).First().Key;
                    int pairValue2 = cardvalues2.GroupBy(e => e).Where(g => g.Count() == 2).First().Key;

                      if (pairValue1 != pairValue2)
                        return (pairValue1 > pairValue2,false);
                      return(false, true); //draw
                }
            
            
            case PokerHandRanking.ThreeofaKind:
                {
                    int treesValue1 = cardvalues1.GroupBy(e => e).Where(g => g.Count() == 3).First().Key;
                    int treesValue2 = cardvalues2.GroupBy(e => e).Where(g => g.Count() == 3).First().Key;

                    if (treesValue1 != treesValue2)
                        return (treesValue1 > treesValue2,false);

                     return FirstPlayerBestHandWhenEqual(cardvalues1, cardvalues2, PokerHandRanking.HighCard);
                }
            case PokerHandRanking.TwoPairs:
                {
                    var pairValues1 = cardvalues1.GroupBy(e => e).Where(g => g.Count() == 2).Select(g => g.Key).Order().ToList();
                    var pairValues2 = cardvalues2.GroupBy(e => e).Where(g => g.Count() == 2).Select(g => g.Key).Order().ToList();
                    //high pair
                    if (pairValues1[1] != pairValues2[1])
                        return (pairValues1[1] > pairValues2[1],false);
                    //low pair
                    if (pairValues1[0] != pairValues2[0])
                        return (pairValues1[0] > pairValues2[0],false);

                    return FirstPlayerBestHandWhenEqual(cardvalues1, cardvalues2, PokerHandRanking.HighCard);
                }
            case PokerHandRanking.OnePair:
                {
                    int pairValue1 = cardvalues1.GroupBy(e => e).Where(g => g.Count() == 2).First().Key;
                    int pairValue2 = cardvalues2.GroupBy(e => e).Where(g => g.Count() == 2).First().Key;
                    if (pairValue1 != pairValue2)
                        return (pairValue1 > pairValue2,false);

                    return FirstPlayerBestHandWhenEqual(cardvalues1, cardvalues2, PokerHandRanking.HighCard);
                }

            default:
                throw new Exception($"Not implemented test for Both {value}");
        }
    }

    private static  int ConvertCardValue(char a)
    {
        return a switch
        {
            '2' => 2,
            '3' => 3,
            '4' => 4,
            '5' => 5,
            '6' => 6,
            '7' => 7,
            '8' => 8,
            '9' => 9,
            'T' => 10,
            'J' => 11,
            'Q' => 12,
            'K' => 13,
            'A' => 14,
            _ => throw new Exception("Illegal cardvalue"),
        };
    }

}
