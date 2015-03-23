<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

<script type="text/javascript" >

    /* Source http://www.html-advisor.com/javascript/hide-email-with-javascript-jquery/ */
    $(function () {
        var spt = $('span.mailme');
        var at = / at /;
        var dot = / dot /g;
        var addr = $(spt).text().replace(at, "@").replace(dot, ".");
        $(spt).after('<a href="mailto:' + addr + '" title="Send an email">' + addr + '</a>').hover(function () { window.status = "Send a letter!"; }, function () { window.status = ""; });
        $(spt).remove();
    });
</script>    
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<div class="row" style="padding-top:30px;">
    <div class="BackToHome col-md-3">
        <i class="fa fa-chevron-left"></i>&nbsp;<a href="/">zur Startseite</a>
    </div>
    <div class="form-horizontal col-md-9">

        <h2 class="PageHeader">Impressum</h2>

        <p style="padding-bottom: 5px;">Angaben gemäß § 5 TMG:<br/></p>
        Robert Mischke Softwareentwicklung<br />
        Warschauer Str. 62<br />
        10243 Berlin<br />

        <h3>Kontakt:</h3>
        <table><tr>
        <td><p>Telefon: </p></td>
        <td><p>030 97005939</p></td></tr>
        <tr><td><p>E-Mail:</p></td>
        <td><p ><span class="mailme">robert at robert-m dot de</span></p></td>
        </tr></table>
        <h3>Verantwortlich für den Inhalt nach § 55 Abs. 2 RStV:</h3>
        <p>Robert Mischke<br />
        <br />
        Warschauer Str. 62<br />
        10243 Berlin</p>
        <p> </p>
        <p>Quelle: <i>erstellt mit dem <a href="http://www.e-recht24.de/impressum-generator.html" target="_blank">Impressum-Generator Einzelunternehmer</a> von eRecht24 u. in Teilen angepasst.</i></p>
        <h3>Haftungsausschluss:</h3>
        <p><strong>Haftung für Inhalte</strong></p>
        <p>Die Inhalte unserer Seiten wurden mit größter Sorgfalt erstellt. 
            Für die Richtigkeit, Vollständigkeit und Aktualität der Inhalte 
            können wir jedoch keine Gewähr übernehmen. Als Diensteanbieter sind wir gemäß § 7 Abs.1 TMG für 
            eigene Inhalte auf diesen Seiten nach den allgemeinen Gesetzen verantwortlich. 
            Nach §§ 8 bis 10 TMG sind wir als Diensteanbieter jedoch nicht 
            verpflichtet, übermittelte oder gespeicherte fremde Informationen zu 
            überwachen oder nach Umständen zu forschen, die auf eine rechtswidrige 
            Tätigkeit hinweisen. Verpflichtungen zur Entfernung oder Sperrung der 
            Nutzung von Informationen nach den allgemeinen Gesetzen bleiben hiervon 
            unberührt. Eine diesbezügliche Haftung ist jedoch erst ab dem 
            Zeitpunkt der Kenntnis einer konkreten Rechtsverletzung möglich. Bei 
            Bekanntwerden von entsprechenden Rechtsverletzungen werden wir diese Inhalte 
            umgehend entfernen.</p>
        <p><strong>Haftung für Links</strong></p>
        <p>Unser Angebot enthält Links zu externen Webseiten Dritter, auf deren 
            Inhalte wir keinen Einfluss haben. Deshalb können wir für diese 
            fremden Inhalte auch keine Gewähr übernehmen. Für die Inhalte 
            der verlinkten Seiten ist stets der jeweilige Anbieter oder Betreiber der 
            Seiten verantwortlich. Die verlinkten Seiten wurden zum Zeitpunkt der Verlinkung 
            auf mögliche Rechtsverstöße überprüft. Rechtswidrige 
            Inhalte waren zum Zeitpunkt der Verlinkung nicht erkennbar. Eine permanente 
            inhaltliche Kontrolle der verlinkten Seiten ist jedoch ohne konkrete Anhaltspunkte 
            einer Rechtsverletzung nicht zumutbar. Bei Bekanntwerden von Rechtsverletzungen 
            werden wir derartige Links umgehend entfernen.</p>
        <p><strong>Datenschutz</strong></p>
        <p>Die Nutzung unserer Webseite ist teils ohne Angabe personenbezogener Daten möglich. Soweit auf unseren Seiten personenbezogene Daten (beispielsweise Name, 
            Anschrift oder eMail-Adressen) erhoben werden, erfolgt dies, soweit möglich, stets auf freiwilliger Basis. Diese Daten werden ohne Ihre ausdrückliche Zustimmung nicht an Dritte weitergegeben.   
        </p>
        <p>Wir weisen darauf hin, dass die Datenübertragung im Internet (z.B. 
            bei der Kommunikation per E-Mail) Sicherheitslücken aufweisen kann. 
            Ein lückenloser Schutz der Daten vor dem Zugriff durch Dritte ist nicht 
            möglich. </p>
        <p>Der Nutzung von im Rahmen der Impressumspflicht veröffentlichten Kontaktdaten 
            durch Dritte zur Übersendung von nicht ausdrücklich angeforderter 
            Werbung und Informationsmaterialien wird hiermit ausdrücklich widersprochen. 
            Die Betreiber der Seiten behalten sich ausdrücklich rechtliche Schritte 
            im Falle der unverlangten Zusendung von Werbeinformationen, etwa durch Spam-Mails, 
            vor.</p><p> </p>
            <p>Quelle: <i><a href="http://www.e-recht24.de/muster-disclaimer.htm" target="_blank">Disclaimer</a> von eRecht24, dem Portal zum Internetrecht von <a href="http://www.e-recht24.de/" target="_blank">Rechtsanwalt</a> Sören Siebert.</i>
        </p>

    </div>
</div>

</asp:Content>
