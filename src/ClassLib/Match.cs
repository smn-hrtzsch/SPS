using System;

/// \brief Abstract match class to set a frame for sport-specific kinds of matches.
/// \details This class provides the basic structure for any kind of sports match, including common properties like MatchID, MatchDate, and Results.
public abstract class Match
{
    /// \brief Unique identifier for the match.
    public uint MatchID { get; protected set; }

    /// \brief Date and time when the match takes place.
    public DateTime MatchDate { get; protected set; }

    /// \brief Result of team 1 in the match.
    public byte? ResultTeam1 { get; protected set; }

    /// \brief Result of team 2 in the match.
    public byte? ResultTeam2 { get; protected set; }

    public SportsTypes SportsType { get; protected set; }

    public ScheduleTypes schedule_type { get; set; }

    public string[] MatchArray { get; protected set; }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            MatchDate,
            ResultTeam1,
            ResultTeam2,
            string.Join(",", MatchArray),
            SportsType
        );
    }

    public bool Team1Won()
    {
        return ResultTeam1 > ResultTeam2;
    }

    public bool Team2Won()
    {
        return ResultTeam1 < ResultTeam2;
    }

    public bool Tie()
    {
        return ResultTeam1 == ResultTeam2;
    }
}
