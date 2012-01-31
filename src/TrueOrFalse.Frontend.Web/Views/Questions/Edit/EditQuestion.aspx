<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<EditQuestionModel>" %>

<%@ Import Namespace="TrueOrFalse.Core" %>

<asp:Content runat="server" ID="head" ContentPlaceHolderID="Head">
    <script src="<%= Url.Content("~/Views/Questions/Edit/EditQuestion.js") %>" type="text/javascript"></script>
    <style type="text/css">
        div.classification  input {width: 75px; background-color: beige;}
    </style>

</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">

	<h2 class="form-title">Frage erstellen</h2>
	<% using (Html.BeginForm()){ %>
    
    
	    <div style="margin-left: 100px; width:412px; padding-bottom: 5px;"><% Html.Message(Model.Message); %></div>

        <%= Html.LabelFor(m => m.Visibility) %>
        <div style="">
            <%= Html.RadioButtonFor(m => m.Visibility, QuestionVisibility.All )%> Alle &nbsp;&nbsp;
            <%= Html.RadioButtonFor(m => m.Visibility, QuestionVisibility.Owner)  %> Nur ich &nbsp;&nbsp;
            <%= Html.RadioButtonFor(m => m.Visibility, QuestionVisibility.OwnerAndFriends)  %> Ich und meine Freunde
        </div><br style="font-size:1px; line-height:1px; height:1px;"/>
		<%= Html.LabelFor(m => m.Question)%>
		<%= Html.TextAreaFor(m => m.Question, new { @style = "height:30px;" })%><br />

        <script>
            $(function () {
                $("#tabs").tabs();
            });
	    </script>
           
            <%= Html.Label("Kategorien")%>
        <div style="width:400px; float:left; font-size: 10px;" class="classification">
            <%= Html.TextBoxFor(m => Model.Category1, new { @id = "cat1" })%>
			<%= Html.TextBoxFor(m => Model.Category2, new { @id = "cat2" })%>
			<%= Html.TextBoxFor(m => Model.Category3, new { @id = "cat3" })%>
			<%= Html.TextBoxFor(m => Model.Category4, new { @id = "cat4" })%>
			<%= Html.TextBoxFor(m => Model.Category5, new { @id = "cat5" })%><br/>
        </div>
		<%= Html.LabelFor(m => m.Character) %>
		<div style="width:400px; float:left; font-size: 10px;" class="classification">
			<%= Html.DropDownListFor(m => Model.Character, Model.CharacterData)%>  &nbsp;&nbsp;
			<input type="text" id="char1" /><input type="text" id="char2" /><input type="text" id="char3" />
		</div>

        <%= Html.LabelFor(m => m.SolutionType ) %>
		<%= Html.DropDownListFor(m => Model.SolutionType, Model.AnswerTypeData, new {@id = "ddlAnswerType"})%> <br />
            
        <% Html.RenderPartial("~/Views/Questions/Edit/EditAnswerControls/AnswerTypeAccurate.ascx", Model); %>

		<%= Html.LabelFor(m => m.Description ) %>
		<%= Html.TextAreaFor( m=>m.Description ) %><br />		   

        <br />
        <label>&nbsp;</label>

        <%= Buttons.Submit("Speichern", inline:true)%>
        <%= Buttons.Submit("Speichern & Neu", inline: true)%>
	    
	<% } %>
</asp:Content>
