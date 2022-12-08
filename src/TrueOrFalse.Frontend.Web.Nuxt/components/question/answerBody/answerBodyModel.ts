import { UserTinyModel } from "~~/components/user/shared/userTinyModel"
import { KnowledgeStatus } from "../knowledgeStatusEnum"
import { LicenseQuestion } from "../license/licenseQuestion"

export class AnswerBody {
    QuestionViewGuid: number | undefined
    CreationDate: string | undefined
    CreationDateNiceText: string | undefined
    AnswerHelp: boolean | undefined

    QuestionId: number | undefined

    Creator: UserTinyModel | undefined
    QuestionChangeAuthor: UserTinyModel | undefined
    IsCreator: boolean | undefined
    IsInWishknowledge: boolean | undefined
    KnowledgeStatus: KnowledgeStatus | undefined

    
      QuestionLastEditedOn: string | undefined
      QuestionText: string | undefined
      QuestionDescription: string | undefined
      QuestionTextMarkdown: string | undefined
     LicenseQuestion: LicenseQuestion | undefined
     IsLastQuestion: boolean = false
    //  QuestionCacheItem Question

    //  HasSound => !.IsNullOrEmpty(SoundUrl)
      SoundUrl: string | undefined
    
      SolutionMetaDataJson: string | undefined
    //  SolutionMetadata SolutionMetadata
      SolutionType: string | undefined
     SolutionTypeInt: number | undefined
    //  QuestionSolution SolutionModel

    IsMobileRequest: boolean | undefined

     IsInWidget: boolean | undefined
     IsForVideo: boolean | undefined
     IsLearningSession: boolean | undefined
    //  LearningSession LearningSession
     IsLastLearningStep: boolean = false
     IsTestSession: boolean | undefined
     TestSessionProgessAfterAnswering: number | undefined
     IsInLearningTab: boolean | undefined
     IsInTestMode: boolean = false

     HasCategories: boolean | undefined
    //  CategoryCacheItem PrimaryCategory

     ShowCommentLink() {
        this.CommentCount != -1 && 
        !this.IsLearningSession && !this.IsTestSession && !this.DisableCommentLink
     } 


     CommentCount: number = -1
     UnsettledCommentCount: number = 0

     DisableCommentLink: boolean | undefined
     DisableAddKnowledgeButton: boolean | undefined

     NextUrl: string | undefined
     GetSolutionUrl: string | undefined
     CountLastAnswerAsCorrectUrl: string | undefined
      CounUnasweredAsCorrect: string | undefined
     TestSessionRegisterAnsweredQuestion: string | undefined
     LearningSessionAmendAfterShowSolution: string | undefined


     TotalActivityPoints: number | undefined
      QuestionTitle: string | undefined
}