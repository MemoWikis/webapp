<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TrueOrFalse.ImageMaintenanceInfo>" %>

<div id="modalImageMaintenance" class="modal">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
<% using (Html.BeginForm("UpdateImage", "Maintenance", null, FormMethod.Post, new {id = "EditQuestionForm", enctype = "multipart/form-data", style = "margin:0px;"}))
   { %>
               
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
                        <b>Beschreibung geparsed:</b>
                        <% if (!String.IsNullOrEmpty(Model.MetaData.DescriptionParsed))
                        { %>
                            <%= Model.MetaData.DescriptionParsed %>
                        <% } %>
                    </p>
                </div>
                <div class="LicenseInfo <%= Model.LicenseStateCssClass%>">
                    <h4>Lizenzen</h4>
                    <p><b>Status:</b>
                        <%= Model.GlobalLicenseStateMessage %>
                    </p>
                    <p>
                        <% if (Model.MainLicense != null)
                        {%>
                            <b>Hauptlizenz: </b><%
                            if (!String.IsNullOrEmpty(Model.MainLicense.LicenseShortName))
                            {%><%=
                                Model.MainLicense.LicenseShortName%>
                            <%} else {%>
                                <%= Model.MainLicense.WikiSearchString %>
                            <%}%>
                               
                        <%} else {%>
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
                    <a href="<%= "/Maintenance/ImageMarkup?imgId=" + Model.ImageId.ToString() %>" target="_blank">Gespeichertes Markup</a>
                    <% if (!String.IsNullOrEmpty(LicenseParser.GetWikiDetailsPageFromSourceUrl(Model.MetaData.SourceUrl))){%>
                        <br/><a href="<%= LicenseParser.GetWikiDetailsPageFromSourceUrl(Model.MetaData.SourceUrl) %>" target="_blank">Bilddetailseite</a>
                    <% } %>
                </div>
                
                <div class="form-horizontal">
                    <div class="form-group">
                    </div>
                </div>
            </div>

            <div class="modal-footer" id="modalFooter" style="text-align: left;">
                <div class="ButtonContainer float-none-xxs">
                    <a href="#" class="btn btn-default" data-dismiss="modal">Abbrechen</a>
                    <a href="#" class="btn btn-primary" id="aSaveImage">Bilddaten speichern</a>
                </div>
            </div>
            <% } %>
        </div>
    </div>
</div>
<script type="text/javascript">
    $(function () {
        $('.PopoverFocus')
            .click(function (e) {
                e.preventDefault();
            })
            .popover(
                {
                    trigger: "focus",
                    placement: "right",
                    html: "true",
                }
            );
        $('.PopoverHover')
            .click(function (e) {
                e.preventDefault();
            })
            .popover(
            {
                trigger: "hover",
                placement: "right",
                html: "true",
            }
        );
    });
</script>