
public static class CardDeckExtender
{
       public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source,
            Random random = default)
        {
            ArgumentNullException.ThrowIfNull(source);
            random ??= Random.Shared;
            T[] array = source.ToArray();
            random.Shuffle(array);
            foreach (T item in array) yield return item;
        }
}

internal class Dealer
{
    IList<Card> Cards { get; set; }
    int nextCardToDraw = 0;
    public Dealer()
    {
        Cards = CreateAllCardsInDeck();
    }
    public  void ShuffleCards()
    {
        Cards = Cards.Shuffle<Card>().ToList();    
        nextCardToDraw = 0;
    }

    public  void DealPrivateCards(List<Player> players)
    {
        for (int i = 0; i < 2; i++)
        {
            foreach (Player player in players)
            {
                player.AddCard(GetNext());
            }
        }
    }

    public IList<Card> DealCommunityCards()
    {
        return GetNext3();
    }

    private static IList<Card> CreateAllCardsInDeck()
    {
        IList<Card> cards = new List<Card>();
        foreach(var suit in Card.Suits)
        {
            for(int rank=2; rank<=14;rank++)
            {
                Card card = new Card(suit, rank);
                cards.Add(card);
            }
        }

        return cards;
    }

    private Card GetNext()
    {
        return Cards[nextCardToDraw++];
    }

    private IList<Card> GetNext3()
    {
        Card[] result =
        {
            GetNext(),
            GetNext(),
            GetNext(),
        };
        return result;
   }
}
