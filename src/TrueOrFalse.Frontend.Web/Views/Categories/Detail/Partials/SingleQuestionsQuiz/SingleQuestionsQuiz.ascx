<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SingleQuestionsQuizModel>" %>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperStart.ascx") %>
                
    <div class="singleQuestionsQuiz">
        <h3><%: Model.Title %></h3>
        <p><%: Model.Text %></p>
    
        <div v-for="id in questions">
            <div class="contentModuleSettings" v-if="canBeEdited">
                <div class="questionCards fullwidth grid editView">
                    Frage: {{id}}
                </div>
            </div>
            <div v-show="!canBeEdited">
                <content-module-widget 
                    :widget-id="widgetId" 
                    widget-type="quiz" 
                    src="http://memucho.local/views/widgets/w.js" 
                    data-t="question" 
                    :data-id="id" 
                    data-width="100%"
                    data-maxwidth="100%"
                    data-logoon="false" 
                    data-hideKnowledgeBtn="true">
                </content-module-widget>     
            </div>
            <div class="SpacerDiv"></div>
        </div>
    </div>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperEnd.ascx") %>