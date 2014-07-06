<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<EditCategoryModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Register Src="~/Views/Categories/Edit/TypeControls/Book.ascx" TagPrefix="uc1" TagName="Book" %>


<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <script src="/Views/Categories/Edit/RelatedCategories.js" type="text/javascript"></script>
    <link href="/Views/Categories/Edit/EditCategory.css" rel="stylesheet" />
    <%= Styles.Render("~/bundles/category") %>
    <%= Scripts.Render("~/bundles/fileUploader") %>
    <%= Scripts.Render("~/bundles/CategoryEdit") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
<% using (Html.BeginForm(Model.IsEditing ? "Edit" : "Create", "EditCategory", null, 
    FormMethod.Post, new { enctype = "multipart/form-data", @id="EditCategoryForm" })){%>
    <div class="row">
        <div class="col-md-9 PageHeader">
            <h2>
                <span class="ColoredBottomBorder Category">
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
                    <a href="<%= Url.Action(Links.Categories, Links.CategoriesController) %>" style="font-size: 12px; margin: 0;">
                        <i class="fa fa-list"></i>&nbsp;zur Übersicht
                    </a><br/>
                    <a href="<%= Links.CategoryDetail(Model.Category) %>" style="font-size: 12px;">
                        <i class="fa fa-eye"></i>&nbsp;Detailansicht
                    </a> 
                <% } %>            
            </div>
        </div>
    </div>
    <div class="row">
        <div class="aside col-md-3 col-md-push-9">
            <img id="categoryImg" src="<%= Model.ImageUrl %>" class="img-responsive" style="border-radius:5px;" />
            <div style="margin-top: 10px;">
                <a href="#" style="position: relative; top: -6px;" id="aImageUpload">[Verwende ein anderes Bild]</a>
            </div>
        </div>
        <div class="col-md-9 col-md-pull-3">
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
            
                <%if(!Model.IsEditing){ %>
                <div class="FormSection">
                    <div class="form-group">
                        <label class="columnLabel control-label">
                            Kategorietyp
                        </label>
                        <div class="columnControlsFull">
                            <div class="radio">
                                <label style="font-weight: normal">
                                    <input type="radio" name="rdoCategoryTypeGroup" value="standard" <%= Model.rdoCategoryTypeGroup == "standard" ? "checked" : "" %>>
                                    Standard
                                    <i class="fa fa-question-circle show-tooltip" title="Für alle normalen Themenkategorien" data-placement="<%= CssJs.TooltipPlacementFormField %>"></i>
                                </label>
                            </div>
                            <div class="radio">
                                <label style="font-weight: normal">
                                    <input type="radio" name="rdoCategoryTypeGroup" value="media" <%= Model.rdoCategoryTypeGroup == "media" ? "checked" : "" %>>
                                    Medien
                                    <i class="fa fa-question-circle show-tooltip" title="Kategorietyp für Fragen, die sich auf ein bestimmtes Buch, einen Zeitungsartikel usw. beziehen und für Quellenangaben in Fragen." data-placement="<%= CssJs.TooltipPlacementFormField %>"></i>
                                    <br/><span style="font-weight: normal;">(Bücher, Zeitungsartikel, Online-Beiträge, Videos etc.)</span>
                                    <select class="form-control" id="ddlCategoryTypeMedia" name="ddlCategoryTypeMedia" style="margin-top: 5px; display: none;" data-selectedValue="<%= Model.ddlCategoryTypeMedia %>" >
                                        <optgroup label="Druckmedien">
                                            <option value="Book"><%= CategoryType.Book.GetName() %></option>
                                            <option value="VolumeChapter"><%= CategoryType.VolumeChapter.GetName() %></option>
                                            <option value="Daily"><%= CategoryType.Daily.GetName() %></option>
                                            <option value="DailyIssue"><%= CategoryType.DailyIssue.GetName() %></option>
                                            <option value="DailyArticle"><%= CategoryType.DailyArticle.GetName() %></option>
                                            <option value="Magazine"><%= CategoryType.Magazine.GetName() %></option>
                                            <option value="MagazineIssue"><%= CategoryType.MagazineIssue.GetName() %></option>
                                            <option value="MagazineArticle"><%= CategoryType.MagazineArticle.GetName() %></option>
                                        </optgroup>
                                        <optgroup label="Internet">
                                            <option value="Website"><%= CategoryType.Website.GetName() %></option>
                                            <option value="WebsiteArticle"><%= CategoryType.WebsiteArticle.GetName() %></option>
                                            <option value="WebsiteVideo"><%= CategoryType.WebsiteVideo.GetName() %></option>
                                        </optgroup>
                                        <optgroup label="Film und Fernsehen">
                                            <option value="Movie"><%= CategoryType.Movie.GetName() %></option>
                                            <option value="TvShow"><%= CategoryType.TvShow.GetName() %></option>
                                            <option value="TvShowEpisode"><%= CategoryType.TvShowEpisode.GetName() %></option>
                                        </optgroup>
                                    </select>
                                </label>
                            </div>
                            <div class="radio">
                                <label style="font-weight: normal">
                                    <input type="radio" name="rdoCategoryTypeGroup" value="education" <%= Model.rdoCategoryTypeGroup == "education" ? "checked" : "" %>>
                                    Aus- und Weiterbildung
                                    <br/><span style="font-weight: normal;">(Studiengänge, Schulfächer, Klassenstufen etc.)</span>
                                    <select class="form-control" id="ddlCategoryTypeEducation" name="ddlCategoryTypeEducation" style="margin-top: 5px; display: none;" data-selectedValue="<%= Model.ddlCategoryTypeEducation %>">
                                        <option value="SchoolSubject"><%= CategoryType.SchoolSubject.GetName() %></option>
                                        <option value="FieldOfStudy"><%= CategoryType.FieldOfStudy.GetName() %></option>
                                        <option value="FieldStudyTrade"><%= CategoryType.FieldStudyTrade.GetName() %></option>
                                        <option value="Course"><%= CategoryType.Course.GetName() %></option>
                                        <option value="Certification"><%= CategoryType.Certification.GetName() %></option>
                                    </select>
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
                <% } %>
<%--temporariliy included partial:--%>
           <%--  <%Html.RenderPartial("~/Views/Categories/Edit/TypeControls/VolumeChapter.ascx", new EditCategoryTypeModel(Model.Category));%>    --%>
                <div class="FormSection">
                    <div id="CategoryDetailsBody">
                        <h4 class="CategoryTypeHeader">Formular wird geladen...</h4>                    
                    </div>
                </div>
                <div class="FormSection JS-ShowWithPartial" style="display: none;">
                    <div class="form-group">
                        <label class="columnLabel control-label">
                            Übergeordnete Kategorie(n)
                            <i class="fa fa-question-circle show-tooltip" title="Hilft, Kategorien in Beziehung zueinander zu setzen. Beispiele: Kategorie Wirbeltiere - übergeordnet: Biologie. Kategorie Algebra - übergeordnet: Mathematik" data-placement="<%= CssJs.TooltipPlacementLabel %>" data-trigger="hover click"></i>
                        </label>
                        <div class="JS-RelatedCategories columnControlsFull">
                            <script type="text/javascript">
                                $(function () {
                                    <%foreach (var category in Model.ParentCategories) { %>
                                        $("#txtNewRelatedCategory").val('<%=category %>');
                                        $("#txtNewRelatedCategory").trigger("initCategoryFromTxt");
                                    <% } %>
                                });
                            </script>
                            <div class="JS-CatInputContainer ControlInline"><input id="txtNewRelatedCategory" class="form-control .JS-ValidationIgnore" type="text" placeholder="Wähle eine Kategorie"  /></div>
                        </div>
                    </div>
                </div>
                <div class="FormSection JS-ShowWithPartial" style="display: none;">
                    <div class="form-group">
                        <div class="noLabel columnControlsFull">
                            <p class="form-control-static"><span class="RequiredField"></span> Pflichtfeld</p>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="noLabel columnControlsFull">
                            <% if (Model.IsEditing){ %>
                                <input type="submit" value="Speichern" class="btn btn-primary" name="btnSave" />
                                <a href="<%=Url.Action("Delete", "Categories") %>" class="btn btn-danger"><i class="fa fa-trash-o"></i> Löschen</a>
                            <% } else { %>
                                <input type="submit" value="Kategorie erstellen" class="btn btn-primary" name="btnSave" />
                            <% } %>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

<% } %>
    
<% Html.RenderPartial("../Shared/ImageUpload/ImageUpload"); %>

</asp:Content>