import { SolutionType } from "./solutionTypeEnum"
export { SolutionType } from "./solutionTypeEnum"

export interface Question {
    id: number
    text: string
    textExtended: string
    solutionType: SolutionType
    solution: string
}