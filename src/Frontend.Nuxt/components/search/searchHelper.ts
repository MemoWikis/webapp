export enum SearchType {
    all = 0,
    category = 1,
    questions = 2,
    users = 3,
    categoryInWiki = 4,

    page = category,
    pageInWiki = categoryInWiki,
}

export enum SearchItemType {
    Page = 'page',
    Question = 'question',
    User = 'user'
}

export interface PageItem {
    type?: "PageItem"
    id: number
    name: string
    url: string | null
    questionCount: number
    imageUrl: string
    miniImageUrl: string
    visibility: number
    isSpoiler?: boolean
    languageCode: string
}

export interface QuestionItem {
    type?: "QuestionItem"
    id: number
    name: string
    url: string | null
    imageUrl: string
    visibility: number
    primaryPageId: number
    primaryPageName: string
    languageCode: string
}

export interface UserItem {
    type?: "UserItem"
    id: number
    name: string
    url: string | null
    imageUrl: string
    visibility: number
    LanguageCodes: string[]
}

export type FullSearch = {
    pages: PageItem[]
    pageCount: number
    questions: QuestionItem[]
    questionCount: number
    users: UserItem[]
    userCount: number
    userSearchUrl: string
}

export type PageResult = {
    pages: PageItem[]
    totalCount: number
}
