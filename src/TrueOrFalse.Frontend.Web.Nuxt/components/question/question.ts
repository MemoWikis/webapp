import { SolutionType } from "./solutionTypeEnum"
export { SolutionType } from "./solutionTypeEnum"

export interface Question {
    Id: number
    Text: string
    TextExtended: string
    SolutionType: SolutionType
    Solution: string
}