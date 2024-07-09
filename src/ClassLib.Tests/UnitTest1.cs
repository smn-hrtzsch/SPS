namespace ClassLib.Tests;

using ClassLib;

public class UnitTest1
{
    [Fact]
    public void TestAdd()
    {
        Taschenrechner taschenrechner = new Taschenrechner();
        Assert.True(taschenrechner.add(1, 3) == 4);
    }

    [Fact]
    public void TestSub()
    {
        KleinerTaschenrechner kleiner_taschenrechner = new KleinerTaschenrechner();
        Assert.True(kleiner_taschenrechner.sub(1, 3) == -2);
    }
}
