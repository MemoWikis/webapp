<%@ Page Title="Spielen" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.PureContent.Master" 
    Inherits="ViewPage<WidgetSetResultModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Head" runat="server">
    <style type="text/css">
        html { height: auto;}
    </style>
    <%= Scripts.Render("~/bundles/js/TestSessionResult") %>
    <link href="/Views/Questions/Answer/LearningSession/LearningSessionResult.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    
    
    <% if(Model.TestSessionResultModel.TestSession.SessionNotFound) { %>
    
        <h2>Uuups...</h2>
        <p>die Testsitzung ist nicht mehr aktuell.</p>

    <% } else { %>

        <% Html.RenderPartial("~/Views/Questions/Answer/TestSession/TestSessionResultHead.ascx", Model.TestSessionResultModel);  %>
        
        <div class="row">
            <div class="col-sm-12">
                <% Html.RenderPartial("~/Views/Questions/Answer/TestSession/TestSessionResultDetails.ascx", Model.TestSessionResultModel);  %>
            </div>
        </div>
    
    <% } %>

</asp:Content>