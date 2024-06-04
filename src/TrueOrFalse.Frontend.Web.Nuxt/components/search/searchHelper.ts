import { Visibility } from "../shared/visibilityEnum"

export enum SearchType {
    all = 0,
    category = 1,
    questions = 2,
    users = 3,
    categoryInWiki = 4,
    moveQuestions = 5,

    topic = category,
    topicInWiki = categoryInWiki
}

export interface TopicItem {
    type?: 'TopicItem'
    id: number
    name: string
    url: string | null
    questionCount: number
    imageUrl: string
    miniImageUrl: string
    visibility: number
    isSpoiler?: boolean
}

export interface QuestionItem {
    type?: 'QuestionItem'
    id: number
    name: string
    url: string | null
    imageUrl: string
    visibility: number
    primaryTopicId: number
    primaryTopicName: string
}

export interface UserItem {
    type?: 'UserItem'
    id: number
    name: string
    url: string | null
    imageUrl: string
    visibility: number
}

export type FullSearch = {
    topics: TopicItem[]
    topicCount: number
    questions: QuestionItem[]
    questionCount: number
    users: UserItem[]
    userCount: number
    userSearchUrl: string
}

export type TopicResult = {
    topics: TopicItem[]
    totalCount: number
}