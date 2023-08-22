import { Visibility } from 'components/shared/visibilityEnum'
import { TinyTopicModel } from 'components/topic/topicStore'
import { KnowledgebarData } from '../knowledgebar/knowledgebarData'

export interface GridTopicItem {
    id: number
    name: string
    questionCount: number
    childrenCount: number
    imageUrl: string
    visibility: Visibility
    parents: TinyTopicModel[]
    knowledgebarData: KnowledgebarData
}