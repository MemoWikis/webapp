<%@ Page Title="temp" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<WelcomeModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Questions/Questions.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/questions") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-sm-9">
        <!-- Link with Icon -->
        <div>
            <h4>Link with Icon</h4>
                For not having the icon underlined at hover.</br>
                Use classes a.SimpleTextLink and span.TextSpan.</br>
                </br>
                <a class="SimpleTextLink">
                    <i class="fa fa-question-circle"></i>
                    <span class="TextSpan">Hilfe & mehr</span>
                </a> 
        </div>
        
    </div>
</asp:Content>