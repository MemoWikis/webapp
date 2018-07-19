<%@ Language="C#" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeModel>"%>
<%@ Import Namespace="System.Web.Optimization" %>
<%= Styles.Render("~/bundles/KnowledgeTopics") %>


<%-- %><div id="app">
    <table class="table">
        <tr>
            <th>Titel</th>
        </tr>
       <tr  v-for="(message, key) in messages">
           <td><image v-bind:src="message.ImageFrontendData.ImageMetaData.SourceUrl" style="width: 30px;"></image></td>
           <td>{{message.CategoryTitel}}</td>
           <td><div class="KnowledgeBarWrapper" v-html="message.KnowlegdeWishPartial" v-on:mouseover="mouseOver"></div></td>
           <td><a data-toggle="modal" v-bind:data-dateid="message.CategoryId" href="#" v-on:click="unpinCategory(message.CategoryId,key)">
                   <i class="fa fa-trash-o show-tooltip" title="" data-placement="top" data-original-title="Löschen">

                   </i> 
               </a>
               <!-- Dropdownmenu -->
               <div class="Button dropdown" style="transform: rotate(90deg);">
                   <% var buttonId = Guid.NewGuid(); %>
                   <a href="#" id="<%=buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                       <i class="fa fa-ellipsis-v"></i>
                   </a>
                   <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="<%=buttonId %>" style="transform: rotate(270deg)">
                       <% if (true)
                          { %>
                           <li><a href="#" rel="nofollow" data-allowed="logged-in" data-allowed-type="date-create"><i class="fa fa-calendar"></i>&nbsp;Thema zum Termin lernen</a></li>         <%-- <%= Links.DateCreateForCategory(Model.Id) %>--%>
                        
                          <%-- %> <li><a href="#" rel="nofollow" data-allowed="logged-in" data-allowed-type="game"><i class="fa fa-gamepad"></i>&nbsp;Spiel starten</a></li>           <%-- <%= Links.GameCreateFromCategory(Model.Id) %>--%>
                    <%--   <% }
                          if(true){ %>
                           <li><a href="#"><i class="fa fa-pencil"></i>&nbsp;bearbeiten</a></li>     <%--<%= Links.CategoryEdit(Url, Model.Name, Model.Id) %>--%>
                   <%--     <% }
                      if (Model.IsInstallationAdmin){ %>
                           <li><a href="#"><i class="fa fa-plus-circle"></i>&nbsp;Frage hinzufügen</a></li>      <%--<%= Links.CreateQuestion(categoryId: Model.Id) %>--%>
                  <%--    <% } %>
                   </ul>
               </div>
           </td>
       </tr>
    </table>
</div>

<script>
    new Vue({
        el: '#app',
        data: {
            messages: []
        },methods: {
            mouseOver(){
                $('.show-tooltip').tooltip();   
            },
            unpinCategory(categoryId,key) {                
                console.log(key);
                $.post("/Api/Category/Unpin/", { categoryId: categoryId }, () => {
                    this.messages.splice(key, 1);
                });
            }
        },
        mounted: function() {
            var self = this;

            $.ajax({
                url: '/Knowledge/GetCatsAndSetsWish',
                method: 'POST',
                datatype: "jsonp",
                success: function(Data) {
                    self.messages = Data;
                    for (var i = 0; i < Data.length;i++){
                        console.log(Data);
                    }
                },
                error: function(error) {
                    console.log(error);
                }
            });
        }
    });
    
</script>
--%>





<link rel='stylesheet prefetch' href='https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css'>

<style class="cp-pen-styles">#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
}

.orange.glyphicon {
  color: orange;
}

th.sortable {
  color: #ec971f;
}
</style>

<body>
    <div id="app">
     <td id="table-wrapper" class="ui container">
       <h2><strong>&lt;Vuetable-2&gt;</strong> with Bootstrap 3</h2>
      <vuetable ref="vuetable"
        api-url="/Knowledge/GetCatsAndSetsWish"
        :fields="fields"
      <%--  :sort-order="sortOrder"--%>
        :css="css.table"
       <%-- pagination-path=""--%>
       <%-- :per-page="3"--%>
        @vuetable:pagination-data="onPaginationData"
        @vuetable:loading="onLoading"
        @vuetable:loaded="onLoaded"
      >
          <template slot="wishKnowledge" scope="props">
              <div class="KnowledgeBarWrapper" v-html="props.rowData.KnowlegdeWishPartial" v-on:mouseover="mouseOver"></div>
          </template>

        <template slot="actions" scope="props">
          <div class="table-button-container">
              <button class="btn btn-warning btn-sm" @click="editRow(props.rowData)">
                <span class="glyphicon glyphicon-pencil"></span> Edit</button>&nbsp;&nbsp;
              <button class="btn btn-danger btn-sm" @click="deleteRow(0)">
                <span class="glyphicon glyphicon-trash"></span> Delete</button>&nbsp;&nbsp;
          </div>
          </template>
        </vuetable>
        <vuetable-pagination ref="pagination"
          :css="css.pagination"
          @vuetable-pagination:change-page="onChangePage"
        ></vuetable-pagination>
      </div>
   

<script>
    Vue.use(Vuetable);

    new Vue({
        el: '#app',
        components: {
            'vuetable-pagination': Vuetable.VuetablePagination
            
        },
        data: {
            fields: [
                {
                    name: 'Titel',
                    title: 'Titel',
                    sortField: 'name'
                },
                //{
                //    name: ,
                //    title: 'bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb'
                //    //sortField: 'email'
                //},
                //'birthdate', 'nickname',
                '__slot:wishKnowledge',
                //{
                //    name: '',
                //    title: ''
                //    //sortField: 'gender'
                //}
               '__slot:actions'
            ],
            //sortOrder: [
            //    { field: 'name', direction: 'asc' }
            //],
            css: {
                table: {
                    tableClass: 'table table-striped table-bordered table-hovered',
                    loadingClass: 'loading',
                    ascendingIcon: 'glyphicon glyphicon-chevron-up',
                    descendingIcon: 'glyphicon glyphicon-chevron-down',
                    handleIcon: 'glyphicon glyphicon-menu-hamburger',
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
        computed: {
            /*httpOptions(){
              return {headers: {'Authorization': "my-token"}} //table props -> :http-options="httpOptions"
            },*/
        
        },
        methods: {
            mouseOver(){
                $('.show-tooltip').tooltip();   
            },
            onPaginationData (paginationData) {
                this.$refs.pagination.setPaginationData(paginationData);
            },
            onChangePage (page) {
                this.$refs.vuetable.changePage(page);
            },
            editRow(rowData) {
                alert("You clicked edit on" + JSON.stringify(rowData));
            },
            deleteRow: function(rowId) {
               // Vue.delete(this.$parent, rowId);
                console.log(this.$parent);


            },
            onLoading() {
                console.log('loading... show your spinner here');
            },
            onLoaded() {
                console.log('loaded! .. hide your spinner here');
            }
        }
    });
//# sourceURL=pen.js
</script>
</body>


