<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<CategoriesModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/Views/Categories/Categories.css") %>
    <%= Scripts.Render("~/bundles/Categories") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="span10">
        
        <div style="margin-bottom: -10px;">
            <% Html.Message(Model.Message); %>
        </div>

        <% using (Html.BeginForm()) { %>
        
            <div style="float: right;">
                <a href="<%= Url.Action("Create", "EditCategory") %>" style="width: 140px" class="btn">
                    <i class="icon-plus-sign"></i>  Kategorie erstellen
                </a>
            </div>
        
            <div class="box-with-tabs">
                <div class="green">
                    <ul class="nav nav-tabs">
                        <li class="active"><a href="#home" >Alle Kategorien (<%= Model.TotalCategories %>)</a></li>
                        <li>
                            <a href="#profile">
                                Meine Kategorien <span id="tabWishKnowledgeCount">(<%= Model.TotalMine %>)</span> <i class="icon-question-sign" id="tabInfoMyKnowledge"></i>
                            </a>
                        </li>
                    </ul>
                </div>
                
                <div class="box box-green">
                    
                    <div class="">
                        <ul class="nav pull-right" style="padding-left: 5px; margin-top: -1px; margin-right: -3px;">
                            <li class="dropdown" id="menu1">
                                <a class="dropdown-toggle btn btn-mini" data-toggle="dropdown" href="#menu1">
                                    Sortieren nach: <%= Model.OrderByLabel %>
                                    <b class="caret"></b>
                                </a>
                                <ul class="dropdown-menu">
                                    <li><a href="#">Anzahl Fragen</a></li>
                                    <li><a href="#">Empfehlungen</a></li>
                                </ul>
                            </li>
                        </ul>
        
                        <div class="pull-right" style="font-size: 14px; margin-top: 0px; margin-right: 7px;"><%= Model.TotalCategoriesInResult %> Treffer</div>
                        
                        <div class="pull-left control-group" style="margin-top: -7px; ">
                            <label style="line-height: 18px; padding-top: 5px;"><nb>Suche</nb>:</label>
                            <%: Html.TextBoxFor(model => model.SearchTerm, new {style="width:297px;", id="txtSearch"}) %>
                            <a class="btn" style="height: 18px; position: relative; top: -5px;" id="btnSearch"><img alt="" src="/Images/Buttons/tick.png" style="height: 18px;"/></a>
                        </div>
                    </div>

                    <div class="box-content" style="clear: both;">
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
