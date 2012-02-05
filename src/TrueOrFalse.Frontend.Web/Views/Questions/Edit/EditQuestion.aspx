<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<EditQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="TrueOrFalse.Core" %>

<asp:Content runat="server" ID="head" ContentPlaceHolderID="Head">
    <script src="<%= Url.Content("~/Views/Questions/Edit/EditQuestion.js") %>" type="text/javascript"></script>
    <style type="text/css">
        div.classification  input {width: 75px; background-color: beige;}
    </style>

</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
    
    
    <% using (Html.BeginForm()){ %>
    
    <div class="form-horizontal" style="padding-top:10px;">
        
            <legend>
                <span >
                    Frage Erstellen  
                    <a href="<%= Url.Action(Links.Questions, Links.QuestionsController) %>"><i class="icon-chevron-left"></i>&nbsp;zu den Fragen</a>                    
                </span>
            </legend>
        
	        <% Html.Message(Model.Message); %>
            
            <div class="control-group">

                <%= Html.LabelFor(m => m.Visibility) %>    
                    
                    <%= Html.RadioButtonFor(m => m.Visibility, QuestionVisibility.All )%> Alle &nbsp;&nbsp;
                    <%= Html.RadioButtonFor(m => m.Visibility, QuestionVisibility.Owner)  %> Nur ich &nbsp;&nbsp;
                    <%= Html.RadioButtonFor(m => m.Visibility, QuestionVisibility.OwnerAndFriends)  %> Ich und meine Freunde
                <%--<label class="checkbox"></label>--%>

            </div>
            
            <div class="control-group">
		        <%= Html.LabelFor(m => m.Question)%>
		        <%= Html.TextAreaFor(m => m.Question, new { @style = "height:50px; width:435px;" })%><br />
            </div>
            
            <p class="help-block form-text">
                Kategorien helfen bei der Einordnung der Frage u. ermöglichen Dir und andere <br />die Fragen wiederzufinden.
            </p>
            <div class="control-group">
                <%= Html.Label("Kategorien")%>
                <div class="classification">
                    <%= Html.TextBoxFor(m => Model.Category1, new { @id = "cat1" })%>
			        <%= Html.TextBoxFor(m => Model.Category2, new { @id = "cat2" })%>
			        <%= Html.TextBoxFor(m => Model.Category3, new { @id = "cat3" })%>
			        <%= Html.TextBoxFor(m => Model.Category4, new { @id = "cat4" })%>
			        <%= Html.TextBoxFor(m => Model.Category5, new { @id = "cat5" })%><br/>
                </div>
            </div>
            
<%--            <div class="control-group">
		        <%= Html.LabelFor(m => m.Character) %>
		        <div class="classification">
			        <%= Html.DropDownListFor(m => Model.Character, Model.CharacterData, new {@style = "width:120px;"} )%>  &nbsp;&nbsp;
			        <input type="text" id="char1" />
                    <input type="text" id="char2" />
                    <input type="text" id="char3" />
		        </div>
            </div>--%>
            
            <div class="control-group">
                <%= Html.LabelFor(m => m.SolutionType ) %>
		        <%= Html.DropDownListFor(m => Model.SolutionType, Model.AnswerTypeData, new {@id = "ddlAnswerType"})%> 
            </div>
            
            <% Html.RenderPartial("~/Views/Questions/Edit/EditAnswerControls/AnswerTypeAccurate.ascx", Model); %>

            <p class="help-block form-text">
                Je ausführlicher die Erklärung, desto besser!<br/>
                Verwende Links u. Bilder aber achte auf die Urheberrechte.
            </p>

            <div class="control-group">
		        <%= Html.LabelFor(m => m.Description ) %>
                <%= Html.TextAreaFor(m => m.Description, new { @style = "height:50px; width:435px;" })%>    
            </div>
            
            <div class="form-actions">
                <input type="submit" value="Speichern" class="btn btn-primary" />&nbsp;&nbsp;&nbsp;
                <input type="submit" value="Speichern & Neu" class="btn " />&nbsp;
            </div>

    </div>
    
<% } %>
</asp:Content>
