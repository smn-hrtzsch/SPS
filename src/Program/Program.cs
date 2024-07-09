using System;
using ClassLib;

class Program {
    public static void Main(string[] args) {
        Taschenrechner taschenrechner = new Taschenrechner();
        int result = taschenrechner.add(2,3);
        Console.WriteLine("Result: {0}", result);
        KleinerTaschenrechner kleiner_taschenrechner = new KleinerTaschenrechner();
        int result2 = kleiner_taschenrechner.sub(2,3);
        Console.WriteLine("Result: {0}", result2);
    }
}