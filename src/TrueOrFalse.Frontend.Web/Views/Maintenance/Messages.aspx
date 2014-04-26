<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<MaintenanceMessagesModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="col-md-9">
        <nav class="navbar navbar-default" style="" role="navigation">
            <div class="container">
                <a class="navbar-brand" href="#">Maintenance</a>
                <ul class="nav navbar-nav">
                    <li><a href="/Maintenance">Allgemein</a></li>
                    <li><a href="/Maintenance/Images">Bilder</a></li>
                    <li class="active"><a href="/Maintenance/Messages">Nachrichten</a></li>
                </ul>
            </div>
        </nav>
        <% Html.Message(Model.Message); %>
        
        <h4 style="margin-top: 20px;">Nachricht senden</h4>
        <div class="form-horizontal">
            <% using (Html.BeginForm("SendMessage", "Maintenance")){%>
        
                <div class="form-group">
                    <%= Html.LabelFor(m => m.TestMsgReceiverId, new {@class="col-sm-2 control-label"} ) %>
                    <div class="col-xs-2">
                        <%= Html.TextBoxFor(m => m.TestMsgReceiverId, new {@class="form-control"} ) %>    
                    </div>Jules-Id: 25, Roberts-Id: 2
                </div>
                <div class="form-group">
                    <%= Html.LabelFor(m => m.TestMsgSubject, new {@class="col-sm-2 control-label"} ) %>
                    <div class="col-xs-6">
                        <%= Html.TextBoxFor(m => m.TestMsgSubject, new {@class="form-control"} ) %>    
                    </div>
                </div>
                <div class="form-group">
                    <%= Html.LabelFor(m => m.TestMsgBody, new {@class="col-sm-2 control-label"} ) %>
                    <div class="col-xs-6">
                        <%= Html.TextAreaFor(m => m.TestMsgBody, new {@class="form-control", rows = 4} ) %>
                    </div>
                </div>

                <div class="form-group" style="">
                    <div class="col-sm-offset-2 col-sm-9">
                        <input type="submit" value="Senden" class="btn btn-primary" name="btnSave" />
                    </div>
                </div>

            <% } %>
        </div>

    </div>

</asp:Content>