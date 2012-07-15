<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<QuestionsModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">

<style type="text/css">

<%--div.column-1 { background-color: green; }
div.column-2 {background-color: seashell; }
div.column-3 { background-color: yellowgreen;}
div.question-row{background-color:silver;}--%>

div.question-row{border-top:1px solid silver;height: 93px;}
.column { display: inline-block;}
div.question-row div.header { margin-bottom: 3px;border-bottom: 1px solid beige ;}
div.column-1 { width: 160px;float: left; padding-top: 5px; }
div.column-2 { width: 530px;float: left; padding-top: 5px;}
div.column-3 { width: 105px;float: left; padding-top: 5px;}
div.question-row div.answersTotal{ width: 40px;}
div.question-row div.percentageBar{ width: 65px;float: right;}

.sliderValue{ margin-left: 10px;}

</style>

<script src="/Views/Questions/SelectUsers.js" type="text/javascript"></script>
<script src="/Views/Questions/Questions.js" type="text/javascript"></script>

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
             <button class="btn btn-filterByAll">von anderen</button>
             <%: Html.HiddenFor(model => model.FilterByMe)%>
             <%: Html.HiddenFor(model => model.FilterByAll)%>
            </div>
            <span class="help-inline">und</span>&nbsp;
            <% foreach (var filterByUser in Model.FilterByUsers)
               { %>
                  <span class="added-usr"><%: filterByUser.Value %><button id="del-usr-<%:filterByUser.Key%>"><i class="icon-remove"></i></button></span>
                  <script type="text/javascript">
                      $(function() {
                          $("#del-usr-<%:filterByUser.Key%>").click(function() {
                              $("#delFilterUserId").val("<%:filterByUser.Key%>");
                          });
                      });
                  </script>
             <% } %>
            <%: Html.HiddenFor(m => m.AddFilterUser, new {id="addFilterUserId"}) %>
            <%: Html.HiddenFor(m => m.DelFilterUser, new {id="delFilterUserId"}) %>
            <input type="text" class="span2" id="txtAddUserFilter"/>
            <button id="addUserFilter"><img alt="" src='/Images/Buttons/tick.png' /></button>
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
            <label></label>
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
     
     
    <div id="modalDelete" class="modal hide fade">
        <div class="modal-header">
            <button class="close" data-dismiss="modal">×</button>
            <h3>Frage löschen</h3>
        </div>
        <div class="modal-body">
            <div class="alert alert-error">
                Die Frage <b>'<span id="spanQuestionTitle"></span>'</b> wird unwiederbringlich gelöscht. Alle damit verknüpften Daten werden entfernt! 
            </div>
        </div>
        <div class="modal-footer">
            <a href="#" class="btn" id="btnCloseQuestionDelete">Schliessen</a>
            <a href="#" class="btn btn-primary btn-danger" id="confirmQuestionDelete">Löschen</a>
        </div>
    </div>

</asp:Content>

