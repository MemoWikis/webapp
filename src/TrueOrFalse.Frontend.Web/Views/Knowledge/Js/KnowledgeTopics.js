
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
                name: "__slot:imageAndTitle",
                title: "Thema/Lernset",
                sortField: 'name',
                dataClass: 'td-topic'
            },
            {
                name: "KnowlegdeWishPartial",
                title: "Wissensstand",
                dataClass: "KnowledgeBarWrapper",
                sortField: "knowledgeBar",
                html: true
            },
            {
                name: '__slot:topicCount',
                dataClass: "topic-count-td",
                title: "Größe"
            },
            {
                name: '__slot:dropDown' ,
                dataClass: "drop-down-td"
            }
            
        ],
        sortOrder: [
            { field: 'name', direction: 'asc' },
            { field: 'knowledgeBar', direction: 'asc' }
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
        onChangePage(page) {
            this.$refs.vuetable.changePage(page);
          this.initializeTooltip();
        },
        deleteRow: function (id, IsCategory, index) {
            var self = this;
            // Controller is /Api/CategoryApi/Unpin 
            if (IsCategory) {
                $.post("/Api/Category/Unpin/",
                    { categoryId: id },
                    function () {
                        Vue.delete(self.$refs.vuetable.tableData, index);
                    });
            } else {
                // Controller is /Api/SetsApi/Unpin
                $.post("/Api/Sets/Unpin/",
                    { setId: id },
                    function () {
                        Vue.delete(self.$refs.vuetable.tableData, index);
                    });
            }
        },
        onLoading() {
        },
        onLoaded() {
            $('.show-tooltip').tooltip();
        },
        switchOnlySelfCreatedChanged: function () {
            this.moreParams.isAuthor = $("#switchShowOnlySelfCreated").is(":checked");

            $.ajax({
                url: '/Knowledge/CountedWUWItoCategoryAndSet?isAuthor=' + this.moreParams.isAuthor,
                method: 'POST',
                async: true,
                datatype: "jsonp",
                success: (Data)=> {
                    this.moreParams.heading = Data;
                },
                error: function(error) {
                    console.log(error);
                }
            });
            this.$refs.vuetable.refresh();
        },
        GetImageSourceUrl(url) {
            if (url === null)
                return "/Images/no-category-picture-350.png";
            return url.SourceUrl;
        }
    }, mounted: function () {
         $.ajax({
             url: '/Knowledge/CountedWUWItoCategoryAndSet?isAuthor=' + false,
             method: 'POST',
             async: true,
             datatype: "jsonp",
             success: (Data) => {
                 this.moreParams.heading = Data;
             },
             error: function (error) {
                 console.log(error);
             }
         });
    }
});


