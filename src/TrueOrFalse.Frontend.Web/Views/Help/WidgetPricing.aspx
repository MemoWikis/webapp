<%@ Page Title="Widget: Angebote und Preise" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Help/Widget.css" rel="stylesheet" />
    <script type="text/javascript" >

        $(function () {
            $("span.mailme")
                .each(function() {
                    var spt = this.innerHTML;
                    var at = / at /;
                    var dot = / dot /g;
                    var addr = spt.replace(at, "@").replace(dot, ".");
                    $(this).after('<a href="mailto:' + addr + '" title="Schreibe eine E-Mail">' + addr + '</a>');
                    $(this).remove();
                });
        });
    </script>    

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="row">
        <div class="col-xs-12">

            <div class="well">

                <h1 class="PageHeader"><span class="ColoredUnderline GeneralMemucho">memucho-Widget: Angebote und Preise</span></h1>
                <p class="teaserText">
                    Die Lerntechnologie und die Lerninhalte von memucho können als Widget leicht in bestehende Webseiten integriert werden. 
                    Egal ob auf dem privaten Blog, der Vereins- oder Unternehmensseite, oder dem Schul- oder Lernmanagementsystem: Nötig ist eine Zeile HTML-Code, 
                    die du einfach von memucho kopieren kannst.
                </p>
                <p class="teaserText">
                    memucho bietet die Einbettung von Fragesätzen als Quiz-Modul, von Video-Fragesätzen und von einzelnen Fragen an. 
                    Die Inhalte und Lernfunktion sind dann nahtlos in deine Seite integriert.
                    Mehr Infos zur Einbettung von Widgets <a href="<%= Links.HelpWidget() %>">erhältst du hier</a>.
                </p>
                
                <div id="switchesPriceType">
                    <a href="#" id="btnPricingIndividual" class="btn btn-primary btn-large">
                        <span class="buttonTitle">Einzelpersonen</span><br /><small>Individuen, Lehrer, Dozenten ...</small>
                    </a>
                    <a href="#" id="btnPricingOrgs" class="btn btn-notSelected btn-large">
                         <span class="buttonTitle">Organisationen</span><br /><small>Verlage, Bildungsanbieter</small>
                    </a>
                </div>
                   
                <div id="pricingIndividuals">
                    
                    <div class="content">
	                    <div class="row">
		                    <!-- Pricing -->
		                    <div class="col-md-4 col-md-offset-2">
			                    <div class="pricing hover-effect">
				                    <div class="pricing-head">
					                    <h3>Basis <span>Für alle </span></h3>
					                    <h4>
					                        <i>Kostenlos</i>
                                            <%--<span>&nbsp;</span>--%>
					                    </h4>
				                    </div>
				                    <ul class="pricing-content list-unstyled">
					                    <li>Unbegrenzte Aufrufe</li>
					                    <li>Werbefrei</li>
					                    <li>Maximal 5 Widgets</li>
				                    </ul>
				                    <div class="pricing-footer">
					                    <p>Die Nutzung von 5 Widgets ist im kostenlosen Basis-Account eingeschlossen.</p>
                                        <%  var isLoggedIn = Sl.R<SessionUser>().IsLoggedIn;
                                            if (!isLoggedIn) { %>
					                            <a href="#" class="btn yellow-crusta">Jetzt registrieren</a>
                                        <% } %>
				                    </div>
			                    </div>
		                    </div>
		                    <div class="col-md-4">
			                    <div class="pricing hover-effect">
				                    <div class="pricing-head">
					                    <h3>Pro <span>Für alle Pro-Nutzer</span></h3>
					                    <h4> 
					                        <i>ca. € </i>3
                                            <span>pro Monat</span>
					                    </h4>
				                    </div>
				                    <ul class="pricing-content list-unstyled">
					                    <li>Unbegrenzte Aufrufe</li>
					                    <li>Werbefrei</li>
                                        <li>Ungegrenzte Anzahl an Widgets</li>
				                    </ul>
				                    <div class="pricing-footer">
					                    <p>
						                     Pro-Nutzer ... automatisch, inklusive .
					                    </p>
					                    <a href="#" class="btn yellow-crusta">Sign Up</a>
				                    </div>
			                    </div>
		                    </div>
		                    <!--//End Pricing -->
	                    </div>
                    </div>                    
                </div>

                <div id="pricingOrgs">
                    
                </div>

                <h2 class="">
                    <span class="ColoredUnderline GeneralMemucho">Vorteile der memucho-Widgets</span>
                </h2>
                
                <p>
                    <strong>Verweildauer:</strong>
                    Quizzes und kleine Interaktionsmöglichkeiten können die Verweildauer der Nutzer auf deiner Seite deutlich erhöhen. 
                </p>
                <p>
                    <strong>Besseres Lernerlebnis:</strong>
                    Lerninhalte in kleinen Häppchen zu wiederholen ist lernpsychologisch sehr effizient. Wenn du eine Bildungsseite hast, 
                </p>
                <p>
                    <strong>Zufriedene Nutzer:</strong>
                    
                </p>
                <p>
                    <strong>SEO:</strong>
                    Wenn sich die Verweildauer von Nutzer auf deiner Webseite erhöht, schätzen sie Suchmaschinen als hilfreich ein und verbessern dein Ranking. 
                    Das ist ein wichtiger SEO-Faktor.
                </p>
                <p>
                    <strong>Freie Inhalte:</strong>
                    Zu vielen Themen findest du bei memucho bereits hochwertige Inhalte, die du frei verwenden und bei Bedarf ergänzen kannst. 
                </p>

            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-xs-12">
            <div class="well explanationBox">

                <h3>Fragen oder Probleme?</h3>
                <p>
                    Sind noch Fragen offen? Kein Problem, melde dich einfach bei uns, wir helfen dir gerne weiter. Christof erreichst du unter 01577-6825707 
                    oder per E-Mail an <span class="mailme">christof at memucho dot de</span>.
                </p>

            </div>
        </div>
    </div>
    
</asp:Content>