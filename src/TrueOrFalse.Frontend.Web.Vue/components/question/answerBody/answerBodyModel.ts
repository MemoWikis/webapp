import { UserTinyModel } from "~~/components/user/shared/userTinyModel"
import { KnowledgeStatus } from "../knowledgeStatusEnum"
import { LicenseQuestion } from "../license/licenseQuestion"

export class AnswerBody {
    QuestionViewGuid: number
    CreationDate: string
    CreationDateNiceText: string
    AnswerHelp: boolean

    QuestionId: number

    Creator: UserTinyModel
    IsCreator: boolean
    IsInWishknowledge: boolean
    KnowledgeStatus: KnowledgeStatus

    
      QuestionLastEditedOn: string
      QuestionText: string
      QuestionDescription: string
      QuestionTextMarkdown: string
     LicenseQuestion: LicenseQuestion
     IsLastQuestion: boolean = false
    //  QuestionCacheItem Question

    //  HasSound => !.IsNullOrEmpty(SoundUrl)
      SoundUrl: string
    
      SolutionMetaDataJson: string
    //  SolutionMetadata SolutionMetadata
      SolutionType: string
     SolutionTypeInt: number
    //  QuestionSolution SolutionModel

    IsMobileRequest: boolean

     IsInWidget: boolean
     IsForVideo: boolean
     IsLearningSession: boolean
    //  LearningSession LearningSession
     IsLastLearningStep: boolean = false
     IsTestSession: boolean
     TestSessionProgessAfterAnswering: number
     IsInLearningTab: boolean
     IsInTestMode: boolean = false

     HasCategories: boolean
    //  CategoryCacheItem PrimaryCategory

     ShowCommentLink() {
        this.CommentCount != -1 && 
        !this.IsLearningSession && !this.IsTestSession && !this.DisableCommentLink
     } 


     CommentCount: number = -1
     UnsettledCommentCount: number = 0

     DisableCommentLink: boolean
     DisableAddKnowledgeButton: boolean

     NextUrl: string
     GetSolutionUrl: string
     CountLastAnswerAsCorrectUrl: string
     CounUnasweredAsCorrect: string
     TestSessionRegisterAnsweredQuestion: string
     LearningSessionAmendAfterShowSolution: string


     TotalActivityPoints: number
      QuestionTitle: string
}