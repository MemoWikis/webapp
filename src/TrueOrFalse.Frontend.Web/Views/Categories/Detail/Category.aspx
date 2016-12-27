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
            
            <%= Model.ContentHtml %>

            <h4>Über- und untergeordnete Kategorien</h4>
            <div class="CategoryRelations well">
                <% if (Model.CategoriesParent.Count > 0) { %>
                    <div>
                        <% foreach (var category in Model.CategoriesParent)
                            { %>
                            <a href="<%= Links.CategoryDetail(category) %>"><span class="label label-category"><%= category.Name %></span></a>
                        <% } %>
                    </div>
                <% }
                    else { %>
                    <div>keine übergeordneten Kategorien</div>
                <%  } %>

                <div class="RelationArrow"><i class="fa fa-arrow-down"></i></div>
                <div class="MainCategory"><span class="label label-category"><%= Model.Name %></span></div>
                <div class="RelationArrow"><i class="fa fa-arrow-down"></i></div>

                <% if(Model.CategoriesChildren.Count > 0){ %>
                    <div>
                        <% foreach(var category in Model.CategoriesChildren){ %>
                            <a href="<%= Links.CategoryDetail(category) %>"><span class="label label-category"><%= category.Name %></span></a>
                        <% } %>
                        <i class="fa fa-plus-circle show-tooltip color-category add-new" 
                            style="font-size: 14px; cursor: pointer"
                            onclick="window.location = '/Kategorien/Erstelle?parent=<%= Model.Category.Id%>'; return false; " 
                            data-original-title="Neue untergeordnete Kategorie erstellen"></i>
                    </div>
                <% } else { %>
                    <div style="margin-top: 0;">keine untergeordneten Kategorien
                        <i class="fa fa-plus-circle show-tooltip color-category add-new" 
                            style="font-size: 14px; cursor: pointer"
                            onclick="window.location = '/Kategorien/Erstelle?parent=<%= Model.Category.Id%>'; return false; " 
                            data-original-title="Neue untergeordnete Kategorie erstellen"></i>
                    </div>
                <%  } %>
            </div>
            <h4>Inhalte</h4>
            <div id="Content" class="Box">
                <% if(Model.CountSets > 0){ %>    
                    <h5 class="ContentSubheading Set"><%= Model.CountSets %> Frage<%= StringUtils.PluralSuffix(Model.CountSets,"sätze","satz") %> in dieser Kategorie</h5>
                    <div class="LabelList">
                    <% foreach(var set in Model.TopSets){ %>
                        <div class="LabelItem LabelItem-Set">
                            <a href="<%= Links.SetDetail(Url, set) %>"><%= set.Name %></a>
                             (<a href="<%= Links.TestSessionStartForSet(set.Name, set.Id) %>"><i class="fa fa-play-circle">&nbsp;</i>Jetzt Wissen testen</a>)
                        </div>
                    <% } %>
                    </div>
                <% } %>
        
                <h5 class="ContentSubheading Question"><%= Model.CountQuestions %> Frage<%= StringUtils.PluralSuffix(Model.CountQuestions,"n") %> in dieser Kategorie</h5>

                <% if (Model.CountQuestions > 0){ %>
                    <div class="LabelList">
                    <% var index = 0;
                        foreach (var question in Model.TopQuestions)
                        {
                            index++; %>
                        <div class="LabelItem LabelItem-Question">
                            <a href="<%= Links.AnswerQuestion(Url, question, paramElementOnPage: index, categoryFilter: Model.Name) %>"><%= question.GetShortTitle(150) %></a>
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
            
                <% if(Model.CountReferences > 0) { %>
                    <h5 class="ContentSubheading Question">Fragen mit diesem Medium als Quellenangabe (<%=Model.CountReferences %>)</h5>
                    <div class="LabelList">
                        <% var index = 0; foreach(var question in Model.TopQuestionsWithReferences){ index++;%>
                            <div class="LabelItem LabelItem-Question">
                                <a href="<%= Links.AnswerQuestion(Url, question, paramElementOnPage: index, categoryFilter:Model.Name) %>" rel="nofollow"><%= question.GetShortTitle(150) %></a>
                            </div>
                        <% } %>
                    </div>
                <% } %>
            
                <% if(Model.TopQuestionsInSubCats.Count > 0){ %>
                    <h5 class="ContentSubheading Question">Fragen in untergeordneten Kategorien</h5>
                    <div class="LabelList">
                    <% var index = 0; foreach(var question in Model.TopQuestionsInSubCats){ index++;%>
                        <div class="LabelItem LabelItem-Question">
                            <a href="<%= Links.AnswerQuestion(question) %>"><%= question.GetShortTitle(150) %></a>
                        </div>
                    <% } %>
                    </div>
                <% } %>
            
                <% if(Model.CountWishQuestions > 0){ %>
                    <h5 class="ContentSubheading Question">In deinem <a href="<%= Links.QuestionsWish() %>">Wunschwissen</a> (<%=Model.CountWishQuestions %>)</h5>
                    <div class="LabelList">
                    <% var index = 0; foreach(var question in Model.TopWishQuestions){index++; %>
                        <div class="LabelItem LabelItem-Question">
                            <a href="<%= Links.AnswerQuestion(Url, question, paramElementOnPage: index, categoryFilter:Model.Name) %>" rel="nofollow"><%= question.GetShortTitle(150) %></a>
                        </div>
                    <% } %>
                    </div>
                <% } %>
            </div>
        </div>
    </div>
</asp:Content>