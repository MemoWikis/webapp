<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="ViewPage<QuestionHistoryModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="FluentNHibernate.Conventions" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/QuestionHistory") %>
    
    <%= Scripts.Render("~/bundles/js/QuestionHistoryDetail") %>
    <%= Scripts.Render("~/bundles/js/diff2html") %>
    <%= Styles.Render("~/Scripts/vendor/diff2html/diff2html.css") %>
    <% 
        Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem
        {
            Text = Model.QuestionText.TruncateAtWord(80),
            Url = Model.QuestionUrl,
            ToolTipText = Model.QuestionText
        });
        Model.TopNavMenu.IsCategoryBreadCrumb = false;
    %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
    <script>
        $(() => {
            <% foreach (var day in Model.Days)
                   foreach (var revModel in day.RevisionModels) { %>
                        ShowQuestionDiff2Html(<%= revModel.RevisionId %>);
            <% } %>
        });
    </script>
    
    <div class="row">
        <div class="col-12">
            <h1><i class="fa fa-list-ul"></i>&nbsp; Bearbeitungshistorie für die Frage</h1>
            <h3 style="text-align: center">
                '<%= Model.QuestionText %>'
            </h3>
            <br />
        </div>
    </div>
    <% foreach (var day in Model.Days) { %>
    
        <div class="row">
            <div class="col-md-12">
                <h3><%= day.Date %></h3>
            </div>
        </div>
        
        <% foreach (var revisionModel in day.RevisionModels)
           {
               var revId = revisionModel.RevisionId;
        %>
            
            <div class="row" style="margin-left: 0px; margin-top: 20px">
                <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                    <div class="panel panel-default" style="color: #203256; background-color: #F6F8FC; border-color: #ddd; padding: 10px">

                        <div class="row panel-heading" role="tab" id="heading_<%= revId %>">
                            <div class="col-xs-3">
                                <a href="<%= Links.UserDetail(revisionModel.Author) %>"><img src="<%= revisionModel.AuthorImageUrl %>" height="20"/></a>
                                <b><a href="<%= Links.UserDetail(revisionModel.Author) %>"><%= revisionModel.AuthorName %></a></b>
                            </div>
                            <div class="col-xs-5 show-tooltip"  data-toggle="tooltip" data-placement="left" title="<%= revisionModel.DateTime %>">
                                vor <%= revisionModel.ElapsedTime %> um <%= revisionModel.Time %>
                            </div>
                            <div class="col-xs-4">
                                    <a class="btn btn-sm btn-primary" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapse_<%= revId %>" aria-expanded="true" aria-controls="collapse_<%= revId %>">
                                        <i class="fa fa-chevron-down"></i> Voransicht
                                    </a>
                                <a class="btn btn-sm btn-default" href="<%= Links.QuestionHistoryDetail(Model.QuestionId, revId) %>">
                                    <i class="fa fa-code-fork"></i> Änderungen anzeigen
                                </a>
                            </div>
                        </div>


                        <div id="collapse_<%= revId %>" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading_<%= revId %>">
                            <div class="panel-body">
                                <% var revisionMessage = "";
                                   if (!revisionModel.NewerRevisionExists)
                                       revisionMessage = "Dies ist die <b>aktuelle Revision</b> der Frage. ";
                                   if (!revisionModel.OlderRevisionExists)
                                   {
                                       revisionMessage += (revisionMessage.IsNotEmpty()) ?
                                           "<br/><br/>Es ist auch die <b>initiale Revision</b> der Frage. " :
                                           "Dies ist die <b>initiale Revision</b> der Frage. ";
                                       revisionMessage += "Deswegen werden im folgenden als Änderungen die Werte dargestellt, mit denen die Frage erstellt wurde.";
                                   }
                                %>
                                <% if (revisionMessage.IsNotEmpty()) { %>
                                    <br />
                                    <div class="alert alert-info" role="alert">
                                        <%= revisionMessage %>
                                    </div>
                                <% } else { %>
                                    <br />
                                <% } %>
                                <div id="noChangesAlert_<%= revId %>" class="alert alert-info" role="alert" style="display: none;">
                                    Zwischen den beiden Revisionen (vom <%= revisionModel.OlderRevision.DateCreated %> und 
                                    vom <%= revisionModel.DateCreated %>) gibt es <b>keine inhaltlichen Unterschiede</b>.
                                </div>

                                <input type="hidden" id="currentQuestionText_<%= revId %>" value="<%= Server.HtmlEncode(revisionModel.QuestionText) %>"/>
                                <input type="hidden" id="prevQuestionText_<%= revId %>" value="<%= Server.HtmlEncode(revisionModel.OlderRevision.QuestionText) %>"/>

                                <input type="hidden" id="currentQuestionTextExtended_<%= revId %>" value="<%= Server.HtmlEncode(revisionModel.QuestionTextExtended) %>"/>
                                <input type="hidden" id="prevQuestionTextExtended_<%= revId %>" value="<%= Server.HtmlEncode(revisionModel.OlderRevision.QuestionTextExtended) %>"/>

                                <input type="hidden" id="currentLicense_<%= revId %>" value="<%= Server.HtmlEncode(revisionModel.License) %>"/>
                                <input type="hidden" id="prevLicense_<%= revId %>" value="<%= Server.HtmlEncode(revisionModel.OlderRevision.License) %>"/>
                                
                                <input type="hidden" id="currentVisibility_<%= revId %>" value="<%= Server.HtmlEncode(revisionModel.Visibility) %>"/>
                                <input type="hidden" id="prevVisibility_<%= revId %>" value="<%= Server.HtmlEncode(revisionModel.OlderRevision.Visibility) %>"/>
                                
                                <input type="hidden" id="currentSolution_<%= revId %>" value="<%= Server.HtmlEncode(revisionModel.Solution) %>"/>
                                <input type="hidden" id="prevSolution_<%= revId %>" value="<%= Server.HtmlEncode(revisionModel.OlderRevision.Solution) %>"/>
                                
                                <input type="hidden" id="currentSolutionDescription_<%= revId %>" value="<%= Server.HtmlEncode(revisionModel.SolutionDescription) %>"/>
                                <input type="hidden" id="prevSolutionDescription_<%= revId %>" value="<%= Server.HtmlEncode(revisionModel.OlderRevision.SolutionDescription) %>"/>

                                <input type="hidden" id="currentSolutionMetadataJson_<%= revId %>" value="<%= Server.HtmlEncode(revisionModel.SolutionMetadataJson) %>"/>
                                <input type="hidden" id="prevSolutionMetadataJson_<%= revId %>" value="<%= Server.HtmlEncode(revisionModel.OlderRevision.SolutionMetadataJson) %>"/>

                                <input type="hidden" id="imageWasChanged_<%= revId %>" value="<%= revisionModel.ImageWasChanged %>"/>
                                
                                <div id="diffPanel_<%= revId %>">
                                    <div id="diffQuestionText_<%= revId %>"></div>
                                    <div id="diffQuestionTextExtended_<%= revId %>"></div>
                                    <%if (revisionModel.ImageWasChanged && revisionModel.ImageFrontendData != null) { %>
                                        <div class="diffImage_<%= revId %>">
                                            <div id="newImageAlert_<%= revId %>" class="panel panel-default">
                                                <div class="panel-heading">Änderung des Bildes. Das aktuelle Bild ist:</div>
                                                <div class="panel-body">
                                                    <%= revisionModel.ImageFrontendData.RenderHtmlImageBasis(350, false, ImageType.Question, "ImageContainer") %>
                                                </div>
                                            </div>
                                        </div>
                                    <% } %>  
                                    <div id="diffLicense_<%= revId %>"></div>
                                    <div id="diffVisibility_<%= revId %>"></div>
                                    <div id="diffSolution_<%= revId %>"></div>
                                    <div id="diffSolutionDescription_<%= revId %>"></div>
                                    <div id="diffSolutionMetadataJson_<%= revId %>"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        <% } %>
    
    <% } %>
</asp:Content>
