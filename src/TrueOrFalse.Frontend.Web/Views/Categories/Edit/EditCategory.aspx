<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<EditCategoryModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <script src="/Views/Categories/Edit/RelatedCategories.js" type="text/javascript"></script>
    <%= Styles.Render("~/bundles/category") %>
    <%= Scripts.Render("~/bundles/fileUploader") %>
    <%= Scripts.Render("~/bundles/CategoryEdit") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    

    <div class="col-md-6  category category" style="margin-bottom: 30px;">
        <h2><% if (Model.IsEditing) { %>
                Kategorie bearbeiten
            <% } else { %>
                Kategorie erstellen
            <% } %>
        </h2>
    </div>
    <div class="col-md-3">
        <div class="pull-right">
            <% if(Model.IsEditing){ %>
                <a href="<%= Url.Action(Links.Categories, Links.CategoriesController) %>" style="font-size: 12px; margin: 0px;">
                    <i class="fa fa-list"></i>&nbsp;zur Übersicht
                </a><br/>
                <a href="<%= Links.CategoryDetail(Url, Model.Category) %>" style="font-size: 12px;">
                    <i class="fa fa-eye"></i>&nbsp;Detailansicht
                </a> 
            <% } %>            
        </div>
    </div>

    <div class="col-md-6">
        <div class="form-horizontal">
            <% using (Html.BeginForm(Model.IsEditing ? "Edit" : "Create", "EditCategory", null, 
                    FormMethod.Post, new { enctype = "multipart/form-data" })){%>
            
                <%: Html.HiddenFor(m => m.ImageIsNew) %>
                <%: Html.HiddenFor(m => m.ImageSource) %>
                <%: Html.HiddenFor(m => m.ImageWikiFileName) %>
                <%: Html.HiddenFor(m => m.ImageGuid) %>
                <%: Html.HiddenFor(m => m.ImageLicenceOwner) %>

                <% Html.Message(Model.Message); %>
    
                <div class="form-group">
                    <%= Html.LabelFor(m => m.Name, new {@class="col-sm-3 control-label"} ) %>
                    <div class="col-xs-4">
                        <%= Html.TextBoxFor(m => m.Name, new {@class="form-control"} ) %>    
                    </div>
                </div>
        
                <div class="form-group">
                    <div class="col-sm-offset-3 col-sm-9">
                        Übergeordnete Kategorien: (Beispielsweise: Person > Politker, Bundesland > Bundeshauptstad, Kanzler > Minister.)
                    </div>
                </div>

                <div class="form-group">
                    <label class="col-sm-3 control-label">In Beziehung zu:</label>

                    <div id="relatedCategories" class="col-sm-9">
                        <script type="text/javascript">
                            $(function () {
                                <%foreach (var category in Model.RelatedCategories) { %>
                                    $("#txtNewRelatedCategory").val('<%=category %>');
                                    $("#addRelatedCategory").click();
                                <% } %>
                            });
                        </script>
                        <input id="txtNewRelatedCategory" type="text" class="form-control" style="width: 190px;" />
                        <a href="#" id="addRelatedCategory" style="display:none">
                            <img alt="" src='/Images/Buttons/add.png' />
                        </a>
                    </div>
                </div>
            
                <div class="form-group" style="margin-top: 30px;">
                    <div class="col-sm-offset-3 col-sm-9">
                        <input type="submit" value="Speichern" class="btn btn-primary" name="btnSave" />
                        <input type="submit" value="Speichern & Neu" class="btn btn-default" name="btnSave btn" />&nbsp;&nbsp;&nbsp;
                        <a href="<%=Url.Action("Delete", "Categories") %>" class="btn btn-danger"><i class="fa fa-trash-o"></i> Löschen</a>
                    </div>
                </div>

            </div>
    </div>
    
    <div class="col-md-3">
        <img id="categoryImg" src="<%= Model.ImageUrl %>" class="img-responsive" style="border-radius:5px;" />
        <div style="margin-top: 10px;">
            <a href="#" style="position: relative; top: -6px;" id="aImageUpload">[Verwende ein anderes Bild]</a>
        </div>
    </div>

<% } %>
    
<% Html.RenderPartial("../Shared/ImageUpload/ImageUpload"); %>

</asp:Content>

