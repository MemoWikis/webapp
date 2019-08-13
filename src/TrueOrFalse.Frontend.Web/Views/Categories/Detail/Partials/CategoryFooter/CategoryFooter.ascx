<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

    <div class="FooterToolbar" style="display: flex;justify-content: space-between">
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

    <div class="FooterLearningPart">
        
        <h4>Lernen</h4>
        
        <% if(Model.CountReferences > 0 || Model.TopQuestionsInSubCats.Count > 0) { %>
        <div id="Content" class="Box">
             <% if(Model.CountReferences > 0) { %>
                <h5 class="ContentSubheading Question">
                    Fragen mit diesem Medium als Quellenangabe (<%=Model.CountReferences %>)
                    <a href="#relatedCountReferences" data-toggle="collapse" class="greyed noTextdecoration" style="font-weight: normal;"><i class="fa fa-caret-down">&nbsp;</i> Inhalte ein-/ausblenden</a>
                </h5>
                <div id="relatedCountReferences" class="collapse">
                    <div class="LabelList">
                        <% var index = 0; foreach(var question in Model.TopQuestionsWithReferences){ index++;%>
                            <div class="LabelItem LabelItem-Question">
                                <a href="<%= Links.AnswerQuestion(question, paramElementOnPage: index, categoryFilter:Model.Name) %>" rel="nofollow"><%= question.GetShortTitle(150) %></a>
                            </div>
                        <% } %>
                    </div>
                </div>
            <% } %>
                
            <% if(Model.TopQuestionsInSubCats.Count > 0){ %>
                <h5 class="ContentSubheading Question">
                    Fragen in untergeordneten Themen
                    <a href="#relatedTopQuestionsList" data-toggle="collapse" class="greyed noTextdecoration" style="font-weight: normal;"><i class="fa fa-caret-down">&nbsp;</i> Inhalte ein-/ausblenden</a>
                </h5>
                <div id="relatedTopQuestionsList" class="collapse">
                    <div class="LabelList">
                        <% var index = 0; foreach(var question in Model.TopQuestionsInSubCats){ index++;%>
                            <div class="LabelItem LabelItem-Question">
                                <a href="<%= Links.AnswerQuestion(question) %>"><%= question.GetShortTitle(150) %></a>
                            </div>
                        <% } %>
                    </div>
                </div>
            <% } %>
        </div>
    <% } %>


    </div>


    <div class="FooterAnalyticsPart"></div>

