<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<MaintenanceToolsModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <nav class="navbar navbar-default" style="" role="navigation">
        <div class="container">
            <a class="navbar-brand" href="#">Maintenance</a>
            <ul class="nav navbar-nav">
                <li><a href="/Maintenance">Allgemein</a></li>
                <li><a href="/Maintenance/Images">Bilder</a></li>
                <li><a href="/Maintenance/Messages">Nachrichten</a></li>
                <li class="active"><a href="/Maintenance/Tools">Tools</a></li>
            </ul>
        </div>
    </nav>
    <% Html.Message(Model.Message); %>
        
    <h4>Tools</h4>
    <a href="<%= Url.Action("Throw500", "Maintenance") %>">
        <i class="fa fa-gavel"></i>
        Exception werfen
    </a><br/>
    
    <a href="<%= Url.Action("CleanUpWorkInProgressQuestions", "Maintenance") %>">
        <i class="fa fa-gavel"></i>
        Clean up work in progress questions
    </a><br/>
    
    <h4 style="margin-top: 20px;">Broadcast concentration level</h4>
    <div class="form-horizontal">
        <% using (Html.BeginForm("SendConcentrationLevel", "Maintenance")){%>
        
            <div class="form-group">
                <%= Html.LabelFor(m => m.TxtConcentrationLevel, new {@class="col-sm-2 control-label"} ) %>
                <div class="col-xs-2">
                    <%= Html.TextBoxFor(m => m.TxtConcentrationLevel, new {@class="form-control"} ) %>    
                </div>
            </div>

            <div class="form-group" style="">
                <div class="col-sm-offset-2 col-sm-9">
                    <input type="submit" value="Senden" class="btn btn-primary" name="btnSave" />
                </div>
            </div>

        <% } %>
    </div>

</asp:Content>