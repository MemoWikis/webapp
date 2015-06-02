<%@ Page Title="Spielen" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
    Inherits="ViewPage<EditDateModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/EditDate") %>
    <%= Scripts.Render("~/bundles/js/EditDate") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
<% using (Html.BeginForm(Model.IsEditing ? "Edit" : "Create", "EditDate", null, 
    FormMethod.Post, new { enctype = "multipart/form-data", id="EditGameForm"})){%>

    <div class="row">
        <div class="PageHeader col-xs-12">
            <h2 class="pull-left">
                <span class="ColoredUnderline Play">
                    Termin erstellen
                </span>
            </h2>
            <div class="headerControls pull-right">
                <div>
                    <a href="<%= Links.Games(Url) %>" style="font-size: 12px; margin: 0;">
                        <i class="fa fa-list"></i>&nbsp;zur Übersicht
                    </a>          
                </div>
            </div>
        </div>
    </div>
        
        <div class="row">
            <div class="col-md-9">
                <% if(!Model.IsLoggedIn){ %>
                    <div class="bs-callout bs-callout-danger" style="margin-top: 0;">
                        <h4>Anmelden oder registrieren</h4>
                        <p>
                            Um Termine zu erstellen,
                            musst du dich <a href="/Anmelden">anmelden</a> oder dich <a href="/Registrieren">registrieren</a>.
                        </p>
                    </div>
                <% }%>
            </div>            
        </div>
        
        <div class="row">
            <div class="col-md-9 xxs-stack">
                <% Html.Message(Model.Message); %>
            </div>
            
            <div class="col-md-9">
                <div class="form-horizontal">
                    <div class="FormSection">
                        
                        <div class="row">
                            <div class="col-lg-4">
                                <div class="form-group">
                                    <label class="columnLabel control-label">
                                        <i class="fa fa-clock-o"></i> &nbsp; Datum
                                    </label>
                                    <div class="col-md-11 col-xs-6">
                                        <div class="input-group">
                                            <input class="form-control" name="StartsInMinutes" value="10" style="height: 30px;" />
                                            <span class="input-group-addon" style="height: 30px;">
                                                (max. 60min)
                                            </span>
                                        </div>
                                    </div>
                                </div>                                
                            </div>
                            <div class="col-lg-4">
                                <div class="form-group">
                                    <label class="columnLabel control-label">
                                        <i class="fa fa-clock-o"></i> &nbsp; Uhrzeit
                                    </label>
                                    <div class="col-md-10 col-xs-6">
                                        <div class="input-group">
                                            <input class="form-control" name="MaxPlayers" value="10" style="height: 30px;" name="amountPlayers" />
                                            <span class="input-group-addon" style="height: 30px;">
                                                (max. 30)
                                            </span>
                                        </div>
                                    </div>
                                </div>                                
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="columnLabel control-label">
                                Fragesätze die zu diesem Termin gewust werden sollen:
                            </label>
                            <div class="columnControlsFull">
                                <input class="form-control" name="Sets"/>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <label class="columnLabel control-label">
                                Öffentlich?
                            </label>
                            <div class="columnControlsFull">
                                <select class="form-control">
                                    <option>Sichtbar für dein Netzwerk (+20 Reputation).</option>
                                    <option>Privat. Nur für dich sichtbar.</option>
                                </select>
                            </div>
                        </div>

                        <div class="form-group">
                            <%= Html.LabelFor(m => m.Details, new { @class = "columnLabel control-label" })%>
                            <div class="columnControlsFull">
                                <%= Html.TextAreaFor(m => m.Details, 
                                    new
                                    {
                                        @class="form-control", 
                                        placeholder = "(kurze) Beschreibung des Termins", 
                                        rows = 2
                                    })%>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <div class="noLabel columnControlsFull">
                                <input type="submit" value="Spiel erstellen " class="btn btn-primary" name="btnSave" 
                                    <% if(!Model.IsLoggedIn){ %> disabled="disabled" <% } %> />
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
<% } %>
</asp:Content>