import { Visibility } from "../shared/visibilityEnum"

export enum SearchType {
    All = 0,
    Category = 1,
    Questions = 2,
    Users = 3,
    CategoryInWiki = 4
}

export interface SearchTopicItem {
    Id: number
    Name: string
    Url: string
    QuestionCount: number
    ImageUrl: string
    MiniImageUrl: string
    IconHtml: string
    Visibility: Visibility
}

export interface TopicItem {
    Id: number
    Name: string
    Url: string
    QuestionCount: number
    ImageUrl: string
    MiniImageUrl: string
    IconHtml: string
    Visibility: number
    IsSpoiler?: boolean
}

export type QuestionItem = {
    Id: number
    Name: string
    Url: string
    ImageUrl: string
    Visibility: number
}

export type UserItem = {
    Id: number
    Name: string
    Url: string
    ImageUrl: string
    Visibility: number
}

export type FullSearch = {
    categories: TopicItem[]
    categoryCount: number
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