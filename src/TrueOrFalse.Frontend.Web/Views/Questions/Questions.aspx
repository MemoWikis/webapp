<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<QuestionsModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">

<style type="text/css">

<%--div.column-1 { background-color: green; }
div.column-2 {background-color: seashell; }
div.column-3 { background-color: yellowgreen;}
div.question-row{background-color:silver;}--%>

div.question-row{border-top:1px solid silver; margin-bottom:20px;}
.column { display: inline-block;}
div.question-row div.header { margin-bottom: 3px;border-bottom: 1px solid beige ;}
div.column-1 { width: 160px;float: left; padding-top: 5px;}
div.column-2 { width: 500px;float: left; padding-top: 5px;}
div.column-3 { width: 140px;float: left; padding-top: 5px;}
div.question-row div.answersTotal{ width: 75px;}
div.question-row div.truePercentage{ width: 37px;}
div.question-row div.falsePercentage{ width: 35px;float: right;}

</style>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="row" >
        <h2 style="float: left;">Fragen</h2>
        <div style="float: right;">
            <a href="<%= Url.Action(Links.CreateQuestion, Links.EditQuestionController) %>" style="width: 120px" class="btn">
                <i class="icon-plus-sign"></i>
                Frage erstellen
            </a>
        </div>
    </div>
    
    <div class="row form-horizontal well" style="padding-bottom: 0px; padding-top: 14px; margin-bottom: 0px;">
        <div class="control-group" style="margin-bottom: 8px;">
            <label><b>Fragen erstellt von</b>:</label>            
            <div class="btn-group" data-toggle="buttons-checkbox" style="display: inline">
              <button class="btn"><i class="icon-user"></i>&nbsp;von mir</button>
              <button class="btn">von allen</button> 
            </div>
            <span class="help-inline">und</span>&nbsp;
            <input type="text" class="span2"/>
        </div>
        <div class="control-group" style="margin-bottom: 8px;">
            <label><b>Kategorien Filter</b>:</label>
            <input type="text" class="span2" />
        </div>
        <div class="control-group" style="margin-bottom: 8px;">
            <label><b>Mindestens</b>:</label>
            
                <span class="help-inline">Relevanz von </span>
                <input class="span1"/>
                
                <span class="help-inline">Qualität von: </span>
                <input class="span1"/>
        </div>
    </div>
    
    <div class="row" style="padding-top:5px; padding-bottom: 3px;">
        <div class="pull-right">3274 Fragen</div>
    </div>

    <% foreach (var row in Model.QuestionRows)
        {
            Html.RenderPartial("QuestionRow", row);
        } %>
    
    <div class="row" style="margin-top:-20px;">
        <div class="pagination" style="text-align:center;" >
            <ul>
                <li class="disabled"><a href="#">«</a></li>
                <% for (var i = 1; i <= Model.Pager.PageCount; i++) { %>
                    <li><%= Html.ActionLink(i.ToString(CultureInfo.InvariantCulture), 
                        Links.Questions, Links.QuestionsController, new {page=i}, null ) %></li>
                <% } %>
                <li class="disabled"><a href="#">»</a></li>
            </ul>
        </div>        
    </div>
</asp:Content>

