<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<EditQuestionSetModel>" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <title><%=Model.PageTitle %></title>
    <%= Scripts.Render("~/bundles/fileUploader") %>
    <%= Scripts.Render("~/bundles/questionSetEdit") %>
    <script type="text/javascript">
        var isEditMode = <%= Model.IsEditing ? "true" : "false" %>;
        var questionSetId = "<%= Model.Id %>";
        var userName = "<%= Model.Username %>";
    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
<style type="text/css">
    .form-horizontal .control-group label.control-label{ width: 120px; }
    .form-horizontal .control-group .controls{ margin-left: 120px; }
    .form-horizontal .info{ margin-left: 130px;}
    .form-horizontal .form-actions { padding-left: 130px; }
</style>

<div class="span10">
    
    <div style="margin-bottom: -10px;">
        <% Html.Message(Model.Message); %>
    </div>

    <% using (Html.BeginForm(Model.IsEditing ? "Edit" : "Create", 
                "EditQuestionSet", null, FormMethod.Post,
                new { enctype = "multipart/form-data", style = "margin:0px;"})){ %>
    
        <%: Html.HiddenFor(m => m.ImageIsNew) %>
        <%: Html.HiddenFor(m => m.ImageSource) %>
        <%: Html.HiddenFor(m => m.ImageWikiFileName) %>
        <%: Html.HiddenFor(m => m.ImageUploadedGuid) %>

        <div class="form-horizontal">
            <div class="box box-main">
                <h1 class="pull-left"><%=Model.FormTitle %></h1>

                <div class="box-content" style="clear: both;">    
                    <div class="row">
                        <div class="span6">
                            <div class="control-group">
                                <%= Html.LabelFor(m => m.Title, new { @class = "control-label" })%>
                                <div class="controls">
                                    <%= Html.TextBoxFor(m => m.Title, new { placeholder = "Titel" }) %>
                                </div>
                            </div>
                            <div class="control-group">
                                <%= Html.LabelFor(m => m.Text, new { @class = "control-label" })%>
                                <div class="controls">
                                    <%= Html.TextAreaFor(m => m.Text, new { style = "height:50px; width:300px;", placeholder = "Beschreibung" }) %>
                                </div>
                            </div>
                            <div class="info">
                                <b>Keine Fragen im Fragesatz.</b>
                                Um Fragen hinzuzufügen, wählen Sie Fragen 
                                auf der <%= Html.ActionLink("Fragen-Übersichtsseite", "Questions", "Questions") %> aus. 
                            </div>
                        </div>
                        <div class="span3" style="position: relative; left: 25px;">
                            <div class="box">
                                <img id="questionSetImg" src="/Images/no-question-set-206.png" />
                            </div>
                            <a href="#" style="position: relative; top: -6px;" id="aImageUpload">[Verwende ein anderes Bild]</a>
                        </div>
                    </div>
                </div>
    
                <div class="form-actions">
                    <input type="submit" class="btn btn-primary" value="Speichern" />
                    <input type="button" class="btn" value="Cancel">
                </div>
            </div>
        </div>
    
    <% } %>
</div>
    
<% Html.RenderPartial("../Shared/ImageUpload/ImageUpload"); %>

</asp:Content>