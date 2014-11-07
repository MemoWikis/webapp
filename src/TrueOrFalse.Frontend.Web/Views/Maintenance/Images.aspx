<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<MaintenanceImagesModel>" %>
<%@ Import Namespace="System.Activities.Statements" %>
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
        

    <table class="ImageTable table">
        <tr>
            <th class="ColumnImage"></th>
            <th class="ColumnInfo">Info</th>
            <th class="ColumnAuthor">Autor</th>
            <th class="ColumnLicense">Lizenz</th>
            <th class="ColumnDescription">Beschreibung</th>
        </tr>
        <%  var index = 0;
            foreach(var imageMaintenanceInfo in Model.ImageMaintenanceInfos){ index++; %>
            <tr class="<%=imageMaintenanceInfo.GetImageLicenseStateCssClass() %>">
                <td class="ColumnImage">
                    <img src="<%= imageMaintenanceInfo.Url_128 %>" style="width: 50px" />
                </td>                    
                <td class="ColumnInfo">
                    <%=  Enum.Parse(typeof(ImageType), imageMaintenanceInfo.MetaData.Type.ToString())  %><br/>
                    ImageId: <%= imageMaintenanceInfo.ImageId %><br/>
                    TypeId: <%= imageMaintenanceInfo.TypeId %>
                </td>
                <td class="ColumnAuthor"><%= imageMaintenanceInfo.MetaData.Author %></td>
                <td class="ColumnLicense">
                    
                    <% if (imageMaintenanceInfo.MainLicense != null)
                       {
                           %>Hauptlizenz:<br/><%
                           if (!String.IsNullOrEmpty(imageMaintenanceInfo.MainLicense.LicenseShortName))
                           {%><%=
                           imageMaintenanceInfo.MainLicense.LicenseShortName%>
                           <%} else {%>
                                <%= imageMaintenanceInfo.MainLicense.WikiSearchString %>
                            <%}%>
                               
                       <%} else {%>
                        Keine (verwendbare) Lizenz gefunden.
                        
                    <%}%>
                    <br/>
                    <%
                       if (!String.IsNullOrEmpty(imageMaintenanceInfo.LicenseStateHtmlList))
                       { %>
                        <a href="#" tabindex="0" class="AllLicenses" data-content="<%= Html.Raw(imageMaintenanceInfo.LicenseStateHtmlList)%>">Alle gefundenen Lizenzen</a>
                    <% }
                       else
                       { %>
                        Keine Lizenzen gefunden.
                    <% } %>
                    
                    <br/><a href="<%= "/Maintenance/ImageMarkup?imgId=" + imageMaintenanceInfo.ImageId.ToString() %>" target="_blank">Gespeichertes Markup</a>
                    <% if (!String.IsNullOrEmpty(LicenseParser.GetWikiDetailsPageFromSourceUrl(imageMaintenanceInfo.MetaData.SourceUrl))){
                    %> <br/><a href="<%= LicenseParser.GetWikiDetailsPageFromSourceUrl(imageMaintenanceInfo.MetaData.SourceUrl) %>" target="_blank">Bilddetailseite</a><% } %>
                </td>
                <td class="ColumnDescription"></td>
            </tr>

        <% } %>
    </table>
    <script type="text/javascript">
        $(function () {
            $('.AllLicenses').click(function(e) {
                e.preventDefault();
            });
            $('.AllLicenses').popover(
                {
                trigger: "focus",
                placement: "right",
                html: "true",
                }
            );
        }
            );
    </script>

</asp:Content>