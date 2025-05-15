import { KnowledgebarData } from '~/components/page/content/grid/knowledgebar/knowledgebarData'

export interface PageData {
    id: number
    name: string
    imgUrl?: string
    questionCount: number
    knowledgebarData: KnowledgebarData
}
