enum SearchType {
    All = 0,
    Category = 1,
    Questions = 2,
    Users = 3
}

Vue.component('search-component',
    {
        props: {
            searchType: SearchType,
            id: [String, Number]
        },
        template: '#search-component',

        data() {
            return {
                debounceSearch: _.debounce(this.search, 300),
                categories: [],
                questions: [],
                users: [],
                selectedItem: null,
                open: false,
                showDropdown: false,
                searchTerm: '',
                searchUrl: '',
                isMounted: false,
                lockDropdown: false,
                noResults: false,
            }
        },
        watch: {
            selectedItem(item) {
                this.$emit('select-item', item);
            },
            searchTerm(term) {
                if (term.length > 0 && this.lockDropdown == false) {
                    this.showDropdown = true;
                    this.debounceSearch();
                }
                else
                    this.showDropdown = false;
            },
        },
        created() {
            var self = this;
            switch (self.searchType) {
                case SearchType.Category:
                    self.url = '/Api/Search/Category';
                    break;
                default:
                    self.url = '/Api/Search/ByName';
            }
        },
        mounted() {
            this.isMounted = true;
        },
        methods: {
            search() {
                this.showDropdown = true;
                var self = this;
                var data = {
                    term: self.searchTerm,
                };

                $.ajax({
                    type: 'Post',
                    contentType: "application/json",
                    url: '/Api/Search/ByName',
                    data: JSON.stringify(data),
                    success: function (result) {
                        self.categories = result.categories;
                        self.questions = result.questions;
                        self.users = result.users;
                        self.noResults = result.categories.length + result.questions.length + result.users.length <= 0;
                        self.totalCount = result.totalCount;
                        self.$nextTick(() => {
                            $('[data-toggle="tooltip"]').tooltip();
                        });
                    },
                });
            },

            selectItem(item) {
                this.selectedItem = item;
            }
        }
    });