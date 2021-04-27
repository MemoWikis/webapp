var pinComponent = Vue.component('pin-category-component', {
    template: '#pin-category-template',

    props: {
        categoryId: [Number, String],
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
        this.loadWishknowledge();
    },
    methods: {
        loadWishknowledge() {
            if (NotLoggedIn.Yes())
                return;
            var self = this;
            self.stateLoad = 'loading';
            var data = {
                categoryId: self.categoryId,
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/Category/GetWishknowledge',
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
                },
            });
        },
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

var categoryCardComponent = Vue.component('category-card-component', {
    props: {
        categoryId: [String, Number],
        editMode: Boolean,
        isCustomSegment: Boolean,
        selectedCategories: Array,
        segmentId: [String, Number],
        hide: String,
    },

    data() {
        return {
            visible: true,
            hover: false,
            dropdownId: null,
            id: parseInt(this.categoryId),
            isSelected: false,
            checkboxId: '',
            showHover: false,
        };
    },
    watch: {
        selectedCategories() {
            this.isSelected = this.selectedCategories.includes(this.id);
        },
        hover(val) {
            if (val && this.editMode)
                this.showHover = true;
            else
                this.showHover = false;
        },
    },
    mounted() {
        this.dropdownId = this.segmentId + '-Dropdown' + this.id ;
        this.checkboxId = this.segmentId + '-Checkbox' + this.id;
        if (this.isCustomSegment)
            this.dropdownId += this.$parent.id;
    },
    methods: {
        thisToSegment() {
            if (!this.isCustomSegment)
                this.$parent.loadSegment(this.id);
        },
        selectCategory() {
            if (this.editMode) {
                this.isSelected = this.selectedCategories.includes(this.id);

                if (this.isSelected)
                    this.$emit('unselect-category', this.id);
                else
                    this.$emit('select-category', this.id);
            }
        },
        removeParent() {
            var self = this;
            var data = {
                parentCategoryIdToRemove: self.$parent.categoryId,
                childCategoryId: self.categoryId,
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/EditCategory/RemoveParent',
                data: JSON.stringify(data),
                success: function (data) {
                    if (data.success == true)
                        self.visible = false;
                },
            });

        },
    }
});

var segmentComponent = Vue.component('segment-component', {
    props: {
        title: String,
        description: String,
        childCategoryIds: String,
        categoryId: String,
        editMode: Boolean,
    },

    data() {
        return {
            categories: [],
            id: null,
            cardsKey: null,
            isCustomSegment: true,
            visible: true,
            selectedCategories: [],
            currentChildCategoryIds: [],
            hover: false,
            showHover: false,
            addCategoryId: "",
            dropdownId: null,
            controlWishknowledge: false,
            timer: null,
        };
    },

    created() {
    },

    mounted() {
        this.id = "Segment-" + this.categoryId;
        if (this.childCategoryIds != null)
            this.currentChildCategoryIds = JSON.parse(this.childCategoryIds);
        var segment = {
            CategoryId: parseInt(this.categoryId),
            Title: this.title,
            ChildCategoryIds: this.categories
        }
        this.addCategoryId = "AddCategoryTo-" + this.id + "-Btn";
        if (this.categoryId != undefined)
            eventBus.$emit('new-segment', segment);
        this.dropdownId = this.id + '-Dropdown';
    },

    watch: {
        hover(val) {
            if (val && this.editMode)
                this.showHover = true;
            else
                this.showHover = false;
        }
    },

    updated() {
    },

    methods: {
        selectCategory(id) {
            if (this.selectedCategories.includes(id))
                return;
            else this.selectedCategories.push(id);
        },
        unselectCategory(id) {
            if (this.selectedCategories.includes(id)) {
                var index = this.selectedCategories.indexOf(id);
                this.selectedCategories.splice(index, 1);
            }
        },
        updateCategoryOrder() {
            this.categories = $("#" + this.id + " > .topic").map((idx, elem) => $(elem).attr("category-id")).get();
        },
        removeSegment() {
            eventBus.$emit('remove-segment', parseInt(this.categoryId));
            this.visible = false;
        },
        addCategory() {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("CreateCategory");
                return;
            }
            var self = this;
            var parent = {
                id: self.categoryId,
                addCategoryBtnId: $("#" + self.addCategoryId),
                moveCategories: false,
            }
            $('#AddCategoryModal').data('parent', parent).modal('show');
        },
        removeChildren() {
            var self = this;
            var data = {
                parentCategoryId: self.categoryId,
                childCategoryIds: self.selectedCategories,
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/EditCategory/RemoveChildren',
                data: JSON.stringify(data),
                success: function (data) {
                    self.selectedCategories.map(categoryId => {
                        self.$refs['card' + categoryId].visible = false;
                    });
                },
            });
        },
        updateKnowledgeBar() {

        },
    },
});