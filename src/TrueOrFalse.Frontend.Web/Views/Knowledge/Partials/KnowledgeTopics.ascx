<%@ Language="C#" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeModel>"%>
<%@ Import Namespace="System.Web.Optimization" %>

<div id="app">
    <h2 id="h2TpopicAndLearnset">{{moreParams.heading}}</h2>

    <div class="col-xs-4 switch" style="text-align: left; font-size: 18px;  width: 27%">Zeige nur von mir erstellte Inhalte</div>
    <div class="col-xs-1 switch">

        <div class="onoffswitch">
            <input type="checkbox" name="onoffswitch" class="onoffswitch-checkbox" id="switchShowOnlySelfCreated" @click="switchOnlySelfCreatedChanged()">
            <label class="onoffswitch-label" for="switchShowOnlySelfCreated">
                <span class="onoffswitch-inner"></span>
                <span class="onoffswitch-switch"></span>
            </label>
        </div>
    </div>
    <div id="table-wrapper" class="ui">
      <vuetable ref="vuetable"
        api-url="/Knowledge/GetCatsAndSetsWish"
        :fields="fields"
        :sort-order="sortOrder"
        :css="css.table"
        pagination-path=""
        :per-page="50"
        :append-params="moreParams"
        @vuetable:pagination-data="onPaginationData"
        @vuetable:loading="onLoading"
        @vuetable:loaded="onLoaded()">

          <!-- Topic ImageAndTitle-->
        <template slot="imageAndTitle" scope="props">
            <div class="image">
                <image class="imageTable"v-bind:src="GetImageSourceUrl(props.rowData.ImageFrontendData.ImageMetaData)"></image>
            </div>
            <div>
                <a v-bind:href="props.rowData.LinkToSetOrCategory">{{props.rowData.Title}}</a>
            </div>
        </template>

        <!-- Topic Count-->      
        <template slot="topicCount" scope="props">
            <div class="topic-count">
                <div v-if="props.rowData.IsCategory"><span>{{props.rowData.LearnSetsCount}} Lernsets mit {{props.rowData.QuestionsCount}} Fragen</span></div>
                <div v-if="!props.rowData.IsCategory"><span>{{props.rowData.QuestionsCount}} Fragen</span></div>
            </div>
        </template>

        <!-- Buttons-->
        <template slot="actions" scope="props">
        <div class="Button">
            <a v-bind:href="props.rowData.LinkStartLearningSession" class="btn btn-link" data-allowed="logged-in" data-allowed-type="learning-session" rel="nofollow">
                <i class="fa fa-lg fa-line-chart" style="font-size: 16px"></i>&nbsp;lernen
            </a>
        </div>
        </template>  

        <!-- Dropdownmenu -->
        <template slot="dropDown" scope="props">
        <div class="Button dropdown" style="float: right">
            <% var buttonId = Guid.NewGuid(); %>
            <a href="#" id="<%=buttonId %>" class="dropdown-toggle fa-rotate-90  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" >
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

<%= Styles.Render("~/bundles/KnowledgeTopics") %>
<%= Scripts.Render("~/bundles/js/Vue") %>
<%= Scripts.Render("/Views/Knowledge/Js/KnowledgeTopics.js") %>
