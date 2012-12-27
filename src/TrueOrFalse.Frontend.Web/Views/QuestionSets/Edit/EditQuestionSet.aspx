<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<EditQuestionSetModel>" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="TrueOrFalse.Web" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/QuestionSets/QuestionSets.css" rel="stylesheet" />
    <script src="/Views/QuestionSets/QuestionSets.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    

<div class="row">
    
    <h4 style="border-bottom: 1px solid lavender; margin-bottom: 20px;">Fragensatz erstellen</h4>

    <% using (Html.BeginForm(Model.IsEditing ? "Edit" : "Create", 
                "EditQuestionSet", null, FormMethod.Post,
                new { enctype = "multipart/form-data", style = "margin:0px;", @class = "form-horizontal" })){ %>
    
        <div style="margin-top: -5px; padding-left: 14px; margin-right: -15px;">
            <% Html.Message(Model.Message); %>
        </div>    

        <div class="control-group">
            <%= Html.LabelFor(m => m.Title, new { @class = "control-label" })%>
            <div class="controls">
                <%= Html.TextBoxFor(m => m.Title, new { placeholder = "Titel" }) %>
            </div>
        </div>
        <div class="control-group">
            <%= Html.LabelFor(m => m.Text, new { @class = "control-label" })%>
            <div class="controls">
                <%= Html.TextAreaFor(m => m.Text, new { style = "height:50px; width:435px;", placeholder = "Beschreibung" }) %>
            </div>
        </div>    
    
        <div style="margin-left: 180px;">
            Um Fragen hinzuzufügen, wählen Sie Fragen auf der <%= Html.ActionLink("Fragen-Übersichtsseite", "Questions", "Questions") %> aus. 
        </div>
    
    
        <div class="form-actions">
            <input type="submit" class="btn btn-primary" value="Speichern" />
            <input type="button" class="btn" value="Cancel">
        </div>
    
    <% } %>
</div>

</asp:Content>