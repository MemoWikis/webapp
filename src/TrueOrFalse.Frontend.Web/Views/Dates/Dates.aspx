<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<DatesModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">    

    <div class="row">
        <div class="PageHeader col-md-9">
            <h2 style="margin-bottom: 20px; margin-top: 0px;" class="pull-left">
                <span class="ColoredUnderline Date">Deine Termine</span>
            </h2> 
        </div>
        <div class="col-md-3">
            <div class="headerControls pull-right">
                <div style="padding-top: 5px;">
                    <a href="<%= Url.Action("Create", "EditDate") %>" class="btn btn-sm pull-right">
                        <i class="fa fa-plus-circle"></i> &nbsp; Termin erstellen
                    </a>
                </div>
            </div>
        </div>
    </div>
        
    <div class="row">
        <div class="col-md-9">

            <% if(!Model.IsLoggedIn){ %>

                <div class="bs-callout bs-callout-danger">
                    <h4>Anmelden oder registrieren</h4>
                    <p>Um Termine zu erstellen, musst du dich <a href="/Anmelden">anmelden</a> 
                       oder dich <a href="/Registrieren">registrieren</a>.</p>
                </div>

            <% }else{ %>
        
                <% if (!Model.Dates.Any()){ %>
                    <div class="bs-callout bs-callout-info"  
                        style="margin-top: 0; margin-bottom: 50px;">
                        <h4>Du hast bisher keine Termine</h4>
                        <p style="padding-top: 5px;">
                            Termine helfen dir dabei, dich optimal auf eine Prüfung vorzubereiten.
                        </p>
                        <p>
                            <a href="<%= Url.Action("Create", "EditDate") %>" class="btn btn-sm" style="margin-top: 10px;">
                                <i class="fa fa-plus-circle"></i> &nbsp; Termin erstellen
                            </a>
                        </p>
                    </div>
                <% } else { %>
                    <% foreach(var date in Model.Dates){ %>
                        <% Html.RenderPartial("DateRow", new DateRowModel(date)); %>
                    <% } %>                
                <% } %>

                <h3 style="margin-bottom: 10px;">
                    <span class="ColoredUnderline Date" style="padding-right: 3px;">Termine im Netzwerk</span>
                </h3>
    
                <div class="bs-callout bs-callout-info"  style="margin-top: 0; <%= Html.CssHide(Model.DatesInNetwork.Any()) %>">
                    <h4>Keine kommenden Termine im Netzwerk</h4>
                    <p>
                        <a href="<%= Url.Action("Create", "EditDate") %>" class="btn btn-sm" style="margin-top: 10px;">
                            <i class="fa fa-plus-circle"></i> &nbsp; Netzwerk erweitern
                        </a>
                    </p>
                </div>

            <% } %>

        </div>        
        <div class="col-md-3"></div>
    </div>
            
</asp:Content>