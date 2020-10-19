<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="CategoryFooter">

    <div class="footerToolbar">
        <div class="wishknowledge">
            <div class="Pin" data-category-id="<%= Model.Id %>">
                <%= Html.Partial("AddToWishknowledgeHeartbeat", new AddToWishknowledge(Model.IsInWishknowledge, false, true)) %>
            </div>

            <div id="PinLabel"><span id="CategoryFooterTotalPins"><%= Model.TotalPins%></span><span> Mal im Wunschwissen</span></div>
        </div>

        <% var buttonId = Guid.NewGuid(); %>
        <div class="Button dropdown footerDropdown">
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
    <div id="Footer-CategoryLearning" class="row">
        <div class="col-sm-12">
            
            <h1>Lernen</h1>
        
            <div style="display: flex; justify-content: space-between; margin-top: 20px;">
                <div class="QuestionCounter">
                    <p><% string aggregatedQuestionCount = "Keine";
                          if (Model.AggregatedQuestionCount > 0)
                              aggregatedQuestionCount = Model.AggregatedQuestionCount.ToString();%>
                        <%= aggregatedQuestionCount %> Frage<%= StringUtils.PluralSuffix(Model.AggregatedQuestionCount, "n") %> im Wissensnetz.</p>
                    <p><% string questionCount = "Keine";
                          if (Model.CategoryQuestionCount > 0)
                              questionCount = Model.CategoryQuestionCount.ToString();%>
                        <%= questionCount %> Frage<%= StringUtils.PluralSuffix(Model.CategoryQuestionCount, "n") %> direkt zugeordnet.</p>
                </div>
                <div class="StartLearningSession">
                    <a href="<%=Links.LearningSessionFooter(Model.Id, Model.Category.Name) %>" id="LearningFooterBtn" data-tab-id="LearningTab" class="btn btn-lg btn-primary footerBtn" >Lernsitzung starten</a> 
                </div>
            </div>
        
            <div class=""style="margin-top: 30px;">
                <% if(Model.CountAggregatedQuestions > 0) { %>
                    <p>Schwerste Frage:  <a href="<%= Links.AnswerQuestion(Model.HardestQuestion) %>" rel="nofollow"><%= Model.HardestQuestion.GetShortTitle(150) %></a></p>
                    <p>Leichteste Frage:  <a href="<%= Links.AnswerQuestion(Model.EasiestQuestion) %>" rel="nofollow"><%= Model.EasiestQuestion.GetShortTitle(150) %></a></p>
                <% } %>
            </div>
        </div>

    </div>
    <%Html.RenderPartial("~/Views/Categories/Detail/Partials/CategoryNetwork/CategoryNetwork.ascx", Model); %>
</div>

