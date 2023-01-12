import { UserTinyModel } from "./userTinyModel"

export interface UserModel {
    Name: string,
    AmountCreatedQuestions: number,
    AmountCreatedSets: number,
    AmountCreatedCategories: number,
    AmountWishCountQuestions: number,
    ImageUrl_250: string,
    ImageIsCustom: boolean,
    ReputationRank: number,
    ReputationTotal: number,
    Reputation: ReputationCalcResult,
    IsMember: boolean,
    IsCurrentUser: boolean,
    User: UserTinyModel,
    UserIdProfile: number,
    DoIFollow: boolean,
    // CategoryCacheItem UserWiki;
    UserWikiId: number,
    UserWikiName: string,
    ShowWiki: boolean
  }

  export class ReputationCalcResult {
    ForQuestionsCreated: number
    ForQuestionsInOtherWishKnowledge: number
    ForUsersFollowingMe: number
    ForPublicWishknowledge: number
    TotalReputation() {
        return this.ForQuestionsCreated + this.ForQuestionsInOtherWishKnowledge + this.ForUsersFollowingMe + this.ForPublicWishknowledge
    }
  }