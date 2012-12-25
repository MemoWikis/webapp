<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<EditQuestionSetModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/QuestionSets/QuestionSets.css" rel="stylesheet" />
    <script src="/Views/QuestionSets/QuestionSets.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    
<h4>Fragensatz erstellen</h4>

<div class="row">
    <% using (Html.BeginForm(Model.IsEditing ? "Edit" : "Create", "EditQuestionSet", null, FormMethod.Post, new {enctype = "multipart/form-data", style = "margin:0px;"})){ %>
   
        
         
        
    <% } %>
</div>

</asp:Content>