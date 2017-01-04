<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>



<h4>Inhalte</h4>
<div id="Content" class="Box">
    <% if(Model.CountSets > 0){ %>    
        <h5 class="ContentSubheading Set">
            <%= Model.CountSets %> Frage<%= StringUtils.PluralSuffix(Model.CountSets,"sätze","satz") %> in dieser Kategorie
        </h5>
        <div class="LabelList">
        <% foreach(var set in Model.Sets){ %>
            <div class="LabelItem LabelItem-Set">
                <div class="EllipsWrapper">
                    <a href="<%= Links.SetDetail(Url, set) %>"><%= set.Name %></a>
                    <span style="font-size: 90%;">
                        (<%= set.QuestionsInSet.Count %> Frage<%= StringUtils.PluralSuffix(set.QuestionsInSet.Count, "n") %>,    
                        <a href="<%= Links.TestSessionStartForSet(set.Name, set.Id) %>"><i class="fa fa-play-circle">&nbsp;</i>Jetzt Wissen testen</a>)
                    </span>
                </div>
            </div>
        <% } %>
        </div>
        
    <% } %>
        
   <%-- <% if (Model.SingleQuestions.Count > 0) { %>
    <h5 class="ContentSubheading Question">
        <%= Model.SingleQuestions.Count %> Einzelfrage<%= StringUtils.PluralSuffix(Model.SingleQuestions.Count, "n") %> in dieser Kategorie
    </h5>
    <div class="LabelList">
        <% var index2 = 0;
           foreach (var question in Model.SingleQuestions)
           {
               index2++; %>
            <div class="LabelItem LabelItem-Question">
                <div class="EllipsWrapper">
                    <a href="<%= Links.AnswerQuestion(Url, question, paramElementOnPage: index2, categoryFilter: Model.Name) %>">
                        <%= question.Text %>
                    </a>
                </div>
            </div>
        <% } %>
    </div>
    <% } %>--%>
    

    <h5 class="ContentSubheading Question">
        <%= Model.CountQuestions %> Frage<%= StringUtils.PluralSuffix(Model.CountQuestions,"n") %> in dieser Kategorie
    </h5>

    <% if (Model.CountQuestions > 0){ %>
        <div class="LabelList">
        <% var index = 0;
            foreach (var question in Model.TopQuestions)
            {
                index++; %>
            <div class="LabelItem LabelItem-Question">
                <div class="EllipsWrapper">
                    <a href="<%= Links.AnswerQuestion(Url, question, paramElementOnPage: index, categoryFilter: Model.Name) %>">
                        <%= question.Text %>
                    </a>
                </div>
            </div>
        <% } %>
        </div>
        <div style="margin-bottom: 15px;">
            <a href="<%: Links.QuestionWithCategoryFilter(Url, Model.Category) %>" class="" rel="nofollow" style="font-style: italic; margin-left: 10px;">
                <i class="fa fa-forward" style="color: #afd534;">&nbsp;</i>Alle <%: Model.CountQuestions %> Frage<%= StringUtils.PluralSuffix(Model.CountQuestions, "n") %> dieser Kategorie zeigen
            </a>
        </div>
    <% }
        else{ %> 
        Bisher gibt es keine Fragen in dieser Kategorie.
            
        <% } %>
            
    <% if(Model.CountWishQuestions > 0){ %>
        <h5 class="ContentSubheading Question">In deinem <a href="<%= Links.QuestionsWish() %>">Wunschwissen</a> (<%=Model.CountWishQuestions %>)</h5>
        <div class="LabelList">
        <% var index = 0; foreach(var question in Model.TopWishQuestions){index++; %>
            <div class="LabelItem LabelItem-Question">
                <div class="EllipsWrapper">
                     <a href="<%= Links.AnswerQuestion(Url, question, paramElementOnPage: index, categoryFilter:Model.Name) %>" rel="nofollow"><%= question.GetShortTitle(150) %></a>
                </div>
            </div>
        <% } %>
        </div>
    <% } %>
</div>
