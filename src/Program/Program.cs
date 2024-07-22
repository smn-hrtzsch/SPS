﻿using System;
using System.Collections.Generic;

class Program
{
    public static void Main(string[] args) 
    {
        //Gerneral Programm variable declaration
        EmailService emailService = new EmailService();
        List<ScheduleTypes> scheduleTypes = new List<ScheduleTypes>() {ScheduleTypes.EM_2024};
        PredictionGame predictionGame = new PredictionGame(emailService, scheduleTypes /*,PathToCSVFile* <- zum Einlesen der Daten in den Member Konstruktor/);

        //Email continuous Integration

        DateTime dateTimeNow = DateTime.Now;
        DateTime dateTimeAtNineThirty = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, 9, 30, 0);
        DateTime dateTimeAtEighteenOClock = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, 18, 0, 0);
        

        if(dateTimeNow == dateTimeAtNineThirty) //get daily Tipp-email
        {
            predictionGame.SendRoutineEmail(EmailTypes.TippTemplate);   
        }

        if(dateTimeNow.DayOfWeek == DayOfWeek.Sunday && dateTimeNow == dateTimeAtEighteenOClock) //get Results-email once a week
        {
            predictionGame.SendRoutineEmail(EmailTypes.ResultTemplate);
        }

        //Email continous Integration end


    }
}
