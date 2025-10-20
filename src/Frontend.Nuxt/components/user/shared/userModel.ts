import { UserTinyModel } from "./userTinyModel"

export interface UserModel {
    name: string,
    amountCreatedQuestions: number,
    amountCreatedSets: number,
    amountCreatedPages: number,
    amountWishCountQuestions: number,
    amountWishCountSets: number,
    imageUrl_250: string,
    imageIsCustom: boolean,
    reputationRank: number,
    reputationTotal: number,
    reputation: ReputationCalcResult,
    isMember: boolean,
    isCurrentUser: boolean,
    user: UserTinyModel,
    userIdProfile: number,
    doIFollow: boolean,
    // CategoryCacheItem UserWiki;
    userWikiId: number,
    userWikiName: string,
    showWiki: boolean
  }

  export class ReputationCalcResult {
    forQuestionsCreated: number = 0
    forQuestionsInOtherWishKnowledge: number = 0
    forUsersFollowingMe: number = 0
    forPublicWishKnowledge: number = 0
    TotalReputation() {
        return this.forQuestionsCreated + this.forQuestionsInOtherWishKnowledge + this.forUsersFollowingMe + this.forPublicWishKnowledge
    }
  }