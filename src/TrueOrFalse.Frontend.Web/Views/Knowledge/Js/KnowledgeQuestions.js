
Vue.use(Vuetable);

new Vue({
    el: '#app',
    components: {
        'vuetable-pagination': Vuetable.VuetablePagination
    },
    data: {
        moreParams: {
            'isAuthor': false
        },

        fields: [
            {
                name: "__slot:questionTitel",
                title: "Frage",
                dataClass: "td-question"
            },
            {
                name: "__slot:knowWas",
                title: 'Wissensstand',
                sortField: "knowWas"
            },
            {
                name: "__slot:authorImage",
                title: "Author",
                sortField: "author",
                dataClass: "author-image-name"
            },
            {
                name: "__slot:category",
                title: "Kategorie",
                sortField: "category",
                dataClass: "category-table"

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
                ascendingIcon: 'fa fa-sort',
                descendingIcon: 'fa fa-sort',
                renderIcon: function () {
                    return '<i class="sort-icon fa fa-sort" style="opacity:1;position:relative;"></i>' ;
                }
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
        },
        onLoaded(props) {
            $("#circle").fadeOut();
            $("#app").css("Opacity", "1");
            $('[data-toggle="tooltip"]').tooltip();
        },
        loading() {
            $("#app").css("Opacity", "0.3");
        },
        switchOnlySelfCreatedChanged: function () {
            console.log("wird geändert");
            $("#app").css("Opacity", "0.3");

            this.moreParams.isAuthor = $("#switchShowOnlySelfCreated").is(":checked");
            this.$refs.vuetable.refresh();
        }
    }
});

$(".onoffswitch-label").on("click", () => { $("#circle").css("display", "block"); });


