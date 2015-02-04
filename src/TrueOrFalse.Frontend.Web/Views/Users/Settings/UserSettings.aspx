<%@ Page Title="Einstellungen" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master"
    Inherits="System.Web.Mvc.ViewPage<UserSettingsModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ContentPlaceHolderID="Head" runat="server">
    <%= Scripts.Render("~/Views/Users/Settings/Validation.js") %>
    <%= Styles.Render("~/Views/Users/Settings/UserSettings.css") %>
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
            
                <% if (Model.ImageIsCustom)
                   { %>
                    <a href="#">[x]</a>       
                <%} %>
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
                        <h3>Einstellungen</h3>
                        <div class="form-group">
                            <label class="columnLabel control-label" style="margin-bottom: 0;">Wunschwissen zeigen</label>
                            <div class="columnControlsFull">
                                <div class="checkbox">
                                    <%= Html.CheckBoxFor(m => m.ShowWishKnowledge)%>   
                                    <label for="ShowWishKnowledge">
                                        Wenn ausgewählt, ist öffentlich sichtbar, welche Fragen in deinem Wunschwissen sind 
                                        (außer private Fragen). Antwortstatistiken werden nicht angezeigt. 
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="FormSection">
                        <h3>Mitgliedschaft</h3>
                        <div class="form-group">
                            <label class="columnLabel control-label">Status:</label>
                            <div class="columnControlsFull">
                                 <p class="form-control-static">Kein Mitglied</p>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="columnControlsFull noLabel">
                                <label class="checkbox">
                                    <%= Html.CheckBoxFor(m => m.AllowsSupportiveLogin)%>
                                    <label for="AllowsSupportiveLogin">
                                        Erlaube Mitarbeitern von MEMuchO zur Fehlerbehebung oder zu deiner Unterstützung, 
                                        sich in deinem Nutzerkonto anzumelden. Das ist nur nach Rücksprache nötig 
                                        (<a href="<%= Url.Action("DatenSicherheit","Help") %>">Mehr zur Datensicherheit</a>).
                                    </label>
                                </label>
                            </div>
                        </div>
                
                        <div class="form-group" style="margin-top: 30px;">
                            <div class="noLabel columnControlsFull">
                                <button type="submit" class="btn btn-primary" name="btnSave" value="ssdfasdfave">Speichern</button>&nbsp;&nbsp;&nbsp;
                            </div>
                        </div>
                    </div>
                </form>
            </div>
        </div>

</asp:Content>
