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
                <a href="<%= Url.Action("Create", "EditSet") %>" class="btn btn-default">
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
                    
                    <div class="pull-left form-group search-container">
                        <label>Suche:</label>
                        <%: Html.TextBoxFor(model => model.SearchTerm, new {@class="form-control", id="txtSearch", formUrl=Model.SearchUrl}) %>
                        <button class="btn btn-default" id="btnSearch"><img src="/Images/Buttons/tick.png"/></button>
                    </div>
                    <div style="clear:both;"></div>
                    
        
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
