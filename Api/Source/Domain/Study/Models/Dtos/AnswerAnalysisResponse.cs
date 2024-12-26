namespace Api.Study.Dtos;

public class AnswerAnalysisResponse
{
    public int Score { get; set; }  // 0-100
    public string Feedback { get; set; } = "";
    public List<string> KeyPointsMissed { get; set; } = new();
    public List<string> KeyPointsCovered { get; set; } = new();
    public string Improvement { get; set; } = "";
} 