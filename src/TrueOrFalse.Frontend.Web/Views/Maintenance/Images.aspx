<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<MaintenanceImagesModel>" %>
<%@ Import Namespace="System.Activities.Statements" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="Newtonsoft.Json" %>
<%@ Import Namespace="TrueOrFalse" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/Views/Maintenance/Images.css") %>
</asp:Content>

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
            <th class="ColumnDescription">Beschreibung</th>
            <th class="ColumnLicense">Lizenz</th>
        </tr>
        <%  var index = 0;
            foreach(var imageMaintenanceInfo in Model.ImageMaintenanceInfos){ index++; %>
        
               <% Html.RenderPartial("ImageMaintenanceRow", imageMaintenanceInfo); %>

        <% } %>
    </table>

    <script type="text/javascript">
        $(function () {
            $('.ImageModal').click(
                function (e) {
                    e.preventDefault();
                    $.ajax({
                        type: 'POST',
                        url: "/Maintenance/ImageModal?imgId=" + $(this).attr('data-image-id'),
                        success: function (result) {
                            $('#modalImageMaintenance').remove();
                            $(result).insertAfter($('table.ImageTable'));
                            $('#modalImageMaintenance').modal('show');
                        },
                    });
                }
            );

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
            }
        );
    </script>
</asp:Content>