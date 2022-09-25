export interface QuestionListItem
{
    CorrectnessProbability: number;
    HasPersonalAnswer: boolean;
    Id: number;
    ImageData: string;
    IsInWishknowledge: boolean;
    LearningSessionStepCount: number;
    LinkToComment: string;
    LinkToDeleteQuestion: string;
    LinkToEditQuestion: string;
    LinkToQuestion: string;
    LinkToQuestionDetailSite: string;
    LinkToQuestionVersions: string;
    Title: string;
    Visibility: number;
    SessionIndex: number;
}