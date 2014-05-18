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
    
    <div class="col-md-9 pageHeader">
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

    <div class="col-md-9">
        <div class="form-horizontal">
            <% using (Html.BeginForm(Model.IsEditing ? "Edit" : "Create", "EditCategory", null, 
                    FormMethod.Post, new { enctype = "multipart/form-data" })){%>
            
                <%: Html.HiddenFor(m => m.ImageIsNew) %>
                <%: Html.HiddenFor(m => m.ImageSource) %>
                <%: Html.HiddenFor(m => m.ImageWikiFileName) %>
                <%: Html.HiddenFor(m => m.ImageGuid) %>
                <%: Html.HiddenFor(m => m.ImageLicenceOwner) %>
                <input type="hidden" id="isCategoryEdit" value="true"/>
                <input type="hidden" id="categoryId" value="<%= Model.IsEditing ?  Model.Category.Id.ToString() : "" %>"/>

                <% Html.Message(Model.Message); %>
    
                <div class="form-group">
                    <%= Html.LabelFor(m => m.Name, new {@class="col-sm-3 control-label"} ) %>
                    <div class="col-xs-7">
                        <%= Html.TextBoxFor(m => m.Name, new {@class="form-control"} ) %>
                    </div>
                </div>

                <div class="form-group">
                    <%= Html.LabelFor(m => m.Description, new {@class="col-sm-3 control-label"} ) %>
                    <div class="col-xs-9">
                        <%= Html.TextAreaFor(m => m.Description, new {@class="form-control"} ) %>
                        <% %>
                    </div>
                </div>
            
                <div class="form-group">
                    <label class="col-sm-3 control-label">Typ</label>
                    <div class="col-xs-9">
                        <select class="form-control" id="ddlCategoryType">
                            <option value="Standard">Standard</option>
                            <optgroup label="Internet">
                                <option value="Website">Webseite</option>
                                <option value="WebsiteArticle">Webseite -> Artikel/Eintrag/Meldung/..</option>
                                <option value="WebsiteVideo">Youtube</option>
                            </optgroup>
                            <optgroup label="Druckmedien">
                                <option value="Book">Buch (auch EBooks)</option>
                                <option value="PrintDaily">Tageszeitung</option>
                                <option value="PrintDailyIssue">Tageszeitung -> Ausgabe</option>
                                <option value="PrintDailyArticle">Tageszeitung -> Artikel</option>
                                <option value="Magazine">Zeitschrift/Magazin</option>
                                <option value="MagazineIssue">Zeitschrift/Magazin -> Ausgabe</option>
                                <option value="MagazineArticle">Zeitschrift/Magazin -> Artikel</option>
                            </optgroup>
                            <optgroup label="Film und Fernsehen">
                                <option value="Movie">Film</option>
                                <option value="TvShow">Fernsehen</option>
                                <option value="TvShowEpisode">Fernsehen - Episode/Ausgabe</option>
                            </optgroup>
                            <optgroup label="Aus- und Weiterbildung">
                                <option value="FieldOfStudy">Studienfach</option>
                                <option value="SchoolSubject">Schulfach</option>
                                <option value="FieldStudyTrade">Ausbildungsberuf</option>
                                <option value="Course">Kurs/Seminar</option>
                                <option value="Certification">Zertifizierung</option>
                            </optgroup>
                        </select>
                    </div>                    
                </div>

                <div id="details-body"></div>
            
                <div class="form-group">
                    <%= Html.LabelFor(m => m.WikipediaURL, new {@class="col-sm-3 control-label"} ) %>
                    <div class="col-xs-9">
                        <%= Html.TextBoxFor(m => m.WikipediaURL, new {@class="form-control"} ) %>    
                    </div>
                </div>
        
                <div class="form-group">
                    <div class="col-sm-offset-3 col-sm-9">
                        Übergeordnete Kategorien: (Beispielsweise: Person > Politker, Bundesland > Bundeshauptstad, Kanzler > Minister.)
                    </div>
                </div>

                <div class="form-group">
                    <label class="col-sm-3 control-label">Übergeordnete Kategorie:</label>

                    <div id="relatedCategories" class="col-sm-9">
                        <script type="text/javascript">
                            $(function () {
                                <%foreach (var category in Model.ParentCategories) { %>
                                    $("#txtNewRelatedCategory").val('<%=category %>');
                                    $("#txtNewRelatedCategory").trigger("initCategoryFromTxt");
                                <% } %>
                            });
                        </script>
                        <input id="txtNewRelatedCategory" type="text" class="form-control" style="width: 190px;" />
                    </div>
                </div>
            
                <div class="form-group" style="margin-top: 30px;">
                    <div class="col-sm-offset-3 col-sm-9">
                        <% if (Model.IsEditing){ %>
                            <input type="submit" value="Speichern" class="btn btn-primary" name="btnSave" />
                            <a href="<%=Url.Action("Delete", "Categories") %>" class="btn btn-danger"><i class="fa fa-trash-o"></i> Löschen</a>
                        <% } else { %>
                            <input type="submit" value="Erstellen" class="btn btn-primary" name="btnSave" />
                        <% } %>
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

