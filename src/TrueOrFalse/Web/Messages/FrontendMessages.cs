public static class FrontendMessageKeys
{
    public static class Success
    {
        public static class Category
        {
            public static readonly string Publish = "success_category_publish";
            public static readonly string SetToPrivate = "success_category_setToPrivate";
            public static readonly string Unlinked = "success_category_unlinked";
            public static readonly string AddedToPersonalWiki = "success_category_addedToPersonalWiki";
            public static readonly string SaveImage = "success_category_saveImage";
        }

        public static class Question
        {
            public static readonly string Created = "success_question_created";
            public static readonly string Saved = "success_question_saved";
            public static readonly string Delete = "success_question_delete";
        }

        public static class User
        {
            public static readonly string ProfileUpdate = "success_user_profileUpdate";
            public static readonly string PasswordChanged = "success_user_passwordChanged";
            public static readonly string PasswordReset = "success_user_passwordReset";
            public static readonly string VerificationMailRequestSent = "success_user_passwordVerificationMailSent";
        }
    }

    public static class Error
    {
        public static class Subscription
        {
            public static readonly string CantAddKnowledge = "error_subscription_cantAddKnowledge";
            public static readonly string CantSavePrivateQuestion = "error_subscription_cantSavePrivateQuestion";
            public static readonly string CantSavePrivateTopic = "error_subscription_cantSavePrivateTopic";
        }

        public static class Page
        {
            public static readonly string ParentIsPrivate = "error_page_parentIsPrivate";
            public static readonly string PublicChildCategories = "error_page_publicChildCategories";
            public static readonly string PublicQuestions = "error_page_publicQuestions";
            public static readonly string NotLastChild = "error_page_notLastChild";
            public static readonly string NoRemainingParents = "error_page_noRemainingParents";
            public static readonly string ParentIsRoot = "error_page_parentIsRoot";
            public static readonly string LoopLink = "error_page_loopLink";
            public static readonly string IsAlreadyLinkedAsChild = "error_page_isAlreadyLinkedAsChild";
            public static readonly string IsNotAChild = "error_page_isNotAChild";
            public static readonly string IsLinkedInNonWuwi = "error_page_isLinkedInNonWuwi";
            public static readonly string ChildIsParent = "error_page_childIsParent";
            public static readonly string NameIsTaken = "error_page_nameIsTaken";
            public static readonly string NameIsForbidden = "error_page_nameIsForbidden";
            public static readonly string RootCategoryMustBePublic = "error_page_rootCategoryMustBePublic";
            public static readonly string MissingRights = "error_page_missingRights";
            public static readonly string TooPopular = "error_page_tooPopular";
            public static readonly string SaveImageError = "error_page_saveImageError";
            public static readonly string PinnedQuestions = "error_page_pinnedQuestions";
            public static readonly string CircularReference = "error_page_circularReference";
            public static readonly string TopicNotSelected = "error_page_topicNotSelected";
            public static readonly string NewTopicIdIsTopicIdToBeDeleted = "error_page_newTopicIdIsTopicIdToBeDeleted";
            public static readonly string NotFound = Route.NotFound;
            public static readonly string NoRights = Route.NoRights;
            public static readonly string Unauthorized = Route.Unauthorized;
            public static readonly string NoChange = "error_page_noChange";
        }

        public static class Question
        {
            public static readonly string MissingText = "error_question_missingText";
            public static readonly string MissingAnswer = "error_question_missingAnswer";
            public static readonly string Save = "error_question_save";
            public static readonly string Creation = "error_question_creation";
            public static readonly string IsInWuwi = "error_question_isInWuwi";
            public static readonly string Rights = "error_question_rights";
            public static readonly string ErrorOnDelete = "error_question_errorOnDelete";
            public static readonly string NoRights = Route.NoRights;
            public static readonly string NotFound = Route.NotFound;
            public static readonly string Unauthorized = Route.Unauthorized;
        }

        public static class User
        {
            public static readonly string NotLoggedIn = "error_user_notLoggedIn";
            public static readonly string EmailInUse = "error_user_emailInUse";
            public static readonly string UserNameInUse = "error_user_userNameInUse";
            public static readonly string PasswordIsWrong = "error_user_passwordIsWrong";
            public static readonly string SamePassword = "error_user_samePassword";
            public static readonly string PasswordNotCorrectlyRepeated = "error_user_passwordNotCorrectlyRepeated";
            public static readonly string InputError = "error_user_inputError";
            public static readonly string PasswordResetTokenIsInvalid = "error_user_passwordResetTokenIsInvalid";
            public static readonly string PasswordResetTokenIsExpired = "error_user_passwordResetTokenIsExpired";
            public static readonly string DoesNotExist = "error_user_doesNotExist";
            public static readonly string InvalidFBToken = "error_user_invalidFBToken";
            public static readonly string PasswordTooShort = "error_user_passwordTooShort";
            public static readonly string LoginFailed = "error_user_loginFailed";
            public static readonly string FalseEmailFormat = "error_user_falseEmailFormat";
            public static readonly string NotFound = Route.NotFound;
        }

        public static class Route
        {
            public static readonly string NoRights = "error_route_noRights";
            public static readonly string NotFound = "error_route_notFound";
            public static readonly string Unauthorized = "error_route_unauthorized";
        }

        public static readonly string Default = "error_default";

        public static class Image
        {
            public static readonly string TooBig = "error_image_tooBig";
        }

        public static class LearningSession
        {
            public static readonly string NoQuestionsAvailableWithCurrentConfig = "error_learningSession_noQuestionsAvailableWithCurrentConfig";
        }
    }

    public static class Info
    {
        public static class Category
        {
        }

        public static class Question
        {
            public static readonly string NewQuestionNotInFilter = "info_question_newQuestionNotInFilter";
            public static readonly string NotInFilter = "info_question_notInFilter";
            public static readonly string IsPrivate = "info_question_isPrivate";
            public static readonly string NotInTopic = "info_question_notInTopic";
        }

        public static readonly string GoogleLogin = "info_googleLogin";
        public static readonly string FacebookLogin = "info_facebookLogin";
        public static readonly string QuestionNotInFilter = "info_questionNotInFilter";

        public static readonly string PasswordResetRequested = "info_passwordResetRequested";
    }
}
