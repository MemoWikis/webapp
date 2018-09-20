
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
            //pagination: {
            //    infoClass: 'pull-left',
            //    wrapperClass: 'vuetable-pagination pull-right',
            //    activeClass: 'btn-primary',
            //    disabledClass: 'disabled',
            //    pageClass: 'btn btn-border',
            //    linkClass: 'btn btn-border',
            //    icons: {
            //        first: '',
            //        prev: '',
            //        next: '',
            //        last: ''
            //    }
           // }
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
        onLoaded(props) {
            console.log(props);
        },
        switchOnlySelfCreatedChanged: function () {
            this.moreParams.isAuthor = $("#switchShowOnlySelfCreated").is(":checked");
            var self = this;
            $.ajax({
                url: '/Knowledge/CountedWUWItoCategoryAndSet?isAuthor=' + this.moreParams.isAuthor,
                method: 'POST',
                async: false,
                datatype: "jsonp",
                success: function(Data) {
                    self.moreParams.heading = Data;
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
        var self = this;
         $.ajax({
             url: '/Knowledge/CountedWUWItoCategoryAndSet?isAuthor=' + false,
             method: 'POST',
             async: false,
             datatype: "jsonp",
             success: function (Data) {
             self.moreParams.heading = Data;
             },
             error: function (error) {
                 console.log(error);
             }
         });
     }
});

