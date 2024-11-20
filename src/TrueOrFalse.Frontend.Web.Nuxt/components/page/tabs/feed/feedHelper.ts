import { Visibility } from "~/components/shared/visibilityEnum"

export enum FeedType {
    Page,
    Question
}

export enum PageChangeType {
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
    ChildPageDeleted = 11,
    QuestionDeleted = 12,
}

const PageChangeTypeNames: { [key in PageChangeType]: string } = {
    [PageChangeType.Create]: 'Erstellt',
    [PageChangeType.Update]: 'Aktualisiert',
    [PageChangeType.Delete]: 'Gelöscht',
    [PageChangeType.Published]: 'Veröffentlicht',
    [PageChangeType.Privatized]: 'Privatisiert',
    [PageChangeType.Renamed]: 'Umbenannt',
    [PageChangeType.Text]: 'Textinhalt',
    [PageChangeType.Relations]: 'Verknüpfungen',
    [PageChangeType.Image]: 'Bild',
    [PageChangeType.Restore]: 'Wiederhergestellt',
    [PageChangeType.Moved]: 'Verschoben',
    [PageChangeType.ChildPageDeleted]: 'Gelöscht',
    [PageChangeType.QuestionDeleted]: 'Gelöscht',
}

export function getPageChangeTypeName(type: PageChangeType): string {
    return PageChangeTypeNames[type]
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
    pageFeedItem?: PageFeedItem
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

export interface RelatedPage {
    id: number
    name: string
}

export interface RelationChanges {
    addedParents: RelatedPage[]
    removedParents: RelatedPage[]
    addedChildren: RelatedPage[]
    removedChildren: RelatedPage[]
}

export interface ContentChange {
    currentContent: string
    diffContent: string
}

export interface DeleteData {
    deleteChangeId: number
    deletedName: string
}

export interface PageFeedItem {
    oldestChangeIdInGroup?: number
    date: string
    type: PageChangeType
    pageChangeId: number
    pageId: number
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