import { SolutionType } from "../solutionTypeEnum"

export interface AnswerBodyModel {
    id: number
    text: string
    textHtml: string
    title: string
    solutionType: SolutionType
    renderedQuestionTextExtended: string
    description: string
    hasTopics: boolean
    primaryTopicId: number
    primaryTopicName: string
    solution: string

    isCreator: boolean
    isInWishknowledge: boolean

    questionViewGuid: number
    isLastStep: boolean
    imgUrl?: string

    isPrivate: boolean
}
export interface Reference {
    referenceId: number
    topicId?: number
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