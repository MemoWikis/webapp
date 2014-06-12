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
    FormMethod.Post, new { enctype = "multipart/form-data" })){%>
    <div class="row">
        <div class="col-md-9 pageHeader">
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
                    <a href="<%= Url.Action(Links.Categories, Links.CategoriesController) %>" style="font-size: 12px; margin: 0px;">
                        <i class="fa fa-list"></i>&nbsp;zur Übersicht
                    </a><br/>
                    <a href="<%= Links.CategoryDetail(Url, Model.Category) %>" style="font-size: 12px;">
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
            
            
                <div class="FormSection">
                    <div class="form-group">
                        <label class="columnLabel control-label">
                            Kategorietyp
                        </label>
                        <div class="columnControlsFull">
                             <%if(!Model.IsEditing){ %>

                                <div class="radio">
                                    <label style="font-weight: normal">
                                        <input type="radio" name="rdoCategoryTypeGroup" value="standard" checked>
                                        Standard
                                        <i class="fa fa-question-circle show-tooltip" title="Für alle normalen Themenkategorien" data-placement="right"></i>
                                    </label>
                                </div>
                                <div class="radio">
                                    <label style="font-weight: normal">
                                        <input type="radio" name="rdoCategoryTypeGroup" value="media">
                                        Medien
                                        <i class="fa fa-question-circle show-tooltip" title="Kategorietyp für Fragen, die sich auf ein bestimmtes Buch, einen Zeitungsartikel usw. beziehen und für Quellenangaben in Fragen." data-placement="right"></i>
                                        <br/><span style="font-weight: normal;">(Bücher, Zeitungsartikel, Online-Beiträge, Videos etc.)</span>
                                        <select class="form-control" id="ddlCategoryTypeMedia" name="ddlCategoryTypeMedia" style="margin-top: 5px; display: none;">
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
                                        <input type="radio" name="rdoCategoryTypeGroup" value="education">
                                        Aus- und Weiterbildung
                                        <br/><span style="font-weight: normal;">(Studiengänge, Schulfächer, Klassenstufen etc.)</span>
                                        <select class="form-control" id="ddlCategoryTypeEducation" name="ddlCategoryTypeEducation" style="margin-top: 5px; display: none;">
                                            <option value="SchoolSubject"><%= CategoryType.SchoolSubject.GetName() %></option>
                                            <option value="FieldOfStudy"><%= CategoryType.FieldOfStudy.GetName() %></option>
                                            <option value="FieldStudyTrade"><%= CategoryType.FieldStudyTrade.GetName() %></option>
                                            <option value="Course"><%= CategoryType.Course.GetName() %></option>
                                            <option value="Certification"><%= CategoryType.Certification.GetName() %></option>
                                        </select>
                                    </label>
                                </div>
                             <% }else{ %>
                                <p class="form-control-static">
                                    <%= Model.Category.Type.GetName() %>
                                </p>
                            <% } %>
                        </div>
                    </div>
                </div>
                 <!-- temporariliy included partial:-->
           <%--  <%Html.RenderPartial("~/Views/Categories/Edit/TypeControls/VolumeChapter.ascx", new EditCategoryTypeModel(Model.Category));%>    --%>
                <div class="FormSection">
                    <div id="CategoryDetailsBody">
                        <h4 class="CategoryTypeHeader">Formular wird geladen...</h4>
                       
                        
                    </div>
                </div>
                <div class="FormSection">
                    <div class="form-group">
                        <label class="columnLabel control-label">
                            Übergeordnete Kategorie(n)
                            <i class="fa fa-question-circle show-tooltip" title="Hilft, Kategorien in Beziehung zueinander zu setzen. Beispiele: Kategorie Wirbeltiere - übergeordnet: Biologie. Kategorie Algebra - übergeordnet: Mathematik" data-placement="right" data-trigger="hover click"></i>

                        </label>

                        <div id="relatedCategories" class="columnControlsFull">
                            <script type="text/javascript">
                                $(function () {
                                    <%foreach (var category in Model.ParentCategories) { %>
                                        $("#txtNewRelatedCategory").val('<%=category %>');
                                        $("#txtNewRelatedCategory").trigger("initCategoryFromTxt");
                                    <% } %>
                                });
                            </script>
                            <div class="CatInputContainer"><input id="txtNewRelatedCategory" class="form-control" type="text" placeholder="Wähle eine Kategorie" /></div>
                        </div>
                    </div>
                </div>
                <div class="FormSection">
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
               <%-- <% if (Model.IsEditing){ %>
                                <input type="submit" value="Speichern" class="btn btn-primary" name="btnSave" />
                                <a href="<%=Url.Action("Delete", "Categories") %>" class="btn btn-danger"><i class="fa fa-trash-o"></i> Löschen</a>
                            <% } else { %>
                                <input type="submit" value="Erstellen" class="btn btn-primary" name="btnSave" />
                            <% } %>--%>
                
                <%--<div class="FormSection" style="background-color: grey;">
                    <%if(!Model.IsEditing){ %>
                
                    <div class="form-group">
                        <label class="columnLabel control-label"></label>
                        <div class="columnControlsFull">
                            <select class="form-control" id="ddlCategoryTypex" name="ddlCategoryType">
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
                            <label class="columnLabel control-label">Typ</label>
                            <div class="columnControlsFull">
                                <p class="form-control-static">
                                    <%= Model.Category.Type.GetName() %>
                                </p>
                            </div>
                        </div>                    
                    <% } %>
                    <div class="form-group">
                        <%= Html.LabelFor(m => m.Name, new {@class="columnLabel control-label"} ) %>
                        <div class="columnControlsFull">
                            <%= Html.TextBoxFor(m => m.Name, new {@class="form-control"} ) %>
                        </div>
                    </div>

                    <div class="form-group">
                        <%= Html.LabelFor(m => m.Description, new {@class="columnLabel control-label"} ) %>
                        <div class="columnControlsFull">
                            <%= Html.TextAreaFor(m => m.Description, new {@class="form-control"} ) %>
                            <% %>
                        </div>
                    </div>

                </div>--%>
            </div>
        </div>
    </div>
    
    

<% } %>
    
<% Html.RenderPartial("../Shared/ImageUpload/ImageUpload"); %>

</asp:Content>