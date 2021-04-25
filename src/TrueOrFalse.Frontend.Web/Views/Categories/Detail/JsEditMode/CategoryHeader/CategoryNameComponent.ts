declare var VueTextareaAutosize: any;
Vue.use(VueTextareaAutosize);
Vue.component('category-name-component',
    {
        props: ['oldCategoryName','categoryId','isLearningTab'],
        data() {
            return {
                categoryName: "",
                categoryNameAllowed: null,
                errorMsg: "",
                disabled: false,
            }
        },
        created() {
            this.categoryName = this.oldCategoryName;
        },
        watch: {
            categoryName(name) {
                if (name == this.oldCategoryName || name.length <= 0)
                    return;
                this.validateName(name);
                eventBus.$emit('content-change');
            }
        },
        mounted() {
            if (this.isLearningTab == 'True') {
                this.controlTab('LearningTab');
            };
            eventBus.$on('request-save', () => this.saveName());
            eventBus.$on('cancel-edit-mode',
                () => {
                    this.categoryName = this.oldCategoryName;
                });
            eventBus.$on('tab-change',
                (tabName) => {
                    this.controlTab(tabName);
                });
        },
        methods: {
            controlTab(tabName) {
                if (tabName == 'TopicTab')
                    this.disabled = false;
                else
                    this.disabled = true;
            },
            validateName: _.debounce(function (name) {
                var self = this;
                if (name.length <= 0) {
                    self.errorMsg = "Bitte gebe einen Thementitel ein.";
                    return;
                }
                $.ajax({
                    type: 'Post',
                    contentType: "application/json",
                    url: '/EditCategory/ValidateName',
                    data: JSON.stringify({ name: name }),
                    success: function (data) {
                        self.categoryNameAllowed = data.categoryNameAllowed;
                        if (data.categoryNameAllowed)
                            eventBus.$emit('name-is-valid', { isValid:true });
                        else {
                            self.errorMsg = data.errorMsg;
                            eventBus.$emit('name-is-valid', { isValid: false, msg: name + data.errorMsg });
                        }

                    },
                });
            }, 500),
            requestSave() {
                eventBus.$emit('request-save');
            },
            saveName() {
                if (this.categoryName == this.oldCategoryName)
                    return;
                var self = this;
                var id = parseInt(this.categoryId);
                var name = this.categoryName;
                $.ajax({
                    type: 'Post',
                    contentType: "application/json",
                    url: '/EditCategory/SaveName',
                    data: JSON.stringify({
                        categoryId: id,
                        name: name
                    }),
                    success: function (result) { 
                        if (result.nameHasChanged) {
                            document.title = name;
                            $('#BreadCrumbTrail > div:last-child a').text(name).attr("href", result.newUrl);
                            window.history.pushState("", name, result.newUrl);
                        } else {
                        }
                    },
                });
            }
        },
    });
