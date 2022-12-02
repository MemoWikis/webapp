import { Visibility } from "~~/components/shared/visibilityEnum"

export interface CategoryCardData {
    Id: number,
    Name: string,
    Visibility: Visibility
    LinkToCategory: string,
    CategoryTypeHtml: string,
    ImgHtml: string,
    KnowledgeBarHtml: string,
    ChildCategoryCount: number,
    QuestionCount: number,
    IsInWishknowledge: boolean,
    IsPersonalHomepage: boolean,
    ImgUrl: string
};