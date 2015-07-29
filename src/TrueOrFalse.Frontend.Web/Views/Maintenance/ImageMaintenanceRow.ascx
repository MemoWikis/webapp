<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ImageMaintenanceInfo>" %>

<tr id="ImgId-<%= Model.MetaData.Id %>" class="<%=Model.LicenseStateCssClass %>">
    <td class="ColumnImage">
        <img src="<%= Model.Url_128 %>" style="width: 50px" />
    </td>                    
    <td class="ColumnInfo">
        <input id="hddImageMaintenanceRowMessage-<%= Model.ImageId %>" class="form-control" name="hddImageMaintenanceRowMessage-<%= Model.ImageId %>" type="hidden" value="<%= !String.IsNullOrEmpty(Model.MaintenanceRowMessage) ? Model.MaintenanceRowMessage : "" %>" />
        Image-Id: <b><%= Model.ImageId %></b><br/>
        <%=  Enum.Parse(typeof(ImageType), Model.MetaData.Type.ToString())  %><br/>
        TypeId: <%= Model.TypeId %>
    </td>
    <td class="ColumnDescription">
        <b>Datei: </b>
        <% if (!String.IsNullOrEmpty(Model.FileName))
        {%>
            <div tabindex="0" class="PopoverHover" data-content="<%= Html.Raw(Model.FileName)%>">
                <%= Model.FileName.TruncateAtWordWithEllipsisText(75, "... <i>[Hover für Volltext]</i>") %>
            </div>
        <%}%>
        <b>Beschreibung: </b>
        <% if (!String.IsNullOrEmpty(Model.Description))
        {%>
            <div tabindex="0" class="Description PopoverHover" data-content="<%= Html.Raw(Model.Description)%>">
                <%= Model.Description.TruncateAtWordWithEllipsisText(100, "... <i>[Hover für Volltext]</i>") %>
            </div>
        <%}%>
    </td>
    <td class="ColumnAuthor">
        <b>Autor: </b>
        <% if (!String.IsNullOrEmpty(Model.Author))
        {%>
            <%= Model.Author %>
        <%}%>
        <br/>
        <b>Attributierung: </b>
        <% if (!String.IsNullOrEmpty(Model.FrontendData.AttributionHtmlString))
        {%>
            <%= Model.FrontendData.AttributionHtmlString %>
        <%}%>
    </td>
    
    <td class="ColumnLicense">
        <a href="#" tabindex="0" class="PopoverHover" data-content="<%= !String.IsNullOrEmpty(Model.GlobalLicenseStateMessage) ? Html.Raw(Model.GlobalLicenseStateMessage).ToString() : ""%>">Status</a>
        <br/>
        <% if (Model.MainLicenseAuthorized != null)
            {
                %>Hauptlizenz:<br/><%
                if (!String.IsNullOrEmpty(Model.MainLicenseAuthorized.LicenseShortName))
                {%><%=
                Model.MainLicenseAuthorized.LicenseShortName%>
                <%} else {%>
                    <%= Model.MainLicenseAuthorized.WikiSearchString %>
                <%}%>
                               
            <%} else if(Model.SuggestedMainLicense != null) {
                  %>vorgeschlagene Hauptlizenz:<br/><%
                if (!String.IsNullOrEmpty(Model.SuggestedMainLicense.LicenseShortName))
                {%><%=
                Model.SuggestedMainLicense.LicenseShortName%>
                <%} else {%>
                    <%= Model.SuggestedMainLicense.WikiSearchString %>
                <%}
            } else { %>
            Keine (verwendbare) Lizenz gefunden.
         <%}%>
        <br/>
        <%
            if (!String.IsNullOrEmpty(Model.LicenseStateHtmlList))
            { %>
            <a href="#" tabindex="0" class="PopoverHover" data-content="<%= Html.Raw(Model.LicenseStateHtmlList)%>">Alle gefundenen Lizenzen</a>
        <% }
            else
            { %>
            Keine Lizenzen gefunden.
        <% } %>
                    
        <br/><a href="<%= "/Maintenance/ImageMarkup?imgId=" + Model.ImageId.ToString() %>" target="_blank">Gespeichertes Markup</a>
        <% if (!String.IsNullOrEmpty(LicenseParser.GetWikiDetailsPageFromSourceUrl(Model.MetaData.SourceUrl))){
        %> <br/><a href="<%= LicenseParser.GetWikiDetailsPageFromSourceUrl(Model.MetaData.SourceUrl) %>" target="_blank">Bilddetailseite</a><% } %>
                    
        <br/><a data-image-id="<%= Model.ImageId %>" class="btn btn-xs ImageMaintenanceModal" href="#">Verwaltung (ImageModal)</a>
    </td>
</tr>
            