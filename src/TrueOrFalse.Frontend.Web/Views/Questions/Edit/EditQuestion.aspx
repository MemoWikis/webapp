<%@ Page Title="Frage erstellen" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master"
    Inherits="ViewPage<EditQuestionModel>" %>

<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="TrueOrFalse" %>
<asp:Content runat="server" ID="head" ContentPlaceHolderID="Head">
    <script src="/Views/Categories/Edit/RelatedCategories.js" type="text/javascript"></script>
    <link href="/Views/Questions/Edit/EditQuestion.css" rel="stylesheet" />
    <script type="text/javascript" src="/Scripts/jquery.jplayer.min.js"></script>
    <link type="text/css" href="/Content/blue.monday/jplayer.blue.monday.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/markdown") %>
    <%= Scripts.Render("~/bundles/questionEdit") %>
    <%= Scripts.Render("~/bundles/fileUploader") %>
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
    
<input type="hidden" id="questionId" value="<%= Model.Id %>"/>
    
<div class="todo-temp">
</div>

<div class="col-md-9">
    
<div class="row">

<div class="col-md-9">
    <div>
        <% Html.Message(Model.Message); %>
    </div>

    <% using (Html.BeginForm(Model.IsEditing ? "Edit" : "Create", "EditQuestion", null, FormMethod.Post, new { enctype = "multipart/form-data", style="margin:0px;" })){ %>

        <div class="form-horizontal" role="form">
            <div class="box box-main">
                <h1 class="pull-left"><%=Model.FormTitle %></h1>
            
                <div class="pull-right">
                    <div>
                        <a href="<%= Url.Action(Links.Questions, Links.QuestionsController) %>" style="font-size: 12px;
                            margin: 0px;"><i class="fa fa-list"></i> zur Übersicht</a>
                    </div>
                    <% if (!Model.ShowSaveAndNewButton){ %>
                        <div style="line-height: 12px">
                            <a href="<%= Url.Action(Links.CreateQuestion, Links.EditQuestionController) %>" style="font-size: 12px;
                                margin: 0px;"><i class="fa fa-plus-circle"></i> Frage erstellen</a>
                        </div>
                    <%} %>
                    
                    <% if(Model.IsEditing){ %>
                        <div style="line-height: 12px; padding-top: 3px;">
                            <a href="<%= Links.AnswerQuestion(Url, Model.Question, (int)Model.Id) %>" style="font-size: 12px;
                                margin: 0px;"><i class="fa fa-check-square"></i> Frage beantworten</a>
                        </div>                    
                    <% } %>
                </div>
                <div class="box-content" style="clear: both;">
                    <div class="form-group">
                        <%= Html.LabelFor(m => m.Visibility, new { @class = "col-sm-2 control-label" })%>
                        <div class="controls">
                            <label class="radio inline">
                            <%= Html.RadioButtonFor(m => m.Visibility, QuestionVisibility.All)%>
                            für alle<br/><span class="smaller">(öffentliche Frage)</span> &nbsp;&nbsp;
                            </label>
                            <label class="radio inline">
                            <%= Html.RadioButtonFor(m => m.Visibility, QuestionVisibility.Owner)  %>
                            für mich<br/><span class="smaller">(private Frage <i class="fa fa-trophy"></i>)</span> &nbsp;&nbsp;
                            </label>
                            <label class="radio inline" style="width: 175px;">
                            <%= Html.RadioButtonFor(m => m.Visibility, QuestionVisibility.OwnerAndFriends)  %>
                            für mich und meine Freunde<br/><span class="smaller">(private Frage <i class="fa fa-trophy"></i>)</span>
                            </label>
                        </div>
                    </div>
                    
                    <div class="form-group">
                        <%= Html.LabelFor(m => m.Question, new { @class = "col-sm-2 control-label" })%>
                        <div class="col-sm-10">
                                <%= Html.TextAreaFor(m => m.Question, new { @class="form-control", style = "height:50px; width:435px;", placeholder = "Bitte gib den Fragetext ein" })%><br />
                            <div style="padding-top: 4px;">
                                <a href="#" id="openExtendedQuestion"><i class="fa fa-plus-circle" style="color: blue;"></i> Erweiterte Beschreibung (z.B.: mit Bildern, Formeln oder Quelltext)</a> 
                            </div>    
                        </div>
                    </div>
                    
                    <div class="form-group markdown" style="display: none" id="extendedQuestion">
                        <%= Html.LabelFor(m => m.QuestionExtended, new { @class = "col-sm-2 control-label" })%>
                        <div class="col-sm-10">
                            <div class="wmd-panel">
                                <div id="wmd-button-bar-1"></div>
                                <%= Html.TextAreaFor(m => m.QuestionExtended, new 
                                    { @class= "wmd-input form-control", id="wmd-input-1", style = "height:150px; width:435px;", placeholder = "Erweiterte Beschreibung" })%><br />
                            </div>                            
                            <div id="wmd-preview-1" class="wmd-panel wmd-preview" style="width:435px;"></div>
                        </div>
                    </div>

                    <%--
                    <div class="form-group">
                        <% if (!String.IsNullOrEmpty(Model.SoundUrl)){
                               Html.RenderPartial("AudioPlayer", Model.SoundUrl); } %>
                        <label for="soundfile" class="control-label">Ton:</label>
                        &nbsp;&nbsp;<input type="file" name="soundfile" id="soundfile" />
                    </div>--%>

                    <div class="form-group">
                        <%= Html.LabelFor(m => m.SolutionType, new { @class = "col-sm-2 control-label" }) %>
                        <div class="col-sm-3">
                            <%= Html.DropDownListFor(m => Model.SolutionType, Model.AnswerTypeData, new {@id = "ddlAnswerType", @class="form-control"})%>
                        </div>
                    </div>
                    <div id="question-body">
                    </div>
                    <script type="text/javascript">
                        function updateSolutionBody() {
                            var selectedValue = $("#ddlAnswerType").val();
                            $.ajax({
                                url: '<%=Url.Action("SolutionEditBody", "EditQuestion") %>?questionId=<%:Model.Id %>&type=' + selectedValue,
                                type: 'GET',
                                beforeSend: function () { /* some loading indicator */ },
                                success: function (data) { $("#question-body").html(data); },
                                error: function (data) { /* handle error */ }
                            });
                        }
                        $("#ddlAnswerType").change(updateSolutionBody);
                        updateSolutionBody();
                    </script>
                    <%--<% Html.RenderPartial("~/Views/Questions/Edit/EditAnswerControls/AnswerTypeAccurate.ascx", Model); %>--%>
                    
                    
                    <div class="form-group">    
                        <label class="col-sm-2 control-label show-tooltip" title = "Kategorien helfen bei der Einordnung der Frage u. ermöglichen Dir und anderen die Fragen wiederzufinden." data-placement = "left">
                            Kategorien
                        </label>

                        <div id="relatedCategories" class="col-sm-10">
                            <script type="text/javascript">
                                $(function () {
                                    <%foreach (var category in Model.Categories) { %>
                                        $("#txtNewRelatedCategory").val('<%=category %>');
                                        $("#addRelatedCategory").click();
                                    <% } %>
                                });
                            </script>
                            <input id="txtNewRelatedCategory" class="form-control" style="width: 190px;" type="text" placeholder="Wähle eine Kategorie" />
                            <a href="#" id="addRelatedCategory" style="display: none">
                                <img alt="" src='/Images/Buttons/add.png' />
                            </a>
                        </div>
                    </div>
                    
                    <div class="form-group markdown">
                        <label class="col-sm-2 control-label show-tooltip"  title = "Je ausführlicher die Erklärung, desto besser! Verwende Links u. Bilder aber achte auf die Urheberrechte." data-placement = "left">
                            Erklärungen
                        </label>
                        <div class="col-sm-10">
                            <div class="wmd-panel">
                                <div id="wmd-button-bar-2"></div>
                                <%= Html.TextAreaFor(m => m.Description, new 
                                    { @class= "form-control wmd-input", id="wmd-input-2", @style = "height:50px; width:435px;", placeholder = "Erklärung der Antwort und Quellen." })%>
                            </div>
                            <div id="wmd-preview-2" class="wmd-panel wmd-preview" style="width:435px;"></div>
                        </div>
                    </div>
                    
                    <div class="form-group">
                        <label class="col-sm-2 control-label">Quellen</label>
                        <div class="row">
                            <div class="col-xs-3">
                                <input class="form-control col-sm-2" type="text" />
                            </div>
                            <div class="col-xs-3">
                                <select class="form-control ">
                                    <option>Wikipedia</option>
                                    <option>Webseite</option>
                                    <option>Offline: Buch</option>
                                    <option>Offline: Zeitung/Zeitschrift</option>
                                </select>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <button type="submit" class="btn btn-primary" name="btnSave" value="ssdfasdfave">Speichern</button>&nbsp;&nbsp;&nbsp;
                        <% if (Model.ShowSaveAndNewButton){ %>
                            <button type="submit" class="btn btn-default" name="btnSave" value="saveAndNew" >Speichern & Neu</button>&nbsp;
                        <% } %>                        
                    </div>
                </div>
            </div>
        </div>
    </div>
    </div>
    <% } %>
</div>
    
<% Html.RenderPartial("../Shared/ImageUpload/ImageUpload"); %>

</asp:Content>