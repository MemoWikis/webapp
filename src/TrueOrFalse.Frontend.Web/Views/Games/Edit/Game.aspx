<%@ Page Title="Spielen" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
    Inherits="ViewPage<GameModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Games/Edit/Game.css" rel="stylesheet" />    
    <%= Scripts.Render("~/bundles/js/Games") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
<% using (Html.BeginForm(Model.IsEditing ? "Edit" : "Create", "Game", null, 
    FormMethod.Post, new { enctype = "multipart/form-data", id="EditGameForm", data_is_editing=Model.IsEditing })){%>

    <div class="row">
        <div class="PageHeader col-xs-12">
            <h2 class="pull-left">
                <span class="ColoredUnderline Category">
                    <% if (Model.IsEditing) { %>
                        Spiel bearbeiten
                    <% } else { %>
                        Spiel erstellen
                    <% } %>
                </span>
            </h2>
            <div class="headerControls pull-right">
                <div>
                    <a href="<%= Links.Games(Url) %>" style="font-size: 12px; margin: 0;">
                        <i class="fa fa-list"></i>&nbsp;zur Übersicht
                    </a><br/>
                    <% if(Model.IsEditing){ %>
                        <a href="<%= Links.GameDetail(Url, Model.Id) %>" style="font-size: 12px;">
                            <i class="fa fa-eye"></i>&nbsp;Detailansicht
                        </a> 
                    <% } %>            
                </div>
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
                            <div class="col-md-5">
                                <div class="form-group">
                                    <label class="columnLabel control-label">
                                        <i class="fa fa-clock-o"></i> &nbsp; Startet spätestens in:
                                    </label>
                                    <div class="col-xs-11">
                                        <div class="input-group">
                                            <input class="form-control" value="10" style="height: 30px;" />
                                            <span class="input-group-addon">
                                                (max. 60min)
                                            </span>
                                        </div>
                                    </div>
                                </div>                                
                            </div>
                            <div class="col-md-5    ">
                                <div class="form-group">
                                    <label class="columnLabel control-label">
                                        <i class="fa fa-users"></i> &nbsp; Anzahl Spieler:
                                    </label>
                                    <div class="col-xs-10">
                                        <div class="input-group">
                                            <input class="form-control" value="10" style="height: 30px;" name="amountPlayers" />
                                            <span class="input-group-addon">
                                                (max. 30)
                                            </span>
                                        </div>
                                    </div>
                                </div>                                
                            </div>
                        </div>                        
                                                                                       
                        <div class="form-group">
                            <label class="columnLabel control-label">Fragesätze mit den gespielt wird:</label>
                            <div class="columnControlsFull">
                                <input class="form-control"/>
                            </div>
                        </div>

                        <div class="form-group">
                            <%= Html.LabelFor(m => m.Comment, new { @class = "columnLabel control-label" })%>
                            <div class="columnControlsFull">
                                <%= Html.TextAreaFor(m => m.Comment, new { @class="form-control", placeholder = "Nachricht an deine Mitspieler (optional)", rows = 3})%>
                            </div>
                        </div>                        
                        
                        <div class="form-group">
                            <div class="noLabel columnControlsFull">
                                <% if (Model.IsEditing){ %>
                                    <input type="submit" value="Spiel speichern" class="btn btn-primary" name="btnSave" />
                                    <a href="<%=Url.Action("Delete", "Game") %>" class="btn btn-danger"><i class="fa fa-trash-o"></i> Löschen</a>
                                <% } else { %>
                                    <input type="submit" value="Spiel erstellen " class="btn btn-primary" name="btnSave" />
                                <% } %>
                            </div>
                        </div>

                    </div>
                </div>
            </div>


        </div>

    </div>

<% } %>
</asp:Content>