using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

public class DataService
{
    private readonly string _pathToMemberDataFile;
    private readonly string _pathToPredictionDataFile;
    private readonly string _pathToScoreDataFile;
    private readonly string _pathToEM_2024File;
    private readonly string _pathToLaLiga_24_25File;

    public DataService(
        string pathToMemberDataFile,
        string pathToPredictionDataFile,
        string pathToScoreDataFile,
        string pathToEM_2024File,
        string pathToLaLiga_24_25File
    )
    {
        _pathToMemberDataFile = pathToMemberDataFile;
        _pathToPredictionDataFile = pathToPredictionDataFile;
        _pathToScoreDataFile = pathToScoreDataFile;
        _pathToEM_2024File = pathToEM_2024File;
        _pathToLaLiga_24_25File = pathToLaLiga_24_25File;
    }

    public List<Schedule<Match?>?> GetSchedules(
        string pathToEM_2024File,
        string pathToLaLiga_24_25_File
    )
    {
        IMatchFactory<FootballMatch?> football_match_factory = new FootballMatchFactory();
        CSVReader<FootballMatch, FootballPrediction>.SetMatchFactory(football_match_factory);

        Schedule<Match?> em_2024;
        Schedule<Match?> laliga_24_25;
        List<Schedule<Match?>?> schedules = new List<Schedule<Match?>?>();

        if (File.Exists(pathToEM_2024File) && File.Exists(pathToLaLiga_24_25_File))
        {
            em_2024 = new Schedule<Match?>(
                pathToEM_2024File,
                SportsTypes.Football,
                ScheduleTypes.EM_2024
            );
            laliga_24_25 = new Schedule<Match?>(
                pathToLaLiga_24_25_File,
                SportsTypes.Football,
                ScheduleTypes.La_Liga_24_25
            );
            // set schedule type for every match in every Schedule
            if (em_2024.Matches != null && laliga_24_25.Matches != null)
            {
                foreach (var match in em_2024.Matches)
                {
                    if (match != null)
                    {
                        match.ScheduleType = ScheduleTypes.EM_2024;
                    }
                }
                foreach (var match in laliga_24_25.Matches)
                {
                    if (match != null)
                    {
                        match.ScheduleType = ScheduleTypes.La_Liga_24_25;
                    }
                }
            }
        }
        else
        {
            throw new InvalidDataException("Schedule Files not Found!");
        }

        return schedules;
    }

    public List<Member<Match?, Prediction?>> LoadMembers()
    {
        IMatchFactory<FootballMatch?> football_match_factory = new FootballMatchFactory();
        CSVReader<FootballMatch, FootballPrediction>.SetMatchFactory(football_match_factory);

        List<Schedule<Match?>?> schedules = GetSchedules(
            _pathToEM_2024File,
            _pathToLaLiga_24_25File
        );
        if (File.Exists(_pathToMemberDataFile))
        {
            return CSVReader<Match?, Prediction?>.GetMemberDataFromCsvFile(
                _pathToMemberDataFile,
                schedules
            );
        }
        else
        {
            throw new InvalidDataException("Member File not found!");
        }
    }

    public void LoadPredictions(PredictionGame predictionGame)
    {
        IMatchFactory<FootballMatch?> football_match_factory = new FootballMatchFactory();
        CSVReader<FootballMatch, FootballPrediction>.SetMatchFactory(football_match_factory);

        List<Schedule<Match?>?> schedules = GetSchedules(
            _pathToEM_2024File,
            _pathToLaLiga_24_25File
        );
        if (File.Exists(_pathToPredictionDataFile))
        {
            CSVReader<Match, Prediction>.GetFootballPredictionsFromCsvFile(
                _pathToPredictionDataFile,
                predictionGame,
                schedules
            );
        }
        else
        {
            throw new InvalidDataException("Prediction File not found!");
        }
    }

    public void LoadScores(PredictionGame predictionGame)
    {
        IMatchFactory<FootballMatch?> football_match_factory = new FootballMatchFactory();
        CSVReader<FootballMatch, FootballPrediction>.SetMatchFactory(football_match_factory);

        if (File.Exists(_pathToScoreDataFile))
        {
            CSVReader<Match, Prediction>.GetScoresFromCsvFile(_pathToScoreDataFile, predictionGame);
        }
        else
        {
            throw new InvalidDataException("Score File not found!");
        }
    }

    public void SaveData(PredictionGame prediction_game)
    {
        CSVWriter<Match, Prediction>.WriteMemberData(_pathToMemberDataFile, prediction_game);
        CSVWriter<Match, Prediction>.TrackFootballPredictionData(
            _pathToPredictionDataFile,
            prediction_game
        );
        CSVWriter<Match, Prediction>.TrackScoreData(_pathToScoreDataFile, prediction_game);
    }
}
