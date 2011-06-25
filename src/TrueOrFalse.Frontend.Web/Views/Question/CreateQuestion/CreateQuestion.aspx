<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CreateQuestionModel>" %>

<%@ Import Namespace="TrueOrFalse.Core" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Models" %>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        fieldset.entry label
        {
            width: 130px;
            display:block;
            float:left;
            text-align:right;
            padding-right:10px;
            padding-top:4px;
        }
        
        fieldset.entry textarea
        {
            width: 320px;    
            height: 80px;
        }
        
    </style>

	<h2>Frage erstellen</h2>
	<% using (Html.BeginForm())
	{ %>
	
	<h2><%= Html.Encode(ViewData["Msg"]) %></h2>
	<fieldset class="entry">
		<legend>Neue Frage erstellen</legend>

        <%= Html.LabelFor(m => m.Visibility) %>
        <%= Html.RadioButtonFor(m => m.Visibility, QuestionVisibility.All )%> Alle &nbsp;&nbsp;
        <%= Html.RadioButtonFor(m => m.Visibility, "2")  %> Alle &nbsp;&nbsp;
        <%= Html.RadioButtonFor(m => m.Visibility, "3")  %><br /><br />
		<label for="Fragetyp">Fragetyp</label>
		<select id="Fragetyp"><option>Freitext</option></select><br />
		<%= Html.LabelFor(m => m.Question)%>
		<%= Html.TextAreaFor(m => m.Question, new { @style = "height:30px;" })%><br />
		<%= Html.LabelFor(m => m.Answer ) %>
		<%= Html.TextAreaFor( m=>m.Answer  ) %><br />
		<%= Html.LabelFor(m => m.Description ) %>
		<%= Html.TextAreaFor( m=>m.Description ) %><br />
		<%= Html.LabelFor(m => m.CategoryMain ) %>
		<%= Html.TextBoxFor(m => m.CategoryMain, new { @style = "width:220px;" })%> <br />
		<%= Html.LabelFor(m => m.CategorySub ) %>
		<%= Html.TextBoxFor(m => m.CategorySub, new { @style = "width:220px;" })%> <br />
		<%= Html.LabelFor(m => m.EducationLink ) %>
		<%= Html.DropDownListFor(m => Model.EducationLink, Model.EducationLinkData)%> <br />
		<%= Html.LabelFor(m => m.Character) %>
		<%= Html.DropDownListFor(m => Model.Character, Model.Character)%> <br />

        <br />
        <label>&nbsp;</label>
        <%= Buttons.Link("Speichern", Links.QuestionCreate, Links.CreateQuestionController, inline:true)%>
        <%= Buttons.Link("Speichern & Neu", Links.QuestionCreate, Links.CreateQuestionController, inline: true)%>
	</fieldset>
	<% } %>
</asp:Content>
