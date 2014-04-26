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
        
        
        <table class="table">
            <tr>
                <th>Id</th>
                <th>TypeId</th>
                <th>InCat</th>
                <th>InQuestion</th>
            </tr>
            <%  var index = 0;
                foreach(var imageMaitenanceInfo in Model.ImageMaintenanceInfos){ index++; %>
                <tr class="<%=imageMaitenanceInfo.GetCssClass() %>">
                    <td><%= imageMaitenanceInfo.ImageId %></td>
                    <td><%= imageMaitenanceInfo.TypeId %></td>
                    <td><%= imageMaitenanceInfo.InCategoryFolder %></td>
                    <td><%= imageMaitenanceInfo.InQuestionFolder %></td>
                </tr>

            <% } %>
        </table>
        
        <a href="/Maintenance/ImageMaintenanceWork" class="btn btn-danger">Bildertypen eindeutig zuordnen</a>

    </div>

</asp:Content>