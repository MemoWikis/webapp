
Vue.use(Vuetable);

new Vue({
    el: '#app',
    components: {
        'vuetable-pagination': Vuetable.VuetablePagination
    },
    data: {
        moreParams: {
            'isAuthor': false,
            'heading': ""
        },

        fields: [
            {
                name: "__slot:image",
                title: "Frage"
            },
            {
                name: 'Titel',
                title: ""

                //sortField: 'name'
            },
            {
                name: "__slot:knowWas",
                title: 'Wissensstand',
                sortField: "knowWas"
            },
            {
                name: "__slot:authorImage",
                title: "Author",
                sortField: "author"
            },
            {
                name: "__slot:category",
                title: "Kategorie",
                sortField: "category"

            }
        ],
        sortOrder: [
            { field: 'knowWas', direction: 'asc' },
            { field: 'author', direction: 'asc' },
            { field: 'category', direction: 'asc' }
        ],
        css: {
            table: {
                tableClass: ' table table-striped table-hovered',
                ascendingIcon: 'glyphicon glyphicon-chevron-up',
                descendingIcon: 'glyphicon glyphicon-chevron-down'
            },
            pagination: {
                infoClass: 'pull-left',
                wrapperClass: 'vuetable-pagination pull-right',
                activeClass: 'btn-primary',
                disabledClass: 'disabled',
                pageClass: 'btn btn-border',
                linkClass: 'btn btn-border',
                icons: {
                    first: '',
                    prev: '',
                    next: '',
                    last: ''
                }
             }
        }
    },

    methods: {
        mouseOver() {
            $('.show-tooltip').tooltip();
        },
        onPaginationData(paginationData) {
            this.$refs.pagination.setPaginationData(paginationData);
        },
        onChangePage(page) {
            this.$refs.vuetable.changePage(page);
        },
        GetImageSourceUrl(url) {
            if (url === null)
                return "/Images/no-category-picture-350.png";
            return url.SourceUrl;
        }

    }
});

