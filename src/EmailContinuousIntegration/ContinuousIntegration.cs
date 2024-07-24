using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

class Program
{
    public static void Main(string[] args)
    {
        //Gerneral Programm variable declaration
        EmailService emailService = new EmailService();
        PredictionGame prediction_game = new PredictionGame(
            emailService /*,PathToCSVFile  <- zum Einlesen der Daten in den Member Konstruktor*/
        );
        string PathToMemberDataFile = "../../csv-files/MemberData.csv";

        if (File.Exists(PathToMemberDataFile))
        {
            prediction_game.Members = CSVReader<Match, Prediction>.GetMemberDataFromCsvFile(
                PathToMemberDataFile
            );
        }

        //Email continuous Integration

        prediction_game.SendDailyEmail();
        //Email continous Integration end
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
