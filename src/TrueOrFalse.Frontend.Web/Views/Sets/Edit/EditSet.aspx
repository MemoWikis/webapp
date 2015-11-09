<%@ Page Title="Fragesatz erstellen" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<EditSetModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <title><%=Model.PageTitle %></title>
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
    
    <style>
       
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
<style type="text/css">

</style>

<div id="questionSetContainer" data-id="<%: Model.Id %>">
    
    <% using (Html.BeginForm(Model.IsEditing ? "Edit" : "Create", 
                "EditSet", null, FormMethod.Post,
                new { enctype = "multipart/form-data", style = "margin:0px;"})){ %>
    
            
        <%: Html.HiddenFor(m => m.ImageIsNew) %>
        <%: Html.HiddenFor(m => m.ImageSource) %>
        <%: Html.HiddenFor(m => m.ImageWikiFileName) %>
        <%: Html.HiddenFor(m => m.ImageGuid) %>
        <%: Html.HiddenFor(m => m.ImageLicenseOwner) %>
    
    
        <div class="row">
            <div class="PageHeader col-xs-12">
                <h2 class="pull-left">
                    <span class="ColoredUnderline Set">
                    <% if (Model.IsEditing) { %>
                        Fragesatz bearbeiten
                    <% } else { %>
                        Fragesatz erstellen
                    <% } %>
                    </span>
                </h2>
                 <div class="headerControls pull-right">
                    <div>
                        <a href="<%= Links.Sets() %>" style="font-size: 12px; margin: 0;">
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
                    <% if(!Model.IsLoggedIn){ %>
                        <div class="bs-callout bs-callout-danger" style="margin-top: 0;">
                            <h4>Anmelden oder registrieren</h4>
                            <p>
                                Um Fragesätze zu erstellen, <br/>
                                musst du dich <a href="/Anmelden">anmelden</a> oder <a href="/Registrieren">registrieren</a>.
                            </p>
                        </div>
                    <% }%>
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
                <div class="form-horizontal">
                    <div class="FormSection">
                        <div class="form-group">
                            <%= Html.LabelFor(m => m.Title, new { @class = "RequiredField control-label columnLabel" })%>
                                <div class="columnControlsFull">
                                    <%= Html.TextBoxFor(m => m.Title, new { @class="form-control" }) %>
                                </div>
                        </div>
                        <div class="form-group">
                            <%= Html.LabelFor(m => m.Text, new { @class = "control-label columnLabel" })%>
                            <div class="columnControlsFull">
                                <%= Html.TextAreaFor(m => m.Text, new { style = "height:50px;", @class="form-control" }) %>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="columnLabel control-label">
                                <span class="show-tooltip" title = "Kategorien helfen bei der Einordnung des Fragesatzes und ermöglichen dir und anderen, Fragesätze wiederzufinden." data-placement = "left">
                                    Kategorien
                                </span>
                            </label>        
                                
                            <div class="JS-RelatedCategories columnControlsFull">
                                <script type="text/javascript">
                                    $(function () {
                                        <%foreach (var category in Model.Categories) { %>
                                        $("#txtNewRelatedCategory")
                                            .val('<%=category.Name %>')
                                            .data('category-id', '<%=category.Id %>')
                                            .trigger("initCategoryFromTxt");
                                        <% } %>
                                    });
                                </script>
                                <div class="JS-CatInputContainer ControlInline">
                                    <input id="txtNewRelatedCategory" class="form-control .JS-ValidationIgnore" type="text" placeholder="Wähle eine Kategorie"  />
                                </div>
                            </div>                                
                        </div>
                        <div class="form-group" style="margin-top: -15px;">
                            <div class="noLabel columnControlsFull">
                                <p class="form-control-static"><span class="RequiredField"></span> Pflichtfeld</p>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="noLabel noControls">
                            
                                <%  if(Model.QuestionsInSet.Count == 0) { %>
                                    <div class="info">
                                        <b>Keine Fragen im Fragesatz.</b> Um Fragen hinzuzufügen, wähle 
                                        <% if(!Model.IsEditing) {  %> nach dem Erstellen <% } %>
                                         Fragen auf der <%= Html.ActionLink("Fragen-Übersichtsseite", "Questions", "Questions", null, new { target = "_blank" }) %> aus. 
                                    </div>
                                <% }else{ %>
                                    <h4 style="padding-left:5px;">Fragen Reihenfolge
                                        <span style="font-size: 11px;">(per Drag'n'Drop)</span>
                                        <span id="revertAction" class="pull-right hide2" style="font-size: 11px; font-weight: normal; position: relative; top: 7px; right: 7px; cursor: pointer">
                                            [Rückgängig]
                                        </span>
                                    </h4>
                                    <ul id="ulQuestions">
                                        <%foreach(var questionInSet in Model.QuestionsInSet){%>
                                            <li class="questionItem ui-state-default Clearfix" data-id="<%=questionInSet.Id %>">
                                                <div class="QuestionTools">
                                                    <i class="fa fa-trash-o icon DeleteButton JS-DeleteButton show-tooltip" title="Aus dem Fragesatz entfernen"></i><br/>
                                                    <% if (Model.IsOwner(questionInSet.Question.Creator.Id)){%>
                                                        <a href="<%= Links.EditQuestion(Url, questionInSet.Question.Id) %>">
                                                            <i class="fa fa-pencil"></i> 
                                                        </a>
                                                    <% } %>
                                                </div>

                                                <div class="draggable-panel" style="float: left;">&nbsp;</div>
                                                <div class="QuestionText">
                                                    <%= questionInSet.Question.Text %>
                                                </div>
                                            
                                            </li>  
                                        <%} %>
                                    </ul>
                                <% } %>
                        </div>
                        </div>
                    </div>
                    <div class="FormSection">
                    
                    <div class="form-group">
                        <div class="noLabel columnControlsFull">
                            <% if (Model.IsEditing){ %>
                                <input type="submit" value="Speichern" class="btn btn-primary" name="btnSave" />
                            <% } else { %>
                                <input type="submit" value="Fragesatz erstellen" class="btn btn-primary" name="btnSave" <% if(!Model.IsLoggedIn){ %> disabled="disabled" <% } %>/>
                            <% } %>
                        </div>
                    </div>
                </div>
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