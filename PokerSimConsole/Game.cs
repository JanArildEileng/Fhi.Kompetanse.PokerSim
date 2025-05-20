using PokerSimConsole;

internal class Game(Dealer dealer, List<Player> players)
{
    const int MaxRaiseInGame= 3;
    int seqnr = 0;
    IList<Card> communityCards = [];

    internal void PlayGame(int dealerPos)
    {
        PlayPreFlop(dealerPos);
       
        PlayFlop(dealerPos);

        //the river - one more community-card
        //the turn -one more community-card
        CheckPotAmout();
        ShowDown(communityCards);
    }

    private void PlayPreFlop(int dealerPos)
    {
        dealer.DealPrivateCards(players);
        BettingRound(dealerPos, []);
    }

     private void PlayFlop(int dealerPos)
    {
        communityCards = dealer.DealCommunityCards();
        BettingRound(dealerPos,communityCards);
    }

    internal void BettingRound(int dealerPos, IList<Card> communityCards)
    {
        int buyInAmount = 2;
        int numberOfRaise = 0;
        bool someOneRaised = false;
        int lastPlayWhoRaised=-1;

        bool raiseAllow(int i) => numberOfRaise < MaxRaiseInGame && (lastPlayWhoRaised != i);
        Player getPlayer(int i) => players[(dealerPos + i) % players.Count()];
        int getCurrentPot = players.Sum(p => p.CurrentPotInvest);
        
        //get bids from player , until no more raise
        do
        {
            someOneRaised = false;
            for (int i = 1; i <= players.Count(); i++)
            {
                if (i == lastPlayWhoRaised)
                        break;

                if (players.Where(p => p.InGame).Count() < 2) break;

                Player player = getPlayer(i);
                if (!player.InGame) continue;
               
                BettingChoiceRecord choiceRecord  = player.PlaceBet(seqnr++, buyInAmount, communityCards, raiseAllow(i),getCurrentPot);

                if (choiceRecord.choicetype == BettingChoiceType.Raise)
                {
                     buyInAmount += choiceRecord.raiseamount;
                     someOneRaised = true;
                     numberOfRaise++;
                     lastPlayWhoRaised = i;
                }
            }
         
        } while (someOneRaised);
   
    }


     internal void CheckPotAmout()
     {
        var playerInGame=players.Where(p => p.InGame).ToList();
        int amout = playerInGame[0].CurrentPotInvest;
        foreach (var player in playerInGame)
        {
            if (player.CurrentPotInvest != amout)
            {
                Console.WriteLine($"Player {player.Name} has different pot amount {player.CurrentPotInvest} than {amout}");
                throw new Exception($"Player {player.Name} has different pot amount {player.CurrentPotInvest} than {amout}");
            }
        }   
    }
   

    internal void ShowDown(IList<Card> communityCards)
    {
        int CurrentPotInvest() => players.Sum(p => p.CurrentPotInvest);
        List<Player> playersInGame; 

        E054PokerHands E054PokerHands = new E054PokerHands();

        while ((playersInGame = players.Where(p => p.InGame).ToList()).Count > 1)
        {
            string firstHand = playersInGame[0].ToHand(communityCards);
            string secondHand = playersInGame[1].ToHand(communityCards);

            (bool firstWin, bool isDraw) = E054PokerHands.FirstPlayerWins(firstHand, secondHand);

            if (isDraw)
            {
                //TODO .. only works for 2 players
                var pot = CurrentPotInvest();
                playersInGame[0].Chips += pot/2;
                playersInGame[1].Chips += pot/2;
                return;
            }
            else
            {
                if (firstWin)
                    playersInGame[1].InGame = false;
                else
                    playersInGame[0].InGame = false;
            }
        }
        playersInGame[0].Chips += CurrentPotInvest();
    }
}