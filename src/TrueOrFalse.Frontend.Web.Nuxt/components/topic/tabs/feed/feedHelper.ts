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
    type: FeedType
    topicFeedItem?: TopicFeedItem,
    questionFeedItem?: QuestionFeedItem
}

export interface TopicFeedItem {
    date: string
    type: TopicChangeType
    categoryChangeId: number
    topicId: number
    visibility: Visibility
    authorName: string
}

export interface QuestionFeedItem {
    date: string
    type: QuestionChangeType
    questionChangeId: number
    questionId: number
    visibility: Visibility
    authorName: string
}