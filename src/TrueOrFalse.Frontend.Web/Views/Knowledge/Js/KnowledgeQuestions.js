
Vue.use(Vuetable);

new Vue({
    el: '#app',
    components: {
        'vuetable-pagination': Vuetable.VuetablePagination
    },
    data: {
        tooltip: 'in neuem Link öffnen',
        moreParams: {
            'isAuthor': false
        },
        fields: [
            {
                name: "__slot:questionTitle",
                title: "Frage",
                dataClass: "td-question"
            },
            {
                name: "__slot:knowWas",
                title: 'Wissensstand',
                sortField: "knowWas",
                dataClass: "td-know-was"
            },
            {
                name: "__slot:authorImage",
                title: "Autor",
                sortField: "author",
                dataClass: "author-image-name"
            },
            {
                name: "__slot:category",
                title: "Thema",
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
                    return '<i class="sort-icon fa fa-sort" style="opacity:1;position:relative;"></i>';
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
        onPaginationData(paginationData) {
            this.$refs.pagination.setPaginationData(paginationData);
        },
        GetCategoryImageSourceUrl(CategoryImageData) {
            if (CategoryImageData == null || CategoryImageData.Url === null || CategoryImageData.Url === $("#hddNoQuestionUrl").val())
                return $("#hddNoCategoryUrl").val();
            else
                return CategoryImageData.Url;
        },
        GetQuestionImageSourceUrl(QuestionImageData) {
            if (QuestionImageData == null || QuestionImageData.SourceUrl === null)
                return $("#hddNoQuestionUrl").val();
            else
                return QuestionImageData.Url;
        },
        onChangePage(page) {
            this.$refs.vuetable.changePage(page);
        },
        onLoaded() {
            $("#app").css("Opacity", "1");
            $('[data-toggle="tooltip"]').tooltip();
            $('#header').text("Du hast " + $('#hddCountQuestion').val() + " Fragen in deinem Wunschwissen");
            $(".spinner").hide();
        },
        loading() {
            $("#app").css("Opacity", "0.3");
        },
        switchOnlySelfCreatedChanged: function () {
            $("#app").css("Opacity", "0.3");
            this.moreParams.isAuthor = $("#switchShowOnlySelfCreated").is(":checked");
            this.$refs.vuetable.refresh();
            $(".spinner").fadeIn();
        }, mounted() {
        }, destroyed() {
        }, beforeUpdate() {
        }
    }
});

$(".onoffswitch-label").on("click", () => { $("#circle").css("display", "block"); });


