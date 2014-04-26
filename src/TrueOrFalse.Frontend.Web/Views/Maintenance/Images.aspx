<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<MaintenanceImagesModel>" %>
<%@ Import Namespace="TrueOrFalse" %>

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
        
        <a href="/Maintenance/ImageMaintenanceWork" class="btn btn-danger" style="margin-bottom: 10px; margin-top: -5px;">Bildertypen eindeutig zuordnen</a>
        
        <table class="table">
            <tr>
                <th>Id</th>
                <th>TypeId</th>
                <th>Type</th>
                <th>InCat</th>
                <th>InQuestion</th>
            </tr>
            <%  var index = 0;
                foreach(var imageMaitenanceInfo in Model.ImageMaintenanceInfos){ index++; %>
                <tr class="<%=imageMaitenanceInfo.GetCssClass() %>">
                    <td><%= imageMaitenanceInfo.ImageId %></td>
                    <td><%= imageMaitenanceInfo.TypeId %></td>
                    <td><%=  Enum.Parse(typeof(ImageType), imageMaitenanceInfo.MetaData.Type.ToString())  %></td>
                    <td><%= imageMaitenanceInfo.InCategoryFolder %></td>
                    <td><%= imageMaitenanceInfo.InQuestionFolder %></td>
                </tr>

            <% } %>
        </table>
    </div>

</asp:Content>