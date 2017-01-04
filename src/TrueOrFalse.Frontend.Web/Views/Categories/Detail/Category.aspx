<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<CategoryModel>"%>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <% Title = "Kategorie: " + Model.Name; %>
    <link rel="canonical" href="<%= Settings.CanonicalHost + Links.CategoryDetail(Model.Name, Model.Id) %>">
    <meta name="description" content="<%= Model.Name.Replace("\"", "'").Replace("„", "'").Replace("“", "'").Truncate(25, true) %> (<%=Model.CountQuestions %> Fragen) <%= String.IsNullOrEmpty(Model.Description) ? "" : ": "+Model.Description.Replace("\"", "'").Replace("„", "'").Replace("“", "'").Truncate(89, true) %> - Lerne mit memucho!"/>
    
    <meta property="og:url" content="<%= Settings.CanonicalHost + Links.CategoryDetail(Model.Name, Model.Id) %>" />
    <meta property="og:type" content="article" />
    <meta property="og:image" content="<%= Model.ImageFrontendData.GetImageUrl(350, false, imageTypeForDummy: ImageType.Category).Url %>" />
</asp:Content>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Categories/Detail/Category.css" rel="stylesheet" /> 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row" >
        <div class="col-xs-12 col-md-2 col-md-push-10">
            <div class="navLinks">
                <a href="<%= Url.Action(Links.CategoriesAction, Links.CategoriesController) %>" style="font-size: 12px;"><i class="fa fa-list"></i>&nbsp;zur Übersicht</a>
                <% if(Model.IsOwnerOrAdmin){ %>
                    <a href="<%= Links.CategoryEdit(Url, Model.Name, Model.Id) %>" style="font-size: 12px;"><i class="fa fa-pencil"></i>&nbsp;bearbeiten</a> 
                <% } %>
                <a href="<%= Links.CreateQuestion(Url, Model.Id) %>" style="font-size: 12px;"><i class="fa fa-plus-circle"></i>&nbsp;Frage hinzufügen</a>
            </div>
        </div>
        <div class="col-xs-12 col-md-10 col-md-pull-2">
            <div id="ItemMainInfo" class="Box">
                <div class="">
                    <div class="row">
                        <div class="col-xs-12 col-sm-3">
                            <div class="ImageContainer">
                                <%= Model.ImageFrontendData.RenderHtmlImageBasis(350, false, ImageType.Category, "ImageContainer") %>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-9">
                            <header>
                                <div>
                                    <% if (Model.Type != "Standard"){ %>
                                        <%= Model.Type %>
                                    <% }
                                    else { %>
                                        Kategorie
                                    <% }  %>
                                </div>
                                <h1 style="margin-top: 5px; font-size: 26px;">
                                    <%= Model.Name %>
                                </h1>
                            </header>
                            <div>
                                 <% if (Model.Type != "Standard") {
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
                            
                            <% if(Model.AnswersTotal > 0) { %>
                                <div class="Divider" style="margin-top: 10px; margin-bottom: 5px;"></div>
                                <div style="margin-top: 6px; font-size: 11px;">
                                    In dieser Kategorie wurden
                                    <%= Model.AnswersTotal + "x Frage" + StringUtils.PluralSuffix(Model.AnswersTotal, "n") %> beantwortet, 
                                    davon <%= Model.CorrectnesProbability %>% richtig.
                                </div>
                            <% } %>                

                            <div class="Divider" style="margin-top: 10px; margin-bottom: 5px;"></div>
                            <div class="BottomBar">
                                <div style="float: left; padding-top: 3px;">
                                    <div class="fb-share-button" style="width: 100%" data-href="<%= Settings.CanonicalHost + Links.CategoryDetail(Model.Name, Model.Id) %>" data-layout="button" data-size="small" data-mobile-iframe="true"><a class="fb-xfbml-parse-ignore" target="_blank" href="https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2Fdevelopers.facebook.com%2Fdocs%2Fplugins%2F&amp;src=sdkpreparse">Teilen</a></div>                
                                </div>
                               <%-- <div class="dropdown">
                                    <% var buttonId = Guid.NewGuid(); %>
                                    <a href="#" id="<%=buttonId %>" <%= Model.QuestionCount == 0 ? "disabled " : "" %>class="dropdown-toggle  btn btn-link ButtonOnHover ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                        <i class="fa fa-ellipsis-v"></i>
                                    </a>
                                    <ul class="dropdown-menu" aria-labelledby="<%=buttonId %>">
                                        <li><a href="<%= Links.StartSetLearningSession(Model.Id) %>" data-allowed="logged-in" data-allowed-type="learning-session" rel="nofollow">Jetzt üben</a></li>
                                        <li><a href="<%= Links.GameCreateFromSet(Model.Id) %>"> Spiel starten</a></li>
                                        <li><a href="<%= Links.DateCreate(Model.Id) %>"> Termin anlegen</a></li>
                                    </ul>
                                </div>--%>
                                <a class="btn btn-primary show-tooltip" href="<%= Links.TestSessionStartForCategory(Model.Name,Model.Id) %>" title="Teste dein Wissen in dieser Kategorie" rel="nofollow">
                                    &nbsp;JETZT TESTEN
                                </a>
                            </div>
                        </div>
                    </div>
                </div>    
            </div>
            
            <% if (string.IsNullOrEmpty(Model.CustomPageHtml)) {

                    if (Model.FeaturedSets.Count > 0){

                        Html.RenderPartial("~/Views/Categories/Detail/Partials/SingleSetCollection.ascx",
                            new SingleSetCollectionModel(Model.FeaturedSets, "Ausgewählte Fragesätze"));

                        Html.RenderPartial("~/Views/Categories/Detail/Partials/CategoryNetwork.ascx", Model);

                        Html.RenderPartial("~/Views/Categories/Detail/Partials/ContentLists.ascx", Model);

                        Html.RenderPartial("~/Views/Categories/Detail/Partials/RelatedContentLists.ascx", Model);


                    } else {

                        Html.RenderPartial("~/Views/Categories/Detail/Partials/ContentLists.ascx", Model);

                        Html.RenderPartial("~/Views/Categories/Detail/Partials/CategoryNetwork.ascx", Model);

                        Html.RenderPartial("~/Views/Categories/Detail/Partials/RelatedContentLists.ascx", Model);
                    }

                } else { %> 
                    <%= Model.CustomPageHtml %>
            <% } %>

        </div>
    </div>
</asp:Content>