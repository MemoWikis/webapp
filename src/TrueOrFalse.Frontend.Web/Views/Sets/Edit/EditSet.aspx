<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<EditSetModel>" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <title><%=Model.PageTitle %></title>
    <%= Scripts.Render("~/bundles/fileUploader") %>
    <%= Scripts.Render("~/bundles/SetEdit") %>
    <script src="/Views/Categories/Edit/RelatedCategories.js" type="text/javascript"></script>
    <script type="text/javascript">
        var isEditMode = <%= Model.IsEditing ? "true" : "false" %>;
        var questionSetId = "<%= Model.Id %>";
        var userName = "<%= Model.Username %>";

        $(function() {
            $('.control-label .show-tooltip').append($("<i class='fa fa-info-circle'></i>"));
        });
    </script>
    
    <style>
        #ulQuestions { list-style-type: none; margin: 0; padding: 0; width: 100%; }
        #ulQuestions li {
            margin: 0 5px 5px 5px; padding: 5px; font-size: 1.0em; line-height: 1.2em; 
            height: 30px; background: none;
        }

        .questionTools {position: relative; right: 0px; float:right;height: 30px;margin-left: 5px; }
        .deleteButton {color: red; cursor: pointer}
        .editButton {color: blue; cursor: pointer}
      
        .ui-state-highlight { height: 1.5em; line-height: 1.2em;}

        .form-horizontal .control-group label.control-label{ width: 120px; }
        .form-horizontal .control-group .controls{ margin-left: 120px; }
        .form-horizontal .info{ margin-left: 130px;}
        .form-horizontal .form-actions { padding-left: 130px; }

        .draggable-panel { height: 30px;width: 30px;background-color: gainsboro;cursor: move;margin-right: 5px; }
        div.questionText {height: 30px;width: 380px; overflow: hidden;  }
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
<style type="text/css">

</style>

<div class="col-md-9" id="questionSetContainer" data-id="<%: Model.Id %>">
    
    <div style="margin-bottom: -10px;">
        <% Html.Message(Model.Message); %>
    </div>

    <% using (Html.BeginForm(Model.IsEditing ? "Edit" : "Create", 
                "EditSet", null, FormMethod.Post,
                new { enctype = "multipart/form-data", style = "margin:0px;"})){ %>
    
            
        <%: Html.HiddenFor(m => m.ImageIsNew) %>
        <%: Html.HiddenFor(m => m.ImageSource) %>
        <%: Html.HiddenFor(m => m.ImageWikiFileName) %>
        <%: Html.HiddenFor(m => m.ImageGuid) %>
        <%: Html.HiddenFor(m => m.ImageLicenceOwner) %>

        <div class="form-horizontal">
            <div class="box box-main">
                <h1 class="pull-left"><%=Model.FormTitle %></h1>
                <div class="pull-right">
                    <div>
                        <a href="<%= Links.Sets(Url) %>" style="font-size: 12px; margin: 0px;"><i class="fa fa-list"></i>&nbsp;zur Übersicht</a><br/>
                        <% if(Model.Set != null){ %>
                            <a href="<%= Links.SetDetail(Url, Model.Set) %>" style="font-size: 12px; margin: 0px;"><i class="fa fa-eye"></i>&nbsp;Detailansicht</a> 
                        <% } %>
                    </div>
                </div>

                <div class="box-content" style="clear: both;">    
                    <div class="row">
                        <div class="col-md-8">
                            <div class="form-group">
                                <%= Html.LabelFor(m => m.Title, new { @class = "col-sm-3 control-label" })%>
                                <div class="col-sm-8">
                                    <%= Html.TextBoxFor(m => m.Title, new { placeholder = "Titel", @class="form-control" }) %>
                                </div>
                            </div>
                            <div class="form-group">
                                <%= Html.LabelFor(m => m.Text, new { @class = "col-sm-3 control-label" })%>
                                <div class="col-sm-8">
                                    <%= Html.TextAreaFor(m => m.Text, new { style = "height:50px;", placeholder = "Beschreibung", @class="form-control" }) %>
                                </div>
                            </div>
                            <div class="form-group">
                                
                                <label class="col-sm-3 control-label">
                                    <span class="show-tooltip" title = "Kategorien helfen bei der Einordnung des Fragesatzes u. ermöglichen Dir und anderen Fragesätze wiederzufinden." data-placement = "left">
                                        Kategorien
                                    </span>
                                </label>        
                                
                                <div id="relatedCategories" class="col-sm-9">
                                    <script type="text/javascript">
                                        $(function () {
                                            <%foreach (var category in Model.Categories) { %>
                                                $("#txtNewRelatedCategory").val('<%=category %>');
                                                $("#addRelatedCategory").click();
                                            <% } %>
                                        });
                                    </script>
                                    <input id="txtNewRelatedCategory" type="text" placeholder="Wähle eine Kategorie" class="form-control" style="width: 190px;" />
                                    <a href="#" id="addRelatedCategory" style="display: none">
                                        <img alt="" src='/Images/Buttons/add.png' />
                                    </a>
                                </div>                                
                            </div>

                            <%  if(Model.QuestionsInSet.Count == 0) { %>
                                <div class="info">
                                    <b>Keine Fragen im Fragesatz.</b> Um Fragen hinzuzufügen, wählen Sie Fragen 
                                    auf der <%= Html.ActionLink("Fragen-Übersichtsseite", "Questions", "Questions") %> aus. 
                                </div>
                            <% }else{ %>
                                <h4 style="padding-left:5px;">Fragen Reihenfolge
                                    <span style="font-size: 11px;">(per Drag'n'Drop)</span>
                                    <span id="revertAction" class="pull-right hide2" style="font-size: 11px; font-weight: normal; position: relative; top: 7px; right: 7px; cursor: pointer">
                                        [Rückgängig]
                                    </span>
                                </h4>
                                <ul id="ulQuestions">
                                    <%foreach(var questionInSet in Model.QuestionsInSet){%>
                                        <li class="ui-state-default" data-id="<%=questionInSet.Id %>">
                                            <div class="questionTools">
                                                <i class="fa fa-trash-o icon deleteButton"></i><br/>
                                                <% if (Model.IsOwner(questionInSet.Question.Creator.Id)){%>
                                                    <a href="<%= Links.EditQuestion(Url, questionInSet.Question.Id) %>">
                                                        <img src="/Images/edit.png"/> 
                                                    </a>
                                                <% } %>
                                            </div>

                                            <div class="draggable-panel" style="float: left;">&nbsp;</div>
                                            <div class="questionText">
                                                <%= questionInSet.Question.Text %>
                                            </div>
                                            
                                        </li>  
                                    <%} %>
                                </ul>
                            <% } %>
                        </div>
                        <div class="col-md-4" >
                            <div class="box" style="margin-right: 25px;">
                                <img id="questionSetImg" src="<%= Model.ImageUrl_206px %>" />
                            </div>
                            <a href="#" style="position: relative; top: -6px;" id="aImageUpload">[Verwende ein anderes Bild]</a>
                        </div>
                    </div>
                </div>
    
                <div class="form-actions">
                    <input type="submit" class="btn btn-primary" value="Speichern" />
                    <input type="button" class="btn btn-default" value="Cancel">
                </div>
            </div>
        </div>
    
    <% } %>
</div>
    
<% Html.RenderPartial("../Shared/ImageUpload/ImageUpload"); %>
    
<div id="modalRevertAction" class="modal">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h3>Letztes Löschen rückgängig machen</h3>
            </div>
            <div class="modal-body">
                <p>NOCH NICHT UMGESETZT</p>
            </div>
            <div class="modal-footer" id="tqsNoSetsFooter">
                <a href="#" class="btn btn-default" data-dismiss="modal">Schließen</a>
                <a href="#" class="btn btn-primary">Jetzt rückgängig machen</a>
            </div>
        </div>
    </div>
</div>

</asp:Content>