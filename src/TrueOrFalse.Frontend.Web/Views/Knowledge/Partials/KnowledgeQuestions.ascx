<%@ Language="C#" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeModel>"%>
<%@ Import Namespace="System.Web.Optimization" %>



<body>
    <div id="app">
        <div id="table-wrapper" class="ui container">
              <vuetable ref="vuetable"
                api-url="/Knowledge/GetQuestionsWish"
                :fields="fields"
                :sort-order="sortOrder"
                :css="css.table"
                pagination-path=""
                :per-page="30"
                @vuetable:pagination-data="onPaginationData"
                @vuetable:loaded ="onLoaded()">
            
                <template slot="image" scope="props">
                    <div class="image" >
                        <image class="imageTable"v-bind:src="GetImageSourceUrl(props.rowData.ImageFrontendData.ImageMetaData)" ></image><span class="title-table">{{props.rowData.Title}}</span>
                    </div>
                </template>
                  
                <template slot="knowWas" scope="props">
                    <div class="know-was">
                    <div v-bind:class="props.rowData.LearningStatus" data-toggle="tooltip" v-bind:title="props.rowData.LearningStatusTooltip"><p></p></div>
                    </div>
                </template>
                      
                <template slot="authorImage" scope="props">
                    <div>
                        <image v-bind:src="props.rowData.AuthorImageUrl.Url" class="image-author"></image>
                        <span class="author-name">{{props.rowData.Author}}</span>
                    </div>
                    
                </template>
              
                <template slot="category" scope="props">
                    <image v-bind:src="GetImageSourceUrl(props.rowData.ImageFrontendData.ImageMetaData)"   class="round"></image><a data-toggle="tooltip" title="Thema in neuem Tab öffnen" v-bind:href="props.rowData.LinkToCategory" target="_blank" class="link-to-category">{{props.rowData.Category}}</a>
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
            </vuetable>
            <vuetable-pagination ref="pagination"
               :css="css.pagination"
               @vuetable-pagination:change-page="onChangePage">
            </vuetable-pagination>
        </div>
    </div>
</body>

<%= Scripts.Render("~/bundles/js/KnowledgeQuestions") %> 
<%= Styles.Render("~/bundles/KnowledgeQuestions") %>

