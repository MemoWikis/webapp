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
            id: [String, Number],
            showSearchIcon: Boolean,
            showSearch: Boolean,
        },
        template: '#search-component',

        data() {
            return {
                debounceSearch: _.debounce(this.search, 300),
                categories: [],
                questions: [],
                users: [],
                selectedItem: '',
                open: false,
                showDropdown: false,
                searchTerm: '',
                searchUrl: '',
                isMounted: false,
                lockDropdown: false,
                noResults: false,
                categoryCount: 0,
                questionCount: 0,
                usersCount: 0,
                userSearchUrl: '',
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
            showSearch(val) {
                if (val)
                    this.$refs.searchInput.focus();
            }
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
            var self = this;
            this.isMounted = true;
            $(document).mouseup(function (e) {
                var container = $("#"+ self.id +"Container");

                if (!container.is(e.target) && container.has(e.target).length === 0) {
                    self.showDropdown = false;
                }
            });
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
                        self.categoryCount = result.categoryCount;
                        self.questionCount = result.questionCount;
                        self.userCount = result.userCount;
                        self.userSearchUrl = result.userSearchUrl;
                        self.$nextTick(() => {
                            $('[data-toggle="tooltip"]').tooltip();
                        });
                    },
                });
            },

            selectItem(item) {
                this.selectedItem = item;
            },
            openUsers() {
                location.href = this.userSearchUrl;
            }
        }
    });