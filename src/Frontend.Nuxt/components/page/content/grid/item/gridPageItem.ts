import { Visibility } from '~~/components/shared/visibilityEnum'
import { TinyPageModel } from '~/components/page/pageStore'
import { KnowledgeSummary } from '~/composables/knowledgeSummary'

export interface GridPageItem {
    id: number
    name: string
    questionCount: number
    childrenCount: number
    imageUrl: string
    visibility: Visibility
    parents: TinyPageModel[]
    knowledgebarData: KnowledgeSummary
    isChildOfPersonalWiki: boolean
    creatorId: number
    canDelete: boolean
    isWiki: boolean
}
