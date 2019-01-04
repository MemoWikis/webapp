<%@ Language="C#" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeModel>"%>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<!-- Table -->
<div id="app">
    
    <h2 id="header"></h2>

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
                <input type="hidden" class="hddCountQuestion" v-bind:value="props.rowData.CountQuestions"/>
                <div class="show-tooltip" data-placement="bottom" data-toggle="tooltip" v-bind:data-original-title="props.rowData.Title">
                    <div class="image">
                        <image class="imageTable" v-bind:src="GetQuestionImageSourceUrl(props.rowData.QuestionMetaData)"></image>
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
                    <div class="author-image show-tooltip" data-toggle="tooltip" data-placement="bottom" v-bind:data-original-title="props.rowData.AuthorName">
                        <image v-bind:src="props.rowData.AuthorImageUrl.Url" class="image-author"></image>
                    </div>
                    <div  class="show-tooltip author-name" data-toggle="tooltip" data-placement="bottom" v-bind:data-original-title="props.rowData.AuthorName">{{props.rowData.AuthorName}}</div>
            </template>
          
            <template slot="category" scope="props">
                <div class="round">
                    <image class="round" v-bind:src="GetCategoryImageSourceUrl(props.rowData.CategoryImageData)" ></image>
                </div>
                <div class="link-to-category">
                <a class="show-tooltip" data-toggle="tooltip" v-bind:data-original-title="props.rowData.TooltipLinkToCategory" v-bind:href="props.rowData.LinkToCategory" data-placement="bottom" target="_blank" >{{props.rowData.Category}}</a> 
                </div>
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

