using ClassLib;

public class GroßerTaschenrechner : Taschenrechner
{
    public int zahl4 = 0;

    public void Print()
    {
        Console.WriteLine("Ich bin dumm :).");
    }

    public GroßerTaschenrechner(int zahl4)
    {
        this.zahl4 = zahl4;
    }

    public GroßerTaschenrechner(int zahl1, int zahl2, int zahl4)
    {
        this.zahl1 = zahl1;
        this.zahl2 = zahl2;
        this.zahl4 = zahl4;
    }
}
