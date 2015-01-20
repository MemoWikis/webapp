<%@ Page Title="Kategorien" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<CategoriesModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/Views/Categories/Categories.css") %>
    <%= Scripts.Render("~/bundles/Categories") %>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div id="category-main">
       
        <% Html.Message(Model.Message); %>
        
        <% using (Html.BeginForm()) { %>
        
            <div class="boxtainer-outlined-tabs">
                <div class="boxtainer-header">
                    <ul class="nav nav-tabs">
                        <li class="active"><a href="#home" >Alle Kategorien (<%= Model.TotalCategories %>)</a></li>
                        <li>
                            <a href="#profile">
                                Mein Wunschwissen <span id="tabWishKnowledgeCount">(<%= Model.TotalMine %>)</span> <i class="fa fa-question-circle" id="tabInfoMyKnowledge"></i>
                            </a>
                        </li>
                    </ul>
                    <div class="" style="float: right; position: absolute; right: 0; top: 5px;">
                        <a href="<%= Url.Action("Create", "EditCategory") %>" class="btn btn-success btn-sm">
                            <i class="fa fa-plus-circle"></i>  Kategorie erstellen
                        </a>
                    </div>
                </div>
                <div class="boxtainer-content">
                    <div class="search-section">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="pull-left form-group">
                                    <% if(!String.IsNullOrEmpty(Model.Suggestion)){ %> 
                                        <div style="padding-bottom: 10px; font-size: large">
                                            Oder suchst du: 
                                            <a href="<%= "/Kategorien/Suche/" + Model.Suggestion %>">
                                                <%= Model.Suggestion %>
                                            </a> ?
                                        </div>
                                    <% } %>

                                    <div class="input-group">
                                        <%: Html.TextBoxFor(model => model.SearchTerm, new {@class="form-control", id="txtSearch"}) %>
                                        <span class="input-group-btn">
                                            <button class="btn btn-default" id="btnSearch"><i class="fa fa-search"></i></button>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
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
                            </div>
                        </div>
                        
                        <div style="clear: both;">
                            <% foreach (var row in Model.CategoryRows){
                                Html.RenderPartial("CategoryRow", row);
                            } %>
                        </div>

                    </div>


                    <% Html.RenderPartial("Pager", Model.Pager); %>
                </div>
            </div>
            
        <% } %>
    </div>
    
    <% Html.RenderPartial("Modals/DeleteCategory"); %>

</asp:Content>
