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
    ChildTopicDeleted = 11,
    QuestionDeleted = 12,
}

const TopicChangeTypeNames: { [key in TopicChangeType]: string } = {
    [TopicChangeType.Create]: 'Erstellt',
    [TopicChangeType.Update]: 'Aktualisiert',
    [TopicChangeType.Delete]: 'Gelöscht',
    [TopicChangeType.Published]: 'Veröffentlicht',
    [TopicChangeType.Privatized]: 'Privatisiert',
    [TopicChangeType.Renamed]: 'Umbenannt',
    [TopicChangeType.Text]: 'Textinhalt',
    [TopicChangeType.Relations]: 'Verknüpfungen',
    [TopicChangeType.Image]: 'Bild',
    [TopicChangeType.Restore]: 'Wiederhergestellt',
    [TopicChangeType.Moved]: 'Verschoben',
    [TopicChangeType.ChildTopicDeleted]: 'Gelöscht',
    [TopicChangeType.QuestionDeleted]: 'Gelöscht',
}

export function getTopicChangeTypeName(type: TopicChangeType): string {
    return TopicChangeTypeNames[type]
}

export enum QuestionChangeType {
    Create = 0,
    Update = 1,
    Delete = 2,
    AddComment = 3,
}

const QuestionChangeTypeNames: { [key in QuestionChangeType]: string } = {
    [QuestionChangeType.Create]: 'Erstellt',
    [QuestionChangeType.Update]: 'Aktualisiert',
    [QuestionChangeType.Delete]: 'Gelöscht',
    [QuestionChangeType.AddComment]: 'Kommentar',
}

export function getQuestionChangeTypeName(type: QuestionChangeType): string {
    return QuestionChangeTypeNames[type]
}

export interface FeedItem {
    index?: number
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

export interface RelatedTopic {
    id: number
    name: string
}

export interface RelationChanges {
    addedParents: RelatedTopic[]
    removedParents: RelatedTopic[]
    addedChildren: RelatedTopic[]
    removedChildren: RelatedTopic[]
}

export interface ContentChange {
    currentContent: string
    diffContent: string
}

export interface DeleteData {
    deleteChangeId: number
    deletedName: string
}

export interface TopicFeedItem {
    oldestChangeIdInGroup?: number
    date: string
    type: TopicChangeType
    categoryChangeId: number
    topicId: number
    title: string
    visibility: Visibility
    author: Author
    nameChange?: NameChange
    relationChanges?: RelationChanges
    deleteData?: DeleteData
}

export interface QuestionFeedItem {
    date: string
    type: QuestionChangeType
    questionChangeId: number
    questionId: number
    text: string
    visibility: Visibility
    author: Author
    comment?: Comment
}

export interface Comment {
    title: string
    id: number
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