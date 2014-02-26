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
        <div class="GradientBunt" style="height: 50px; width: 50px;">
            <i class="fa fa-bars" style="color: white; font-size: 40px; display: block; margin-left: 15%; margin-top: 20px;"></i>
        </div>
        <div style="margin-top: 20px;">
            <h4>Z-Index-Problem</h4>
            <div class="" style="width: 50px; height: 50px; background-color: red; position: relative; z-index: 1;">D1</div>
            <div class="" style="width: 50px; height: 50px; background-color: green; margin-top: 0px; position: relative; z-index: 2;">
                <div class="" style="width: 20px; height: 20px; background-color: blue; top: -10px; position: relative; z-index: auto;">D3</div>
            </div>
        </div>
        
    </div>
</asp:Content>