import { Visibility } from '~~/components/shared/visibilityEnum'
import { TinyPageModel } from '~/components/page/pageStore'
import { KnowledgebarData } from '../knowledgebar/knowledgebarData'

export interface GridPageItem {
    id: number
    name: string
    questionCount: number
    childrenCount: number
    imageUrl: string
    visibility: Visibility
    parents: TinyPageModel[]
    knowledgebarData: KnowledgebarData
    isChildOfPersonalWiki: boolean
    creatorId: number
    canDelete: boolean
}