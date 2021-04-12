declare var VueTextareaAutosize: any;
Vue.use(VueTextareaAutosize);
Vue.component('category-name-component',
    {
        props: ['oldCategoryName','categoryId'],
        data() {
            return {
                categoryName: "",
                categoryNameAllowed: null,
                errorMsg: "",
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
            eventBus.$on('request-save', () => this.saveName());
        },
        methods: {
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
                        if (!data.categoryNameAllowed)
                            self.errorMsg = data.errorMsg;
                    },
                });
            }, 500),
            saveName() {
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
                            self.$parent.categoryName = name;
                            self.$parent.editCategoryName = false;
                            $('#BreadCrumbTrail > div:last-child a').text(name).attr("href", result.newUrl);
                        }
                    },
                });
            }
        },
    });
