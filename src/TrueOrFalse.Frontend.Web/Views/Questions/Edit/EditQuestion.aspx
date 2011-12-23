<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<EditQuestionModel>" %>

<%@ Import Namespace="TrueOrFalse.Core" %>

<asp:Content runat="server" ID="head" ContentPlaceHolderID="Head">
    <script src="<%= Url.Content("~/Views/Questions/Edit/EditQuestion.js") %>" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        $(function () {
            $('#Question').defaultText("Bitte geben Sie eine Frage ein.");
            $('#Answer').defaultText("Antwort eingeben.");
            $('#Description').defaultText("Erklärung der Antwort und Quellen.");

			for (var i = 0; i <= 5; i++)
            {
				$("#cat" + i).autocomplete({
            		source:  '<%= Url.Action("ByName", "CategoryApi") %>',
            		minLength : 1
				});                
            }

        });

    </script>

    <style type="text/css">
        div.classification  input {width: 75px; background-color: beige;}
    </style>

</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">

	<h2 class="form-title">Frage erstellen</h2>
	<% using (Html.BeginForm())
    { %>
    
        <% Html.Message(Model.Message); %>
	
	    <h2><%= Html.Encode(ViewData["Msg"]) %></h2>

	    
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
           
             <%= Html.Label("Klassifizierung")%>
            <div style="width:400px; float:left; font-size: 10px;" class="classification">
            	<%= Html.TextBoxFor(m => Model.Category1, new { @id = "cat1" })%>
				<input type="text" id="cat2" value="<%= Model.Category2 %>" />
				<input type="text" id="cat3" value="<%= Model.Category3 %>" />
				<input type="text" id="cat4" value="<%= Model.Category4 %>" />
				<input type="text" id="cat5" value="<%= Model.Category5 %>" /><br/>
            </div>
			<%= Html.LabelFor(m => m.Character) %>
			<div style="width:400px; float:left; font-size: 10px;" class="classification">
				<%= Html.DropDownListFor(m => Model.Character, Model.CharacterData)%>  &nbsp;&nbsp;
				<input type="text" id="char1" /><input type="text" id="char2" /><input type="text" id="char3" />
			</div>

            <%= Html.LabelFor(m => m.AnswerType ) %>
		    <%= Html.DropDownListFor(m => Model.AnswerType, Model.AnswerTypeData, new {@id = "ddlAnswerType"})%> <br />
            
            <% Html.RenderPartial("~/Views/Questions/Edit/EditAnswerControls/AnswerTypeAccurate.ascx", Model); %>

		    <%= Html.LabelFor(m => m.Description ) %>
		    <%= Html.TextAreaFor( m=>m.Description ) %><br />		   
            <%= Html.LabelFor(m => m.EducationLink ) %>
		    <%= Html.DropDownListFor(m => Model.EducationLink, Model.EducationLinkData)%> <br />

            <br />
            <label>&nbsp;</label>

            <%= Buttons.Submit("Speichern", inline:true)%>
            <%= Buttons.Submit("Speichern & Neu", inline: true)%>
	    
	<% } %>
</asp:Content>
