<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<MaintenanceImagesModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="col-md-9">        
        <div style="margin:0 0 0 -10px; position: relative;" class="container-fluid">
            <nav class="navbar navbar-default" style="" role="navigation">
                <div class="container">
                    <a class="navbar-brand" href="#">Maintenance</a>
                    <ul class="nav navbar-nav">
                        <li><a href="/Maintenance">Allgemein</a></li>
                        <li class="active"><a href="/Maintenance/Images">Bilder</a></li>
                        <li><a href="/Maintenance/Messages">Nachrichten</a></li>
                    </ul>
                </div>
            </nav>
        </div>
        <% Html.Message(Model.Message); %>

    </div>

</asp:Content>