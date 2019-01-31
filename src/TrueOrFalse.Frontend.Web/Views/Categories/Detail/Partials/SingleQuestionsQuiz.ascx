<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SingleQuestionsQuizModel>" %>

<content-module inline-template >
    <li class="module" markdown="<%: Model.Markdown %>" v-if="!isDeleted">
        <div class="ContentModule" @mouseenter="updateHoverState(true)" @mouseleave="updateHoverState(false)">
            <div class="ModuleBorder" :class="{ active : hoverState }">
                
                <div class="singleQuestionsQuiz">
                    <h3><%: Model.Title %></h3>
                    <p><%: Model.Text %></p>

                    <% foreach (var question in Model.Questions)
                       { %>
                        <script src="https://memucho.de/views/widgets/w.js" data-t="question" data-id="<%= question.Id %>" data-width="100%" data-maxwidth="100%" data-logoon="false" data-hideKnowledgeBtn="true"></script>
                        <div class="SpacerDiv"></div>
                    <% } %>
                </div>

            </div>    
        </div>     
    </li>        
</content-module> 
