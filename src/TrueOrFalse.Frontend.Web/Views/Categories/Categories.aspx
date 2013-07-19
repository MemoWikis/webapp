<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<CategoriesModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Scripts.Render("~/Views/Categories/Categories.js") %>
    <%= Styles.Render("~/Views/Categories/Categories.css") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="span10">
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
                    <div class="form-horizontal">
                        <div class="control-group" style="margin-bottom: 15px; margin-top: -7px; ">
                            <label style="line-height: 18px; padding-top: 5px;"><nb>Suche</nb>:</label>
                            <%: Html.TextBoxFor(model => model.SearchTerm, new {style="width:297px;", id="txtSearch"}) %>
                            <a class="btn" style="height: 18px;" id="btnSearch"><img alt="" src="/Images/Buttons/tick.png" style="height: 18px;"/></a>
                        </div>
                        <div style="clear:both;"></div>
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
