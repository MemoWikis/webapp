declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

var pub = Vue.component('publish-category-component', {
    template: '#publish-category',
    data() {
        return {
            currentCategoryId: null,
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
        this.currentCategoryId = $("#hhdCategoryId").val();
    },
    mounted() {
        var self = this;
        eventBus.$on('open-publish-category-modal', (id) => self.openPublishModal(id));
    },

    methods: {
        openPublishModal(id) {
            this.categoryId = id;
            this.resetModal();
            this.getCategoryPublishModalData();
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
                    $('#PublishCategoryModal').modal('show');
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
                    if (result.success) {
                        $('#PublishCategoryModal').modal('hide');

                        var publishCurrentCategory = self.currentCategoryId == self.categoryId;

                        Alerts.showSuccess({
                            text: messages.success.category.publish,
                            reload: publishCurrentCategory,
                        });

                        if (!publishCurrentCategory)
                            eventBus.$emit('publish-category', self.categoryId);

                        if (self.publishQuestions)
                            self.publishPrivateQuestions();

                    } else {
                        $('#PublishCategoryModal').modal('hide');
                        Alerts.showError({
                            text: messages.error.category[result.key]
                        });
                    }
                },
            });
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
                success: function() {
                },
            });
        }
    },
});