namespace ClassLib.Tests;
using ClassLib;

public class UnitTest1
{
    [Fact]
    public void TestAdd()
    {
        Taschenrechner taschenrechner = new Taschenrechner();
        Assert.True(taschenrechner.add(1,3)==4);
    }
}