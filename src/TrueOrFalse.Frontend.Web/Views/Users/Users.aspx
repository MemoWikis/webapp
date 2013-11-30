<%@ Page Title="Nutzer" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<UsersModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/Views/Users/Users.css") %>
    <%= Scripts.Render("~/bundles/Users") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="col-md-9">
        <% using (Html.BeginForm()) { %>
    
            <div style="float: right;">
                <a href="#" class="btn btn-default">
                    <i class="fa fa-plus-circle"></i> Benutzer einladen 
                </a>
            </div>
            <div class="box-with-tabs">
                <div class="green">
                    <ul class="nav nav-tabs">
                        <li class="active"><a href="#home" >Alle Nutzer (<%= Model.TotalSets %>)</a></li>
                        <li>
                            <a href="#profile">
                                Mein Netzwerk <span id="tabWishKnowledgeCount">(<%= Model.TotalMine %>)</span> <i class="fa fa-question-circle" id="tabInfoMyKnowledge"></i>
                            </a>
                        </li>
                    </ul>
                </div>
        
                <div class="box box-green">
                    
                    <div class="pull-left form-group search-container">
                        <label>Suche:</label>
                        <%: Html.TextBoxFor(model => model.SearchTerm, new {@class="form-control", id="txtSearch"}) %>
                        <a class="btn btn-default" id="btnSearch"><img src="/Images/Buttons/tick.png"/></a>
                    </div>
                    <div style="clear:both;"></div>

                    <div class="box-content">
                        
                        <div>
                            <% Html.Message(Model.Message); %>
                        </div>

                        <% foreach(var row in Model.Rows){
                            Html.RenderPartial("UserRow", row);
                        } %>
    
                    </div>
                    <% Html.RenderPartial("Pager", Model.Pager); %>
                </div>
            </div>
    <% } %>
    </div>
</asp:Content>
