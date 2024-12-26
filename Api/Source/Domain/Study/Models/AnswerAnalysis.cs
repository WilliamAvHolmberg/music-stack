namespace Api.Study;

public class AnswerAnalysis
{
    public required int Score { get; set; }
    public required string Feedback { get; set; }
    public required string[] KeyPointsMissed { get; set; }
    public required string[] KeyPointsCovered { get; set; }
    public required string Improvement { get; set; }
} 