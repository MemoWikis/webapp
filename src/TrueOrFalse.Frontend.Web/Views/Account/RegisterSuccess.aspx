<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row" style="padding-top:30px;">
        <div class="col-md-3" style="padding-top:7px;">
            <i class="fa fa-chevron-left"></i>&nbsp;<a href="/">zur Startseite</a>
        </div>
        <div class="form-horizontal col-md-9">
	        <h2>Registrierung Erfolgreich!</h2>

	        <p>Sie sind nun vollständig registriert.</p>
	
	        <p>Um Ihre E-Mail Adresse zu verifizieren, erhalten Sie bald von uns eine Nachricht.</p>

	        <p>Schauen Sie sich ein wenig um: </p>
	        <ul>
		        <li><a href="<%= Url.Action(Links.Questions, Links.QuestionsController) %>">Definieren Sie was sie wissen möchten</a></li>
		        <li>Erweitern Sie Ihr Profil</li>
		        <li>Finden Sie interessante Kontake.</li>
	        </ul>
        </div>
    </div>

</asp:Content>
