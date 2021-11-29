declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

Vue.component('category-to-private-component', {
    template: "#category-to-private",
    data() {
        return {
            categoryId: null,
            questionsToPrivate: false,
            allQuestionsToPrivate: false,
            confirmLicense: false,
            categoryName: "",
            personalQuestionIds: [],
            personalQuestionCount: 0,
            allQuestionIds: [],
            allQuestionCount: 0,
            publishSuccess: false,
            publishRequestConfirmation: false,
            forceAllQuestionsToPrivate: false,
            setToPrivateConfirmation: false,
        };
    },

    created() {
        this.categoryId = $("#hhdCategoryId").val();
    },
    destroyed() {
    },

    mounted() {
        var self = this;
        eventBus.$on('set-category-to-private', (id) => self.openSetCategoryToPrivateModal());
    },

    methods: {
        openSetCategoryToPrivateModal() {
            this.resetModal();
            this.getSetCategoryToPrivateModalData();
        },
        resetModal() {
            this.publishQuestions = false;
            this.checkedLicense = false;
            this.categoryName = "";
            this.personalQuestionIds = [];
            this.personalQuestionCount = 0;
            this.allQuestionIds = [];
            this.allQuestionCount = 0;
            this.publishSuccess = false;
            this.publishRequestConfirmation = false;
            this.publishRequestMessage = "";
        },
        getSetCategoryToPrivateModalData() {
            var self = this;
            var data = {
                categoryId: self.categoryId,
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/GetCategoryToPrivateModalData',
                data: JSON.stringify(data),
                success: function (result) {
                    if (result.success == false) {
                        let data = {
                            msg: messages.error.category[result.key],
                        }
                        eventBus.$emit('show-error', data);
                    } else {
                        self.categoryName = result.categoryName;
                        self.personalQuestionIds = result.personalQuestionIds;
                        self.personalQuestionCount = result.personalQuestionCount;
                        self.allQuestionIds = result.allQuestionIds;
                        self.allQuestionCount = result.allQuestionCount;
                        $('#CategoryToPrivateModal').modal('show');
                    }
                },
            });
        },
        setCategoryToPrivate() {
            var self = this;
            var data = {
                categoryId: self.categoryId,
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/EditCategory/SetCategoryToPrivate',
                data: JSON.stringify(data),
                success: function (result) {
                    if (result.success == true) {
                        $('#CategoryToPrivateModal').modal('hide');
                        if (self.questionsToPrivate || self.allQuestionsToPrivate)
                            self.setQuestionsToPrivate();
                        let data = {
                            msg: messages.success.category.setToPrivate,
                            reload: true,
                        }
                        eventBus.$emit('show-success', data);

                    } else {
                        $('#CategoryToPrivateModal').modal('hide');
                        let data = {
                            msg: messages.error.category[result.key]
                        };
                        eventBus.$emit('show-error', data);
                    }
                },
            });
        },
        setQuestionsToPrivate() {
            var self = this;
            var data = {
                questionIds: self.forceAllQuestionsToPrivate ? self.allQuestionsIds : self.personalQuestionIds,
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/EditQuestion/SetQuestionsToPrivate',
                data: JSON.stringify(data),
                success: function() {
                },
            });
        }
    },
});