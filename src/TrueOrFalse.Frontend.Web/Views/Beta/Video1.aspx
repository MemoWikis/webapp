<%@ Page Title="memucho in Beta" Language="C#" MasterPageFile="~/Views/Shared/Site.Beta.Master" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Web.Optimization" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Beta/Beta.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/beta") %>
    
    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">    
    
    <div class="row">
        <div class="col-md-8 col-md-offset-2" style="margin-bottom: 200px">
            <h1 style="margin-top: 30px; margin-bottom: 20px;" class="animated">
                Video Übungsplan
            </h1>
                
            <div class="row">
                <iframe width="853" height="480" src="https://www.youtube.com/embed/SGXqP-Df_Y8?rel=0" frameborder="0" allowfullscreen></iframe>
            </div>

        </div>
    </div>


</asp:Content>