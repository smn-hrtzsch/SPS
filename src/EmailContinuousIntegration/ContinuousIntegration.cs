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

        Member<Prediction, Match> member = new Member<Prediction, Match>(
            "SportsPrediction",
            "System",
            "sportspredictionsystem@gmail.com",
            "1234"
        );
        predictionGame.Register(member);

        //Email continuous Integration

        predictionGame.SendDailyEmail();
        //Email continous Integration end

        //this is a comment for testing: 0
    }
}


// //Email continuous Integration

//         DateTime dateTimeNow = DateTime.Now;
//         DateTime dateTimeAtNineThirty = new DateTime(
//             dateTimeNow.Year,
//             dateTimeNow.Month,
//             dateTimeNow.Day,
//             9,
//             30,
//             0
//         );
//         DateTime dateTimeAtEighteenOClock = new DateTime(
//             dateTimeNow.Year,
//             dateTimeNow.Month,
//             dateTimeNow.Day,
//             18,
//             0,
//             0
//         );

//         if (dateTimeNow == dateTimeAtNineThirty) //get daily Tipp-email
//         {
//             predictionGame.SendDailyEmail();
//         }

//         if (dateTimeNow.DayOfWeek == DayOfWeek.Sunday && dateTimeNow == dateTimeAtEighteenOClock) //get Results-email once a week
//         {
//             predictionGame.SendDailyEmail();
//         }

//         //Email continous Integration end
