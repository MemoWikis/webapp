<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ImageMaintenanceInfo>" %>

<div id="modalImageMaintenance" class="modal">
    <div class="modal-dialog modal-lg" style="width: 1200px">
        <div class="modal-content">
            <div class="modal-body" id="modalBody">
    
                <div class="ImageInfo">
                    <img src="<%= Model.FrontendData.GetImageUrl(350).Url %>" id="Image" />
                    <div class="FluidColumn">
                        <p>
                            <b>Id:</b>
                            <%= Model.ImageId %>
                        </p>
                        <p>
                            <b>Autor geparsed:</b>
                            <% if (!String.IsNullOrEmpty(Model.MetaData.AuthorParsed)){ %>
                                <span style="background-color: greenyellow"><%= Model.MetaData.AuthorParsed %></span>
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
                            <% if (!String.IsNullOrEmpty(Model.MetaData.DescriptionParsed)){ %>
                                <span style="background-color: greenyellow"><%= Model.MetaData.DescriptionParsed %></span>
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
                        <% if (Model.FrontendData.ImageMetaDataExists
                               && Model.FrontendData.ImageParsingNotifications.GetAllNotifications().Any())
                           {%>
                            <div>
                                <p>
                                    <b>Autoparsing-Benachrichtigungen:</b><br/>
                                </p>
                               <% foreach (var notification in Model.FrontendData.ImageParsingNotifications.GetAllNotifications()){
                                   if(!String.IsNullOrEmpty(notification.NotificationText))%>
                                    <ul>
                                        <li><%= notification.NotificationText %></li>
                                    </ul>
                               <%}%>
                            </div>
                             
                           <% } %>
                        
                        
                <div class="ModalLicenseInfo <%= Model.LicenseStateCssClass%>">
                    <h4>Lizenzen</h4>
                    
                    <div class="row">
                        <div class="col-lg-6">
                            <% if (Model.MainLicenseAuthorized != null){ %>
                                <b>Hauptlizenz: </b>
                                <% if (!String.IsNullOrEmpty(Model.MainLicenseAuthorized.LicenseShortName)){ %> <%=
                                    Model.MainLicenseAuthorized.LicenseShortName%>
                                    <%} else {%>
                                        <%= Model.MainLicenseAuthorized.WikiSearchString %>
                                    <%}%>
                                <%} else if(Model.SuggestedMainLicense != null) { %>
                                    <b>vorgeschlagene Hauptlizenz: </b>
                                    <% if (!String.IsNullOrEmpty(Model.SuggestedMainLicense.LicenseShortName)){%>
                                        <%=Model.SuggestedMainLicense.LicenseShortName%>
                                    <%} else {%>
                                        <%= Model.SuggestedMainLicense.WikiSearchString %>
                                    <%}
                                } else { %>
                                    Keine (verwendbare) Lizenz gefunden.
                                <%}%>
                            
                                <br />
                                <% if (!String.IsNullOrEmpty(Model.LicenseStateHtmlList)) { %>
                                    <a href="#" tabindex="1" class="PopoverHover" data-content="<%= Html.Raw(Model.LicenseStateHtmlList)%>">Alle gefundenen Lizenzen</a>
                                <% } else { %>
                                    Keine Lizenzen gefunden.
                                <% } %>
                        </div>
                        <div class="col-lg-6">
                            <a href="<%= "/MaintenanceImages/ImageMarkup?imgId=" + Model.ImageId.ToString() %>" target="_blank">Gespeichertes Markup</a>
                            <br />
                            <% if (!String.IsNullOrEmpty(LicenseParser.GetWikiDetailsPageFromSourceUrl(Model.MetaData.SourceUrl))){%>
                                <a href="<%= LicenseParser.GetWikiDetailsPageFromSourceUrl(Model.MetaData.SourceUrl) %>" target="_blank">Wiki-Bilddetailseite</a> (<i class="fa fa-exclamation-triangle"></i> gespeichertes Markup ist in der Regel älter!) <br />
                            <% } %>      
                            <a href="<%= Model.TypeUrl %>" target="_blank"><%=  Enum.Parse(typeof(ImageType), Model.MetaData.Type.ToString())  %>, TypeId: <%= Model.TypeId %></a>                      
                            <div>
                                <% if (Model.TypeNotFound) { %>
                                    <i class="fa fa-exclamation-triangle">&nbsp;</i>Typ nicht gefunden.
                                <% } %>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12" style="margin-top: 6px; margin-bottom: 6px;">
                            <b>Status:</b> <%= Model.GlobalLicenseStateMessage %>                            
                        </div>
                    </div>

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

                    </div>
                    
                    

                </div>
                
                

            </div>

            <div class="modal-footer" id="modalFooter" style="text-align: left;">
                <div class="col-lg-6">
                    <% var dataContent = @"<span style='color: red'>ACHTUNG! Wirklich neu laden? Bitte vorher sicherstellen, dass das Bild mit dem alten übereinstimmt.</span>"; %>
                    <a id="ReloadImage" class="btn btn-danger memo-button" 
                        tabindex="0" role="button"
                        data-toggle="popover" data-trigger="hover" data-content="<%= dataContent %>">
                        <i class="fa fa-refresh"></i> Bild neu laden
                    </a>
                    <% dataContent = @"<span style='color: red'>ACHTUNG! Das Bild wird unwiederbringlich gelöscht.</span>"; %>
                    <a id="DeleteImage" class="btn btn-danger memo-button" 
                        tabindex="0" role="button"
                        data-toggle="popover" data-trigger="hover" data-content="<%= dataContent %>">
                        <i class="fa fa-trash-o"></i>&nbsp; Bild löschen
                    </a>
                </div>
                <div class="col-lg-6" style="text-align: right;">
                    <a href="#" class="btn btn-default memo-button" data-dismiss="modal">Abbrechen</a>
                    <%--<button type="submit" class="btn btn-primary" id="SaveImageData">Bilddaten speichern</button>--%>
                    <button type="submit" class="btn btn-primary memo-button" id="SaveImageDataAndClose">Bilddaten speichern und schließen</button>       
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