import { SolutionType } from "../solutionTypeEnum"

export interface AnswerBodyModel {
    id: number
    text: string
    title: string
    encodedTitle: string
    solutionType: SolutionType
    renderedQuestionTextExtended: string
    description: string
    hasTopics: boolean
    primaryTopicUrl: string
    primaryTopicName: string
    solution: string

    isCreator: boolean
    isInWishknowledge: boolean

    questionViewGuid: number
    isLastStep: boolean
    imgUrl?: string
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
    answerReferences: Reference[]
}