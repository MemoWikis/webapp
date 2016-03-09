<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<CMSModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <nav class="navbar navbar-default" style="" role="navigation">
        <div class="container">
            <a class="navbar-brand" href="#">Maintenance</a>
            <ul class="nav navbar-nav">
                <li><a href="/Maintenance">Allgemein</a></li>
                <li><a href="/Maintenance/Images">Bilder</a></li>
                <li><a href="/Maintenance/Messages">Nachrichten</a></li>
                <li><a href="/Maintenance/Tools">Tools</a></li>
                <li class="active"><a href="/Maintenance/CMS">CMS</a></li>
            </ul>
        </div>
    </nav>
    <% Html.Message(Model.Message); %>
        
    <div class="row">
        <div class="col-md-10">
            <h4 class="">CMS</h4>
        </div>
    </div>
    <div>
        <% using (Html.BeginForm("CMS", "Maintenance")){%>
        
            <div class="form-group">
                <label class="control-label"><span style="font-weight: bold">Vorgeschlagene Spiele</span> (Kategorien-Ids kommasepariert)</label>
                <%= Html.TextBoxFor(m => m.SuggestedGames, new {@class="form-control"} ) %>    
            </div>
        
            <input type="submit" value="Speichern" class="btn btn-primary" name="btnSave" />

        <% } %>
    </div>

</asp:Content>