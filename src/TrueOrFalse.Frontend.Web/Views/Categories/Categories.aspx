<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<CategoriesModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/Views/Categories/Categories.css") %>
    <%= Scripts.Render("~/bundles/Categories") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="col-md-9">
       
        <% Html.Message(Model.Message); %>
        
        <% using (Html.BeginForm()) { %>
        
            
        
            <div class="boxtainer-outlined-tabs">
                <div class="boxtainer-header">
                    <ul class="nav nav-tabs">
                        <li class="active"><a href="#home" >Alle Kategorien (<%= Model.TotalCategories %>)</a></li>
                        <li>
                            <a href="#profile">
                                Meine Kategorien <span id="tabWishKnowledgeCount">(<%= Model.TotalMine %>)</span> <i class="fa fa-question-circle" id="tabInfoMyKnowledge"></i>
                            </a>
                        </li>
                    </ul>
                    <div style="float: right; position: absolute; right: 0; top: 5px;">
                        <a href="<%= Url.Action("Create", "EditCategory") %>" class="btn btn-default">
                            <i class="fa fa-plus-circle"></i>  Kategorie erstellen
                        </a>
                    </div>
                </div>
                <div class="boxtainer-content">
                    
                    <div class="search-section">
                        <ul class="nav pull-right">
                            <li class="dropdown" id="menu1">
                                <button class="dropdown-toggle btn btn-default btn-xs" data-toggle="dropdown" href="#menu1">
                                    Sortieren nach: <%= Model.OrderByLabel %>
                                    <b class="caret"></b>
                                </button>
                                <ul class="dropdown-menu">
                                    <li>
                                        <a href="<%= Request.Url.AbsolutePath + "?orderBy=byQuestions" %>">
                                            <% if (Model.OrderBy.QuestionCount.IsCurrent()){ %><i class="icon-ok"></i> <% } %> Anzahl Fragen
                                        </a>
                                    </li>
                                    <li>
                                        <a href="<%= Request.Url.AbsolutePath + "?orderBy=byDate" %>">
                                            <% if (Model.OrderBy.CreationDate.IsCurrent()){ %><i class="icon-ok"></i> <% } %>  Datum erstellt
                                        </a>
                                    </li>
                                </ul>
                            </li>
                        </ul>
        
                        <div class="pull-right" style="font-size: 14px; margin-top: 0px; margin-right: 7px;"><%= Model.TotalCategoriesInResult %> Treffer</div>
                        
                        <div class="pull-left form-group">
                            <label>Suche:</label>
                            <%: Html.TextBoxFor(model => model.SearchTerm, new {@class="form-control", id="txtSearch"}) %>
                            <button class="btn btn-default" id="btnSearch"><img src="/Images/Buttons/tick.png" /></button>
                        </div>
                    </div>

                    <div class="" style="clear: both;">
                        <% foreach (var row in Model.CategoryRows){
                            Html.RenderPartial("CategoryRow", row);
                        } %>
                    </div>
                    <% Html.RenderPartial("Pager", Model.Pager); %>
                </div>
            </div>
            
        <% } %>
    </div>

</asp:Content>
