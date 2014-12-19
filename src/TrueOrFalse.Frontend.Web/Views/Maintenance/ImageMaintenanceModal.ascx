<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TrueOrFalse.ImageMaintenanceInfo>" %>
<%@ Import Namespace="TrueOrFalse" %>

<div id="modalImageMaintenance" class="modal">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <button class="close" data-dismiss="modal">×</button>
                <h3 class="modal-title">Bildverwaltung</h3>
            </div>

            <div class="modal-body" id="modalBody">
                <div class="ImageInfo">
                    <img src="<%= Model.Url_128 %>" style="display: block; float: left; padding-right: 10px; padding-bottom: 5px;"/>
                    <p>
                        <b>Id:</b>
                        <%= Model.ImageId %>
                    </p>
                    <p>
                        <b>Autor geparsed:</b>
                        <% if (!String.IsNullOrEmpty(Model.MetaData.AuthorParsed))
                        { %>
                            <%= Model.MetaData.AuthorParsed %>
                        <% } %>
                    </p>
                    <p>
                        <b>Autor/Attribution manuell:</b>
                        <input id="AuthorManuallyAdded" class="form-control" name="AuthorManuallyAdded" type="text" value="<%= 
                                                        !String.IsNullOrEmpty(Model.ManualImageData.AuthorManuallyAdded) ?
                                                        Model.ManualImageData.AuthorManuallyAdded :
                                                        "" %>">
                    </p>
                    <p>
                        <b>Beschreibung geparsed:</b>
                        <% if (!String.IsNullOrEmpty(Model.MetaData.DescriptionParsed))
                        { %>
                            <%= Model.MetaData.DescriptionParsed %>
                        <% } %>
                    </p>
                     <p>
                        <b>Beschreibung manuell:</b>
                        <textarea id="DescriptionManuallyAdded" class="form-control" name="DescriptionManuallyAdded" type="text" value=""><%=
                                !String.IsNullOrEmpty(Model.ManualImageData.DescriptionManuallyAdded) ?
                                Model.ManualImageData.DescriptionManuallyAdded:
                                ""
                        %></textarea>
                    </p>
                </div>
                <div class="LicenseInfo <%= Model.LicenseStateCssClass%>">
                    <h4>Lizenzen</h4>
                    <p>
                        <% if (Model.MainLicenseAuthorized != null)
                            {
                                %><b>Hauptlizenz: </b><%
                                if (!String.IsNullOrEmpty(Model.MainLicenseAuthorized.LicenseShortName))
                                {%><%=
                                Model.MainLicenseAuthorized.LicenseShortName%>
                                <%} else {%>
                                    <%= Model.MainLicenseAuthorized.WikiSearchString %>
                                <%}%>
                               
                            <%} else if(Model.SuggestedMainLicense != null) {
                                  %><b>vorgeschlagene Hauptlizenz: </b><%
                                if (!String.IsNullOrEmpty(Model.SuggestedMainLicense.LicenseShortName))
                                {%><%=
                                Model.SuggestedMainLicense.LicenseShortName%>
                                <%} else {%>
                                    <%= Model.SuggestedMainLicense.WikiSearchString %>
                                <%}
                            } else { %>
                            Keine (verwendbare) Lizenz gefunden.
                         <%}%>
                    </p>
                    <p>
                        <%
                            if (!String.IsNullOrEmpty(Model.LicenseStateHtmlList))
                            { %>
                            <a href="#" tabindex="1" class="PopoverHover" data-content="<%= Html.Raw(Model.LicenseStateHtmlList)%>">Alle gefundenen Lizenzen</a>
                        <% }
                            else
                            { %>
                            Keine Lizenzen gefunden.
                        <% } %>
                    </p>
                    <p>
                        <a href="<%= "/Maintenance/ImageMarkup?imgId=" + Model.ImageId.ToString() %>" target="_blank">Gespeichertes Markup</a>
                    </p>
                    <% if (!String.IsNullOrEmpty(LicenseParser.GetWikiDetailsPageFromSourceUrl(Model.MetaData.SourceUrl))){%>
                    <p>
                        <a href="<%= LicenseParser.GetWikiDetailsPageFromSourceUrl(Model.MetaData.SourceUrl) %>" target="_blank">Bilddetailseite</a> (Achtung: gespeichertes Markup ist in der Regel älter!)
                    </p>
                    <% } %>
                    <p><b>Status:</b>
                        <%= Model.GlobalLicenseStateMessage %>
                    </p>
                    <p>
                        <b>Hauptlizenz ändern</b>
                        <%= Html.DropDownListFor(m => m.SelectedMainLicenseId, Model.ParsedLicenses, new { @class = "form-control" })%>
                        Hauptlizenz wird nur gespeichert, wenn Bild gleichzeitig freigegeben wird.
                    </p>
                    <p>
                        <b>Freigabe:</b>
                        <select  id="ManualImageEvaluation" class="form-control" name="ManualImageEvaluation">
                            <option value="<%= ManualImageEvaluation.ImageNotEvaluated %>">Nicht evaluiert</option>
                            <option value="<%= ManualImageEvaluation.ImageCheckedForCustomAttributionAndAuthorized %>">Bild geprüft(!) und freigegeben</option>
                            <option value="<%= ManualImageEvaluation.NotAllRequirementsMetYet %>">(Noch) nicht alle Anforderungen erfüllt</option>
                            <option value="<%= ManualImageEvaluation.ImageManuallyRuledOut %>">Bild ausgeschlossen</option>
                        </select>
                    </p>
                    <p>
                        <b>Anmerkungen:</b>
                        <textarea id="Remarks" class="form-control" name="Remarks" type="text" value=""><%=
                                !String.IsNullOrEmpty(Model.ManualImageData.ManualRemarks) ?
                                Model.ManualImageData.ManualRemarks:
                                ""
                        %></textarea>
                    </p>
                    
                </div>
                
                <div class="form-horizontal">
                    <div class="form-group">
                    </div>
                </div>
            </div>

            <div class="modal-footer" id="modalFooter" style="text-align: left;">
                <div class="ButtonContainer float-none-xxs">
                    <a href="#" class="btn btn-default" data-dismiss="modal">Abbrechen</a>
                    <%--<button type="submit" class="btn btn-primary" id="SaveImageData">Bilddaten speichern</button>--%>
                    <button type="submit" class="btn btn-primary" id="SaveImageDataAndClose">Bilddaten speichern und schließen</button>
                </div>
            </div>
        </div>
    </div>
</div>
<script id="ImageMaintenanceModalScript" type="text/javascript">
    $(function() {
        fnInitPopover($('#modalImageMaintenance'));
        new ImageMaintenanceModal(<%= Model.ImageId %>, '<%=Model.ManualImageData.ManualImageEvaluation.ToString() %>');
    });
</script>