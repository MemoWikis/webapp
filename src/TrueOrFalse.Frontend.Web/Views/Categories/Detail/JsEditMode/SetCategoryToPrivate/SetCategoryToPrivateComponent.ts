declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

Vue.component('publish-category-component',{
    data() {
        return {
            categoryId: null,
            setQuestionsPrivate: false,
            confirmLicense: false,
            categoryName: "",
            personalQuestionIds: [],
            personalQuestionCount: 0,
            allQuestionIds: [],
            allQuestionCount: 0,
            publishSuccess: false,
            publishRequestConfirmation: false,
            forceAllQuestionsToPrivate: false,
        };
    },

    created() {
        this.categoryId = $("#hhdCategoryId").val();
    },
    destroyed() {
    },

    mounted() {
    },

    methods: {
        openSetCategoryToPrivateModal() {
            this.resetModal();
            this.getSetCategoryToPrivateModalData();
            $('#SetToPrivateModal').modal('show');
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
                url: '/GetSetCategoryToPrivateModalData',
                data: JSON.stringify(data),
                success: function (result) {
                    if (result.success == false) {

                    }
                    self.categoryName = result.categoryName;
                    self.personalQuestionIds = result.personalQuestionIds;
                    self.personalQuestionCount = result.personalQuestionCount;
                    self.allQuestionIds = result.allQuestionIds;
                    self.allQuestionCount = result.allQuestionCount;
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
                    if (result.success) {
                        $('#SetToPrivateModal').modal('hide');
                        if (self.setQuestionsToPrivate)
                            self.setQuestionsToPrivateHandler();
                        let data = {
                            msg: messages.success.category.setToPrivate,
                            reload: true,
                        }
                        eventBus.$emit('show-success', data);

                    } else {
                        $('#SetToPrivateModal').modal('hide');
                        let data = {
                            msg: messages.error.category[result.key]
                        };
                        eventBus.$emit('show-error', data);
                    }
                },
            });
        },
        setQuestionsToPrivateHandler() {
            var self = this;
            var data = {
                questionIds: self.forceAllQuestionsToPrivate ? self.allQuestionsIds : self.personalQuestionIds,
                allQuestions: self.forceAllQuestionsToPrivate
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