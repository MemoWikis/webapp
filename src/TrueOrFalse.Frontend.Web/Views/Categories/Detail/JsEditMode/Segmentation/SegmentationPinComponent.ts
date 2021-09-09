Vue.component('pin-category-component', {
    template: '#pin-category-template',

    props: {
        categoryId: [Number, String],
        initialWishknowledgeState: Boolean,
        isHistoric: Boolean,
    },
    data() {
        return {
            isInWishknowledge: false,
            isLoading: false,
            stateLoad: 'notAdded',
        }
    },
    watch: {
    },
    mounted() {
        this.isInWishknowledge = this.initialWishknowledgeState;
        if (this.isInWishknowledge)
            this.stateLoad = 'added';
    },
    methods: {
        addToWishknowledge() {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("PinCategory");
                return;
            }

            if (this.stateLoad == 'loading')
                return;
            var self = this;
            self.stateLoad = 'loading';
            var data = {
                categoryId: self.categoryId,
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/Api/Category/Pin',
                data: JSON.stringify(data),
                success: function (result) {
                    if (result == 'True')
                        self.isInWishknowledge = true;
                    else
                        self.isInWishknowledge = false;
                    if (self.isInWishknowledge)
                        self.stateLoad = 'added';
                    else
                        self.stateLoad = 'notAdded';

                    self.renderNewKnowledgeBar();
                },
            });
        },
        removeFromWishknowledge() {
            var self = this;
            if (this.stateLoad == 'loading')
                return;
            self.stateLoad = 'loading';
            var data = {
                categoryId: self.categoryId,
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/Api/Category/UnPin',
                data: JSON.stringify(data),
                success: function (result) {
                    if (result == 'False')
                        self.isInWishknowledge = true;
                    else
                        self.isInWishknowledge = false;
                    if (self.isInWishknowledge)
                        self.stateLoad = 'added';
                    else
                        self.stateLoad = 'notAdded';

                    $("#JS-RemoveQuestionsCat").attr("data-category-id", self.categoryId);
                    $("#UnpinCategoryModal").modal('show');

                    self.renderNewKnowledgeBar();
                },
            });
        },

        renderNewKnowledgeBar() {
            var self = this;
            $.ajax({
                url: '/Category/RenderNewKnowledgeSummaryBar?categoryId=' + self.categoryId,
                type: 'GET',
                success: data => {
                    $(".category-knowledge-bar[data-category-id='" + self.categoryId + "']").replaceWith(data);
                    $('.show-tooltip').tooltip();
                },
            });

            var parentId = $('#hhdCategoryId').val();
            $.ajax({
                url: '/Category/RenderNewKnowledgeSummaryBar?categoryId=' + parentId,
                type: 'GET',
                success: data => {
                    $(".category-knowledge-bar[data-category-id='" + parentId + "']").replaceWith(data);
                    $('.show-tooltip').tooltip();
                },
            });
        }
    }
});
