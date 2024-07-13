public class Member
{
    public uint MemberID { get; }
    private static uint MemberIDCounter = 0;
    private string? forename { get; set; }
    private string? surname { get; set; }
    private string EmailAdress { get; set; }
    private List<Schedules> ParticipatingSchedules;
    private List<Prediction> PredictionsToDo;
    private List<Score> Scores;

    public Member(string forname, string surname, string EmailAdress) { }

    public void AddSchedule(uint ScheduleID) { }

    public void RemoveSchedule(uint ScheduleID) { }

    public void AddPrediction(uint PredictionID) { }

    public void RemovePrediction(uint PredictionID) { }

    public Prediction SearchPrediction(uint PredictionID) { }

    public void AddScore(ScheduleTypes PredictedSchedule) { }

    public void RemoveSchedule(uint ScoreID) { }

    public void UpdateScore(ScheduleTypes PredictedSchedule, Prediction prediction) { }
}
