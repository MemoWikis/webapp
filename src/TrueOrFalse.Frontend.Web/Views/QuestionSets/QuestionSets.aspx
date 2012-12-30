<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<QuestionSetsModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/QuestionSets/QuestionSets.css" rel="stylesheet" />
    <script src="/Views/QuestionSets/QuestionSets.js" type="text/javascript"></script>
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <% using (Html.BeginForm()) { %>
        <ul class="nav nav-tabs" style="padding-top: 14px;  ">
            <li class="active"><a href="#home" >Alle  (<%= Model.TotalQuestionSets %>)</a></li>
            <li>
                <a href="#profile">
                    Meine Fragesätze <span id="tabWishKnowledgeCount">(<%= Model.TotalMine %>)</span> <i class="icon-question-sign" id="tabInfoMyKnowledge"></i>
                </a>
            </li>
        </ul>
        <div style="float: right; margin-top: -55px;">
            <a href="<%= Url.Action("Create", "EditQuestionSet") %>" style="width: 140px" class="btn">
                <i class="icon-plus-sign"></i>
                Fragesatz erstellen
            </a>
        </div>
        <div class="row form-horizontal " style="background-color: white; padding-top:15px; margin-top: -21px; margin-bottom: 0px; padding-bottom: 0px; border: 1px solid #ebebeb; border-bottom: none;">
            
            <div class="control-group" style="margin-bottom: 8px;">
                <label style="line-height: 18px; padding-top: 5px;"><nb>Suche</nb>:</label>
                <%: Html.TextBoxFor(model => model.SearchTerm, new {style="width:297px;", id="txtSearch"}) %>
                <a class="btn" style="height: 18px;" id="btnSearch"><img alt="" src="/Images/Buttons/tick.png" style="height: 18px;"/></a>
            </div>

            <div class="control-group" style="margin-bottom: 8px; background-color: white;" >
                <label style="line-height: 18px; padding-top: 5px;"><b>Erstellt</b>:</label>
                <div class="btn-group" style="display: inline">
                    <button class="btn btn-filterByMe"><i class="icon-user"></i>&nbsp;von mir</button>
                    <button class="btn btn-filterByAll">von anderen</button>
                    <%: Html.HiddenFor(model => model.FilterByMe)%>
                    <%: Html.HiddenFor(model => model.FilterByAll)%>
                </div>
            </div>
        </div>
    <% } %>
    
    <% foreach(var row in Model.Rows){
        Html.RenderPartial("QuestionSetRow", row);
    } %>
</asp:Content>
