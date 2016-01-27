
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

class TrainingDateLoader {
    static Run(trainingPlanId: number): TrainingPlan {


        return new TrainingPlan();
    }
}
