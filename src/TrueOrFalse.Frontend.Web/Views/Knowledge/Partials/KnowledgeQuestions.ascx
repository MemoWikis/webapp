﻿<%@ Language="C#" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeModel>"%>
<%@ Import Namespace="System.Web.Optimization" %>

<!-- Spinner-->
<div id="circle">
    <div class="circle-inner">
        <div class="circle-inner">
            <div class="circle-inner">
                <div class="circle-inner">
                    
                </div>
            </div>
        </div>
    </div>
</div>

<!-- Table -->
<div id="app">

    <!--Switch-->
    <div class=" switch" style="text-align: left; font-size: 18px; width: 27%; float: left; ">Zeige nur von mir erstellte Inhalte</div>
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
            api-url="/Knowledge/GetQuestionsWish"
            :fields="fields"
            :sort-order="sortOrder"
            :css="css.table"
            pagination-path=""
            :per-page="30"
            :append-params="moreParams"
            @vuetable:pagination-data="onPaginationData"
            @vuetable:initialized ="loading()"
            @vuetable:loaded ="onLoaded()">
        
            <template slot="questionTitle" scope="props">
                <div data-toggle="tooltip" v-bind:title="props.rowData.Title">
                    <div class="image">
                        <image class="imageTable" v-bind:src="GetImageSourceUrl(props.rowData.ImageFrontendData.ImageMetaData)" ></image>
                    </div>
                    <div class="title-table"><a v-bind:href="props.rowData.LinkToQuestion">{{props.rowData.Title}}</a></div>
                </div>
            </template>
              
            <template slot="knowWas" scope="props">
                <div class="know-was show-tooltip" data-placement="bottom" v-bind:data-original-title ="props.rowData.LearningStatusTooltip" data-toggle="tooltip">
                    <div v-bind:class="props.rowData.LearningStatus"></div>
                </div>
            </template>
                  
            <template slot="authorImage" scope="props">
                <div data-toggle="tooltip" v-bind:title="props.rowData.AuthorName">
                    <div class="author-image">
                        <image v-bind:src="props.rowData.AuthorImageUrl.Url" class="image-author"></image>
                    </div>
                    <div class="author-name">{{props.rowData.AuthorName}}</div>
                </div>
            </template>
          
            <template slot="category" scope="props">
                <image v-bind:src="GetImageSourceUrl(props.rowData.ImageFrontendData.ImageMetaData)"   class="round"></image><a data-toggle="tooltip" title="Thema in neuem Tab öffnen" v-bind:href="props.rowData.LinkToCategory" target="_blank" class="link-to-category">{{props.rowData.Category}}</a> 
            </template>

            <!-- Buttons-->
            <template slot="actions" scope="props">
                <div class="Button">
            <a v-bind:href="props.rowData.LinkStartLearningSession" class="btn btn-link" data-allowed="logged-in" data-allowed-type="learning-session" rel="nofollow">
                <i class="fa fa-lg fa-line-chart">&nbsp;lernen</i>
            </a>
                </div>
            </template>  
        </vuetable>
        <vuetable-pagination ref="pagination"
           :css="css.pagination"
           @vuetable-pagination:change-page="onChangePage">
        </vuetable-pagination>
    </div>
</div>


<%= Scripts.Render("~/bundles/js/KnowledgeQuestions") %> 
<%= Styles.Render("~/bundles/KnowledgeQuestions") %>

