import { Visibility } from "~/components/shared/visibilityEnum"

export enum FeedType {
    Topic,
    Question
}

export enum TopicChangeType {
    Create = 0,
    Update = 1,
    Delete = 2,
    Published = 3,
    Privatized = 4,
    Renamed = 5,
    Text = 6,
    Relations = 7,
    Image = 8,
    Restore = 9,
    Moved = 10,
}

export enum QuestionChangeType {
    Create = 0,
    Update = 1,
    Delete = 2
}

export interface FeedItem {
    date: string
    title: string
    type: FeedType
    topicFeedItem?: TopicFeedItem
    questionFeedItem?: QuestionFeedItem
    author: Author
}

export interface Author {
    id: number
    name: string
    imageUrl: string
}

export interface NameChange {
    oldName: string
    newName: string
}

export interface TopicFeedItem {
    date: string
    type: TopicChangeType
    categoryChangeId: number
    topicId: number
    title: string
    visibility: Visibility
    author: Author
    nameChange?: NameChange
}

export interface QuestionFeedItem {
    date: string
    type: QuestionChangeType
    questionChangeId: number
    questionId: number
    text: string
    visibility: Visibility
    author: Author
}

export interface FeedItemGroupByAuthor {
    dateLabel: string
    author: Author
    feedItems: FeedItem[]
}

export const getTime = (date: string):string => {
    const options = { hour: '2-digit', minute: '2-digit' } as Intl.DateTimeFormatOptions
    return new Date(date).toLocaleString('de-DE', options)
}