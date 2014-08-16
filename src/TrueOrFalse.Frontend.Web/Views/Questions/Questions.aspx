<%@ Page Title="Fragen" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<QuestionsModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Questions/Questions.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/questions") %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SubHeader" runat="server">
     <div id="MobileSubHeader" class="MobileSubHeader DesktopHide">
        <div class=" container">
            <div id="mobilePageHeader" class="">
                <h3 class="">
                    Fragen
                </h3>
                <a href="<%= Links.CreateQuestion(Url) %>" class="btnAddQuestion btn btn-success btn-sm">
                    <i class="fa fa-plus-circle"></i>
                    Frage erstellen
                </a>
            </div>
            <nav id="mobilePageHeader2" class="navbar navbar-default" style="display: none;">
                <h4>
                    Fragen
                </h4>
            </nav>
        </div>
        <div class="MainFilterBarWrapper">
            <div id="MainFilterBarBackground" class="btn-group btn-group-justified">
                <div class="btn-group">
                    <a class="btn btn-default disabled">.</a>
                </div>
            </div>
            <div class="container">
                <div id="MainFilterBar" class="btn-group btn-group-justified JS-Tabs">
                
                    <div id="AllQuestions" class="btn-group  <%= Model.ActiveTabAll ? "active" : ""  %> JS-<%= SearchTab.All.ToString() %>">
                        <a  href="<%= Links.QuestionsAll() %>" type="button" class="btn btn-default">
                            <%  string von = "";
                                if (Model.ActiveTabAll && Model.TotalQuestionsInSystem != Model.TotalQuestionsInResult)
                                von = Model.TotalQuestionsInResult + " von ";  %>
                                Alle (<span class="JS-Amount"><%= von + Model.TotalQuestionsInSystem %></span>)
                        </a>
                    </div>
                    <div id="WuWiQuestions" class="btn-group <%= Model.ActiveTabWish ? "active" : "" %> JS-<%= SearchTab.Wish.ToString() %>">
                        <a  href="<%= Links.QuestionsWish() %>" type="button" class="btn btn-default">
                            <% von = "";
                            if (Model.ActiveTabWish && Model.TotalWishKnowledge != Model.TotalQuestionsInResult)
                            von = Model.TotalQuestionsInResult + " von "; %>
                            Wunsch<span class="hidden-xxs">wissen</span> (<span class="tabWishKnowledgeCount JS-Amount"><%= von + Model.TotalWishKnowledge %></span>)
                            <i class="fa fa-question-circle show-tooltip" id="tabInfoMyKnowledge" title="Wissen das Du jederzeit aktiv nutzen möchtest." data-placement="right"></i>
                        </a>
                    </div>
                    <div id="MyQuestions" class="btn-group <%= Model.ActiveTabMine ? "active" : "" %> JS-<%= SearchTab.Mine.ToString() %>">
                        <a href="<%= Links.QuestionsMine() %>" type="button" class="btn btn-default">
                        <%  von = "";
                            if (Model.ActiveTabMine && Model.TotalQuestionsMine != Model.TotalQuestionsInResult)
                                von = Model.TotalQuestionsInResult + " von "; %>                        
                        Meine (<span class="JS-Amount"><%= von + Model.TotalQuestionsMine %></span>)
                            <i class="fa fa-question-circle show-tooltip" title="Fragen die von Dir erstellt wurden." data-placement="right"></i>
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div id="question-main">
    
    <% using (Html.BeginForm()){ %>
                
     <div class="boxtainer-outlined-tabs">
         
        <div class="boxtainer-header MobileHide">
                <a href="<%= Links.CreateQuestion(Url) %>" class="btnAddQuestion btn btn-success btn-sm">
                    <i class="fa fa-plus-circle"></i>
                    Frage erstellen
                </a>
            <ul class="nav nav-tabs JS-Tabs">
                <li class="<%= Model.ActiveTabAll ? "active" : ""  %> JS-<%= SearchTab.All.ToString() %>">
                    <a href="<%= Links.QuestionsAll() %>">
                        <%  string von = "";
                            if (Model.ActiveTabAll && Model.TotalQuestionsInSystem != Model.TotalQuestionsInResult)
                                von = Model.TotalQuestionsInResult + " von ";  %>
                        Alle Fragen (<span class="JS-Amount"><%= von + Model.TotalQuestionsInSystem %></span>)
                    </a>
                </li>
                <li class="<%= Model.ActiveTabWish ? "active" : ""  %> JS-<%= SearchTab.Wish.ToString() %>">
                    <a href="<%= Links.QuestionsWish() %>">
                        <% von = "";
                           if (Model.ActiveTabWish && Model.TotalWishKnowledge != Model.TotalQuestionsInResult)
                               von = Model.TotalQuestionsInResult + " von "; %>
                        Mein Wunschwissen <span class="tabWishKnowledgeCount">(<span class="JS-Amount"><%= von + Model.TotalWishKnowledge %></span>)</span>
                        <i class="fa fa-question-circle show-tooltip" id="tabInfoMyKnowledge" 
                           title="Wissen das Du jederzeit aktiv nutzen möchtest." data-placement="right"></i>
                    </a>
                </li>
                <li class="<%= Model.ActiveTabMine ? "active" : ""  %> JS-<%= SearchTab.Mine.ToString() %>">
                    <a href="<%= Links.QuestionsMine() %>">
                        <%  von = "";
                            if (Model.ActiveTabMine && Model.TotalQuestionsMine != Model.TotalQuestionsInResult)
                               von = Model.TotalQuestionsInResult + " von "; %>                        
                        Meine Fragen (<span class="JS-Amount"><%= von + Model.TotalQuestionsMine %></span>)
                        <i class="fa fa-question-circle show-tooltip" title="Fragen die von Dir erstellt wurden." data-placement="right"></i>
                    </a>
                </li>
            </ul>
        </div>
            
        <div class="boxtainer-content">
            
            <% if(!Model.NotAllowed){ %>
                <div class="search-section">
                    <div class="SearchQuestionsForm form-horizontal">
                        <div class="form-group">
                            <div class="input-group">
                                <input type="text" class="form-control" id="txtSearch" formUrl="<%:Model.SearchUrl %>" name="SearchTerm" value="<%:Model.SearchTerm %>" />
                                <span class="input-group-btn">
                                    <button class="btn btn-default" id="btnSearch"><i class="fa fa-search"></i></button>
                                </span>
                            </div>
                            <% if(!String.IsNullOrEmpty(Model.Suggestion)){ %> 
                                <div class="col-xs-12" style="padding-top: 10px; font-size: large">
                                    Oder suchst du: 
                                    <a href="<%= Model.SearchUrl + "/" + Model.Suggestion %>">
                                        <%= Model.Suggestion %>
                                    </a> ?
                                </div>
                            <% } %>
                        </div>
                        <div class="form-group">
                            <div class="JS-RelatedCategories">
                                <script type="text/javascript">
                                    $(function () {
                                        <%foreach (var category in Model.FilteredCategories) { %>
                                        $("#txtCategoryFilter")
                                            .val('<%=category.Name %>')
                                            .data('category-id', '<%=category.Id %>')
                                            .trigger("initCategoryFromTxt");
                                        <% } %>
                                    });
                                </script>
                                <div class="JS-CatInputContainer ControlInline">
                                    <input id="txtCategoryFilter" class="form-control .JS-ValidationIgnore" type="text" placeholder="Wähle eine Kategorie"  />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <ul class="nav pull-left">
                                <li class="dropdown" id="menu2">
                                    <button class="dropdown-toggle btn btn-default btn-xs" data-toggle="dropdown" href="#menu2">
                                        <i class="fa fa-check-square-o"></i>
                                        Auswahl <span id="selectionCount"></span>
                                        <b class="caret"></b>
                                    </button>
                                    <ul class="dropdown-menu">
                                        <li><a id="selectAll">Alle</a></li>
                                        <li><a id="selectNone">Keine</a></li>
                                        <li><a id="selectMemorizedByMe">+ von mir gemerkte</a></li>
                                        <li><a id="selectCreatedByMe">+ von mir erstellte</a></li>
                                        <li><a id="selectedNotMemorizedByMe">+ <i>nicht</i> von mir gemerkte</a></li>
                                        <li><a id="selectNotCraetedByMe">+ <i>nicht</i> von mir erstellt</a></li>
                                    </ul>
                                </li>
                            </ul>
                        
                            <a href="#" class="btn btn-default btn-xs" style="display: none; margin-left: 7px;" id="btnSelectionToSet" data-placement="bottom" data-original-title="Ausgewählte zu Fragesatz hinzufügen">
                                <i class="fa fa-list-ol"></i> 
                            </a>
                            <a href="#" class="btn btn-default btn-xs" style="display: none; margin-left: 7px;" id="btnSelectionDelete" data-placement="bottom" data-original-title="Ausgewählte löschen">
                                <i class="fa fa-trash-o"></i> 
                            </a>
                            <a href="#" class="btn btn-default btn-xs" style=" margin-left: 7px;" id="btnExport" data-placement="bottom" data-original-title="Herunterladen">
                                <i class="fa fa-cloud-download"></i>
                            </a>                        
                        </div>
                        <div class="col-xs-7">
                            <ul class="nav pull-right">
                                <li class="dropdown" id="menu1">
                                    <button class="dropdown-toggle btn btn-default btn-xs" data-toggle="dropdown" href="#menu1">
                                        <span class="hidden-xxs">Sortieren nach:</span> <%= Model.OrderByLabel %>
                                        <b class="caret"></b>
                                    </button>
                                    <ul class="dropdown-menu">
                                        <li>
                                            <a href="<%= Request.Url.AbsolutePath + "?orderBy=byRelevance" %>">
                                                <% if (Model.OrderBy.OrderByPersonalRelevance.IsCurrent()){ %><i class="fa fa-check"></i> <% } %>Gemerkt
                                            </a>
                                        </li>
                                        <li>
                                            <a href="<%= Request.Url.AbsolutePath + "?orderBy=byDateCreated" %>">
                                                <% if (Model.OrderBy.OrderByCreationDate.IsCurrent()){ %><i class="fa fa-check"></i> <% } %>Datum erstellt
                                            </a>
                                        </li>
                                        <li>
                                            <a href="<%= Request.Url.AbsolutePath + "?orderBy=byViews" %>">
                                                <% if (Model.OrderBy.OrderByViews.IsCurrent()){ %><i class="fa fa-check"></i> <% } %>Gesehen
                                            </a>
                                        </li>
                                    </ul>
                                </li>
                            </ul>
                            <div class="pull-right" id="resultCount" style="font-size: 14px; margin-right: 7px; margin-top: 2px;"><%= Model.TotalQuestionsInResult %> Fragen</div>
                        </div>
                    </div>
                </div>
                <% } %>
            <%} %>
            <div id="JS-SearchResult">
                <% Html.RenderPartial("QuestionsSearchResult", Model.SearchResultModel); %>
            </div>
        </div>
    </div>
</div>
        
<% Html.RenderPartial("Modals/ToQuestionSet"); %>
<% Html.RenderPartial("Modals/DeleteQuestion"); %>
<% /* MODAL-TAB-INFO-MyKnowledge****************************************************************/ %>
<div id="modalTabInfoMyKnowledge" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button class="close" data-dismiss="modal">×</button>
                <h3>Hilfe: Tab - Mein Wunschwissen</h3>
            </div>
            <div class="modal-body">
                Es werden nur die Fragen gezeigt, die Du Dir <b>merken</b> möchtest, also Fragen deren Antworten zu Deinem Wunschwissen gehören. 
            </div>
            <div class="modal-footer">
                <a href="#" class="btn btn-warning" data-dismiss="modal">Mmh, nicht ganz klar.</a>
                <a href="#" class="btn btn-info" data-dismiss="modal">Danke, ich habe verstanden!</a>
            </div>
        </div>
    </div>
</div>

</asp:Content>