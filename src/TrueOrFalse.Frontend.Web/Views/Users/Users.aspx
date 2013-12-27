<%@ Page Title="Nutzer" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<UsersModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/Views/Users/Users.css") %>
    <%= Scripts.Render("~/bundles/Users") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="col-md-9">
        <% using (Html.BeginForm()) { %>

            <div class="boxtainer-outlined-tabs">
                <div class="boxtainer-header">
                    <ul class="nav nav-tabs">
                        <li class="active"><a href="#home" >Alle Nutzer (<%= Model.TotalSets %>)</a></li>
                        <li>
                            <a href="#profile">
                                Mein Netzwerk <span id="tabWishKnowledgeCount">(<%= Model.TotalMine %>)</span> <i class="fa fa-question-circle" id="tabInfoMyKnowledge"></i>
                            </a>
                        </li>
                    </ul>
                </div>
        
                <div class="boxtainer-content">
                    <div class="search-section">
                        <div class="row">
                            <div class="col-md-6">                    
                                <div class="pull-left form-group search-container">
                                    <label>Suche:</label>
                                    <%: Html.TextBoxFor(model => model.SearchTerm, new {@class="form-control", id="txtSearch"}) %>
                                    <a class="btn btn-default" id="btnSearch"><img src="/Images/Buttons/tick.png"/></a>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <!-- order by -->
                            </div>
                        </div>
                        
                        <% Html.Message(Model.Message); %>

                        <div style="clear: both;">
                            <% foreach(var row in Model.Rows){
                                Html.RenderPartial("UserRow", row);
                            } %>
                        </div>

                    </div>

                    <% Html.RenderPartial("Pager", Model.Pager); %>
                </div>
            </div>
    <% } %>
    </div>
</asp:Content>
