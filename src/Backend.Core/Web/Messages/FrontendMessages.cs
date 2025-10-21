public static class FrontendMessageKeys
{
    public static class Success
    {
        public static class Page
        {
            public static readonly string Publish = "success.page.publish";
            public static readonly string SetToPrivate = "success.page.setToPrivate";
            public static readonly string Unlinked = "success.page.unlinked";
            public static readonly string AddedToPersonalWiki = "success.page.addedToPersonalWiki";
            public static readonly string SaveImage = "success.page.saveImage";
        }

        public static class Question
        {
            public static readonly string Created = "success.question.created";
            public static readonly string Saved = "success.question.saved";
            public static readonly string Delete = "success.question.delete";
        }

        public static class User
        {
            public static readonly string ProfileUpdate = "success.user.profileUpdate";
            public static readonly string PasswordChanged = "success.user.passwordChanged";
            public static readonly string PasswordReset = "success.user.passwordReset";
            public static readonly string VerificationMailRequestSent = "success.user.passwordVerificationMailSent";
            public static readonly string SupportLoginUpdated = "success.user.supportLoginUpdated";
            public static readonly string WishKnowledgeVisibilityUpdated = "success.user.wishKnowledgeVisibilityUpdated";
        }
    }

    public static class Error
    {
        public static class Subscription
        {
            public static readonly string CantAddKnowledge = "error.subscription.cantAddKnowledge";
            public static readonly string CantSavePrivateQuestion = "error.subscription.cantSavePrivateQuestion";
            public static readonly string CantSavePrivatePage = "error.subscription.cantSavePrivatePage";
        }

        public static class Page
        {
            public static readonly string ParentIsPrivate = "error.page.parentIsPrivate";
            public static readonly string PublicChildPages = "error.page.publicChildPages";
            public static readonly string PublicQuestions = "error.page.publicQuestions";
            public static readonly string NotLastChild = "error.page.notLastChild";
            public static readonly string NoRemainingParents = "error.page.noRemainingParents";
            public static readonly string ParentIsRoot = "error.page.parentIsRoot";
            public static readonly string LoopLink = "error.page.loopLink";
            public static readonly string IsAlreadyLinkedAsChild = "error.page.isAlreadyLinkedAsChild";
            public static readonly string IsNotAChild = "error.page.isNotAChild";
            public static readonly string IsLinkedInNonWishKnowledge = "error.page.isLinkedInNonWishKnowledge";
            public static readonly string ChildIsParent = "error.page.childIsParent";
            public static readonly string NameIsTaken = "error.page.nameIsTaken";
            public static readonly string NameIsForbidden = "error.page.nameIsForbidden";
            public static readonly string RootPageMustBePublic = "error.page.rootPageMustBePublic";
            public static readonly string MissingRights = "error.page.missingRights";
            public static readonly string TooPopular = "error.page.tooPopular";
            public static readonly string SaveImageError = "error.page.saveImageError";
            public static readonly string PinnedQuestions = "error.page.pinnedQuestions";
            public static readonly string CircularReference = "error.page.circularReference";
            public static readonly string PageNotSelected = "error.page.pageNotSelected";
            public static readonly string NewPageIdIsPageIdToBeDeleted = "error.page.newPageIdIsPageIdToBeDeleted";
            public static readonly string NotFound = "error.page.notFound";
            public static readonly string NoRights = "error.page.noRights";
            public static readonly string Unauthorized = "error.page.unauthorized";
            public static readonly string NoChange = "error.page.noChange";
            public static readonly string CannotDeletePageWithChildPage = "error.page.cannotDeletePageWithChildPage";
            public static readonly string ConvertErrorNoParents = "error.page.convertErrorNoParents";
        }

        public static class Question
        {
            public static readonly string MissingText = "error.question.missingText";
            public static readonly string MissingAnswer = "error.question.missingAnswer";
            public static readonly string Save = "error.question.save";
            public static readonly string Creation = "error.question.creation";
            public static readonly string IsInWishKnowledge = "error.question.isInWishKnowledge";
            public static readonly string Rights = "error.question.rights";
            public static readonly string ErrorOnDelete = "error.question.errorOnDelete";
            public static readonly string NoRights = "error.question.noRights";
            public static readonly string NotFound = "error.question.notFound";
            public static readonly string Unauthorized = "error.question.unauthorized";
        }

        public static class User
        {
            public static readonly string NotLoggedIn = "error.user.notLoggedIn";
            public static readonly string EmailInUse = "error.user.emailInUse";
            public static readonly string UserNameInUse = "error.user.userNameInUse";
            public static readonly string PasswordIsWrong = "error.user.passwordIsWrong";
            public static readonly string SamePassword = "error.user.samePassword";
            public static readonly string PasswordNotCorrectlyRepeated = "error.user.passwordNotCorrectlyRepeated";
            public static readonly string InputError = "error.user.inputError";
            public static readonly string PasswordResetTokenIsInvalid = "error.user.passwordResetTokenIsInvalid";
            public static readonly string PasswordResetTokenIsExpired = "error.user.passwordResetTokenIsExpired";
            public static readonly string DoesNotExist = "error.user.doesNotExist";
            public static readonly string InvalidFBToken = "error.user.invalidFBToken";
            public static readonly string PasswordTooShort = "error.user.passwordTooShort";
            public static readonly string LoginFailed = "error.user.loginFailed";
            public static readonly string FalseEmailFormat = "error.user.falseEmailFormat";
            public static readonly string NotFound = "error.user.notFound";
            public static readonly string NoRemainingWikis = "error.user.noRemainingWikis";
        }

        public static class Route
        {
            public static readonly string NoRights = "error.route.noRights";
            public static readonly string NotFound = "error.route.notFound";
            public static readonly string Unauthorized = "error.route.unauthorized";
        }

        public static readonly string Default = "error.default";

        public static class Image
        {
            public static readonly string TooBig = "error.image.tooBig";
        }

        public static class LearningSession
        {
            public static readonly string NoQuestionsAvailableWithCurrentConfig =
                "error.learningSession.noQuestionsAvailableWithCurrentConfig";
        }

        public static class Ai
        {
            public static readonly string GenerateFlashcards = "error.ai.generateFlashcards";

            public static readonly string NoFlashcardsCreatedCauseLimitAndPageIsPrivate =
                "error.ai.noFlashcardsCreatedCauseLimitAndPageIsPrivate";
        }

        public static class Skill
        {
            public static readonly string AlreadyExists = "error.skill.alreadyExists";
            public static readonly string NotFound = "error.skill.notFound";
            public static readonly string AddFailed = "error.skill.addFailed";
            public static readonly string RemoveFailed = "error.skill.removeFailed";
        }
    }

    public static class Info
    {
        public static class Page
        {
        }

        public static class Question
        {
            public static readonly string NewQuestionNotInFilter = "info.question.newQuestionNotInFilter";
            public static readonly string NotInFilter = "info.question.notInFilter";
            public static readonly string IsPrivate = "info.question.isPrivate";
            public static readonly string NotInPage = "info.question.notInPage";
        }

        public static readonly string GoogleLogin = "info.googleLogin";
        public static readonly string FacebookLogin = "info.facebookLogin";
        public static readonly string QuestionNotInFilter = "info.questionNotInFilter";

        public static readonly string PasswordResetRequested = "info.passwordResetRequested";

        public static class Ai
        {
            public static readonly string FlashcardsCreatedWillBePublicCauseLimit =
                "info.ai.flashcardsCreatedWillBePublicCauseLimit";

            public static readonly string SomeFlashcardsCreatedWillBePublicCauseLimit =
                "info.ai.someFlashcardsCreatedWillBePublicCauseLimit";
        }
    }
}