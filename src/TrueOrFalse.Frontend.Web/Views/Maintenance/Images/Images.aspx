<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="System.Web.Mvc.ViewPage<ImagesModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Scripts.Render("~/bundles/js/MaintenanceImages") %>
    <%= Styles.Render("~/bundles/MaintenanceImages") %>
      <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Administrativ", Url = "/Maintenance", ToolTipText = "Administrativ"});
         Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Bilder", Url = "/MaintenanceImages/Images", ToolTipText = "Bilder"});
        Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
</asp:Content>

<asp:Content ID="MaintenanceImages" ContentPlaceHolderID="MainContent" runat="server">
   
<form method="POST" action="/MaintenanceImages/Images">
    <div style="margin:0 0 0 -10px; position: relative;" class="container-fluid">
        <nav class="navbar navbar-default" style="" role="navigation">
            <div class="container">
                <a class="navbar-brand" href="#">Maintenance</a>
                <ul class="nav navbar-nav">
                    <li><a href="/Maintenance">Allgemein</a></li>
                    <li class="active"><a href="/MaintenanceImages/Images">Bilder</a></li>
                    <li><a href="/Maintenance/Messages">Nachrichten</a></li>
                    <li><a href="/Maintenance/Tools">Tools</a></li>
                    <li><a href="/Maintenance/CMS">CMS</a></li>
                    <li><a href="/Maintenance/ContentCreatedReport">Cnt-Created</a></li>
                    <li><a href="/Maintenance/Statistics">Stats</a></li>
                </ul>
            </div>
        </nav>
    </div>
    <% Html.Message(Model.Message); %>

    <a href="/MaintenanceImages/ReparseMarkup_OfNoneAuthorized" class="btn btn-success" style="margin-bottom: 10px; margin-top: -5px;">
        Markup für Bilder ohne Hauptlizenz parsen...
    </a>
        
    <a href="/MaintenanceImages/ReparseMarkup_OfNoneAuthorized_AndLoadFromWikipedia" class="btn btn-success" style="margin-bottom: 10px; margin-left: -2px; margin-top: -5px;">
        ... und von Wikimedia laden
    </a>
    
    <a href="/MaintenanceImages/SetAllImageLicenseStati" class="btn btn-success" style="margin-bottom: 10px; margin-top: -5px;" >Set stati</a>
    
    <div class="row">
        <div class="col-lg-6">
            <div style="float:left">
                Zeige:
            </div>            
            <div style="float:left">
                <ul style="list-style-type: none;" id="ulLicenseStatus">
                    <li class="open">
                        <label>
                            <%= Html.CheckBoxFor(m => m.CkbOpen) %>
                            offene 
                        </label>                    
                    </li>
                    <li class="approved">
                        <label>
                            <%= Html.CheckBoxFor(m => m.CkbApproved) %>
                            bestätigte 
                        </label>                    
                    </li>
                    <li class="excluded">
                        <label>
                            <%= Html.CheckBoxFor(m => m.CkbExcluded) %>
                            ausgeschlossene 
                        </label>                    
                    </li>
                </ul>
                <div style="margin-left: 14px; margin-top: -5px;">
                    Treffer Gesamt: <%= Model.TotalResults %>       
                </div>
            </div>
        </div>
        <div class="col-lg-6">
            <% Html.RenderPartial("Pager", Model.Pager); %><br />
        </div>
    </div>

    <table class="ImageTable table">
        <tr>
            <th class="ColumnImage"></th>
            <th class="ColumnInfo"></th>
            <th class="ColumnDescription">Beschreibung</th>
            <th class="ColumnAuthor">Lizenzinfos</th>
            <th class="ColumnLicense">Lizenzverwaltung</th>
        </tr>
        <%  var index = 0;
            foreach(var imageMaintenanceInfo in Model.ImageMaintenanceInfos){ index++; %>
               <% Html.RenderPartial("/Views/Maintenance/Images/ImageMaintenanceRow.ascx", imageMaintenanceInfo); %>
        <% } %>
    </table>

    <% Html.RenderPartial("Pager", Model.Pager); %>
    
    <a href="/MaintenanceImages/ReparseMarkup_All_AndLoadFromWikipedia" class="btn btn-warning" style="margin-bottom: 10px; margin-top: -5px;" disabled>Markup von Wikimedia für alle laden und parsen</a>
    <br/><a href="/MaintenanceImages/ParseMarkupFromDb" class="btn btn-primary" style="margin-bottom: 10px; margin-top: -5px;" disabled>Markup aus lokaler DB parsen</a>

    <script type="text/javascript">
        $(function () {
            fnInitImageMaintenanceModal($('.ImageMaintenanceModal'));
            fnInitPopover($('body'));
        });
    </script>
    
</form>
</asp:Content>