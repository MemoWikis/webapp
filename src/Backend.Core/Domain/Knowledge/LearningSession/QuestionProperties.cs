public record struct QuestionProperties()
{
    public bool NotLearned;
    public bool NeedsLearning;
    public bool NeedsConsolidation;
    public bool Solid;
    public bool InWishKnowledge;
    public bool NotInWishKnowledge;
    public bool CreatedByCurrentUser;
    public bool NotCreatedByCurrentUser;
    public bool Private;
    public bool Public;
    public int PersonalCorrectnessProbability;

    // Flags: If all Flags are true, 
    // question will be added to learning session
    public bool AddToLearningSession = true;
}