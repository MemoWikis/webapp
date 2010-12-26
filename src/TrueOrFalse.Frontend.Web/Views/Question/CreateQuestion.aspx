<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CreateQuestionModel>" %>

<%@ Import Namespace="TrueOrFalse.Frontend.Web.Models" %>
<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
	About Us
</asp:Content>
<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
	<h2>
		About</h2>
	<% using (Html.BeginForm())
	{ %>
	
	<h2><%= Html.Encode(ViewData["Question"]) %></h2>
	<fieldset>
		<legend>Neue Frage erstellen</legend>
		<label for="Fragetyp">
			Fragetyp</label>
		<select id="Fragetyp">
			<option>Freitext</option>
		</select>
		<%= Html.LabelFor(m => m.Question) %>
		<%= Html.TextAreaFor( m=>m.Question ) %>
		<%= Html.LabelFor(m => m.Answer ) %>
		<%= Html.TextAreaFor( m=>m.Answer  ) %>
		<input type="submit" value="Speichern" />
		<input type="submit" value="Speichern & Neu" />
	</fieldset>
	<% } %>
</asp:Content>
