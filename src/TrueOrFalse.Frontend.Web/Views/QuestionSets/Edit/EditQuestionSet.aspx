<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<EditQuestionSetModel>" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <title><%=Model.PageTitle %></title>
    <%= Scripts.Render("~/bundles/fileUploader") %>
    <%= Scripts.Render("~/bundles/questionSet") %>
    <%= Styles.Render("~/bundles/questionSetCss") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="span10">
    
    <div style="margin-bottom: -10px;">
        <% Html.Message(Model.Message); %>
    </div>

    <% using (Html.BeginForm(Model.IsEditing ? "Edit" : "Create", 
                "EditQuestionSet", null, FormMethod.Post,
                new { enctype = "multipart/form-data", style = "margin:0px;"})){ %>
    
        <div class="form-horizontal">
            <div class="box box-main">
                <h1 class="pull-left"><%=Model.FormTitle %></h1>

                <div class="box-content" style="clear: both;">    
                    <div class="row">
                        <div class="span6 green">
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
                        </div>
                        <div class="span3" style="background-color: orange">
                            asdfsdf
                        </div>
                    </div>
    
                    <div class="info">
                        Um Fragen hinzuzufügen, wählen Sie Fragen auf der <%= Html.ActionLink("Fragen-Übersichtsseite", "Questions", "Questions") %> aus. 
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
    
<% Html.RenderPartial("../Shared/ImageUpload"); %>

</asp:Content>