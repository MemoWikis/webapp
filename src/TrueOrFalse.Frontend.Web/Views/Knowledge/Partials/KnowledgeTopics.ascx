<%@ Language="C#" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeModel>"%>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
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
 td {
     height: 30px;
 }
 .imageTable {
     border-radius: 100%;  
     width: 30px;
     height: 30px;
 }

</style>



<body>

    <div id="app">
     <div id="table-wrapper" class="ui container">
       <h2><strong>&lt;Vuetable-2&gt;</strong> with Bootstrap 3</h2>
      <vuetable ref="vuetable"
        api-url="/Knowledge/GetCatsAndSetsWish"
        :fields="fields"
        :sort-order="sortOrder"
        :css="css.table"
        pagination-path=""
        :per-page="50"
        @vuetable:pagination-data="onPaginationData"
        @vuetable:loading="onLoading"
        @vuetable:loaded="onLoaded">
          
          <template slot="wishKnowledge" scope="props">
              <div class="KnowledgeBarWrapper" v-html="props.rowData.KnowlegdeWishPartial" v-on:mouseover="mouseOver"></div>
          </template>

          <template slot="image" scope="props">
              <div class="bo boimg-1">
                <image class="imageTable"v-bind:src="props.rowData.ImageFrontendData.ImageMetaData.SourceUrl" ></image>
              </div>
          </template>

          <template slot="wishKnowledge" scope="props">
              <div class="KnowledgeBarWrapper" v-html="props.rowData.KnowlegdeWishPartial" v-on:mouseover="mouseOver"></div>
          </template>
          
          <template slot="topicCount" scope="props">
              <div v-if="props.rowData.IsCategory"><span>{{props.rowData.LearnSetsCount}} Lernsets mit {{props.rowData.QuestionsCount}} Fragen</span></div>
              <div v-if="!props.rowData.IsCategory"><span>{{props.rowData.QuestionsCount}} Fragen</span></div>
          </template>

          <!-- Buttons-->
        <template slot="actions" scope="props" >
            <div class="Button " style="float: left">
                <a v-bind:href="props.rowData.LinkStartLearningSession" class="btn btn-link" data-allowed="logged-in" data-allowed-type="learning-session" rel="nofollow">
                    <i class="fa fa-lg fa-line-chart">&nbsp;</i>lernen
                </a>
            </div> 
            <!-- Dropdownmenu -->
            <div class="Button dropdown" >
                <% var buttonId = Guid.NewGuid(); %>
                <a href="#" id="<%=buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" style="transform: rotate(90deg)">
                    <i class="fa fa-ellipsis-v"></i>
                </a>
                <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="<%=buttonId %>"  >
                <% if (Model.IsLoggedIn)
                   { %>
                    <li><a v-bind:href="props.rowData.DateToLearningTopicLink" rel="nofollow" data-allowed="logged-in" data-allowed-type="date-create"><i class="fa fa-calendar"></i>&nbsp;Prüfungstermin festlegen</a></li>
                    <li><a v-bind:href="props.rowData.StartGameLink" rel="nofollow" data-allowed="logged-in" data-allowed-type="game"><i class="fa fa-gamepad"></i>&nbsp;Spiel starten</a></li>

                    <li style="margin-top: 2rem;"><a v-bind:href="props.rowData.EditCategoryOrSetLink" rel="nofollow" data-allowed="logged-in"><i class="fa fa-calendar"></i>&nbsp;Lernset/Category bearbeiten</a></li>
                    <li><a v-bind:href="props.rowData.CreateQuestionLink" data-allowed="logged-in"><i class="fa fa-plus-circle"></i>&nbsp;Frage erstellen und hinzufügen</a></li>
                  
                    <li style="margin-top: 2rem;"><a target="_blank" v-bind:href="props.rowData.ShareFacebookLink"><i class="fa fa-pencil"></i>&nbsp; Lernset auf Facebook teilen </a></li>     

                    <li @click="deleteRow(props.rowData.Id, props.rowData.IsCategory, props.rowIndex)"><a href="#"><i class="fa fa-pencil"></i>&nbsp; Thema aus Wunschwissen entfernen </a></li> 
                <% }  %>
            </ul>
        </div>
          </template>
        </vuetable>
        <vuetable-pagination ref="pagination"
          :css="css.pagination"
          @vuetable-pagination:change-page="onChangePage">
        </vuetable-pagination>
    </div>
</div>
</body>

<%= Scripts.Render("~/bundles/js/KnowledgeTopics") %> 

