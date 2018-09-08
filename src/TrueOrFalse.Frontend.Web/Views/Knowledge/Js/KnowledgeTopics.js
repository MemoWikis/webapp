 let v=new Vue({
    el: "#countCatAndSet",
    data: {
        datas: []
    },
    methods: {
   
     }, mounted: function () {
        var self = this;
        $.ajax({
            url: '/Knowledge/CountedWUWItoCategoryAndSet',
            method: 'POST',
            datatype: "jsonp",
            success: function (Data) {
                self.datas = Data;
                console.log(self.datas[0] );
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
});

Vue.use(Vuetable);

 new Vue({
     el: '#app',
     components: {
         'vuetable-pagination': Vuetable.VuetablePagination
     },
     data: {
         test: "true",
         moreParams: {
             'isAuthor' : false
        },
        fields: [
            '__slot:image',

            {
                name: 'Titel',
                title: 'Titel',
                sortField: 'name'
            },
            {
                name: "KnowlegdeWishPartial",
                title: "Wissensstand",
                dataClass: "KnowledgeBarWrapper",
                sortField: "knowledgeBar",
                html: true
            },
            '__slot:topicCount',
            '__slot:actions',
            '__slot:dropDown'
        ],
        sortOrder: [
            { field: 'name', direction: 'asc' },
            { field: 'knowledgeBar', direction: 'asc' }
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
        editRow(rowData) {

            $.post("/Api/Category/Pin/",
                { categoryId: 683 },
                function () {

                });
            $.post("/Api/Category/Pin/",
                { categoryId: 686 },
                function () {

                });
            $.post("/Api/Category/Pin/",
                { categoryId: 744 },
                function () {

                });
            $.post("/Api/Sets/Pin/",
                { setId: 279 },
                function () {

                });
            $.post("/Api/Sets/Pin/",
                { setId: 409 },
                function () {

                });
            $.post("/Api/Sets/Pin/",
                { setId: 414 },
                function () {

                });
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
            console.log('loading... show your spinner here');
        },
        onLoaded() {
            console.log('loaded! .. hide your spinner here');
        },
        moreParamsFunction: function () {
            this.moreParams.isAuthor = $("#myonoffswitch").is(":checked");
            this.$refs.vuetable.refresh();

            //var self = this;
            //setTimeout(function () {
                
            //}, 0)
            
        },
        GetImageSourceUrl(url) {
            if (url == null)
                return "/Images/no-category-picture-350.png";
            return url.SourceUrl;
        }
     }
});

