import { KnowledgeStatus } from "../knowledgeStatusEnum"
import { PageItem } from "~~/components/search/searchHelper"
import { Visibility } from "~~/components/shared/visibilityEnum"

export interface AnswerQuestionDetailsResult {
    questionId: number
    knowledgeStatus: KnowledgeStatus
    personalProbability: number
    personalColor: string
    avgProbability: number
    personalAnswerCount: number
    personalAnsweredCorrectly: number
    personalAnsweredWrongly: number
    overallAnswerCount: number
    overallAnsweredCorrectly: number
    overallAnsweredWrongly: number
    isInWishKnowledge: boolean
    pages: PageItem[]

    visibility: Visibility
    dateNow: number
    endTimer: number
    creator: {
        id: number
        name: string
    }
    creationDate: Date
    totalViewCount: number
    wishknowledgeCount: number
    licenseId: number
}
