interface IComments {
    id: number,
    creatorName: string,
    creationDate: string,
    creationDateNiceText: string,
    imageUrl: string,
    creatorUrl: string,
    title: string,
    text: string,
    shouldBeImproved: boolean,
    shouldBeDeleted: boolean,
    isSettled: boolean,
    shouldReasons: string[],
    answers: IComments[],
    settledAnswersCount: number,
    showSettledAnswers: boolean,
}


Vue.component('comment-section-component',
    {
        props: ['questionId'],

        data() {
            return {
                isLoggedIn: IsLoggedIn.Yes,
                comments: [] as IComments[],
                settledComments: [] as IComments[],
                currentUserImageUrl: '',
                currentUserId: 0,
                showSettledComments: false,
                commentsLoaded: false,
                currentUserName: '',
                isAdmin: false,
                allLoaded: false

        };
        },
        template: '#comment-section-component',
        created() {
            var self = this;
            self.allLoaded = false;
            Utils.ShowSpinner();
            this.getCurrentUserImgUrl();
            this.getCurrentUserId();
            this.getCurrentUserAdmin();
            this.getCurrentUserName();
            this.getComments();
            this.getSettledComments();
            eventBus.$on('new-comment-added', function () {
                self.getComments();
                self.getSettledComments();
            });

        },
        methods: {
            getComments() {
                const self = this;
                $.post("/AnswerComments/GetComments?questionId=" + self.questionId, data => {
                    self.comments = JSON.parse(data) as IComments[];
                    this.commentsLoaded = true;
                    eventBus.$emit('send-comments-count', self.questionId, self.comments.length);
                    Utils.HideSpinner();
                    self.allLoaded = true;
                });
            },
            getSettledComments() {
                const self = this;
                $.post("/AnswerComments/GetSettledComments?questionId=" + self.questionId, data => {
                    self.settledComments = JSON.parse(data) as IComments[];
                    this.commentsLoaded = true;
                });
            },
            getCurrentUserImgUrl() {
                const self = this;
                $.post("/AnswerComments/GetCurrentUserImgUrl", url => {
                    self.currentUserImageUrl = url as String;
                });
            },
            getCurrentUserId() {
                $.post("/AnswerComments/GetCurrentUserId", id => {
                    this.currentUserId = parseInt(id);
                });
            },
            getCurrentUserName() {
                $.post("/AnswerComments/GetCurrentUserName", name => {
                    this.currentUserName = name;
                });
            },
            getCurrentUserAdmin() {
                $.post("/AnswerComments/GetCurrentUserAdmin", admin => {
                    this.isAdmin = admin;
                });
            }
        }
    });