import { KnowledgeSummary } from '~/composables/knowledgeSummary'

export interface PageData {
    id: number
    name: string
    imgUrl?: string
    questionCount: number
    knowledgebarData: KnowledgeSummary
    popularity: number
    creatorName?: string
    isPublic?: boolean
}
