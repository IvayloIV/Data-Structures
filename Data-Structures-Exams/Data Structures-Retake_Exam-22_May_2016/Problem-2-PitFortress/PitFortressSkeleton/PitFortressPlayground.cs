public class PitFortressPlayground
{
    public static void Main()
    {
        var pitFotress = new PitFortressCollection();
        pitFotress.AddPlayer("Gosho", 2);
        pitFotress.AddPlayer("Pesho", 3);
        pitFotress.AddMinion(0);
        pitFotress.AddMinion(5);
        pitFotress.AddMinion(3);
        pitFotress.SetMine("Gosho", 1, 2, 100);
        pitFotress.SetMine("Pesho", 4, 1, 20);
        pitFotress.PlayTurn();
        pitFotress.PlayTurn();
    }
}
