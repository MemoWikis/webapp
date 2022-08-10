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