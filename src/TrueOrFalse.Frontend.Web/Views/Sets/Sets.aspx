<%@ Page Title="Fragesätze" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<SetsModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/Views/Sets/Sets.css") %>
    <%= Scripts.Render("~/bundles/Sets") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="col-md-10">
        <% using (Html.BeginForm()) { %>
    
            <div style="float: right;">
                <a href="<%= Url.Action("Create", "EditSet") %>" style="width: 140px" class="btn btn-default">
                    <i class="fa fa-plus-circle"></i> Fragesatz erstellen
                </a>
            </div>
            <div class="box-with-tabs">
                <div class="green">
                    <ul class="nav nav-tabs">
                        <li class="<%= Model.ActiveTabAll ? "active" : ""  %>">
                            <a href="<%= Links.Sets(Url) %>" >Alle Fragesätze (<%= Model.TotalSets %>)</a>
                        </li>
                        <li class="<%= Model.ActiveTabWish ? "active" : ""  %>">
                            <a href="<%= Links.SetsWish(Url) %>">Mein Wunschwissen <span id="tabWishKnowledgeCount">(<%= Model.TotalWish %>)</span></a>
                        </li>
                        <li class="<%= Model.ActiveTabMine ? "active" : ""  %>">
                            <a href="<%= Links.SetsMine(Url) %>">
                                Meine Fragesätze (<%= Model.TotalMine %>)
                                <i class="fa fa-question-circle show-tooltip" title="Fragesätze die von Dir erstellt wurden"></i>
                            </a>
                        </li>
                    </ul>
                </div>
        
                <div class="box box-green">
                    <div class="form-horizontal">
                        <div class="control-group" style="margin-bottom: 15px; margin-top: -7px; ">
                            <label style="line-height: 18px; padding-top: 5px;"><nb>Suche</nb>:</label>
                            <%: Html.TextBoxFor(model => model.SearchTerm, new {style="width:297px;", id="txtSearch", formUrl=Model.SearchUrl}) %>
                            <a class="btn btn-default" style="height: 18px;" id="btnSearch"><img alt="" src="/Images/Buttons/tick.png" style="height: 18px;"/></a>
                        </div>
                        <div style="clear:both;"></div>
                    </div>
        
                    <div class="box-content">
                        <% foreach(var row in Model.Rows){
                            Html.RenderPartial("SetRow", row);
                        } %>
    
                    </div>
                    <% Html.RenderPartial("Pager", Model.Pager); %>
                </div>
            </div>
    <% } %>
    </div>
    
    <% Html.RenderPartial("Modals/DeleteSet"); %>
</asp:Content>
