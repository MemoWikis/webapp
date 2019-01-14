<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="ViewPage<QuestionHistoryDetailModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/QuestionHistoryDetail") %>
    <%= Scripts.Render("~/bundles/js/QuestionHistoryDetail") %>
    <%= Scripts.Render("~/bundles/js/diff2html") %>
    <%= Styles.Render("~/Scripts/vendor/diff2html/diff2html.css") %>
    <%  Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem {Text = Model.CurrentQuestionText.TruncateAtWord(80), Url = Model.QuestionUrl, ToolTipText = Model.CurrentQuestionText});
        Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="row">
        <div class="col-12">
            <h1><i class="fa fa-code-fork"></i> &nbsp; Änderungen für die Frage </h1>
            <h3 style="text-align: center">
                '<%= Model.CurrentQuestionText %>'
            </h3>
            <br />
        </div>
    </div>
    
    <div class="Buttons">
        <div class="row">
            <div class="col-md-3 col-lg-3">            
                <a href="<%= Links.UserDetail(Model.Author) %>"><img src="<%= Model.AuthorImageUrl %>" height="20"/></a>
                <b><a href="<%= Links.UserDetail(Model.Author) %>"><%= Model.AuthorName %></a></b><br/>
                vom <%= Model.CurrentDateCreated %> 
            </div>
            
            <div class="col dropdown" style="float: right">   
                <%--TODO FK <a class="btn btn-primary" href="<%= Links.QuestionDetail(Model.QuestionText, Model.QuestionId, Model.RevisionId) %>">
                    <i class="fa fa-desktop"></i> &nbsp; Anzeige dieser Revision
                </a>--%>
                <a class="btn btn-default" href="<%= Links.QuestionHistory(Model.QuestionId) %>">
                    <i class="fa fa-list-ul"></i> &nbsp; Zur Bearbeitungshistorie
                </a>
                <% var buttonSetId = Guid.NewGuid(); %>
                <a href="#" id="<%= buttonSetId %>" class="dropdown-toggle btn btn-link btn-sm ButtonEllipsis" 
                   type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                    <i class="fa fa-ellipsis-v" style="font-size: 18px; margin-top: 2px;"></i>
                </a>
                <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="<%= buttonSetId %>">
                    <li>
                        <% if (new SessionUser().IsLoggedIn) {
                            if (Model.NextRevExists) { %>
                                <a id="restoreButton" data-allowed="logged-in" onclick="$('#alertConfirmRestore').show();">
                                    <i class="fa fa-undo"></i> &nbsp; Wiederherstellen
                                </a>
                            <% } else { %>
                                <a id="editButton" data-allowed="logged-in" href="<%= Links.EditQuestion(Model.QuestionText, Model.QuestionId) %>">
                                    <i class="fa fa-edit"></i> &nbsp; Thema bearbeiten
                                </a>
                            <% } %>
                        <% } %>
                    </li>
                    <li>
                        <a href="<%= Links.HistoryOfEverything(1) %>">
                            <i class="fa fa-list"></i> &nbsp; Zur Bearbeitungshistorie aller Themen
                        </a>
                    </li>
                </ul>
            </div>
        </div>
        
        <% if (new SessionUser().IsLoggedIn && Model.NextRevExists) { %>
            <div id="alertConfirmRestore" class="row" style="display: none">
                <br/>
                <div class="alert alert-warning" role="alert">
                    <div class="col-12">
                        Der aktuelle Stand wird durch diese Version ersetzt. Wollen Sie das wirklich?
                    </div>
                    <br/>
                    <div class="col-12">
                        <nav>
                            <%--TODO FK <a class="btn btn-default navbar-btn" href="<%= Links.QuestionRestore(Model.QuestionId, Model.RevisionId) %>">
                                <i class="fa fa-undo"></i> Ja, Wiederherstellen
                            </a>--%>
                            <a class="btn btn-primary navbar-btn" onclick="$('#alertConfirmRestore').hide();">
                                <i class="fa fa-remove"></i> Nein, Abbrechen
                            </a>
                        </nav>
                    </div>
                </div>
            </div>
        <% } %>
        
        <div class="row">
            <div class="col-12">
                <% if (!Model.PrevRevExists) {  %>
                    <br />
                    <div class="alert alert-info" role="alert">
                        Dies ist die <b>initiale Revision</b> des Themas, weswegen hier keine Änderungen angezeigt werden können.
                    </div>
                <% } else { %>
                    <% if (!Model.NextRevExists) { %>
                        <br />
                        <div class="alert alert-info" role="alert">
                            Dies ist die <b>aktuelle Revision</b> des Themas.
                        </div>
                    <% } else { %>
                        <br />
                    <% } %>
                    <div id="noChangesAlert" class="alert alert-info" role="alert" style="display: none;">
                        Zwischen den beiden Revisionen (vom <%= Model.PrevDateCreated %> und 
                        vom <%= Model.CurrentDateCreated %>) gibt es <b>keine inhaltlichen Unterschiede</b>.
                    </div>
                    
                    <input type="hidden" id="currentDateCreated" value="<%= Model.CurrentDateCreated %>" />
                    <input type="hidden" id="prevDateCreated" value="<%= Model.PrevDateCreated %>" />

                    <input type="hidden" id="currentQuestionText" value="<%= Server.HtmlEncode(Model.CurrentQuestionText) %>"/>
                    <input type="hidden" id="prevQuestionText" value="<%= Server.HtmlEncode(Model.PrevQuestionText) %>"/>

                    <input type="hidden" id="currentQuestionTextExtended" value="<%= Server.HtmlEncode(Model.CurrentQuestionTextExtended) %>"/>
                    <input type="hidden" id="prevQuestionTextExtended" value="<%= Server.HtmlEncode(Model.PrevQuestionTextExtended) %>"/>

                    <input type="hidden" id="currentLicense" value="<%= Server.HtmlEncode(Model.CurrentLicense) %>"/>
                    <input type="hidden" id="prevLicense" value="<%= Server.HtmlEncode(Model.PrevLicense) %>"/>
                    
                    <input type="hidden" id="currentVisibility" value="<%= Server.HtmlEncode(Model.CurrentVisibility) %>"/>
                    <input type="hidden" id="prevVisibility" value="<%= Server.HtmlEncode(Model.PrevVisibility) %>"/>
                    
                    <input type="hidden" id="currentSolution" value="<%= Server.HtmlEncode(Model.CurrentSolution) %>"/>
                    <input type="hidden" id="prevSolution" value="<%= Server.HtmlEncode(Model.PrevSolution) %>"/>
                    
                    <input type="hidden" id="currentSolutionDescription" value="<%= Server.HtmlEncode(Model.CurrentSolutionDescription) %>"/>
                    <input type="hidden" id="prevSolutionDescription" value="<%= Server.HtmlEncode(Model.PrevSolutionDescription) %>"/>

                    <input type="hidden" id="currentSolutionMetadataJson" value="<%= Server.HtmlEncode(Model.CurrentSolutionMetadataJson) %>"/>
                    <input type="hidden" id="prevSolutionMetadataJson" value="<%= Server.HtmlEncode(Model.PrevSolutionMetadataJson) %>"/>
                    
                    <div id="diffPanel">
                        <div id="diffQuestionText"></div>
                        <div id="diffQuestionTextExtended"></div>
                        <%--TODO FK <%if (Model.CurrentImageWasUpdated) { %>
                            <div class="diffImage">
                                <div id="newImageAlert" class="panel panel-default">
                                    <div class="panel-heading">Änderung des Bildes. Das aktuelle Bild ist:</div>
                                    <div class="panel-body">
                                        <%= Model.CurrentImageFrontendData.RenderHtmlImageBasis(350, false, ImageType.Question, "ImageContainer") %>
                                    </div>
                                </div>
                            </div>
                        <% } %> --%> 
                        <div id="diffLicense"></div>
                        <div id="diffVisibility"></div>
                        <div id="diffSolution"></div>
                        <div id="diffSolutionDescription"></div>
                        <div id="diffSolutionMetadataJson"></div>
                        <div id="noRelationsAlert" class="alert alert-info" role="alert" style="display: none;">
                            Es können <b>keine Beziehungsdaten</b> angezeigt werden, da für die gewählten Revisionen keine enstsprechenden Daten vorliegen.
                        </div>
                    </div>
                <% } %>
            </div>
        </div>
    </div>

</asp:Content>
