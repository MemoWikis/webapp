﻿public record struct QuestionProperties()
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

    public bool AddToLearningSession = true;
}