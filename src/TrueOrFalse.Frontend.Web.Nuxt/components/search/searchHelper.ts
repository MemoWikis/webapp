import { Visibility } from "../shared/visibilityEnum"

export enum SearchType {
    All = 0,
    Category = 1,
    Questions = 2,
    Users = 3,
    CategoryInWiki = 4,

    Topic = Category,
    TopicInWiki = CategoryInWiki
}

export interface TopicItem {
    Type?: 'TopicItem'
    Id: number
    Name: string
    Url: string | null
    QuestionCount: number
    ImageUrl: string
    MiniImageUrl: string
    Visibility: number
    IsSpoiler?: boolean
}

export interface QuestionItem {
    Type?: 'QuestionItem'
    Id: number
    Name: string
    Url: string | null
    ImageUrl: string
    Visibility: number
    PrimaryTopicId: number
    PrimaryTopicName: string
}

export interface UserItem {
    Type?: 'UserItem'
    Id: number
    Name: string
    Url: string | null
    ImageUrl: string
    Visibility: number
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