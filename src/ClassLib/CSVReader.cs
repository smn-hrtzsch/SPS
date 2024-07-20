using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class CSVReader<M>
    where M : Match, new()
{
    private static IMatchFactory<M> MatchFactory;

    public static void SetMatchFactory(IMatchFactory<M> match_factory)
    {
        MatchFactory = match_factory;
    }

    /// \brief Reads the match data from a CSV file.
    /// \param PathToMatchDataCsvFile The path to the CSV file containing match data.
    /// \param MatchID The unique identifier of the match.
    /// \return An array of strings containing the match data.
    public static string[] GetMatchDataFromCsvFile(string PathToMatchDataCsvFile, int line_number)
    {
        string? needed_line = File.ReadLines(PathToMatchDataCsvFile)
            .Skip(line_number)
            .FirstOrDefault();
        if (needed_line != null)
        {
            string[] FootballMatchData = needed_line.Split(';');
            return FootballMatchData;
        }
        else
        {
            throw new InvalidOperationException("Die Zeile ist leer!");
        }
    }

    public static List<M> GetScheduleFromCsvFile(string PathToCsvFile, SportsTypes sport_type)
    {
        if (MatchFactory == null)
        {
            throw new InvalidOperationException("Match factory is not set.");
        }
        List<M> schedule = new List<M>();
        var all_lines = File.ReadLines(PathToCsvFile).ToList();
        for (int line_number = 1; line_number < all_lines.Count; line_number++)
        {
            switch (sport_type)
            {
                case SportsTypes.Football:
                    M match = MatchFactory.CreateMatch(PathToCsvFile, line_number, sport_type);
                    schedule.Add(match);
                    break;
                // could be extended by serveral sport types
            }
        }
        return schedule;
    }
}
