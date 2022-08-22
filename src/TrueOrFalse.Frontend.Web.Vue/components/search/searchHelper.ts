import { Visibility } from "../shared/visibilityEnum"

export enum SearchType {
    All = 0,
    Category = 1,
    Questions = 2,
    Users = 3,
    CategoryInWiki = 4
}

export class SearchTopicItem
{
     Id: number
     Name: string
     Url: string
     QuestionCount: number
     ImageUrl: string
     MiniImageUrl: string
     IconHtml: string
     Visibility: Visibility
}

export class CategoryItem {
    Id: number
    Name: string
    Url: string
    QuestionCount: number
    ImageUrl: string
    MiniImageUrl: string
    IconHtml: string
    Visibility: number
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
    categories: CategoryItem[]
    categoryCount: number
    questions: QuestionItem[]
    questionCount: number
    users: UserItem[]
    userCount: number
    userSearchUrl: string
}

export type TopicResult = {
    topics: CategoryItem[]
    totalCount: number
}