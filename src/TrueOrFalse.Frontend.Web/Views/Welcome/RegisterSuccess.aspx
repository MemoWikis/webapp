<%@ Page Title="Registrierung erfolgreich" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row" style="padding-top:30px;">
        <div class="BackToHome col-md-3">
            <i class="fa fa-chevron-left"></i>&nbsp;<a href="/">zur Startseite</a>
        </div>
        <div class="form-horizontal col-md-9">
	        <h2 class="PageHeader">Registrierung erfolgreich!</h2>

	        <p>
	            Schön, dass du dabei bist! Du bist nun registriert. <br/>
                Bitte bestätige noch deine Email-Adresse über den Link, den wir dir gerade per Email geschickt haben.
            </p>

	        <p>Trotzdem kannst du schon loslegen!
	            <ul>
		            <li>Stöbere in den <a href="<%= Url.Action(Links.Questions, Links.QuestionsController) %>">vorhandenen 
                        Fragen</a> und füge einige zu deinem Wunschwissen hinzu (klicke dazu auf das Herz neben der Frage).</li>
                    <li>Erstelle eine <a href="<%= Links.CreateQuestion(Url) %>">neue Frage</a>.</li>
		            <li>Füge <a href="<%= Url.Action(Links.UserSettings, Links.UserSettingsController) %>">deinem Benutzerkonto</a> ein Bild von dir hinzu.</li>
                    <li>Suche <a href="<%= Url.Action("Users", "Users") %>">deine Freunde</a> und füge sie zu deinem Netzwerk hinzu.</li>
	        </ul>
            </p>
        </div>
    </div>

</asp:Content>
