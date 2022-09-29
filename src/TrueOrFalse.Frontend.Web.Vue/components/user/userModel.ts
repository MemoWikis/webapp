export interface UserModel {
    Name: string,
    AmountCreatedQuestions: number,
    AmountCreatedSets: number,
    AmountCreatedCategories: number,
    AmountWishCountQuestions: number,
    AmountWishCountSets: number,
    ImageUrl_250: string,
    ImageIsCustom: boolean,
    ReputationRank: number,
    ReputationTotal: number,
  
    // ReputationCalcResult Reputation;
  
    IsMember: boolean,
    IsCurrentUser: boolean,
    IsActiveTabKnowledge: boolean,
    IsActiveTabBadges: boolean,
  
    // UserTinyModel User;
  
    UserIdProfile: number,
    DoIFollow: boolean,
  
    // CategoryCacheItem UserWiki;
  
    ShowWiki: boolean
  }