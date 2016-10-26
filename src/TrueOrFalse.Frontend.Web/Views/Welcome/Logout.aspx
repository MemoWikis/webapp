﻿<%@ Page Title="Ausgeloggt" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="ViewPage<dynamic>" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <link rel="canonical" href="<%= Settings.CanonicalHost %><%= Url.Action("Logout", "Welcome") %>">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row" style="margin-bottom: 100px;">
        <div class="BackToHome col-md-3">
            <a href="/"><i class="fa fa-chevron-left">&nbsp;</i>Zur Startseite</a>
        </div>
    
        <div class="form-horizontal col-md-9">
            <h2>Ausgeloggt</h2>
            <p>Du wurdest erfolgreich ausgeloggt.</p>    
        </div>        
    </div>

</asp:Content>


