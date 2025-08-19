import { KnowledgeSummary } from '~/composables/knowledgeSummary'

export interface PageData {
    id: number
    name: string
    imgUrl?: string
    questionCount: number
    knowledgebarData: KnowledgeSummary
    creatorName?: string
    isPublic?: boolean
}
