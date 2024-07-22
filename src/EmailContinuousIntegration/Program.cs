using System;
using System.Collections.Generic;

class Program
{
    public static void Main(string[] args)
    {
        //Gerneral Programm variable declaration
        EmailService emailService = new EmailService();
        PredictionGame predictionGame = new PredictionGame(
            emailService /*,PathToCSVFile  <- zum Einlesen der Daten in den Member Konstruktor*/
        );

        //Email continuous Integration

        predictionGame.SendRoutineEmail();

        Console.WriteLine("Hallo Simon");
        //Email continous Integration end
    }
}
