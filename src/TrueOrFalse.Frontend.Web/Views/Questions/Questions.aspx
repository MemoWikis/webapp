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

<script src="/Views/Questions/SelectUsers.js" type="text/javascript"></script>

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
        <% using (Html.BeginForm()) { %>
        <div class="control-group" style="margin-bottom: 8px;">
            <label><b>Fragen erstellt von</b>:</label>            
            <div class="btn-group" style="display: inline">
             <button class="btn btn-filterByMe"><i class="icon-user"></i>&nbsp;von mir</button>
             <button class="btn btn-filterByAll">von allen</button>
            </div>
            <script type="text/javascript">
                $(function () {
                    $('.btn-filterByMe').click(function () {
                        $(this).toggleClass('active');
                        $('#FilterByMe').val($(this).hasClass('active'));
                    });
                    if ($('#FilterByMe').val().toLowerCase() == "true") {
                        $('.btn-filterByMe').addClass('active');
                    }

                    $('.btn-filterByAll').click(function () {
                        $(this).toggleClass('active');
                        $('#FilterByAll').val($(this).hasClass('active'));
                    });
                    if ($('#FilterByAll').val().toLowerCase() == "true") {
                        $('.btn-filterByAll').addClass('active');
                    }
                    <%foreach (var user in Model.FilterByUsers) { %>
                        window.addingUserId = <%=user.Key %>;
                        $("#txtAddUserFilter").val('<%=user.Value %>');
                        $("#addUserFilter").click();
                    <% } %>
                })
            </script>
            <span class="help-inline">und</span>&nbsp;
            <input type="text" class="span2" id="txtAddUserFilter"/>
            <a href="#" id="addUserFilter"><img alt="" src='/Images/Buttons/tick.png' /></a>
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
        <div class="control-group" style="margin-bottom: 8px;">
            <%: Html.HiddenFor(model => model.FilterByMe)%>
            <%: Html.HiddenFor(model => model.FilterByAll)%>
            <label></label>
            <%= Buttons.Submit("Filtern", inline: true)%>    
        </div>
        <% } %>
    </div>


    <div class="row" style="padding-top:5px; padding-bottom: 3px;">
        <div class="pull-right"><%= Model.TotalQuestions %> Fragen</div>
    </div>

    <% foreach (var row in Model.QuestionRows)
        {
            Html.RenderPartial("QuestionRow", row);
        } %>
    
     <% Html.RenderPartial("Pager", Model.Pager); %>

</asp:Content>

