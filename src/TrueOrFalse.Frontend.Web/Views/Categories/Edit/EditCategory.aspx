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
    
<% using (Html.BeginForm(Model.IsEditing ? "Edit" : "Create", "EditCategory", null, 
    FormMethod.Post, new { enctype = "multipart/form-data" })){%>
    
    <div class="col-md-9 pageHeader">
        <h2>
            <span class="underlined Category">
                <% if (Model.IsEditing) { %>
                Kategorie bearbeiten
                <% } else { %>
                Kategorie erstellen
                <% } %>

            </span>
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
                        
            <%: Html.HiddenFor(m => m.ImageIsNew) %>
            <%: Html.HiddenFor(m => m.ImageSource) %>
            <%: Html.HiddenFor(m => m.ImageWikiFileName) %>
            <%: Html.HiddenFor(m => m.ImageGuid) %>
            <%: Html.HiddenFor(m => m.ImageLicenceOwner) %>
            <input type="hidden" id="isCategoryEdit" value="true"/>
            <input type="hidden" id="isEditing" value="<%= Model.IsEditing ?  "true" : "false" %>"/>
            <input type="hidden" id="categoryId" value="<%= Model.IsEditing ?  Model.Category.Id.ToString() : "" %>"/>
            <input type="hidden" id="categoryType" value="<%= Model.IsEditing ? Model.Category.Type.ToString() : "" %>"/>

            <% Html.Message(Model.Message); %>
            
            <div class="form-group">
                <label class="col-sm-3 control-label">
                    Kategorietyp
                </label>
                <div class="col-xs-9">

                    <div class="radio">
                        <label style="font-weight: normal">
                            <input type="radio" name="optionsRadios" value="option1" checked>
                            Standard
                        </label>
                    </div>
                    <div class="radio">
                        <label style="font-weight: normal">
                            <input type="radio" name="optionsRadios" value="option1">
                            Medien (Bücher, Zeitungsartikel, Videos etc.)
                        </label>
                    </div>
                    <div class="radio">
                        <label style="font-weight: normal">
                            <input type="radio" name="optionsRadios" value="option1">
                            Aus- und Weiterbildung (Studiengänge, Schulfächer, Klassenstufen etc.)
                        </label>
                    </div>
                </div>
            </div>

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
            
            <%if(!Model.IsEditing){ %>
                
                <div class="form-group">
                    <label class="col-sm-3 control-label">Typ</label>
                    <div class="col-xs-9">
                        <select class="form-control" id="ddlCategoryType" name="ddlCategoryType">
                            <option value="Standard"><%= CategoryType.Standard.GetName() %></option>
                            <optgroup label="Internet">
                                <option value="Website"><%= CategoryType.Website.GetName() %></option>
                                <option value="WebsiteArticle"><%= CategoryType.WebsiteArticle.GetName() %></option>
                                <option value="WebsiteVideo"><%= CategoryType.WebsiteVideo.GetName() %></option>
                            </optgroup>
                            <optgroup label="Druckmedien">
                                <option value="Book"><%= CategoryType.Book.GetName() %></option>
                                <option value="Daily"><%= CategoryType.Daily.GetName() %></option>
                                <option value="DailyIssue"><%= CategoryType.DailyIssue.GetName() %></option>
                                <option value="DailyArticle"><%= CategoryType.DailyArticle.GetName() %></option>
                                <option value="Magazine"><%= CategoryType.Magazine.GetName() %></option>
                                <option value="MagazineIssue"><%= CategoryType.MagazineIssue.GetName() %></option>
                                <option value="MagazineArticle"><%= CategoryType.MagazineArticle.GetName() %></option>
                            </optgroup>
                            <optgroup label="Film und Fernsehen">
                                <option value="Movie"><%= CategoryType.Movie.GetName() %></option>
                                <option value="TvShow"><%= CategoryType.TvShow.GetName() %></option>
                                <option value="TvShowEpisode"><%= CategoryType.TvShowEpisode.GetName() %></option>
                            </optgroup>
                            <optgroup label="Aus- und Weiterbildung">
                                <option value="FieldOfStudy"><%= CategoryType.FieldOfStudy.GetName() %></option>
                                <option value="SchoolSubject"><%= CategoryType.SchoolSubject.GetName() %></option>
                                <option value="FieldStudyTrade"><%= CategoryType.FieldStudyTrade.GetName() %></option>
                                <option value="Course"><%= CategoryType.Course.GetName() %></option>
                                <option value="Certification"><%= CategoryType.Certification.GetName() %></option>
                            </optgroup>
                        </select>
                    </div>
                </div>
            <% }else{ %>
                <div class="form-group">
                    <label class="col-sm-3 control-label">Typ</label>
                    <div class="col-xs-9">
                        <p class="form-control-static">
                            <%= Model.Category.Type.GetName() %>
                        </p>
                    </div>
                </div>                    
            <% } %>

            <div id="details-body"></div>
                    
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
                    <div id="CatInputContainer"><input id="txtNewRelatedCategory" class="form-control" type="text" placeholder="Wähle eine Kategorie" /></div>
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