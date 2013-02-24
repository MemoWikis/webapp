
<%@ Page Title="Fragen" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<QuestionsModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Questions/Questions.css" rel="stylesheet" />
    <script src="/Views/Questions/Js/SelectUsers.js" type="text/javascript"></script>
    <script src="/Views/Questions/Js/Questions.js" type="text/javascript"></script>
    <script src="/Views/Questions/Js/Page.min.js" type="text/javascript"></script>
    <script src="/Views/Questions/Js/QuestionRowDelete.min.js" type="text/javascript"></script>
    <script src="/Views/Questions/Js/QuestionRowSelection.min.js" type="text/javascript"></script>
    <script src="/Views/Questions/Js/ToQuestionSet.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    
<div class="span10">
    <% using (Html.BeginForm()){ %>
            
    <div style="float: right;">
        <a href="<%= Url.Action(Links.CreateQuestion, Links.EditQuestionController) %>" style="width: 120px" class="btn">
            <i class="icon-plus-sign"></i>
            Frage erstellen
        </a>
    </div>
    
    <div class="box-with-tabs">
        
        <div class="green">
            <ul class="nav nav-tabs" >
                <li class="active"><a href="#home">Alle Fragen (<%= Model.TotalQuestionsInSystem %>)</a></li>
                <li>
                    <a href="#profile">
                        Mein Wunschwissen <span id="tabWishKnowledgeCount">(<%= Model.TotalWishKnowledge %>)</span> <i class="icon-question-sign" id="tabInfoMyKnowledge"></i>
                    </a>
                </li>
            </ul>
        </div>
            
        <div class="box box-green">
            <div class="form-horizontal">
                <div class="control-group" style="margin-bottom: 8px;">
                    <label style="line-height: 18px; padding-top: 5px;"><b>Suche</b>:</label>
                    <%: Html.TextBoxFor(model => model.SearchTerm, new {style = "width:297px;", id = "txtSearch"}) %>
                    <a class="btn" style="height: 18px;" id="btnSearch"><img alt="" src="/Images/Buttons/tick.png" style="height: 18px;"/></a>
                </div>

                <div class="control-group" style="margin-bottom: 8px;">
                    <label style="line-height: 18px; padding-top: 5px;"><b>Erstellt</b>:</label>
                    <div class="btn-group" style="display: inline">
                        <button class="btn btn-filterByMe"><i class="icon-user"></i>&nbsp;von mir</button>
                        <button class="btn btn-filterByAll">von anderen</button>
                        <%: Html.HiddenFor(model => model.FilterByMe) %>
                        <%: Html.HiddenFor(model => model.FilterByAll) %>
                    </div>
                    <span class="help-inline">und</span>&nbsp;
                    <% foreach (var filterByUser in Model.FilterByUsers)
                       { %>
                        <span class="added-usr"><%: filterByUser.Value %><button id="del-usr-<%: filterByUser.Key %>"><i class="icon-remove"></i></button></span>
                        <script type="text/javascript">
                            $(function() {
                                $("#del-usr-<%: filterByUser.Key %>").click(function() {
                                    $("#delFilterUserId").val("<%: filterByUser.Key %>");
                                });
                            });
                        </script>
                    <% } %>
                    <%: Html.HiddenFor(m => m.AddFilterUser, new {id = "addFilterUserId"}) %>
                    <%: Html.HiddenFor(m => m.DelFilterUser, new {id = "delFilterUserId"}) %>
                    <input type="text" class="span2" id="txtAddUserFilter"/>
                    <button id="addUserFilter"><img alt="" src='/Images/Buttons/tick.png' /></button>
                </div>
            </div>
            <% } %>
        
        

            <div style="padding-bottom: 5px;">
        
                <ul class="nav pull-left" style="margin: 0px; margin-left: -3px;">
                    <li class="dropdown" id="menu2">
                        <a class="dropdown-toggle btn btn-mini" data-toggle="dropdown" href="#menu2">
                            <i class="icon-check"></i>
                            Auswahl <span id="selectionCount"></span>
                            <b class="caret"></b>
                        </a>
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
                &nbsp;
                <a href="#" class="btn btn-mini hide" id="btnSelectionToSet" data-placement="bottom" data-original-title="Ausgewählte zu Fragesatz hinzufügen">
                    <i class="icon-folder-open"></i> 
                </a>&nbsp;
                <a href="#" class="btn btn-mini hide" id="btnSelectionDelete" data-placement="bottom" data-original-title="Ausgewählte löschen">
                    <i class="icon-trash"></i> 
                </a>

                <ul class="nav pull-right" style="padding-left: 5px; margin: 0px; margin-right: -3px;">
                    <li class="dropdown" id="menu1">
                        <a class="dropdown-toggle btn btn-mini" data-toggle="dropdown" href="#menu1">
                            Sortieren nach: <%= Model.OrderByLabel %>
                            <b class="caret"></b>
                        </a>
                        <ul class="dropdown-menu">
                            <li><a href="<%= Url.Action("OrderByPersonalRelevance", "Questions") %>"><% if (Model.OrderBy.OrderByPersonalRelevance.IsCurrent())
                                                                                                        { %><i class="icon-ok"></i> <% } %>Merken</a></li>
                            <li><a href="<%= Url.Action("OrderByQuality", "Questions") %>"><% if (Model.OrderBy.OrderByQuality.IsCurrent())
                                                                                              { %><i class="icon-ok"></i> <% } %>Qualität</a></li>
                            <li><a href="<%= Url.Action("OrderByCreationDate", "Questions") %>"><% if (Model.OrderBy.OrderByCreationDate.IsCurrent())
                                                                                                   { %><i class="icon-ok"></i> <% } %>Erstellungsdatum</a></li>
                            <li><a href="<%= Url.Action("OrderByViews", "Questions") %>"><% if (Model.OrderBy.OrderByViews.IsCurrent())
                                                                                            { %><i class="icon-ok"></i> <% } %>Ansichten</a></li>
                            <li class="divider"></li>
                            <li><a href="#">Empfehlungen</a></li>
                        </ul>
                    </li>
                </ul>
        
                <div class="pull-right" style="font-size: 14px; margin-top: 3px; margin-right: 7px;"><%= Model.TotalQuestionsInResult %> Fragen</div>
       
            </div>
        
            <div class="box-content">

                <% foreach (var row in Model.QuestionRows){
                       Html.RenderPartial("QuestionRow", row);
                } %>
            </div>
    
            <% Html.RenderPartial("Pager", Model.Pager); %>
    
            <% Html.RenderPartial("Modals/ToQuestionSet"); %>
            <% Html.RenderPartial("Modals/DeleteQuestion"); %>
    
            <% /* MODAL-TAB-INFO-MyKnowledge****************************************************************/ %>
    
            <div id="modalTabInfoMyKnowledge" class="modal hide fade">
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
</div>
    
  
</asp:Content>