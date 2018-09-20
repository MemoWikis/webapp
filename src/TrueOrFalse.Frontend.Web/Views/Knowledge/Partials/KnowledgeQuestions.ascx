<%@ Language="C#" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeModel>"%>
<%@ Import Namespace="System.Web.Optimization" %>

<link rel='stylesheet prefetch' href='https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css'>
<%= Styles.Render("~/bundles/KnowledgeQuestions") %>
<body>
 <div class="container-fluid">

    <div id="app">

        <div id="table-wrapper" class="ui container">
          <vuetable ref="vuetable"
            api-url="/Knowledge/GetQuestionsWish"
            :fields="fields"
            :sort-order="sortOrder"
            :css="css.table"
            pagination-path=""
            :per-page="50"
            @vuetable:loading="onLoading"
            @vuetable:loaded="onLoaded()">
            
            <template slot="image" scope="props">
                <div class="image" >
                <image class="imageTable"v-bind:src="GetImageSourceUrl(props.rowData.ImageFrontendData.ImageMetaData)" ></image>
                </div>
            </template>
              
              <template slot="knowWas" scope="props">
                  <div v-bind:class="props.rowData.LearningStatus" id = 'box1'><p></p></div>
              </template>
                  
            <template slot="authorImage" scope="props">
               <div >
                    <image v-bind:src="props.rowData.AuthorImageUrl.Url" class="round"></image>
               </div>
                <div class="imageAuthor">{{props.rowData.Author}}</div>
                
            </template>
              
              <template slot="category" scope="props">
                  <image v-bind:src="GetImageSourceUrl(props.rowData.ImageFrontendData.ImageMetaData)" class="round">&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; {{props.rowData.Category}}</image>
                  <div></div>
              </template>

            <!-- Buttons-->
            <template slot="actions" scope="props">
            <div class="Button">
                <a v-bind:href="props.rowData.LinkStartLearningSession" class="btn btn-link" data-allowed="logged-in" data-allowed-type="learning-session" rel="nofollow">
                    <i class="fa fa-lg fa-line-chart">&nbsp;lernen</i>
                </a>
            </div>
            </template>  

            <!-- Dropdownmenu -->
            <template slot="dropDown" scope="props">
            <div class="Button dropdown" style="float: right">
                <% var buttonId = Guid.NewGuid(); %>
                <a href="#" id="<%=buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" >
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
                    <li @click="editRow()"><a href="#"><i class="fa fa-pencil"></i>&nbsp; Themen hinzufügen nur DeveloperHelperLink </a></li> 
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
 </div>
</body>

<%= Scripts.Render("~/bundles/js/KnowledgeQuestions") %> 

