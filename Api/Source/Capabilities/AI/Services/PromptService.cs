using Api.AI;

namespace Api.AI;

public interface IPromptService
{
    AIRequest CreateFlashcardRequest(string content, string model);
    AIRequest CreateConceptAnalysisRequest(string content, string model);
    AIRequest CreateAnswerAnalysisRequest(string question, string correctAnswer, string userAnswer, string model);
}

public class PromptService : IPromptService
{
    private const string FLASHCARD_SYSTEM_PROMPT = @"You are a flashcard generation assistant. Your task is to create effective flashcards from the provided content.

IMPORTANT: You must ONLY return a valid JSON object with this exact structure:
{
    ""flashcards"": [
        {
            ""question"": ""What is X?"",
            ""answer"": ""X is..."",
            ""importance"": 1
        }
    ]
}

Guidelines for flashcard creation:
- Create clear, specific questions
- Provide concise answers in the same language as the content
- Set importance level (1=essential, 2=important, 3=supplementary)
- Ensure proper JSON formatting with double quotes
- Do not include any explanatory text or markdown formatting
- Do not include backticks or code blocks";

    private const string CONCEPT_ANALYSIS_SYSTEM_PROMPT = @"You are a learning assistant that analyzes text content and extracts key concepts and their relationships.
Return ONLY a JSON object with this exact structure:
{
    ""concepts"": [
        {
            ""content"": ""concept text"",
            ""level"": 0,
            ""order"": 0
        }
    ],
    ""relationships"": [
        {
            ""sourceConcept"": 0,
            ""targetConcept"": 1,
            ""type"": ""contains|requires|implements|defines|influences|supports|relates_to""
        }
    ]
}";

    private const string ANSWER_ANALYSIS_SYSTEM_PROMPT = @"You are an answer analysis assistant. Your task is to analyze a student's answer and provide feedback.

IMPORTANT: 
1. Return ONLY a valid JSON object with this exact structure:
{
    ""score"": 85,
    ""feedback"": ""Your answer was mostly correct..."",
    ""keyPointsCovered"": [""point 1"", ""point 2""],
    ""keyPointsMissed"": [""point 3"", ""point 4""],
    ""improvement"": ""To improve your answer...""
}

2. ALWAYS provide feedback in the same language as the user's answer. If the user answers in Swedish, respond in Swedish. If they answer in English, respond in English, etc.

3. Keep the feedback concise but helpful.";

    public AIRequest CreateFlashcardRequest(string content, string model)
    {
        var cleanContent = content.Replace("Generate flashcards from this content:", "").Trim();
        
        return new AIRequest
        {
            Model = model,
            MaxTokens = 8192,
            Messages = new[]
            {
                new AIMessage(AIMessageRole.System, FLASHCARD_SYSTEM_PROMPT),
                new AIMessage(AIMessageRole.User, $"Content to process: {cleanContent}")
            }
        };
    }

    public AIRequest CreateConceptAnalysisRequest(string content, string model)
    {
        return new AIRequest
        {
            Model = model,
            MaxTokens = 8192,
            Messages = new[]
            {
                new AIMessage(AIMessageRole.System, CONCEPT_ANALYSIS_SYSTEM_PROMPT),
                new AIMessage(AIMessageRole.User, content)
            }
        };
    }

    public AIRequest CreateAnswerAnalysisRequest(
        string question,
        string correctAnswer,
        string userAnswer,
        string model)
    {
        return new AIRequest
        {
            Model = model,
            MaxTokens = 8192,
            Messages = new[]
            {
                new AIMessage(AIMessageRole.System, ANSWER_ANALYSIS_SYSTEM_PROMPT),
                new AIMessage(
                    AIMessageRole.User,
                    $"Question: {question}\nCorrect Answer: {correctAnswer}\nUser Answer: {userAnswer}"
                )
            }
        };
    }
} 