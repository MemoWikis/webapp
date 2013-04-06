<%@ Page Title="Fragesätze" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<QuestionSetsModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Scripts.Render("~/Views/QuestionSets/QuestionSets.js") %>
    <%= Styles.Render("~/Views/QuestionSets/QuestionSets.css") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="span10">
        <% using (Html.BeginForm()) { %>
    
            <div style="float: right;">
                <a href="<%= Url.Action("Create", "EditQuestionSet") %>" style="width: 140px" class="btn">
                    <i class="icon-plus-sign"></i> Fragesatz erstellen
                </a>
            </div>
            <div class="box-with-tabs">
                <div class="green">
                    <ul class="nav nav-tabs">
                        <li class="active"><a href="#home" >Alle Fragesätze (<%= Model.TotalQuestionSets %>)</a></li>
                        <li>
                            <a href="#profile">
                                Meine Fragesätze <span id="tabWishKnowledgeCount">(<%= Model.TotalMine %>)</span> <i class="icon-question-sign" id="tabInfoMyKnowledge"></i>
                            </a>
                        </li>
                    </ul>
                </div>
        
                <div class="box box-green">
                    <div class="form-horizontal">
                        <div class="control-group" style="margin-bottom: 15px; margin-top: -7px; ">
                            <label style="line-height: 18px; padding-top: 5px;"><nb>Suche</nb>:</label>
                            <%: Html.TextBoxFor(model => model.SearchTerm, new {style="width:297px;", id="txtSearch"}) %>
                            <a class="btn" style="height: 18px;" id="btnSearch"><img alt="" src="/Images/Buttons/tick.png" style="height: 18px;"/></a>
                        </div>
                        <div style="clear:both;"></div>
                    </div>
        
                    <div class="box-content">
                        <% foreach(var row in Model.Rows){
                            Html.RenderPartial("QuestionSetRow", row);
                        } %>
    
                    </div>
                    <% Html.RenderPartial("Pager", Model.Pager); %>
                </div>
            </div>
    <% } %>
    </div>
</asp:Content>
