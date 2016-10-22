<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<CategoryModel>"%>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="NHibernate.Properties" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Categories/Detail/Category.css" rel="stylesheet" />
    <% Title = "Kategorie: " + Model.Name; %>
    <link rel="canonical" href="<%= Settings.CanonicalHost %><%= Links.CategoryDetail(Model.Name, Model.Id) %>" />
    <meta name="description" content="<%= Model.Name.Replace("\"", "'").Replace("„", "'").Replace("“", "'").Truncate(25, true) %> (<%=Model.CountQuestions %> Fragen) <%= String.IsNullOrEmpty(Model.Description) ? "" : ": "+Model.Description.Replace("\"", "'").Replace("„", "'").Replace("“", "'").Truncate(89, true) %> - Lerne mit memucho!"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row" >
        <div class="xxs-stack col-xs-12">
            <div class="row" style="margin-bottom: 20px;">
                <div class="col-xs-3 col-xs-push-9 xxs-stack">
                    <div class="navLinks">
                        <a href="<%= Url.Action(Links.CategoriesAction, Links.CategoriesController) %>" style="font-size: 12px;"><i class="fa fa-list"></i>&nbsp;zur Übersicht</a>
                        <% if(Model.IsOwnerOrAdmin){ %>
                            <a href="<%= Links.CategoryEdit(Url, Model.Name, Model.Id) %>" style="font-size: 12px;"><i class="fa fa-pencil"></i>&nbsp;bearbeiten</a> 
                        <% } %>
                        <a href="<%= Links.CreateQuestion(Url, Model.Id) %>" style="font-size: 12px;"><i class="fa fa-plus-circle"></i>&nbsp;Frage hinzufügen</a>
                    </div>
                </div>
                <div class="PageHeader col-xs-9 col-xs-pull-3 xxs-stack category">
                    <% if (Model.Type == "Standard") { %>
                        <h1 style="margin-top: 0; margin-bottom: 10px;"><span class="ColoredUnderline Category"><%= Model.Name %></span></h1>
                    <% } else { %>
                        <h1 style="margin-top: 0; margin-bottom: 10px;">
                            <span style="display: inline-block;">
                                <span class="ColoredUnderline Category" style="display: inline; margin-right: 5px;"><%= Model.Name %></span>
                                <span class="CategoryType">(<%= Model.Type %>)</span>
                            </span>
                        </h1>
                    <% }
                    
                    if (Model.Type != "Standard") {
                        Html.RenderPartial("Reference", Model.Category);
                    }
                    
                    if (!String.IsNullOrEmpty(Model.Description)) {%>
                        <div class="Description"><span><%= Model.Description %></span></div>
                    <% } %>
                    
                    <% if (!String.IsNullOrEmpty(Model.WikiUrl)){ %>
                        <div class="WikiLink" style="margin-top: 10px;">
                            <a href="<%= Model.WikiUrl %>" target="_blank" class="show-tooltip" title="<div style='white-space: normal; word-wrap: break-word; text-align:left; '>Link&nbsp;auf&nbsp;Wikipedia:&nbsp;<%= Model.WikiUrl %></div>" data-placement="left" data-html="true">
                                <img src="/Images/wiki-24.png" style="margin-top: -1px;" /><%= Model.WikiUrl %>
                            </a>
                        </div>
                    <% } %>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-3 col-xs-push-9 xxs-stack">
            <div class="CategoryImage">
                <div class="ImageContainer">
                    <%= Model.ImageFrontendData.RenderHtmlImageBasis(350, false, ImageType.Category, "ImageContainer") %>
                </div>
            </div>
            
            <div>
                <% if(Model.AnswersTotal > 0) { %>
                    <div style="margin-top: 6px;">
                        In dieser Kategorie wurden
                        <%= Model.AnswersTotal + "x Frage" + StringUtils.Plural(Model.AnswersTotal, "n") %> beantwortet, 
                        davon <%= Model.CorrectnesProbability %>% richtig.
                    </div>
                <% } %>                
            </div>
        </div>
        <div class="col-xs-9 col-xs-pull-3 xxs-stack">
            <div class="CategoryRelations well">
                <% if (Model.CategoriesParent.Count > 0)
                   { %>
                    <h4 style="margin-top: 0;">Elternkategorien</h4>
                    <div>
                        <% foreach (var category in Model.CategoriesParent)
                           { %>
                            <a href="<%= Links.CategoryDetail(category) %>"><span class="label label-category"><%= category.Name %></span></a>
                        <% } %>
                    </div>
                <% }
                   else { %>
                    <h4 style="margin-top: 0;">keine Elternkategorien</h4>
                <%  } %>

                <div class="RelationArrow"><i class="fa fa-arrow-down"></i></div>
                <div class="MainCategory"><span class="label label-category"><%= Model.Name %></span></div>
                <div class="RelationArrow"><i class="fa fa-arrow-down"></i></div>

                <% if(Model.CategoriesChildren.Count > 0){ %>
                    <h4 style="margin-top: 0;">
                        Kindkategorien
                    </h4>
                    <div>
                        <% foreach(var category in Model.CategoriesChildren){ %>
                            <a href="<%= Links.CategoryDetail(category) %>"><span class="label label-category"><%= category.Name %></span></a>
                        <% } %>
                        <i class="fa fa-plus-circle show-tooltip cat-color add-new" 
                           style="font-size: 14px; color: #99ccff; cursor: pointer"
                           onclick="window.location = '/Kategorien/Erstelle?parent=<%= Model.Category.Id%>'; return false; " 
                           data-original-title="Neue Kindkategorie erstellen"></i>
                    </div>
                <% } else { %>
                    <h4 style="margin-top: 0; margin-bottom: 2px;">keine Kindkategorien
                        <i class="fa fa-plus-circle show-tooltip cat-color add-new" 
                            style="font-size: 14px; color: #99ccff; cursor: pointer"
                            onclick="window.location = '/Kategorien/Erstelle?parent=<%= Model.Category.Id%>'; return false; " 
                            data-original-title="Neue Kindkategorie erstellen"></i>
                    </h4>
                <%  } %>
            </div>
            <h4 style="margin-top: 0;">Fragen in dieser Kategorie (<%= Model.CountQuestions %>)</h4>

            <% if (Model.CountQuestions > 0)
               { %>
                <% var index = 0;
                   foreach (var question in Model.TopQuestions)
                   {
                       index++; %>
                    <div style="white-space: nowrap; overflow: hidden; -moz-text-overflow:ellipsis; text-overflow:ellipsis;">
                        - <a href="<%= Links.AnswerQuestion(Url, question, paramElementOnPage: index, categoryFilter: Model.Name) %>"><%= question.GetShortTitle(150) %></a>
                    </div>
                <% } %>
                <div style="margin: 10px 0;">
                    <a class="btn btn-primary show-tooltip" href="<%= Links.TestSessionStartForCategory(Model.Id) %>" title="Teste dein Wissen in dieser Kategorie" rel="nofollow">
                        <i class="fa fa-play-circle">&nbsp;</i>Jetzt testen
                    </a>
                    <a href="<%: Links.QuestionWithCategoryFilter(Url, Model.Category) %>" class="" rel="nofollow" style="font-style: italic; margin-left: 10px;">
                        <i class="fa fa-forward" style="color: #afd534;">&nbsp;</i>Alle <%: Model.CountQuestions %> Frage<%= StringUtils.Plural(Model.CountQuestions, "n") %> dieser Kategorie zeigen
                    </a>
                </div>
            <% }
               else{ %> 
                Bisher gibt es keine Fragen in dieser Kategorie.
            
                <% } %>
            
            <% if(Model.CountReferences > 0) { %>
                <h4 style="margin-top: 0;">Fragen mit dieser Kategorie als Referenz (<%=Model.CountReferences %>)</h4>
            
                <% var index = 0; foreach(var question in Model.TopQuestionsWithReferences){ index++;%>
                    <div style="white-space: nowrap; overflow: hidden; -moz-text-overflow:ellipsis; text-overflow:ellipsis;">
                        - <a href="<%= Links.AnswerQuestion(Url, question, paramElementOnPage: index, categoryFilter:Model.Name) %>" rel="nofollow"><%= question.GetShortTitle(150) %></a>
                    </div>
                <% } %>

            <% } %>
            
            <% if(Model.TopQuestionsInSubCats.Count > 0){ %>
                <div style="margin-bottom: 18px">
                    <h4 style="margin-top: 0;">Fragen in Kindkategorien</h4>
                    <% var index = 0; foreach(var question in Model.TopQuestionsInSubCats){ index++;%>
                        <div style="white-space: nowrap; overflow: hidden; -moz-text-overflow:ellipsis; text-overflow:ellipsis;">
                            - <a href="<%= Links.AnswerQuestion(question) %>"><%= question.GetShortTitle(150) %></a>
                        </div>
                    <% } %>
                </div>
            <% } %>
            
            <% if(Model.CountWishQuestions > 0){ %>
                <h4 style="margin-top: 0;">In deinem <a href="<%= Links.QuestionsWish() %>">Wunschwissen</a> (<%=Model.CountWishQuestions %>)</h4>
                <% var index = 0; foreach(var question in Model.TopWishQuestions){index++; %>
                    <div style="white-space: nowrap; overflow: hidden; -moz-text-overflow:ellipsis; text-overflow:ellipsis;">
                        - <a href="<%= Links.AnswerQuestion(Url, question, paramElementOnPage: index, categoryFilter:Model.Name) %>" rel="nofollow"><%= question.GetShortTitle(150) %></a>
                    </div>
                <% } %>
            
            <% } %>
            
            <% if(Model.CountSets > 0){ %>    
                <h4><%if(Model.CountSets == 1){ %> In einem Fragesatz <% }
                      else{ %> In <%=Model.CountSets %> Fragesätzen<% } %></h4>
                <% foreach(var set in Model.TopSets){ %>
                    <div>
                        - <a href="<%= Links.SetDetail(Url, set) %>"><%= set.Name %></a>
                    </div>
                <% } %>
            <% } %>
        
            <% if (Model.CountCreators > 0){ %>
                <h4>Ersteller (<%=Model.CountCreators %>)</h4>
            <% } %>
        </div>         
    </div>
</asp:Content>