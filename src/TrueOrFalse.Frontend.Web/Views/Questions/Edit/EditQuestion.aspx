<%@ Page Title="Frage erstellen" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master"
    Inherits="ViewPage<EditQuestionModel>" %>

<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="TrueOrFalse" %>
<asp:Content runat="server" ID="head" ContentPlaceHolderID="Head">
    <script src="/Views/Categories/Edit/RelatedCategories.js" type="text/javascript"></script>
    <script src="/Views/Questions/Edit/EditQuestion.js" type="text/javascript"></script>
    <link href="/Views/Questions/Edit/EditQuestion.css" rel="stylesheet" />
    <style type="text/css">
        div.classification input
        {
            width: 75px;
            background-color: beige;
        }
    </style>
    <script type="text/javascript" src="/Scripts/jquery.jplayer.min.js"></script>
    <link type="text/css" href="/Content/blue.monday/jplayer.blue.monday.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
    

   
    
<div class="span10">
    
    <% using (Html.BeginForm(Model.IsEditing ? "Edit" : "Create", "EditQuestion", null, FormMethod.Post, new { enctype = "multipart/form-data", style="margin:0px;" })){ %>
    

        <div class="form-horizontal" style="padding-top: 0px;">
            <div class="box box-main">
                <h1 class="pull-left"><%=Model.PageTitle %></h1>
            
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
            <div style="margin-top: -5px; padding-left: 14px; margin-right: -15px;">
                <% Html.Message(Model.Message); %>
            </div>
            <div class="control-group">
                <%= Html.LabelFor(m => m.Visibility, new { @class = "control-label" })%>
                <div class="controls">
                    <label class="radio inline">
                    <%= Html.RadioButtonFor(m => m.Visibility, QuestionVisibility.All)%>
                    Alle &nbsp;&nbsp;
                    </label>
                    <label class="radio inline">
                    <%= Html.RadioButtonFor(m => m.Visibility, QuestionVisibility.Owner)  %>
                    Nur ich &nbsp;&nbsp;
                    </label>
                    <label class="radio inline" style="width: 175px;">
                    <%= Html.RadioButtonFor(m => m.Visibility, QuestionVisibility.OwnerAndFriends)  %>
                    Ich und meine Freunde
                    </label>
                </div>
            </div>
            <div class="control-group">
                <%= Html.LabelFor(m => m.Question, new { @class = "control-label" })%>
                <div class="controls">
                    <%= Html.TextAreaFor(m => m.Question, new { style = "height:50px; width:435px;", placeholder = "Bitte geben Sie eine Frage ein" })%><br />
                </div>
            </div>
            <p class="help-block help-text">
                Kategorien helfen bei der Einordnung der Frage u. ermöglichen Dir und anderen
                <br />
                die Fragen wiederzufinden.
            </p>
            <div class="control-group">
                <%= Html.Label("Kategorien", new { @class = "control-label" })%>
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
                <% if (!String.IsNullOrEmpty(Model.ImageUrl))
                   {%> <img alt="" src="<%=string.Format(Model.ImageUrl, 128) %>" /> <%} %>
                <label for="imagefile" class="control-label">Bild:</label>
                &nbsp;&nbsp;<input type="file" name="imagefile" id="imagefile" />
            </div>
            <div class="control-group">
                <% if (!String.IsNullOrEmpty(Model.SoundUrl)){
                       Html.RenderPartial("AudioPlayer", Model.SoundUrl); } %>
                <label for="soundfile" class="control-label">Ton:</label>
                &nbsp;&nbsp;<input type="file" name="soundfile" id="soundfile" />
            </div>
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
            <p class="help-block help-text">
                Je ausführlicher die Erklärung, desto besser!<br />
                Verwende Links u. Bilder aber achte auf die Urheberrechte.
            </p>
            <div class="control-group">
                <%= Html.LabelFor(m => m.Description, new { @class = "control-label" })%>
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
