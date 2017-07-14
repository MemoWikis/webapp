<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<EditSetModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="TrueOrFalse.Web" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <% Title = Model.PageTitle; %>
    <% if (Model.IsEditing) { %>
        <link rel="canonical" href="<%= Settings.CanonicalHost %><%= Links.QuestionSetEdit(Model.Set.Name, Model.Set.Id) %>">
    <% } else {  %>
        <link rel="canonical" href="<%= Settings.CanonicalHost %><%= Links.SetCreate() %>">
    <% } %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Sets/Edit/EditSet.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/fileUploader") %>
    <%= Scripts.Render("~/bundles/SetEdit") %>
    <script type="text/javascript">
        var isEditMode = <%= Model.IsEditing ? "true" : "false" %>;
        var questionSetId = "<%= Model.Id %>";
        var userName = "<%= Model.Username %>";

        $(function() {
            $('.control-label .show-tooltip').append($("<i class='fa fa-info-circle'></i>"));
        });
    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript" src="/Views/Sets/Edit/Questions.js"></script>
  

    
<style type="text/css">

</style>

<div id="questionSetContainer" data-id="<%: Model.Id %>">
    
    <% using (Html.BeginForm(Model.IsEditing ? "Edit" : "Create",
           "EditSet", null, FormMethod.Post,
           new {id = "EditSetForm", enctype = "multipart/form-data", style = "margin:0px;"}))
       { %>
        
        <%: Html.HiddenFor(m => m.ImageIsNew) %>
        <%: Html.HiddenFor(m => m.ImageSource) %>
        <%: Html.HiddenFor(m => m.ImageWikiFileName) %>
        <%: Html.HiddenFor(m => m.ImageGuid) %>
        <%: Html.HiddenFor(m => m.ImageLicenseOwner) %>

        <div class="row">
            <div class="PageHeader col-xs-12">
                <h2 class="pull-left">
                    <span class="ColoredUnderline Set">
                    <% if (Model.IsEditing)
                       { %>
                        Lernset bearbeiten
                    <% }
                       else
                       { %>
                        Lernset erstellen
                    <% } %>
                    </span>
                </h2>
                 <div class="headerControls pull-right">
                    <div>
                        <a href="<%= Links.SetsAll() %>" style="font-size: 12px; margin: 0;">
                            <i class="fa fa-list"></i>&nbsp;zur Übersicht
                        </a><br/>
                        <% if (Model.Set != null)
                           { %>
                            <a href="<%= Links.SetDetail(Url, Model.Set) %>" style="font-size: 12px;">
                                <i class="fa fa-eye"></i>&nbsp;Detailansicht
                            </a> 
                        <% } %>            
                    </div>
                    </div>
                 <div class="PageHeader col-xs-12">
                    <% if (!Model.IsLoggedIn)
                       { %>
                        <div class="bs-callout bs-callout-danger" style="margin-top: 0;">
                            <h4>Einloggen oder registrieren</h4>
                            <p>
                                Um Lernsets zu erstellen, <br/>
                                musst du dich <a href="#" data-btn-login="true">einloggen</a> oder <a href="/Registrieren">registrieren</a>.
                            </p>
                        </div>
                    <% } %>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-9 xxs-stack">
                <% Html.Message(Model.Message); %>
            </div>
        </div>
        <div class="row">
            <div class="aside col-md-3 col-md-push-9">
                <img id="questionSetImg" src="<%= Model.ImageUrl_206px %>" class="img-responsive" style="border-radius:5px;" />
                <div style="margin-top: 10px;">
                    <a href="#" style="position: relative; top: -6px; font-size: 90%;" id="aImageUpload">[Verwende ein anderes Bild]</a>
                </div>
            </div>
            <div class="col-md-9 col-md-pull-3">
                <div class="form-horizontal rowBase">
                    <div class="FormSection">
                        <div class="form-group">
                            <%= LabelExtensions.LabelFor(Html, m => m.Title, new {@class = "RequiredField control-label columnLabel"}) %>
                            <div class="columnControlsFull">
                                <%= Html.TextBoxFor(m => m.Title, new {@class = "form-control"}) %>
                            </div>
                        </div>
                        <div class="form-group">
                            <%= LabelExtensions.LabelFor(Html, m => m.Text, new {@class = "control-label columnLabel"}) %>
                            <div class="columnControlsFull">
                                <%= Html.TextAreaFor(m => m.Text, new {@class = "form-control"}) %>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <label class="columnLabel control-label">
                                <span class="show-tooltip" title="Die Url aus der Adresszeile des Browser, zum Beispiel: https://www.youtube.com/watch?v=iQ6NxvQRfq4">
                                    Youtube Url (optional)
                                </span>
                            </label>
                            <div class="columnControlsFull">
                                <%= Html.TextBoxFor(m => m.VideoUrl, new {@class = "form-control"}) %>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="columnLabel control-label">
                                <span class="show-tooltip" title="Themen helfen bei der Einordnung des Lernsets und ermöglichen dir und anderen, Lernsets wiederzufinden.">
                                    Themen
                                </span>
                            </label>        
                                
                            <div class="JS-RelatedCategories columnControlsFull">
                                <script type="text/javascript">
                                    $(function () {
                                        <% foreach (var category in Model.Categories)
                                           { %>
                                        $("#txtNewRelatedCategory")
                                            .val('<%= category.Name %>')
                                            .data('category-id', '<%= category.Id %>')
                                            .trigger("initCategoryFromTxt");
                                        <% } %>
                                    });
                                </script>
                                <div class="JS-CatInputContainer ControlInline">
                                    <input id="txtNewRelatedCategory" class="form-control .JS-ValidationIgnore" type="text" placeholder="Wähle ein Thema"  />
                                </div>
                            </div>                                
                        </div>
                        <div class="form-group" style="margin-top: -15px;">
                            <div class="noLabel columnControlsFull">
                                <p class="form-control-static"><span class="RequiredField"></span> Pflichtfeld</p>
                            </div>
                        </div>
                        
                        <div class="FormSection">
                            <div class="form-group">
                                <div class="noLabel columnControlsFull <%= Model.IsEditing ? "separationBorderTop" : "" %>">
                                    <% if (Model.IsEditing) { %>
                                        <input type="submit"  value="Speichern" class="btn btn-primary" name="btnSave" id="btnSave"  />
                                    <% } else { %>
                                        <input type="submit" value="Lernset erstellen" class="btn btn-primary" 
                                               name="btnSave" <% if (!Model.IsLoggedIn) { %> disabled="disabled" <% } %>/>
                                    <% } %>
                                </div>
                            </div>
                        </div>


                        <% if (Model.IsEditing) { %>
                            <div class="form-group">
                                <div class="noLabel noControls">
                            
                                    <h4 style="margin-top: 0px;">Reihenfolge der Fragen
                                        <span style="font-size: 11px;">(per Drag'n'Drop)</span>
                                        <span id="revertAction" class="pull-right hide2" style="font-size: 11px; font-weight: normal; position: relative; top: 7px; right: 7px; cursor: pointer">
                                            [Rückgängig]
                                        </span>
                                    </h4>

                                    <% if (Model.QuestionsInSet.Count == 0) { %>

                                        <div class="alert alert-info" style="margin-top: 15px; margin-bottom: 5px;">
                                            <p>
                                                <b>Dein Lernset enthält noch keine Fragen.</b>
                                                Du kannst <a href="<%= Links.CreateQuestion(setId: Model.Id) %>" target="_blank">eine neue Frage erstellen</a> für dieses Lernset.
                                                Oder du fügst vorhandene Fragen hinzu, indem du sie auf der 
                                                <%= Html.ActionLink("Fragen-Übersichtsseite", "Questions", "Questions", null, new {target = "_blank"}) %> auswählst und 
                                                auf "Zum Lernset hinzufügen" klickst.
                                            </p>
                                        </div>
                                    
                                    <% } else { %>

                                        <ul id="ulQuestions">
                                            <% foreach (var questionInSet in Model.QuestionsInSet)
                                               { %>
                                                <li class="questionItem ui-state-default Clearfix" data-id="<%= questionInSet.Id %>">
                                                    <div class="QuestionTools">
                                                        <i class="fa fa-trash-o icon DeleteButton JS-DeleteButton show-tooltip" title="Aus dem Lernset entfernen (die Frage wird nicht gelöscht)"></i><br/>
                                                        <% if (Model.IsOwner(questionInSet.Question.Creator.Id))
                                                           { %>
                                                            <a href="<%= Links.EditQuestion(Url, questionInSet.Question.Text, questionInSet.Question.Id) %>">
                                                                <i class="fa fa-pencil"></i> 
                                                            </a>
                                                        <% } %>
                                                    </div>

                                                    <div class="draggable-panel" style="float: left;"><i class="fa fa-bars" aria-hidden="true"></i></div>
                                                    <div class="QuestionText">
                                                        <a href="#" data-action="open-details"><i class="fa fa-chevron-right"></i></a>
                                                        <a href="#" data-action="close-details" class="hide2"><i class="fa fa-chevron-down"></i></a>
                                                        <%= questionInSet.Question.Text %>
                                                        <%= MarkdownInit.Run().Transform(questionInSet.Question.TextExtended) %>
                                                        <div>
                                                            Richtige Antwort: <b><%= questionInSet.Question.GetSolution().GetCorrectAnswerAsHtml() %></b>
                                                        </div>
                                                    </div>
                                                    <div style="display: inline-block; float: right; width: 65px;">
                                                        <input type="text" class="form-control show-tooltip" value="<%= Timecode.ToString(questionInSet.Timecode) %>"
                                                            data-in-set-id="<%= questionInSet.Id %>"
                                                            data-input="video-timecode"
                                                            data-original-title="Falls du oben ein Video angegeben hast: Zeitpunkt zu dem das Video pausiert und die Frage gezeigt wird." />
                                                    </div>
                                                </li>  
                                            <% } %>
                                        </ul>
                                    <% } %>
                                </div>
                            </div>
                        <% } %>
                        
                        <div class="row">
                            <div class="col-md-12">
                                <h3>Füge vorhanden Fragen zum Lernset hinzu</h3>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12" id="questionSearch">
                                <input id="questionId" type="text" class="form-control" placeholder="Tippe um Fragen zu finden"/>
                            </div>
                        </div>    
                        
                        <div class="row">
                            <div class="col-md-12">
                                <h5>Ergebnis</h5>
                            </div>                            
                        </div>
                        
                        <div id="questions" class=""></div>
                        
                        <div class="row">
                            <div class="col-md-12">
                                <button id="learnSetSave" class="btn btn-primary" type="submit"><i class="fa fa-plus"></i> zum Lernset hnzufügen</button>
                            </div>
                            <div id="responseForm" class="col-md-12">
                                <p id="response"></p>
                            </div>
                        </div>
                        
                        <% if (Model.IsEditing){ %>
                            <div class="row">
                                <div class="col-xs-12" style="padding-bottom: 10px" id="createNewQuestionInLearnSet">
                                
                                    <a href="<%= Links.CreateQuestion(setId: Model.Id) %>" class="btn btn-primary" target="_blank" id="btnCreateNewQuestionInLearnSet">
                                        <i class="fa fa-plus"></i> Neue Frage im Lernset erstellen
                                    </a>

                                </div>
                            </div>
                        <% } %>

                    </div>     
                                        
                    <% if (!Model.IsEditing) { %>
                        <div class="FormSection">
                            <div class="form-group">
                                <div class="noLabel columnControlsFull separationBorderTop">
                                    <div class="info">
                                        <b>Fragen hinzufügen:</b><br/>
                                        Nach dem Erstellen des Lernsets kannst du auf der 
                                        <%= Html.ActionLink("Fragen-Übersichtsseite", "Questions", "Questions", null, new {target = "_blank"}) %> vorhandene Fragen auswählen 
                                        und deinem Lernset hinzufügen. Du kannst auch neue Fragen für dein Lernset erstellen.
                                    </div>
                                </div>
                            </div>
                        </div>
                    <% } %>

                </div>
            
            </div>
        </div>
    
    <% } %>
</div>

<% Html.RenderPartial("~/Views/Images/ImageUpload/ImageUpload.ascx"); %>
    
<div id="modalRevertAction" class="modal">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h3>Letztes Löschen rückgängig machen</h3>
            </div>
            <div class="modal-body">
                <p>NOCH NICHT UMGESETZT</p>
            </div>
            <div class="modal-footer" id="tqsNoSetsFooter">
                <a href="#" class="btn btn-default" data-dismiss="modal">Schließen</a>
                <a href="#" class="btn btn-primary">Jetzt rückgängig machen</a>
            </div>
        </div>
    </div>
</div>

</asp:Content>