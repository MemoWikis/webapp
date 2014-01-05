<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<EditCategoryModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <script src="/Views/Categories/Edit/RelatedCategories.js" type="text/javascript"></script>
    <%= Styles.Render("~/bundles/category") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="col-md-9 category">
        <h2><% if (Model.IsEditing) { %>
                Kategorie bearbeiten
            <% } else { %>
                Kategorie erstellen
            <% } %>
        </h2>

        <div class="form-horizontal" style="margin-top: 30px;">
            <% using (Html.BeginForm(Model.IsEditing ? "Edit" : "Create", "EditCategory", null, 
                    FormMethod.Post, new { enctype = "multipart/form-data" })){%>

                <% Html.Message(Model.Message); %>
    
                <div class="form-group">
                    <%= Html.LabelFor(m => m.Name, new {@class="col-sm-2 control-label"} ) %>
                    <div class="col-xs-3">
                        <%= Html.TextBoxFor(m => m.Name, new {@class="form-control"} ) %>    
                    </div>
                </div>
    
                <div class="form-group">
                    <label for="file" class="col-sm-2 control-label">Bild:</label>
                
                    <div class="col-sm-10">    
                        <img alt="" src="<%= Model.ImageUrl %>" />
                        <input type="file" name="file" id="file" />
                    </div>

                    
                </div>
        
                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        Übergeordnete Kategorien: (Beispielsweise: Person > Politker, Bundesland > Bundeshauptstad, Kanzler > Minister.)
                    </div>
                </div>

                <div class="form-group">
                    <label class="col-sm-2 control-label">In Beziehung zu:</label>

                    <div id="relatedCategories" class="col-sm-10">
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
                    <div class="col-sm-offset-2 col-sm-10">
                        <input type="submit" value="Speichern" class="btn btn-primary" name="btnSave" />
                        <input type="submit" value="Speichern & Neu" class="btn btn-default" name="btnSave btn" />&nbsp;&nbsp;&nbsp;
                        <a href="<%=Url.Action("Delete", "Categories") %>" class="btn btn-danger"><i class="fa fa-trash-o"></i> Löschen</a>
                    </div>
                </div>

            </div>
    </div>

<% } %>

</asp:Content>

