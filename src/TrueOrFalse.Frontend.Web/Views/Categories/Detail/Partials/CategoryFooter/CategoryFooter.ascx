<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="CategoryFooter" style="margin-top:100px">
    <div class="FooterToolbar" style="display: flex; justify-content: space-between;border-bottom: solid 1px #d6d6d6;">
        <div class="Wishknowledge" style="display:flex">
            <div style="display: inline-block; font-size: 16px; font-weight: normal;" class="Pin" data-category-id="<%= Model.Id %>">
                <%= Html.Partial("AddToWishknowledgeButton", new AddToWishknowledge(Model.IsInWishknowledge, false, true)) %>
            </div>
            <div>
                <span>n</span> Mal im Wunschwissen
            </div>
        </div>
        

        <% var buttonId = Guid.NewGuid(); %>

        <div class="Button dropdown">
            <a href="#" id="<%= buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                <i class="fa fa-ellipsis-v"></i>
            </a>
            <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="<%= buttonId %>">
                <li><a href="<%= Links.CategoryHistory(Model.Id) %>"><i class="fa fa-code-fork"></i>&nbsp;Bearbeitungshistorie</a></li>
                <li><a href="<%= Links.CategoryEdit(Url, Model.Name, Model.Id) %>" data-allowed="logged-in" ><i class="fa fa-pencil"></i>&nbsp;bearbeiten</a></li>
                <li><a href="<%= Links.CreateQuestion(categoryId: Model.Id) %>" data-allowed="logged-in" ><i class="fa fa-plus-circle"></i>&nbsp;Frage hinzufügen</a></li>
                <li><a href="<%= Links.CategoryCreate(Model.Id) %>" data-allowed="logged-in" ><i class="fa fa-plus-circle"></i>&nbsp;Unterthema hinzufügen</a></li>
            </ul>
        </div>
    </div>

    <div class="LearningSection" style="border-bottom: solid 1px #d6d6d6;padding:30px 0">
        
        <h1>Lernen</h1>
        
        <div style="display: flex; justify-content: space-between; margin-top: 20px;">
            <div class="QuestionCounter">
                <p><%= Model.AggregatedQuestionCount %> Frage<%= StringUtils.PluralSuffix(Model.AggregatedQuestionCount, "n") %> im Wissensnetz. <span class="btn-link">Lernen / Anzeigen</span></p>
                <p><%= Model.CategoryQuestionCount %> Frage<%= StringUtils.PluralSuffix(Model.CategoryQuestionCount, "n") %> direkt zugeordnet. <span class="btn-link">Lernen / Anzeigen</span></p>
            </div>
            <div class="StartLearnignSession">
                <div class="btn btn-lg btn-primary">Lernsitzung starten</div>       
            </div>
        </div>
        
        <div style="margin-top: 30px;">
             <% if(Model.CountAggregatedQuestions > 0) { %>
                <p>Schwerste Frage:  <a href="<%= Links.AnswerQuestion(Model.HardestQuestion) %>" rel="nofollow"><%= Model.HardestQuestion.GetShortTitle(150) %></a></p>
                <p>Leichteste Frage:  <a href="<%= Links.AnswerQuestion(Model.EasiestQuestion) %>" rel="nofollow"><%= Model.EasiestQuestion.GetShortTitle(150) %></a></p>
            <% } %>
        </div>
    </div>

    <div class="AnalyticsSection" style="padding:30px 0">

        <div style="display: flex;">
            <div class="analyticsImg"></div>
            <div class="startAnalytics">
                <h1>Wissensnetz</h1>

            </div>

        </div>

    </div>

</div>


