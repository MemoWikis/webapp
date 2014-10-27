<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<MaintenanceImagesModel>" %>
<%@ Import Namespace="TrueOrFalse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   
    <div style="margin:0 0 0 -10px; position: relative;" class="container-fluid">
        <nav class="navbar navbar-default" style="" role="navigation">
            <div class="container">
                <a class="navbar-brand" href="#">Maintenance</a>
                <ul class="nav navbar-nav">
                    <li><a href="/Maintenance">Allgemein</a></li>
                    <li class="active"><a href="/Maintenance/Images">Bilder</a></li>
                    <li><a href="/Maintenance/Messages">Nachrichten</a></li>
                    <li><a href="/Maintenance/Tools">Tools</a></li>
                </ul>
            </div>
        </nav>
    </div>
    <% Html.Message(Model.Message); %>
        
    <a href="/Maintenance/ImageUpdateLicenseData" class="btn btn-warning" style="margin-bottom: 10px; margin-top: -5px;">Lizenzinformation laden von Wikimedia</a>
    <a href="/Maintenance/ImageUpdateMarkupFromDb" class="btn btn-primary" style="margin-bottom: 10px; margin-top: -5px;">Lizenzinformation update von lokaler DB</a>
        

    <table class="table">
        <tr>
            <th style="width: 75px;"></th>
            <th style="width: 70px;">Info</th>
            <th>Author</th>
            <th>License</th>
            <th>Description</th>
        </tr>
        <%  var index = 0;
            foreach(var imageMaintenanceInfo in Model.ImageMaintenanceInfos){ index++; %>
            <tr class="<%=imageMaintenanceInfo.GetCssClass() %>">
                <td>
                    <img src="<%= imageMaintenanceInfo.Url_128 %>" style="width: 50px" />
                </td>                    
                <td>
                    <%=  Enum.Parse(typeof(ImageType), imageMaintenanceInfo.MetaData.Type.ToString())  %><br/>
                    ImageId: <%= imageMaintenanceInfo.ImageId %><br/>
                    TypeId: <%= imageMaintenanceInfo.TypeId %>
                </td>
                <td></td>
                <td></td>
                <td></td>
            </tr>

        <% } %>
    </table>

</asp:Content>