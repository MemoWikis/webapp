﻿<%@ Language="C#" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeModel>"%>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%= Styles.Render("~/bundles/KnowledgeTopics") %>


<link rel='stylesheet prefetch' href='https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css'>

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

