<%@ Page Title="Registrierung erfolgreich" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="System.Web.Mvc.ViewPage<BaseModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <% 
        Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem { Text = "Registrierung erfolgreich", Url = "/Register/RegisterSuccess", ToolTipText = "Registrierung erfolgreich" });
        Model.TopNavMenu.IsCategoryBreadCrumb = false;
    %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row" style="padding-top:30px;">
        <div class="BackToHome col-md-3">
            <a href="/"><i class="fa fa-chevron-left">&nbsp;</i>Zur Startseite</a>
        </div>
        <div class="form-horizontal col-md-9">
	        <h2 class="PageHeader">Registrierung erfolgreich!</h2>

	        <p>
	            Schön, dass du dabei bist! Du bist nun registriert. <br/>
                Bitte bestätige noch deine E-Mail-Adresse über den Link, den wir dir gerade per E-Mail geschickt haben.
            </p>

	        <p>Trotzdem kannst du schon loslegen!</p>
            <div class="col-xs-12 well well-sm">
                <div class="row">
                    <div class="col-xs-6 col-lg-3" style="text-align: center; font-size: 100%; padding: 10px;">
                      <i class="fa fa-book fa-2x" style="color: #2C5FB2"></i><br/>
                        <p><b>Wissen entdecken</b></p>
                        <p>
                            Stöbere bei den <a href="<%= Url.Action("Sets", "Sets")%>">Lernsets</a> oder den 
                            <a href="<%= Url.Action(Links.Questions, Links.QuestionsController) %>">Fragen</a> nach interessantem Wissen und füge es 
                            deinem <span style="white-space: nowrap;"><i class='fa fa-heart' style='color:#b13a48;'></i>&nbsp;Wunschwissen</span> hinzu.
                        </p>
                    </div>
                    <div class="col-xs-6 col-lg-3" style="text-align: center; font-size: 100%; padding: 10px;">
                      <i class="fa fa-pencil fa-2x" style="color: #2C5FB2"></i><br/>
                        <p><b>Wissen erstellen</b></p>
                        <p>
                            <a href="<%= Links.CreateQuestion() %>">Erstelle eigene Fragen</a>, die du gerne lernen möchtest und füge sie zu eigenen Lernsets zusammen.
                        </p>
                    </div>
                    <div class="col-xs-6 col-lg-3" style="text-align: center; font-size: 100%; padding: 10px;">
                      <i class="fa fa-camera fa-2x" style="color: #2C5FB2"></i><br/>
                        <p><b>Dein Profilbild</b></p>
                        <p>
		                    Füge <a href="<%= Url.Action(Links.UserSettingsAction, Links.UserSettingsController) %>">deinem Benutzerkonto</a> ein Bild von dir hinzu.
                        </p>
                    </div>
                    <div class="col-xs-6 col-lg-3" style="text-align: center; font-size: 100%; padding: 10px;">
                      <i class="fa fa-users fa-2x" style="color: #2C5FB2"></i><br/>
                        <p><b>Freunden folgen</b></p>
                        <p>
                            Suche <a href="<%= Url.Action("Users", "Users") %>">deine Freunde</a> und füge sie zu deinem memucho-Netzwerk hinzu.
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
