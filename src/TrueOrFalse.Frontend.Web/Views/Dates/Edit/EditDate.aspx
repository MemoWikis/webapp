﻿<%@ Page Title="Termin erstellen" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
    Inherits="ViewPage<EditDateModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/EditDate") %>
    <%= Scripts.Render("~/bundles/js/EditDate") %>
    
    <script type="text/javascript">
        $(function() {
            $('.clockpicker').clockpicker();

            $('.input-group.date').datepicker({
                language: "de",
                calendarWeeks: true,
                autoclose: true,
                todayHighlight: true,
                startDate: new Date().toString(),
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
<% using (Html.BeginForm(Model.IsEditing ? "Edit" : "Create", "EditDate", null, 
    FormMethod.Post, new { enctype = "multipart/form-data", id="EditGameForm"})){%>
    
    <input type="hidden" name="DateId" value="<%= Model.DateId %>"/>

    <div class="row">
        <div class="PageHeader col-xs-12">
            <h2 class="pull-left">
                <span class="ColoredUnderline Play">
                    <% if(Model.IsEditing){ %>
                        Termin bearbeiten
                    <% } else { %>
                        Termin erstellen
                    <% } %>
                </span>
            </h2>
            <div class="headerControls pull-right">
                <div>
                    <a href="<%= Links.Dates(Url) %>" style="font-size: 12px; margin: 0;">
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
                            musst du dich <a href="/Anmelden">anmelden</a> oder <a href="/Registrieren">registrieren</a>.
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
                                        Datum
                                    </label>
                                    <div class="col-md-11 col-xs-6">
                                        <div class="input-group date">
                                            <input class="form-control" name="Date" value="<%= Model.Date.ToString("dd.MM.yyyy") %>" style="height: 30px;" />
                                            <span class="input-group-addon" style="height: 30px;">
                                                <i class="fa fa-calendar"></i>
                                            </span>
                                        </div>
                                    </div>
                                </div>                                
                            </div>
                            <div class="col-lg-3">
                                <div class="form-group">
                                    <label class="columnLabel control-label">
                                        Uhrzeit
                                    </label>
                                    <div class="col-md-10 col-xs-6">
                                        <div class="input-group clockpicker" data-autoclose="true">
                                            <input class="form-control" name="Time" value="<%= Model.Time %>" style="height: 30px;" name="amountPlayers" />
                                            <span class="input-group-addon" style="height: 30px;">
                                                <i class="fa fa-clock-o"></i>
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
                            <div class="JS-Sets columnControlsFull">
                                <script type="text/javascript">
                                    $(function () {
                                        <%foreach (var set in Model.Sets) { %>
                                        $("#txtSet")
                                            .val('<%=set.Name %>')
                                            .data('set-id', '<%=set.Id %>')
                                            .trigger("initSetFromTxt");
                                        <% } %>
                                    });
                                </script>
                                <div class="JS-SetInputContainer ControlInline ">
                                    <input id="txtSet" class="form-control .JS-ValidationIgnore" type="text" placeholder="Ordne einen Fragesatz zu"  />
                                </div>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <label class="columnLabel control-label">
                                Öffentlich?
                            </label>
                            <div class="columnControlsFull">
                                <select class="form-control" name="Visibility">
                                    <option value="inNetwork" <%= Model.Selected("inNetwork") %>>Sichtbar für dein Netzwerk (+10 Reputation je Kopie).</option>
                                    <option value="private" <%= Model.Selected("private") %>>Privat. Nur für dich sichtbar.</option>
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
                                <input type="submit" class="btn btn-primary" name="btnSave" 
                                    value="<%= Model.IsEditing ? "Termin bearbeiten" :  "Termin erstellen " %>"
                                    <% if(!Model.IsLoggedIn){ %> disabled="disabled" <% } %> />
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
<% } %>
</asp:Content>