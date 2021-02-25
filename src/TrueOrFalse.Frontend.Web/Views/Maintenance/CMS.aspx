<%@ Page Title="CMS" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="System.Web.Mvc.ViewPage<CMSModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="Head">
    <%= Scripts.Render("~/bundles/js/MaintenanceCMS") %>
      <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Administrativ", Url = "/Maintenance", ToolTipText = "Administrativ"});
         Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "CMS", Url = "/Maintenance/CMS", ToolTipText = "CMS"});
        Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
    <meta id="blablabla"/>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <nav class="navbar navbar-default" style="" role="navigation">
        <div class="container">
            <a class="navbar-brand" href="#">Maintenance</a>
            <ul class="nav navbar-nav">
                <li><a href="/Maintenance">Allgemein</a></li>
                <li><a href="/MaintenanceImages/Images">Bilder</a></li>
                <li><a href="/Maintenance/Messages">Nachrichten</a></li>
                <li><a href="/Maintenance/Tools">Tools</a></li>
                <li class="active"><a href="/Maintenance/CMS">CMS</a></li>
                <li><a href="/Maintenance/ContentCreatedReport">Cnt-Created</a></li>
                <li><a href="/Maintenance/Statistics">Stats</a></li>
            </ul>
        </div>
    </nav>
    <% Html.Message(Model.Message); %>

    <div>
        <h2>Tools zur Content-Pflege</h2>
        
        <div id="categoryNetworkNavigationWrapper">
            <h4>Themen-Navigation</h4>
            <a href="#" class="networkNavigationUpdate" data-category-id="682"><span class="label label-category">Schule</span></a>
            <a href="#" class="networkNavigationUpdate" data-category-id="687"><span class="label label-category">Studium</span></a>
            <a href="#" class="networkNavigationUpdate" data-category-id="689"><span class="label label-category">Zertifikate</span></a>
            <a href="#" class="networkNavigationUpdate" data-category-id="709"><span class="label label-category">Allgemeinwissen</span></a>

            <div id="categoryNetworkNavigation">
                <% Html.RenderPartial("~/Views/Categories/Navigation/CategoryNetworkNavigation.ascx", new CategoryNetworkNavigationModel(709)); %>
            </div>
        </div>
        

        <div id="showLooseCategories">
            <h4 style="margin-top: 40px;">Lose Themen</h4>
            <p>
                Themen anzeigen, die nicht in eines der vier Oberthemen eingehangen sind: 
                <a href="#" id="btnShowLooseCategories" class="btn btn-default">Themen anzeigen</a>
            </p>
            <div id="showLooseCategoriesResult" style="margin-left: 25px; padding: 0 10px 10px;"></div>
        </div>

        <div id="showCategoriesWithNonAggregatedChildren">
            <h4 style="margin-top: 40px;">Themen mit unbearbeitetem Aggregierungsstatus</h4>
            <p>
                Themen anzeigen, die Unterthemen haben, über deren Aggregierungs-Status noch nicht entschieden ist: 
                <a href="#" id="btnShowCategoriesWithNonAggregatedChildren" class="btn btn-default">Themen anzeigen</a>
            </p>
            <div id="showCategoriesWithNonAggregatedChildrenResult" style="margin-left: 25px; padding: 0 10px 10px;"></div>
        </div>

        <div id="showCategoriesInSeveralRootCategories">
            <h4 style="margin-top: 40px;">Themen in verschiedenen Bäumen</h4>
            <p>
                Themen anzeigen, die in mind. 2 der Root-Kategorien eingehangen sind: 
                <a href="#" id="btnShowCategoriesInSeveralRootCategories" class="btn btn-default">Themen anzeigen</a>
            </p>
            <div id="showCategoriesInSeveralRootCategoriesResult" style="margin-left: 25px; padding: 0 10px 10px;"></div>
        </div>
    </div>

</asp:Content>