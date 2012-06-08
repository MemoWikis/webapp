<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<EditCategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
<script src="/Views/Categories/Edit/RelatedCategories.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<h2 class="form-title"><% if (Model.IsEditing) { %>
                            Kategorie bearbeiten
                       <% } else { %>
                            Kategorie erstellen
                       <% } %>
</h2>


<% using (Html.BeginForm("Edit", "EditCategory", null,FormMethod.Post,new { enctype = "multipart/form-data" }))
   { %>

    <% Html.Message(Model.Message); %>

    <%= Html.LabelFor(m => m.Name ) %>
    <%= Html.TextBoxFor(m => m.Name ) %>

    <h3>Steht in enger Beziehung zu den Kategorien (ausgehend)</h3>
    <div id="relatedCategories">
        <script type="text/javascript">
            $(function() {
                <%foreach (var category in Model.RelatedCategories) { %>
                    $("#txtNewRelatedCategory").val('<%=category %>');
                    $("#addRelatedCategory").click();
                <% } %>
            });
        </script>
        <input id="txtNewRelatedCategory" />
        <a href="#" id="addRelatedCategory" style="display:none"><img alt="" src='/Images/Buttons/add.png' /></a>
    </div>
    <img alt="" src="<%=string.Format(Model.ImageUrl, 128) %>" />
    <label for="imageUpload">Bild:</label>
    <input type="file" accept="image/*" name="file" id="file" />
    <br/><br/><br/>
    <label>&nbsp;</label>
    <%= Buttons.Submit("Speichern", inline:true)%>
    <%= Buttons.Submit("Speichern & Neu", inline: true)%>
    <%= Buttons.Link("Löschen", inline: true, actionName: Links.DeleteCategory, controllerName: Links.CategoriesController, buttonIcon: ButtonIcon.Delete)%>

<% } %>

</asp:Content>

