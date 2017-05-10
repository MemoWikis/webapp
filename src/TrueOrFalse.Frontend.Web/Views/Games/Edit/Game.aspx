<%@ Page Title="Spielen" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
    Inherits="ViewPage<GameModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <link rel="canonical" href="<%= Settings.CanonicalHost %><%= Links.GameCreate() %>">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/Game") %>
    <%= Scripts.Render("~/bundles/js/Game") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
<% using (Html.BeginForm("Create", "Game", null, 
    FormMethod.Post, new { enctype = "multipart/form-data", id="EditGameForm"})){%>

    <div class="row">
        <div class="PageHeader col-xs-12">
            <h2 class="pull-left">
                <span class="ColoredUnderline Play">
                    Spiel erstellen
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
        <div class="col-sm-9">
            <% if(!Model.IsLoggedIn){ %>
                <div class="bs-callout bs-callout-danger" style="margin-top: 0;">
                    <h4>Einloggen oder registrieren</h4>
                    <p>
                        Um Spiele zu erstellen,
                        musst du dich <a href="#" data-btn-login="true">einloggen</a> oder <a href="/Registrieren">registrieren</a>.
                    </p>
                </div>
            <% }%>
        </div>            
    </div>
        
    <div class="row">
        <div class="col-md-3 col-md-push-9">
            <div class="alert alert-warning">
                <p>
                    <strong>Spiele im Test-Betrieb:</strong> Der Echtzeit-Quiz-Modus befindet sich noch im Testbetrieb. Das heißt, es kann manchmal zu unerwarteten Fehlern kommen. 
                    Spaß macht es aber trotzdem - also probiere es ruhig aus!
                </p>
            </div>
        </div>
        <div class="col-md-9 col-md-pull-3">
            <div>
                <% Html.Message(Model.Message); %>
            </div>
            <div class="form-horizontal">
                <div class="FormSection">
                    <div class="form-group">
                        <label class="columnLabel control-label">
                            <span class="show-tooltip" title="Wähle hier die Fragesätze, zu denen zu spielen möchtest">
                                <strong>Fragesätze, mit denen gespielt wird:</strong> <i class="fa fa-question-circle"></i>
                            </span>
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
                                    InitLabelTooltips();
                                });
                            </script>
                            <div class="JS-SetInputContainer ControlInline ">
                                <input id="txtSet" class="form-control .JS-ValidationIgnore" type="text" placeholder="Beginne zu tippen"  />
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6 col-sm-12">
                            <div class="form-group">
                                <label class="columnLabel control-label">
                                    <i class="fa fa-clock-o"></i> &nbsp; Startet spätestens in: 
                                    &nbsp;<i class="fa fa-question-circle show-tooltip" title="Gib hier an, wie lange du maximal auf Mitspieler warten möchtest." data-placement="right"></i>
                                </label>
                                <div class="col-md-11 col-sm-8 col-xs-11">
                                    <div class="input-group">
                                        <input class="form-control" name="StartsInMinutes" value="5" style="height: 30px;" />
                                        <span class="input-group-addon" style="height: 30px;">
                                            (max. 10min)
                                        </span>
                                    </div>
                                </div>
                            </div>                                
                        </div>
                        <div class="col-md-6 col-sm-12">
                            <div class="form-group">
                                <label class="columnLabel control-label">
                                    <i class="fa fa-users"></i> &nbsp; Max. Anzahl Spieler:
                                </label>
                                <div class="col-md-10 col-sm-8 col-xs-11">
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
                        
                    <div class="row">
                        <div class="col-md-6 col-sm-12">
                            <div class="form-group">
                                <label class="columnLabel control-label">
                                    <i class="fa fa-retweet"></i>&nbsp; Anzahl Runden (=Fragen)
                                </label>
                                <div class="col-md-11 col-sm-8 col-xs-11">
                                    <div class="input-group">
                                        <input class="form-control" name="Rounds" value="15" style="height: 30px;" />
                                        <span class="input-group-addon" style="height: 30px;">
                                            (max. 100)
                                        </span>
                                    </div>
                                </div>
                            </div>                                
                        </div>
                    </div>
                        
                    <div class="row">
                        <div class="col-lg-7">
                            <div class="form-group">
                                <div class="col-md-11 col-sm-8 col-xs-11">
                                        <div class="checkbox">
                                        <label>
                                            <%= Html.CheckBoxFor(x => x.OnlyMultipleChoice) %> Nur Multiple-Choice-Fragen
                                        </label>
                                    </div>
                                </div>
                            </div>                                
                        </div>
                    </div>
                    
                    <div class="row">
                        <div class="col-lg-7">
                            <div class="form-group">
                                <div class="col-md-11 col-sm-8 col-xs-11">
                                        <div class="checkbox">
                                        <label>
                                            <span class="show-tooltip" title="Wenn du keine Mitspieler erwartest, kannst du einfach gegen memucho spielen.">
                                                <%= Html.CheckBoxFor(x => x.WithSystemAvgPlayer) %> Mit memucho als Spieler <i class="fa fa-question-circle"></i>
                                            </span>
                                        </label>
                                    </div>
                                </div>
                            </div>                                
                        </div>
                    </div>
                                                                     

<%--                        <div class="form-group">
                        <%= Html.LabelFor(m => m.Comment, new { @class = "columnLabel control-label" })%>
                        <div class="columnControlsFull">
                            <%= Html.TextAreaFor(m => m.Comment, 
                                new
                                {
                                    @class="form-control", 
                                    placeholder = "Nachricht an deine Mitspieler (optional)", 
                                    rows = 3
                                })%>
                        </div>
                    </div>--%>
                        
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