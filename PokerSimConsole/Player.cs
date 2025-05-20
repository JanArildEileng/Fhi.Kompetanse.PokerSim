using PokerSimConsole.Strategy;
using System;

internal class Player
{
    public const int MaxRaise=10;

    private readonly IBettingStrategy bettingStrategy;
    public string Name { get; }
    public int Chips { get; set; }
    public IList<Card> PrivateCards { get; set; } = [];
    public bool InGame { get; internal set; }
    public int CurrentPotInvest { get; internal set; }


    bool raiseAllow;
    int buyInAmount;

    public Player(string name,  IBettingStrategy bettingStrategy)
    {
        Name = name;
        this.bettingStrategy = bettingStrategy;
    }

    internal void Reset()
    {
        InGame = true;
        CurrentPotInvest = 0;
        PrivateCards = [];
    }

    internal void AddCard(Card card)
    {
        PrivateCards.Add(card);
    }


    internal BettingChoiceRecord PlaceBet(int seqnr, int buyInAmount, IList<Card> enumerable, bool raiseAllow, int currentPot)
    {
        this.raiseAllow = raiseAllow;
        this.buyInAmount = buyInAmount;

        if (seqnr == 0)
        {
            return Call();
        }

        if (seqnr == 1)
        {
            return Call();
        }

        return bettingStrategy.PlaceBet(this, buyInAmount, enumerable, currentPot);
    }

    public BettingChoiceRecord Call()
    {
        int delta = (buyInAmount - CurrentPotInvest);
        CurrentPotInvest = buyInAmount;
        Chips -= delta;
        return new BettingChoiceRecord(BettingChoiceType.Call, 0);
    }

    public BettingChoiceRecord Fold()
    {
        int delta = (buyInAmount - CurrentPotInvest);
        if (delta == 0)
        {
             return new BettingChoiceRecord(BettingChoiceType.Call, 0);
        }   

        InGame = false;
        return new BettingChoiceRecord(BettingChoiceType.Fold, 0);
    }


    public BettingChoiceRecord Raise(int raiseamount)
    {
        if (!raiseAllow)
        {
              return  Call();
        }

        //first buyin
        int shortage = buyInAmount - CurrentPotInvest;
        Chips -= shortage;

        //cant raise more than available chips
        raiseamount = Chips > raiseamount ? raiseamount : Chips;

        //cant raise more than MaxRaise
         raiseamount = raiseamount < MaxRaise ? raiseamount : MaxRaise;


        CurrentPotInvest = buyInAmount + raiseamount;
        Chips -= raiseamount;

        return new BettingChoiceRecord(BettingChoiceType.Raise, raiseamount);
    }


    internal string ToHand(IList<Card> commonCards)
    {
        string s1 = PrivateCards[0].ToString();
        string s2 = PrivateCards[1].ToString();
        if (commonCards.Count == 0)
            return $"{s1} {s2}";

        string s3 = commonCards[0].ToString();
        string s4 = commonCards[1].ToString();
        string s5 = commonCards[2].ToString();

        return $"{s1} {s2} {s3} {s4} {s5}";
    }
}