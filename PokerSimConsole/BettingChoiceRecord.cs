public enum BettingChoiceType
{
    Fold,
    Call,
    Raise
}

public record BettingChoiceRecord(BettingChoiceType choicetype,int raiseamount);
