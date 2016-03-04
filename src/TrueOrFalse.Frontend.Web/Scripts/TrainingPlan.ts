class TrainingPlan {
    Dates: TrainingDate[];
    RemainingTime: string;
    RemainingDates: number;
    QuestionCount: number;
}

class TrainingDate {
    RemainingTime: string;
    QuestionCount: number;
    DateTime: string;
    TimeRemaining: string;
}

class TrainingPlanSettings {
    QuestionsPerDate_Minimum: number;
    QuestionsPerDate_IdealAmount: number;
    SpacingBetweenSessionsInMinutes: number;
    AnswerProbabilityThreshold: number;
}

class KnowledgeSummary
{
    NotLearned;
    NeedsLearning;
    NeedsConsolidation;
    Solid;
}

class TrainingDateLoader {
    static Run(trainingPlanId: number): TrainingPlan {
        return new TrainingPlan();
    }
}