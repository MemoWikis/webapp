<%@ Page Title="Einstellungen" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master"
    Inherits="System.Web.Mvc.ViewPage<UserSettingsModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ContentPlaceHolderID="Head" runat="server">
    <%= Scripts.Render("~/Views/Users/Account/Settings/Validation.js") %>
    <%= Styles.Render("~/Views/Users/Account/Settings/UserSettings.css") %>
    <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Nutzer", Url = "/Nutzer", ToolTipText = "Nutzer"});
        Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Profilseite", Url = Url.Action(Links.UserAction, Links.UserController, new { name =  Model.Name, id = Model.UserId}), ToolTipText = "Profilseite"});
        Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Einstellungen", Url = Url.Action(Links.UserSettingsAction, Links.UserSettingsController), ToolTipText = "Einstellungen"});
        Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
    <style>
        .column{ width: 167px;float: left; padding-right: 4px;}
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
        <div class="row">
            <div class="col-xs-9 xxs-stack" style="margin-bottom: 10px;">
                <h2 class="pull-left" style="margin-bottom: 10px; margin-top: 0;  font-size: 30px;">
                    <span class="ColoredUnderline User">Dein Nutzerkonto</span>
                    <i class="fa fa-wrench show-tooltip" title="Hier kannst du deine Einstellungen bearbeiten."></i>
                </h2>
            </div>

            <div class="col-xs-3 xxs-stack">
                <div class="navLinks">
                    <a href="<%= Url.Action("Users", "Users")%>" style="font-size: 12px; margin: 0"><i class="fa fa-list"></i>&nbsp;zur Übersicht</a>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-9 xxs-stack">
                <% Html.Message(Model.Message); %>
            </div>
        </div>
    
        <% if (!Model.IsLoggedIn) { %>
            <div class="row">
                <div class="col-xs-9 xxs-stack">
                    Um Einstellungen vorzunehmen, musst du dich <a href="#" data-btn-login="true">einloggen</a>.
                </div>
            </div>
            <% return;
           }
        %>

        <div class="row">
            <div class="col-md-3 col-md-push-9" >
                <img alt="" src="<%=Model.ImageUrl_200%>" class="img-responsive" style="border-radius:5px;" />
            
                <script type="text/javascript">
                    $(function () {
                        $("#imageUploadLink").click(function () {
                            $("#imageUpload").show();
                        });
                    })
                </script>
                <a id="imageUploadLink" href="#">Profilbild ändern</a>
                <div id="imageUpload" style="display: none">
                    <% using (Html.BeginForm("UploadPicture", "UserSettings", null, FormMethod.Post, new { enctype = "multipart/form-data" }))
                       { %>
                        <input type="file" accept="image/*" name="file" id="file" />
                        <input class="cancel" type="submit" value="Hochladen" />
                    <% } %>
                </div>
            
                <%--noch nicht umgesetzt:
                    <% if (Model.ImageIsCustom)
                   { %>
                    <a href="#"><i title="Profilbild löschen" class="fa fa-trash-o show-tooltip" data-placement="left"></i></a>       
                <%} %>--%>
            </div>
            <div class="xxs-stack col-xs-12 col-md-9 col-md-pull-3">
                <form id="UserSettingsForm" class="form-horizontal" method="POST">
                    <div class="FormSection">
                        <div class="form-group">
                            <%= Html.LabelFor(m => m.Name, new { @class = "RequiredField columnLabel control-label" })%>
                            <div class="columnControlsFull">
                                <%= Html.TextBoxFor(m => m.Name, new { @class = "form-control" })%>
                            </div>
                        </div>                    
                        <div class="form-group">
                            <%= Html.LabelFor(m => m.Email, new { @class = "RequiredField columnLabel control-label" })%>
                            <div class="columnControlsFull">
                                <%= Html.TextBoxFor(m => m.Email, new { @class = "form-control" })%>
                            </div>
                        </div>
                    </div>
                    <div class="FormSection">
                        <div class="form-group" style="padding-top: 20px;">
                            <label class="columnLabel control-label">
                                <% if (Model.IsMember) { %>
                                    <span class="bold">Du bist Mitglied!</span><br/>
                                    <a href="<%= Links.Membership() %>">Deine Mitgliedschaft</a> läuft bis zum <%= String.Format("{0:d}", Model.Membership.PeriodEnd) %>.
                                <% } else { %>
                                    <span class="bold">Du bist zur Zeit kein Mitglied.</span><br/>
                                    <a class="btn btn-primary" href="<%= Links.Membership() %>" style="margin-top: 12px;"><i class="fa fa-thumbs-up">&nbsp;&nbsp;</i>Jetzt Mitglied werden</a>
                                <% } %>
                            </label>
                        </div>
                    </div>
                    <% if (Model.WidgetHosts.Any()) { %>
                        <div class="FormSection">
                            <h3>Widgets</h3>
                            <p>Du verwendest Widgets auf folgenden Hosts:</p>
                            <ul>
                                <% foreach (var host in Model.WidgetHosts) { %>
                                       <li><%=host %></li>
                                <% } %>
                            </ul>
                            <a href="<%= Links.WidgetStats() %>" class="btn btn-default" style="margin: 20px 0;">Zur Widget-Statistik</a>
                            <p>Wenn du gerne einen Host hinzufügen möchtest, <a href="<%= Links.Contact %>">melde dich bei uns.</a></p>
                        </div>
                    <% } %>
                    <div class="FormSection">
                        <h3>Passwort</h3>
                        <p>
                            Um dein Passwort neu zu setzen, 
                            gehe zu: <a href="<%: Url.Action("PasswordRecovery", Links.WelcomeController) %>">"Passwort vergessen"</a>.
                        </p>
                        <p style="margin-top: 3px;">(Später werden wir das Setzen eines neuen Passworts auch hier ermöglichen.)</p>
                    </div>
                    <div class="FormSection">
                        <h3>Einstellungen</h3>
                        <div class="form-group">
                            <div class="columnControlsFull">
                                <div class="checkbox">
                                    <%= Html.CheckBoxFor(m => m.ShowWishKnowledge)%>
                                    <label for="ShowWishKnowledge" class="CheckboxTitle">Wunschwissen zeigen</label><br/>
                                    <label for="ShowWishKnowledge">
                                        Wenn ausgewählt, ist öffentlich sichtbar, welche Fragen in deinem Wunschwissen sind 
                                        (außer private Fragen). Antwortstatistiken werden nicht angezeigt. 
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="columnControlsFull noLabel">
                                <label class="checkbox">
                                    <%= Html.CheckBoxFor(m => m.AllowsSupportiveLogin)%>
                                    <label for="AllowsSupportiveLogin" class="CheckboxTitle">Support-Login zulassen</label><br/>
                                    <label for="AllowsSupportiveLogin">
                                        Achtung: Das ist nur nach Rücksprache mit dem memucho-Team nötig! Wenn du den Support-Login aktivierst, 
                                        können sich Mitarbeiter von memucho zur Fehlerbehebung oder zu deiner Unterstützung 
                                        in deinem Nutzerkonto einloggen, selbstverständlich ohne dein Passwort zu benötigen oder sehen zu können.
                                    </label>
                                </label>
                            </div>
                        </div>

                        <h4>Benachrichtigungen</h4>
                        <div class="form-group">
                            <div class="columnControlsFull noLabel">
                                <label class="dropdownSelect" for="KnowledgeReportInterval">
                                    <label class="dropdownTitle">
                                        Wissensbericht per E-Mail: &nbsp;
                                    </label>
                                    <select style="display: inline; width: 180px;" data-val="true" data-val-required="Das Feld &quot;KnowledgeReportNotificationInterval&quot; ist erforderlich." id="KnowledgeReportInterval" name="KnowledgeReportInterval" class="valid form-control">
                                        <option value="Daily" <%= Model.KnowledgeReportInterval == UserSettingNotificationInterval.Daily ? "selected='selected'" : "" %>>Täglich</option>
                                        <option value="Weekly" <%= ((Model.KnowledgeReportInterval == UserSettingNotificationInterval.Weekly) || (Model.KnowledgeReportInterval == UserSettingNotificationInterval.NotSet)) ? "selected='selected'" : "" %>>Wöchentlich</option>
                                        <option value="Monthly" <%= Model.KnowledgeReportInterval == UserSettingNotificationInterval.Monthly ? "selected='selected'" : "" %>>Monatlich</option>
                                        <option value="Quarterly" <%= Model.KnowledgeReportInterval == UserSettingNotificationInterval.Quarterly ? "selected='selected'" : "" %>>Alle drei Monate</option>
                                        <option value="Never" <%= Model.KnowledgeReportInterval == UserSettingNotificationInterval.Never ? "selected='selected'" : "" %>>Nie</option>
                                    </select>                                
                                    <label class="additionalInfo">
                                        Der Wissensreport informiert dich über deinen aktuellen Wissensstand von deinem 
                                        <span style="white-space: nowrap;"><i class="fa fa-heart" style="color:#b13a48;">&nbsp;</i>Wunschwissen</span>, 
                                        über anstehende Termine und über neue Inhalte bei memucho. Er wird nur verschickt, wenn du Wunschwissen hast.
                                    </label>
                                </label>
                            </div>
                        </div>                                    


                        <div class="form-group" style="margin-top: 30px;">
                            <div class="noLabel columnControlsFull">
                                <button type="submit" class="btn btn-primary" name="btnSave" value="ssdfasdfave">Speichern</button>
                            </div>
                        </div>
                    </div>
                </form>
            </div>
        </div>

</asp:Content>
