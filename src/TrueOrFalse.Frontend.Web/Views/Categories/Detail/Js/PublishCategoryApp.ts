declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

new Vue({
    el: '#PublishCategoryApp',
    data() {
        return {
            categoryId: null,
            publishQuestions: false,
            checkedLicense: false,
            categoryname: "",
            questionIds: [],
            questionCount: 0,
            publishSuccess: false,
            publishRequestConfirmation: false,
            publishRequestMessage: "",
        };
    },

    created() {
        this.categoryId = $("#hhdCategoryId").val();
    },
    destroyed() {
    },

    methods: {
        openPublishModal() {
            this.resetModal();
            this.getCategoryPublishModalData();
            $('#PublishCategoryModal').modal('show');
        },
        resetModal() {
            this.publishQuestions = false;
            this.checkedLicense = false;
            this.categoryname = "";
            this.questionIds = [];
            this.questionCount = 0;
            this.publishSuccess = false;
            this.publishRequestConfirmation = false;
            this.publishRequestMessage = "";
        },
        getCategoryPublishModalData() {
            var self = this;
            var data = {
                categoryId: self.categoryId,
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/GetCategoryPublishModalData',
                data: JSON.stringify(data),
                success: function (result) {
                    self.categoryName = result.categoryName;
                    self.questionIds = result.questionIds;
                    self.questionCount = result.questionCount;
                },
            });
        },
        publishCategory() {
            var self = this;
            var data = {
                categoryId: self.categoryId,
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/EditCategory/PublishCategory',
                data: JSON.stringify(data),
                success: function (result) {
                    self.publishRequestConfirmation = true;
                    self.publishSuccess = result.success;
                    self.publishRequestMessage = result.message;
                    if (self.publishQuestions && result.success)
                        self.publishPrivateQuestions();
                },
            })
        },
        publishPrivateQuestions() {
            var self = this;
            var data = {
                questionIds: self.questionIds,
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/EditQuestion/PublishQuestions',
                data: JSON.stringify(data),
                success: function () {
                },
            })
        }
    },
});