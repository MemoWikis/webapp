﻿declare var eventBus: any;
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
            setToPrivateConfirmation: false,
        };
    },

    created() {
        this.categoryId = $("#hhdCategoryId").val();
    },
    destroyed() {
    },

    watch: {
        questionsToPrivate(val) {
            if (val)
                this.allQuestionsToPrivate = false;
        },
        allQuestionsToPrivate(val) {
            if (val)
                this.questionsToPrivate = false;
        }
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
                        Alerts.showError({
                            text: messages.error.category[result.key]
                        });
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
                        Alerts.showSuccess({
                            text: messages.success.category.setToPrivate,
                            reload: true,
                        });

                    } else {
                        $('#CategoryToPrivateModal').modal('hide');
                        Alerts.showError({
                            text: messages.error.category[result.key]
                        });
                    }
                },
            });
        },
        setQuestionsToPrivate() {
            var self = this;
            var data = {
                questionIds: self.allQuestionsToPrivate ? self.allQuestionIds : self.personalQuestionIds,
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