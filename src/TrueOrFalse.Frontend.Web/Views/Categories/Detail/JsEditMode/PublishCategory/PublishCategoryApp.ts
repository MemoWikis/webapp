declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

new Vue({
    el: '#PublishCategoryApp',
    data() {
        return {
            categoryId: null,
            publishQuestions: false,
            confirmLicense: false,
            categoryName: "",
            questionIds: [],
            questionCount: 0,
            publishSuccess: false,
            publishRequestConfirmation: false,
            blinkTimer: null,
            blink: false,
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
        openPublishModal() {
            this.resetModal();
            this.getCategoryPublishModalData();
            $('#PublishCategoryModal').modal('show');
        },
        resetModal() {
            this.blinkTimer = null;
            this.blink = false;
            this.publishQuestions = false;
            this.checkedLicense = false;
            this.categoryName = "";
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

            if (!self.confirmLicense) {
                self.blinkTimer = null;
                self.blink = true;
                self.blinkTimer = setTimeout(() => {
                        self.blink = false;
                    },
                    2000);
                return;
            }
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