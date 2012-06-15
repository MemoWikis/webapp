<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<EditQuestionModel>" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="TrueOrFalse.Core.Web" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="TrueOrFalse.Core" %>

<asp:Content runat="server" ID="head" ContentPlaceHolderID="Head">
    <script src="/Views/Categories/Edit/RelatedCategories.js" type="text/javascript"></script>
    <script src="/Views/Questions/Edit/EditQuestion.js" type="text/javascript"></script>
    <style type="text/css">
        div.classification  input {width: 75px; background-color: beige;}
    </style>

</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">

    <% using (Html.BeginForm()){ %>
    
    <div class="form-horizontal" style="padding-top:10px;">
        
        <legend style="background-color: ">
            <span>
                <%=Model.PageTitle %>
            </span>
            <div class="pull-right" style="vertical-align: bottom;">
                <div style="background-color: white; line-height: 12px ">
                    <a href="<%= Url.Action(Links.Questions, Links.QuestionsController) %>" style="font-size: 12px; margin: 0px;"><i class="icon-th-list"></i> zur Übersicht</a><br/>
                </div>
                <% if (!Model.ShowSaveAndNewButton){ %>
                    <div style="line-height: 12px">
	                    <a href="<%= Url.Action(Links.CreateQuestion, Links.EditQuestionController) %>" style="font-size: 12px; margin: 0px;"><i class="icon-plus-sign"></i> Frage erstellen</a>
                    </div>
                <%} %>
            </div>
        </legend>
        
        <div style=" margin-top: -5px; padding-left:14px; margin-right:-15px;">
	        <% Html.Message(Model.Message); %>
        </div>
            
        <div class="control-group">
            <%= Html.LabelFor(m => m.Visibility, new { @class = "control-label" })%>    
            <div class="controls">                    
                <%= Html.RadioButtonFor(m => m.Visibility, QuestionVisibility.All )%> Alle &nbsp;&nbsp;
                <%= Html.RadioButtonFor(m => m.Visibility, QuestionVisibility.Owner)  %> Nur ich &nbsp;&nbsp;
                <%= Html.RadioButtonFor(m => m.Visibility, QuestionVisibility.OwnerAndFriends)  %> Ich und meine Freunde
            </div>

        </div>
            
        <div class="control-group">
		    <%= Html.LabelFor(m => m.Question, new { @class = "control-label" })%>
            <div class="controls">
		        <%= Html.TextAreaFor(m => m.Question, new { @style = "height:50px; width:435px;" })%><br />
            </div>
        </div>
            
        <p class="help-block help-text">
            Kategorien helfen bei der Einordnung der Frage u. ermöglichen Dir und anderen <br />die Fragen wiederzufinden.
        </p>
        
        <div class="control-group">
            <%= Html.Label("Kategorien", new { @class = "control-label" })%>
            <div id="relatedCategories" class="controls">
                <script type="text/javascript">
                    $(function() {
                        <%foreach (var category in Model.Categories) { %>
                        $("#txtNewRelatedCategory").val('<%=category %>');
                        $("#addRelatedCategory").click();
                        <% } %>
                    });
                </script>
                <input id="txtNewRelatedCategory" />
                <a href="#" id="addRelatedCategory" style="display:none"><img alt="" src='/Images/Buttons/add.png' /></a>
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
            <%= Html.LabelFor(m => m.SolutionType, new { @class = "control-label" }) %>
            <div class="controls">
                <%= Html.DropDownListFor(m => Model.SolutionType, Model.AnswerTypeData, new {@id = "ddlAnswerType"})%> 
            </div>
        </div>
        
        <div id="question-body"></div>
        
        <script type="text/javascript">
            $("#ddlAnswerType").change(function () {
                var selectedValue = $(this).val();
                $.ajax({
                    url: '<%=Url.Action("SolutionEditBody") %>?type=' + selectedValue,
                    type: 'GET',
                    beforeSend: function () {
                        //some loading indicator
                    },
                    success: function (data) {
                        $("#question-body").html(data);
                    },
                    error: function (data) {
                        //handle error
                    }
                });
            });
        </script>

        <%--<% Html.RenderPartial("~/Views/Questions/Edit/EditAnswerControls/AnswerTypeAccurate.ascx", Model); %>--%>

        <p class="help-block help-text">
            Je ausführlicher die Erklärung, desto besser!<br/>
            Verwende Links u. Bilder aber achte auf die Urheberrechte.
        </p>

        <div class="control-group">
		    <%= Html.LabelFor(m => m.Description, new { @class = "control-label" })%>
            <div class="controls">
                <%= Html.TextAreaFor(m => m.Description, new { @style = "height:50px; width:435px;" })%>
            </div>
        </div>
            
        <div class="form-actions">
            <input type="submit" value="Speichern" class="btn btn-primary" name="btnSave" />&nbsp;&nbsp;&nbsp;
            <% if(Model.ShowSaveAndNewButton){ %>
                <input type="submit" value="Speichern & Neu" class="btn" name="btnSaveAndNew" />&nbsp;
            <% } %>
        </div>

    </div>
    
<% } %>
</asp:Content>
