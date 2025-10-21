import { SolutionType } from "../solutionTypeEnum"

export interface AnswerBodyModel {
    id: number
    text: string
    textHtml: string
    title: string
    solutionType: SolutionType
    renderedQuestionTextExtended: string
    description: string
    hasPages: boolean
    primaryPageId: number
    primaryPageName: string
    solution: string

    isCreator: boolean
    isInWishKnowledge: boolean

    questionViewGuid: number
    isLastStep: boolean
    imgUrl?: string

    isPrivate: boolean
}
export interface Reference {
    referenceId: number
    pageId?: number
    referenceType: string
    additionalInfo: string
    referenceText: string
}
export interface SolutionData {
    answerAsHTML: string
    answer: string
    answerDescription: string
    answerDescriptionHtml: string
    answerReferences: Reference[]
}