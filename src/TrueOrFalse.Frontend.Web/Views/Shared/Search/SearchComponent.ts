Vue.component('search-component',
    {
        props: {
        },
        template: '',

        data() {
            return {
                searchBoxId: null,
                debounceSearch: _.debounce(this.search, 300),
                categories: [],
                questions: [],
                users: [],
                selectedItem: null,
            }
        },
        watch: {
            selectedItem(val) {
                this.$parent.selectedItem = val;
            }
        },
        created() {
            this.searchBoxId = this._uid;
        },
        mounted() {
        },
        methods: {
            search() {
                this.showDropdown = true;
                var self = this;
                var data = {
                    term: self.searchTerm,
                    categoriesToFilter: self.categoriesToFilter,
                };

                $.ajax({
                    type: 'Post',
                    contentType: "application/json",
                    url: '/Api/Search/ByNameForVue',
                    data: JSON.stringify(data),
                    success: function (result) {
                        self.categories = result.categories;
                        self.questions = result.questions;
                        self.users = result.users;
                        self.totalCount = result.totalCount;
                        self.$nextTick(() => {
                            $('[data-toggle="tooltip"]').tooltip();
                        });
                    },
                });
            },
        }
    });