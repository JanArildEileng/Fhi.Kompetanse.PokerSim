public record Card(string suit, int rank)
{
     public static string[] Suits ={"C","D","H","S"};
    public override string ToString()
    {
        string r=rank switch
        {
            1 =>  "A",
            10 => "T",
            11 => "J",
            12 => "Q",
            13 => "K",
            14 => "A",
            _ => rank.ToString()
        };

        return $"{r}{suit}";
    }

}