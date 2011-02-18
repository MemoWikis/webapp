<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CreateQuestionModel>" %>

<%@ Import Namespace="TrueOrFalse.Frontend.Web.Models" %>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
	<h2>Frage erstellen</h2>
	<% using (Html.BeginForm())
	{ %>
	
	<h2><%= Html.Encode(ViewData["Msg"]) %></h2>
	<fieldset>
		<legend>Neue Frage erstellen</legend>
		<label for="Fragetyp">Fragetyp</label>
		<select id="Fragetyp"><option>Freitext</option></select><br />

		<%= Html.LabelFor(m => m.Question) %><br />
		<%= Html.TextAreaFor( m=>m.Question ) %><br />
		<%= Html.LabelFor(m => m.Answer ) %><br />
		<%= Html.TextAreaFor( m=>m.Answer  ) %><br />
		<input type="submit" value="Speichern" />
		<input type="submit" value="Speichern & Neu" />
	</fieldset>
	<% } %>
</asp:Content>
