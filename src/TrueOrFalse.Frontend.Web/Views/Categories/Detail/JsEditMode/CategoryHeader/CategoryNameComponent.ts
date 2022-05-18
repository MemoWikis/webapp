declare var VueTextareaAutosize: any;
Vue.use(VueTextareaAutosize);

Vue.component('category-name-component',
    {
        props: ['originCategoryName','categoryId','isLearningTab'],
        data() {
            return {
                oldCategoryName: "",
                categoryName: "",
                categoryNameAllowed: null,
                errorMsg: "",
                disabled: false,
                nameHasChanged: false,
            }
        },
        created() {
            this.oldCategoryName = this.originCategoryName;
            this.categoryName = this.oldCategoryName;
        },
        watch: {
            categoryName(name) {
                if (name == this.oldCategoryName || name.length <= 0)
                    return;
                this.nameHasChanged = true;
                eventBus.$emit('content-change', 'categoryName');
            }
        },
        mounted() {
            if (this.isLearningTab == 'True') {
                this.controlTab('LearningTab');
            };
            eventBus.$on('request-save', () => {
                if(this.nameHasChanged)
                    this.saveName(); 
            });
            eventBus.$on('cancel-edit-mode',
                () => {
                    this.categoryName = this.oldCategoryName;
                });
            eventBus.$on('tab-change',
                (tabName) => {
                    this.controlTab(tabName);
                });
            var self = this;
            $(window).resize(() => {
                self.$refs.categoryNameArea.resize();
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
                        if (data.categoryNameAllowed) {
                            eventBus.$emit('name-is-valid', { isValid: true });
                            self.nameHasChanged = true;
                        }
                        else {
                            let errorMsg = messages.error.category[data.key];
                            self.errorMsg = errorMsg;
                            Alerts.showError({
                                text: name + ' ' + errorMsg
                            });
                            eventBus.$emit('name-is-valid', { isValid: false, msg: name + ' ' + errorMsg });
                        }
                    },
                });
            }, 500),
            saveName() {
                if (this.categoryName == this.oldCategoryName)
                    return;
                var self = this;
                var id = parseInt(this.categoryId);
                var name = this.categoryName;
                this.validateName(name);
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
                            self.oldCategoryName = result.categoryName;
                            var saveMessage = "Das Thema wurde gespeichert.";
                            eventBus.$emit('save-success');
                            document.title = name;
                            $('#BreadCrumbTrail > div:last-child a').text(name).attr("href", result.newUrl);
                            window.history.pushState("", name, result.newUrl);
                            self.nameHasChanged = false;
                        } else {
                            var saveMessage = "Das Thema konnte nicht gespeichert werden.";
                        }
                        eventBus.$emit('save-msg', saveMessage);
                    },
                });
            },
        },
    });
