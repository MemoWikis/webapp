<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TrueOrFalse.ImageMaintenanceInfo>" %>
<%@ Import Namespace="TrueOrFalse" %>

<tr id="ImgId-<%= Model.MetaData.Id %>" class="<%=Model.LicenseStateCssClass %>">
    <td class="ColumnImage">
        <img src="<%= Model.Url_128 %>" style="width: 50px" />
    </td>                    
    <td class="ColumnInfo">
        <%=  Enum.Parse(typeof(ImageType), Model.MetaData.Type.ToString())  %><br/>
        ImageId: <%= Model.ImageId %><br/>
        TypeId: <%= Model.TypeId %>
        <br/>TEST: <%= Model.Test %>
    </td>
    <td class="ColumnAuthor">
        <% if (!String.IsNullOrEmpty(Model.Author))
        {%>
            <%= Model.Author %>
        <%}%>
    </td>
     <td class="ColumnDescription">
        <% if (!String.IsNullOrEmpty(Model.Description))
        {%>
            <div tabindex="0" class="Description PopoverHover" data-content="<%= Html.Raw(Model.Description)%>">
                <%= Model.Description.TruncateAtWordWithEllipsisText(150, "... [Hover für Volltext]") %>
            </div>
        <%}%>
    </td>
    <td class="ColumnLicense">
        <a href="#" tabindex="0" class="PopoverHover" data-content="<%= !String.IsNullOrEmpty(Model.GlobalLicenseStateMessage) ? Html.Raw(Model.GlobalLicenseStateMessage).ToString() : ""%>">Status</a>
        <br/>
        <% if (Model.MainLicense != null)
            {
                %>Hauptlizenz:<br/><%
                if (!String.IsNullOrEmpty(Model.MainLicense.LicenseShortName))
                {%><%=
                Model.MainLicense.LicenseShortName%>
                <%} else {%>
                    <%= Model.MainLicense.WikiSearchString %>
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
                    
        <br/><a data-image-id ="<%= Model.ImageId %>" class="ImageModal" href="#">Verwaltung (ImageModal)</a>
    </td>
</tr>
            