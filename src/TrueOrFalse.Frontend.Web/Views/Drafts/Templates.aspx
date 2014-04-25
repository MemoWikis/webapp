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
                For not having the icon underlined at hover.<br/>
                Use classes a.SimpleTextLink and span.TextSpan.<br/>
            <br/>
                <a class="SimpleTextLink">
                    <i class="fa fa-question-circle"></i>
                    <span class="TextSpan">Hilfe & mehr</span>
                </a> 
        </div>
        <!-- Header -->
        <h1>Lorem ipsum - h1 plain</h1>
        <h1><span class="underlined">Lorem ipsum - h1 underlined</span></h1>
        <h1 class="underlined">Lorem ipsum - h1 underlined complete</h1>
        <br/>
        <h2>Lorem ipsum - h2 plain</h2>
        <h2><span class="underlined">Lorem ipsum - h2 underlined</span></h2>
        <h2 class="underlined">Lorem ipsum - h2 underlined complete</h2>
        <br/>
        <h3>Lorem ipsum - h3 plain</h3>
        <h3><span class="underlined">Lorem ipsum - h3 underlined</span></h3>
        <h3 class="underlined">Lorem ipsum - h3 underlined complete</h3>
        <br/>
        <h4>Lorem ipsum - h4 plain</h4>
        <h4><span class="underlined">Lorem ipsum - h4 underlined</span></h4>
        <h4 class="underlined">Lorem ipsum - h4 underlined complete</h4>
        <br/>
        
        <div>
            <div class="alert alert-success">Test Success</div>
            <div class="alert alert-info">Test Info</div>
            <div class="alert alert-warning">Test Warning</div>
            <div class="alert alert-danger">Test Danger</div>

        </div>


    </div>
   
</asp:Content>