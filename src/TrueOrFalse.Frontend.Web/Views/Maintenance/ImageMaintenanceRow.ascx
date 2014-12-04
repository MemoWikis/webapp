<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TrueOrFalse.ImageMaintenanceInfo>" %>
<%@ Import Namespace="TrueOrFalse" %>

<tr class="<%=Model.GetImageLicenseStateCssClass() %>">
                <td class="ColumnImage">
                    <img src="<%= Model.Url_128 %>" style="width: 50px" />
                </td>                    
                <td class="ColumnInfo">
                    <%=  Enum.Parse(typeof(ImageType), Model.MetaData.Type.ToString())  %><br/>
                    ImageId: <%= Model.ImageId %><br/>
                    TypeId: <%= Model.TypeId %>
                </td>
                <td class="ColumnAuthor"><%= Model.MetaData.AuthorParsed %></td>
                <td class="ColumnLicense">
                    
                    <% if (Model.MainLicense != null)
                       {
                           %>Hauptlizenz:<br/><%
                           if (!String.IsNullOrEmpty(Model.MainLicense.LicenseShortName))
                           {%><%=
                           Model.MainLicense.LicenseShortName%>
                           <%} else {%>
                                <%= Model.MainLicense.WikiSearchString %>
                            <%}%>
                               
                       <%} else {%>
                        Keine (verwendbare) Lizenz gefunden.
                        
                    <%}%>
                    <br/>
                    <%
                       if (!String.IsNullOrEmpty(Model.LicenseStateHtmlList))
                       { %>
                        <a href="#" tabindex="0" class="AllLicenses" data-content="<%= Html.Raw(Model.LicenseStateHtmlList)%>">Alle gefundenen Lizenzen</a>
                    <% }
                       else
                       { %>
                        Keine Lizenzen gefunden.
                    <% } %>
                    
                    <br/><a href="<%= "/Maintenance/ImageMarkup?imgId=" + Model.ImageId.ToString() %>" target="_blank">Gespeichertes Markup</a>
                    <% if (!String.IsNullOrEmpty(LicenseParser.GetWikiDetailsPageFromSourceUrl(Model.MetaData.SourceUrl))){
                    %> <br/><a href="<%= LicenseParser.GetWikiDetailsPageFromSourceUrl(Model.MetaData.SourceUrl) %>" target="_blank">Bilddetailseite</a><% } %>
                    
                    <br/><a data-image-id ="<%= Model.ImageId %>" class="ImageModal" href="#">ImageModal</a>
                </td>
                <td class="ColumnDescription">
                    <% if(!String.IsNullOrEmpty(Model.MetaData.DescriptionParsed)) %>
                </td>
            </tr>