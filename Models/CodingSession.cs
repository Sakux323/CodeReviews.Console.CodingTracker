namespace CodingTracker.Models;

public class CodingSession
{
    public int Id { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public double Duration { get; set; }
}
