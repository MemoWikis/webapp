﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<ToolsModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Scripts.Render("~/bundles/MaintenanceTools") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

   <% Html.RenderPartial("AntiForgeryToken"); %>
    
    <nav class="navbar navbar-default" style="" role="navigation">
        <div class="container">
            <a class="navbar-brand" href="#">Admin</a>
            <ul class="nav navbar-nav">
                <li><a href="/Maintenance">Allgemein</a></li>
                <li><a href="/Maintenance/Images">Bilder</a></li>
                <li><a href="/Maintenance/Messages">Nachrichten</a></li>
                <li class="active"><a href="/Maintenance/Tools">Tools</a></li>
                <li><a href="/Maintenance/CMS">CMS</a></li>
            </ul>
        </div>
    </nav>
    <% Html.Message(Model.Message); %>
        
    <h4>Tools</h4>
    <a href="<%= Url.Action("Throw500", "Maintenance") %>" data-url="toSecurePost">
        <i class="fa fa-gavel"></i>
        Exception werfen
    </a><br/>
    
    <a href="<%= Url.Action("CleanUpWorkInProgressQuestions", "Maintenance") %>" data-url="toSecurePost">
        <i class="fa fa-gavel"></i>
        Clean up work in progress questions
    </a><br/>
    
    <a href="<%= Url.Action("TrainingReminderCheck", "Maintenance") %>" data-url="toSecurePost">
        <i class="fa fa-gavel"></i>
        Training Reminder Check
    </a><br/>
    
    <h4 style="margin-top: 20px;">Update concentration level</h4>
    <div class="form-horizontal">
        
        <div class="row">
            <div class="col-sm-offset-2 col-sm-9" id="msgConcentrationLevel" style="padding:10px">
            </div>
        </div>
        
        <div class="form-group">
            <div class="col-sm-2" style="text-align: right">Connected:</div>
            <div class="col-xs-2" id="connectedUsers"></div>
        </div>

        <% using (Html.BeginForm("SendConcentrationLevel", "Maintenance")){%>
        
            <div class="form-group">
                <%= Html.LabelFor(m => m.TxtConcentrationLevel, new {@class="col-sm-2 control-label"} ) %>
                <div class="col-xs-2">
                    <%= Html.TextBoxFor(m => m.TxtConcentrationLevel, new {@class="form-control"} ) %>    
                </div>
            </div>
        
            <div class="form-group">
                <%= Html.LabelFor(m => m.TxtUserId, new {@class="col-sm-2 control-label"} ) %>
                <div class="col-xs-2">
                    <%= Html.TextBoxFor(m => m.TxtUserId, new {@class="form-control"} ) %>    
                </div>
            </div>

            <div class="form-group" style="">
                <div class="col-sm-offset-2 col-sm-9">
                    <input type="submit" value="Senden" class="btn btn-primary"  id="btnSendBrainWaveValue"  />
                </div>
            </div>

        <% } %>
    </div>

</asp:Content>