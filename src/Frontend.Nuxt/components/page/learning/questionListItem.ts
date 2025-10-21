import { KnowledgeStatus } from "~~/components/question/knowledgeStatusEnum"

export interface QuestionListItem {
    correctnessProbability: number
    hasPersonalAnswer: boolean
    id: number
    imageData: string
    isInWishKnowledge: boolean
    learningSessionStepCount: number
    linkToComment: string
    linkToDeleteQuestion: string
    linkToEditQuestion: string
    linkToQuestion: string
    linkToQuestionDetailSite: string
    linkToQuestionVersions: string
    title: string
    visibility: number
    sessionIndex: number
    creatorId: number
    knowledgeStatus: KnowledgeStatus
}