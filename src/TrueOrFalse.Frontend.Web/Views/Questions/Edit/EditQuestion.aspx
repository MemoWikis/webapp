﻿<%@ Page Title="Frage erstellen" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master"
    Inherits="ViewPage<EditQuestionModel>" %>

<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="TrueOrFalse" %>
<asp:Content runat="server" ID="head" ContentPlaceHolderID="Head">
    <script src="/Views/Categories/Edit/RelatedCategories.js" type="text/javascript"></script>
    <script src="/Views/Questions/Edit/EditQuestion.js" type="text/javascript"></script>
    <link href="/Views/Questions/Edit/EditQuestion.css" rel="stylesheet" />
    <script type="text/javascript" src="/Scripts/jquery.jplayer.min.js"></script>
    <link type="text/css" href="/Content/blue.monday/jplayer.blue.monday.css" rel="stylesheet" />
    <%= Styles.Render("~/bundles/markdownCss") %>
    <%= Scripts.Render("~/bundles/markdown") %>
    
    <script type="text/javascript">
        $(function () {
            var converter1 = Markdown.getSanitizingConverter();

            converter1.hooks.chain("preBlockGamut", function (text, rbg) {
                return text.replace(/^ {0,3}""" *\n((?:.*?\n)+?) {0,3}""" *$/gm, function (whole, inner) {
                    return "<blockquote>" + rbg(inner) + "</blockquote>\n";
                });
            });

            var editor1 = new Markdown.Editor(converter1);

            editor1.run();
        })();
    </script>
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
    
<div class="todo-temp"></div>

<div class="span10">
    
    <div>
        <% Html.Message(Model.Message); %>
    </div>

    <% using (Html.BeginForm(Model.IsEditing ? "Edit" : "Create", "EditQuestion", null, FormMethod.Post, new { enctype = "multipart/form-data", style="margin:0px;" })){ %>
        <div class="form-horizontal">
            <div class="box box-main">
                <h1 class="pull-left"><%=Model.FormTitle %></h1>
            
                <div class="pull-right">
                    <div>
                        <a href="<%= Url.Action(Links.Questions, Links.QuestionsController) %>" style="font-size: 12px;
                            margin: 0px;"><i class="icon-list"></i> zur Übersicht</a>
                    </div>
                    <% if (!Model.ShowSaveAndNewButton){ %>
                        <div style="line-height: 12px">
                            <a href="<%= Url.Action(Links.CreateQuestion, Links.EditQuestionController) %>" style="font-size: 12px;
                                margin: 0px;"><i class="icon-plus-sign"></i>Frage erstellen</a>
                        </div>
                    <%} %>
                </div>
                <div class="box-content" style="clear: both;">
                    <div class="control-group">
                        <%= Html.LabelFor(m => m.Visibility, new { @class = "control-label" })%>
                        <div class="controls">
                            <label class="radio inline">
                            <%= Html.RadioButtonFor(m => m.Visibility, QuestionVisibility.All)%>
                            für alle<br/>(öffentliche Frage) &nbsp;&nbsp;
                            </label>
                            <label class="radio inline">
                            <%= Html.RadioButtonFor(m => m.Visibility, QuestionVisibility.Owner)  %>
                            für mich<br/>(private Frage <i class="icon-trophy"></i>) &nbsp;&nbsp;
                            </label>
                            <label class="radio inline" style="width: 175px;">
                            <%= Html.RadioButtonFor(m => m.Visibility, QuestionVisibility.OwnerAndFriends)  %>
                            für mich und meine Freunde (private Frage <i class="icon-trophy"></i>)
                            </label>
                        </div>
                    </div>
                    
                    <div class="control-group">
                        <%= Html.LabelFor(m => m.Question, new { @class = "control-label" })%>
                        <div class="controls">
                            <div class="wmd-panel">
                                <div id="Div1"></div>
                                <%= Html.TextAreaFor(m => m.Question, new { style = "height:50px; width:435px;", placeholder = "Bitte gib den Fragetext ein" })%><br />
                            </div>
                            <div><i class="icon-plus-sign" style="color: blue;"></i> <a href="#">Erweiterte Beschreibung (z.B.: mit Bildern, Formeln oder Quelltext)</a> </div>    
                        </div>
                        
                    </div>
                    
                    

                    <div class="control-group" style="display: none">
                        <%= Html.LabelFor(m => m.QuestionExtended, new { @class = "control-label" })%>
                        <div class="controls">
                            <div class="wmd-panel">
                                <div id="wmd-button-bar"></div>
                                <%= Html.TextAreaFor(m => m.QuestionExtended, new 
                                    { @class= "wmd-input", id="wmd-input", style = "height:150px; width:435px;", placeholder = "Erweiterte Beschreibung" })%><br />
                            </div>
                            
                            <div id="wmd-preview" class="wmd-panel wmd-preview" style="width:435px;"></div>

                        </div>
                    </div>

<%--                    <div class="control-group">
                        <% if (!String.IsNullOrEmpty(Model.ImageUrl_128))
                           {%> <img alt="" src="<%= Model.ImageUrl_128 %>" /> <%} %>
                        <label for="imagefile" class="control-label">Bild:</label>
                        &nbsp;&nbsp;<input type="file" name="imagefile" id="imagefile" />
                    </div>
                    <div class="control-group">
                        <% if (!String.IsNullOrEmpty(Model.SoundUrl)){
                               Html.RenderPartial("AudioPlayer", Model.SoundUrl); } %>
                        <label for="soundfile" class="control-label">Ton:</label>
                        &nbsp;&nbsp;<input type="file" name="soundfile" id="soundfile" />
                    </div>--%>
                    <div class="control-group">
                        <%= Html.LabelFor(m => m.SolutionType, new { @class = "control-label" }) %>
                        <div class="controls">
                            <%= Html.DropDownListFor(m => Model.SolutionType, Model.AnswerTypeData, new {@id = "ddlAnswerType"})%>
                        </div>
                    </div>
                    <div id="question-body">
                    </div>
                    <script type="text/javascript">
                        function updateSolutionBody() {
                            var selectedValue = $("#ddlAnswerType").val();
                            $.ajax({
                                url: '<%=Url.Action("SolutionEditBody") %>?questionId=<%:Model.Id %>&type=' + selectedValue,
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
                    
                    <div class="control-group">
                        
                        <div class ="control-label"><span class="show-tooltip" title = "Kategorien helfen bei der Einordnung der Frage u. ermöglichen Dir und anderen die Fragen wiederzufinden." data-placement = "left">Kategorien</span></div> 

                        <div id="relatedCategories" class="controls">
                            <script type="text/javascript">
                                $(function () {
                                    <%foreach (var category in Model.Categories) { %>
                                    $("#txtNewRelatedCategory").val('<%=category %>');
                                    $("#addRelatedCategory").click();
                                    <% } %>
                                });
                            </script>
                            <input id="txtNewRelatedCategory" />
                            <a href="#" id="addRelatedCategory" style="display: none">
                                <img alt="" src='/Images/Buttons/add.png' />
                            </a>
                        </div>
                    </div>
                    
                    <div class="control-group">
                        <div class="control-label"><span class="show-tooltip" title = "Je ausführlicher die Erklärung, desto besser! Verwende Links u. Bilder aber achte auf die Urheberrechte." data-placement = "left">Erklärungen</span></div>
                        <div class="controls">
                            <%= Html.TextAreaFor(m => m.Description, new { @style = "height:50px; width:435px;", placeholder = "Erklärung der Antwort und Quellen." })%>
                        </div>
                    </div>
                </div>
                <div class="form-actions">
                    <button type="submit" class="btn btn-primary" name="btnSave" value="save">Speichern</button>&nbsp;&nbsp;&nbsp;
                    <% if (Model.ShowSaveAndNewButton){ %>
                        <button type="submit" class="btn" name="btnSave" value="saveAndNew" >Speichern & Neu</button>&nbsp;
                    <% } %>
                </div>
            </div>
        </div>
    <% } %>
</div>
</asp:Content>
