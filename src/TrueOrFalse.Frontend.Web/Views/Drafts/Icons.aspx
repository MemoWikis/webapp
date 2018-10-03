<%@ Page Title="temp" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="ViewPage<WelcomeModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Questions/Questions.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/questions") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
    <h2>Icons Vorschläge</h2>
        <div style="font-size: 20px;">Gemerkt:<i class="fa fa-thumb-tack" style="color: red;"></i><i class="fa"></i> </div>
        <div style="font-size: 20px;">Qualität: <i class="fa fa-star"></i> </div>
        <div style="font-size: 20px;">Gesehen: <i class="fa fa-eye"></i> </div>
    </div>
    <div>
        <div style="float: left;"><div style="font-size: 60px; position: relative; padding-right: 10px;"><i class="fa fa-star" style="color: orange;"></i><div style="font-size: 18px; position: absolute; top: 30px; left: 15px;">5,7</div></div></div>
        <div style="float: left;"><div style="font-size: 74px; position: relative; padding-right: 10px; top: -10px;"><i class="fa fa-thumb-tack" style="color: yellowgreen;"></i><div style="font-size: 18px; position: absolute; top: 40px; left: 11px;">5,7</div></div></div>
        <div style="float: left;"><div style="font-size: 74px; position: relative; padding-right: 10px; top: -10px;"><i class="fa fa-eye" style="color: lightblue;"></i><div style="font-size: 18px; position: absolute; top: 40px; left: 25px;">5,7</div></div></div>
    </div>
    <div>
        <div class="PinIcon" style="float: left;"><div style="font-size: 74px; position: relative; padding-right: 10px; top: -10px;"><i class="fa fa-thumb-tack" style="color: lightgrey;"></i><div style="font-size: 18px; position: absolute; top: 40px; left: 11px;"></div></div></div>
    </div>
</asp:Content>