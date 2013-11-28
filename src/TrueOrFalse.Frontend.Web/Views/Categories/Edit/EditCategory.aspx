<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<EditCategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
<script src="/Views/Categories/Edit/RelatedCategories.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="col-md-10">
        <div class="box box-main">
            <h1 class="form-title"><% if (Model.IsEditing) { %>
                                        Kategorie bearbeiten
                                   <% } else { %>
                                        Kategorie erstellen
                                   <% } %>
            </h1>

            <div class="form-horizontal">
                <div class="box-content">
                <% using (Html.BeginForm(Model.IsEditing ? "Edit" : "Create", "EditCategory", null, 
                       FormMethod.Post, new { enctype = "multipart/form-data" })){%>

                        <% Html.Message(Model.Message); %>
    
                        <div class="control-group">
                            <%= Html.LabelFor(m => m.Name, new {@class="control-label"} ) %>
                            <div class="controls">
                                <%= Html.TextBoxFor(m => m.Name ) %>    
                            </div>
        
                        </div>
    
                        <div class="control-group">
                            <img alt="" src="<%= Model.ImageUrl %>" />
                            <label for="file" class="control-label">Bild:</label>
                            <input type="file" name="file" id="file" />
                        </div>
        
                        <p class="help-block help-text">
                            Übergeordnete Kategorien: (Beispielsweise: Person > Politker, Bundesland > Bundeshauptstad, Kanzler > Minister.)
                        </p>

                        <div id="relatedCategories" class="control-group">
                            <label class="control-label">In Beziehung zu:</label>
                            <div class="controls">
                                <script type="text/javascript">
                                    $(function () {
                                        <%foreach (var category in Model.RelatedCategories) { %>
                                            $("#txtNewRelatedCategory").val('<%=category %>');
                                            $("#addRelatedCategory").click();
                                        <% } %>
                                    });
                                </script>
                                <input id="txtNewRelatedCategory" type="text" />
                                <a href="#" id="addRelatedCategory" style="display:none"><img alt="" src='/Images/Buttons/add.png' /></a>
                            </div>
                        </div>
                    </div>
                <div class="form-actions">
                    <input type="submit" value="Speichern" class="btn btn-primary" name="btnSave" />
                    <input type="submit" value="Speichern & Neu" class="btn" name="btnSave btn" />&nbsp;&nbsp;&nbsp;
                    <a href="<%=Url.Action("Delete", "Categories") %>" class="btn btn-danger"><i class="icon-trash"></i> Löschen</a>

                </div>
            
            </div>
        </div>    
    </div>


<% } %>

</asp:Content>

