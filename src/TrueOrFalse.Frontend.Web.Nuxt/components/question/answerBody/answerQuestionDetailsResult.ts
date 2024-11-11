import { KnowledgeStatus } from '../knowledgeStatusEnum'
import { PageItem } from '~~/components/search/searchHelper'
import { Visibility } from '~~/components/shared/visibilityEnum'

export interface AnswerQuestionDetailsResult {
    knowledgeStatus: KnowledgeStatus,
    personalProbability: number,
    personalColor: string,
    avgProbability: number,
    personalAnswerCount: number,
    personalAnsweredCorrectly: number,
    personalAnsweredWrongly: number,
    overallAnswerCount: number,
    overallAnsweredCorrectly: number,
    overallAnsweredWrongly: number,
    isInWishknowledge: boolean,
    pages: PageItem[],

    visibility: Visibility,
    dateNow: number,
    endTimer: number,
    creator:
    {
        id: number,
        name: string
    },
    creationDate: string,
    totalViewCount: number,
    wishknowledgeCount: number,
    license:
    {
        isDefault: boolean,
        shortText: string,
        fullText: string
    }
}