import { Visibility } from "~/components/shared/visibilityEnum"

export enum FeedType {
    Page,
    Question,
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
    [PageChangeType.Create]: "page.feed.helper.pageChangeType.created",
    [PageChangeType.Update]: "page.feed.helper.pageChangeType.updated",
    [PageChangeType.Delete]: "page.feed.helper.pageChangeType.deleted",
    [PageChangeType.Published]: "page.feed.helper.pageChangeType.published",
    [PageChangeType.Privatized]: "page.feed.helper.pageChangeType.privatized",
    [PageChangeType.Renamed]: "page.feed.helper.pageChangeType.renamed",
    [PageChangeType.Text]: "page.feed.helper.pageChangeType.text",
    [PageChangeType.Relations]: "page.feed.helper.pageChangeType.relations",
    [PageChangeType.Image]: "page.feed.helper.pageChangeType.image",
    [PageChangeType.Restore]: "page.feed.helper.pageChangeType.restored",
    [PageChangeType.Moved]: "page.feed.helper.pageChangeType.moved",
    [PageChangeType.ChildPageDeleted]:
        "page.feed.helper.pageChangeType.childPageDeleted",
    [PageChangeType.QuestionDeleted]:
        "page.feed.helper.pageChangeType.questionDeleted",
}

export function getPageChangeTypeKey(type: PageChangeType): string {
    return PageChangeTypeNames[type]
}

export enum QuestionChangeType {
    Create = 0,
    Update = 1,
    Delete = 2,
    AddComment = 3,
}

const QuestionChangeTypeNames: { [key in QuestionChangeType]: string } = {
    [QuestionChangeType.Create]: "page.feed.helper.questionChangeType.created",
    [QuestionChangeType.Update]: "page.feed.helper.questionChangeType.updated",
    [QuestionChangeType.Delete]: "page.feed.helper.questionChangeType.deleted",
    [QuestionChangeType.AddComment]:
        "page.feed.helper.questionChangeType.comment",
}

export function getQuestionChangeTypeKey(type: QuestionChangeType): string {
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

export const getTime = (date: string): string => {
    const options = {
        hour: "2-digit",
        minute: "2-digit",
    } as Intl.DateTimeFormatOptions
    return new Date(date).toLocaleString("de-DE", options)
}
