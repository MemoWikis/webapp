<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<MessagesModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <nav class="navbar navbar-default" style="" role="navigation">
        <div class="container">
            <a class="navbar-brand" href="#">Maintenance</a>
            <ul class="nav navbar-nav">
                <li><a href="/Maintenance">Allgemein</a></li>
                <li><a href="/MaintenanceImages/Images">Bilder</a></li>
                <li class="active"><a href="/Maintenance/Messages">Nachrichten</a></li>
                <li><a href="/Maintenance/Tools">Tools</a></li>
                <li><a href="/Maintenance/CMS">CMS</a></li>
                <li><a href="/Maintenance/ContentCreatedReport">Cnt-Created</a></li>
                <li><a href="/Maintenance/ContentStats">Cnt Stats</a></li>
                <li><a href="/Maintenance/Statistics">Stats</a></li>
            </ul>
        </div>
    </nav>
    <% Html.Message(Model.Message); %>
        
    <div class="row">
        <div class="col-md-10 col-sm-offset-2">
            <h4 style="margin-top: 20px;" class="">Nachricht senden</h4>
        </div>
    </div>
    
    <div class="form-horizontal">
        <% using (Html.BeginForm("SendMessage", "Maintenance")){%>

            <%= Html.AntiForgeryToken() %>
            <div class="form-group">
                <%= Html.LabelFor(m => m.TestMsgReceiverId, new {@class="col-sm-2 control-label"} ) %>
                <div class="col-xs-2">
                    <%= Html.TextBoxFor(m => m.TestMsgReceiverId, new {@class="form-control"} ) %>    
                </div>Jules Id: 25, Roberts Id: 2, Christofs Id: 33
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
    
    <div>
        <hr/>
    </div>


    <div class="row">
        <div class="col-md-10 col-sm-offset-2">
            <h4 style="margin-top: 20px;" class="">Knowledge-Report per E-Mail</h4>
        </div>
    </div>

    <div class="form-horizontal">
        <% using (Html.BeginForm("SendKnowledgeReportMessage", "Maintenance")){%>

            <%= Html.AntiForgeryToken() %>
        
            <div class="form-group" style="">
                <div class="col-sm-offset-2 col-sm-9">
                    <p>
                        Sende einen Wissensbericht (KnowledgeReport) per HTML-E-Mail an dich selbst. 
                        Versand wird in Tabelle gespeichert und damit beim nächsten regulären Versand berücksichtigt.
                    </p>

                    <input type="submit" value="SendKnowledgeReport" class="btn btn-primary" name="btnSave" />
                </div>
            </div>

        <% } %>
    </div>

</asp:Content>