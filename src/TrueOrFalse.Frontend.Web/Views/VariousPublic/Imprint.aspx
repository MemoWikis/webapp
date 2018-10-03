<%@ Page Title="Impressum & Datenschutzerklärung" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="System.Web.Mvc.ViewPage<BaseModel>" %>

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
    
    <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem {Text = "Impressum & Datenschutz", Url = "/Impressum", ToolTipText = "Impressum & Datenschutz"});
       Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
    
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<div class="row" style="padding-top:30px;">
    <div class="BackToHome col-md-3">
        <a href="/"><i class="fa fa-chevron-left">&nbsp;</i>Zur Startseite</a>
    </div>
    <div class="form-horizontal col-md-9">

        <h1 class="PageHeader">Impressum & Datenschutzerkläung</h1>
            
        <h2>1. Impressum</h2>
    
        <h3>Angaben gemäß § 5 TMG:</h3>
        memucho GmbH<br />
        Kirchstraße 1, <br />
        15745 Wildau<br />
        
        <h3>Registereintrag:</h3>
        Registergericht: Amtsgericht Cottbus, HRB 13499 CB<br />
        <h3>Vertreten durch:</h3>
        Geschäftsführer:<br />
        Christof Mauersberger, Robert Mischke<br />

        <h3>Kontakt:</h3>

        Telefon: <br/>
        +49-1577-6825707<br/>
        <span class="mailme">team at memucho dot de</span>
        
        <h3>Verantwortlich für den Inhalt nach § 55 Abs. 2 RStV:</h3>
        <p>Christof Mauersberger, Robert Mischke<br />

        <h3>Haftungsausschluss</h3>
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
            vor.
        </p>
    
        <p><strong>Nutzer unter 16 Jahren</strong><a name="under16">&nbsp;</a></p>
        <p>
            Wenn Du unter 16 Jahren alt bist, darfst Du dich bei memucho nur mit Zustimmung Deiner Eltern registieren.
        </p>
        <p>
            Die Zustimmung durch Deine Eltern kann per Email oder Telefon erfolgen. 
            Hier können Deine Eltern mit uns in Kontakt treten: <a href="/Kontakt">Kontakt</a>
        </p>
        <p>
            Du kannst Die Seite natürlich anonym Nutzen, also ohne Registrierung nutzen.
        </p>

        <p></p>
            <p>(Eine) Quelle: <i><a href="http://www.e-recht24.de/muster-disclaimer.htm" target="_blank">Disclaimer</a> von eRecht24, dem Portal zum Internetrecht von <a href="http://www.e-recht24.de/" target="_blank">Rechtsanwalt</a> Sören Siebert.</i>
        </p>
        
        <br/>
        <h2>2. Datenschutzerklärung</h2>
        <p>Personenbezogene Daten (nachfolgend zumeist nur „Daten“ genannt) werden von uns nur im Rahmen der Erforderlichkeit sowie zum Zwecke der Bereitstellung eines funktionsfähigen und nutzerfreundlichen Internetauftritts, inklusive seiner Inhalte und der dort angebotenen Leistungen, verarbeitet.</p>
        <p>Gemäß Art. 4 Ziffer 1. der Verordnung (EU) 2016/679, also der Datenschutz-Grundverordnung (nachfolgend nur „DSGVO“ genannt), gilt als „Verarbeitung“ jeder mit oder ohne Hilfe automatisierter Verfahren ausgeführter Vorgang oder jede solche Vorgangsreihe im Zusammenhang mit personenbezogenen Daten, wie das Erheben, das Erfassen, die Organisation, das Ordnen, die Speicherung, die Anpassung oder Veränderung, das Auslesen, das Abfragen, die Verwendung, die Offenlegung durch Übermittlung, Verbreitung oder eine andere Form der Bereitstellung, den Abgleich oder die Verknüpfung, die Einschränkung, das Löschen oder die Vernichtung.</p>
        <p>Mit der nachfolgenden Datenschutzerklärung informieren wir Sie insbesondere über Art, Umfang, Zweck, Dauer und Rechtsgrundlage der Verarbeitung personenbezogener Daten, soweit wir entweder allein oder gemeinsam mit anderen über die Zwecke und Mittel der Verarbeitung entscheiden. Zudem informieren wir Sie nachfolgend über die von uns zu Optimierungszwecken sowie zur Steigerung der Nutzungsqualität eingesetzten Fremdkomponenten, soweit hierdurch Dritte Daten in wiederum eigener Verantwortung verarbeiten.</p>
        <p>Unsere Datenschutzerklärung ist wie folgt gegliedert:</p>
        <p>I. Informationen über uns als Verantwortliche<br>II. Rechte der Nutzer und Betroffenen<br>III. Informationen zur Datenverarbeitung</p>
        <h3>I. Informationen über uns als Verantwortliche</h3>
        <p>Verantwortlicher Anbieter dieses Internetauftritts im datenschutzrechtlichen Sinne ist:</p>
        <p>Siehe weiter Kontakt oben</p>
        <h3>II. Rechte der Nutzer und Betroffenen</h3>
        <p>Mit Blick auf die nachfolgend noch näher beschriebene Datenverarbeitung haben die Nutzer und Betroffenen das Recht</p>
        <ul>
        <li>auf Bestätigung, ob sie betreffende Daten verarbeitet werden, auf Auskunft über die verarbeiteten Daten, auf weitere Informationen über die Datenverarbeitung sowie auf Kopien der Daten (vgl. auch Art. 15 DSGVO);</li>
        <li>auf Berichtigung oder Vervollständigung unrichtiger bzw. unvollständiger Daten (vgl. auch Art. 16 DSGVO);</li>
        <li>auf unverzügliche Löschung der sie betreffenden Daten (vgl. auch Art. 17 DSGVO), oder, alternativ, soweit eine weitere Verarbeitung gemäß Art. 17 Abs. 3 DSGVO erforderlich ist, auf Einschränkung der Verarbeitung nach Maßgabe von Art. 18 DSGVO;</li>
        <li>auf Erhalt der sie betreffenden und von ihnen bereitgestellten Daten und auf Übermittlung dieser Daten an andere Anbieter/Verantwortliche (vgl. auch Art. 20 DSGVO);</li>
        <li>auf Beschwerde gegenüber der Aufsichtsbehörde, sofern sie der Ansicht sind, dass die sie betreffenden Daten durch den Anbieter unter Verstoß gegen datenschutzrechtliche Bestimmungen verarbeitet werden (vgl. auch Art. 77 DSGVO).</li>
        </ul>
        <p>Darüber hinaus ist der Anbieter dazu verpflichtet, alle Empfänger, denen gegenüber Daten durch den Anbieter offengelegt worden sind, über jedwede Berichtigung oder Löschung von Daten oder die Einschränkung der Verarbeitung, die aufgrund der Artikel 16, 17 Abs. 1, 18 DSGVO erfolgt, zu unterrichten. Diese Verpflichtung besteht jedoch nicht, soweit diese Mitteilung unmöglich oder mit einem unverhältnismäßigen Aufwand verbunden ist. Unbeschadet dessen hat der Nutzer ein Recht auf Auskunft über diese Empfänger.</p>
        <p><strong>Ebenfalls haben die Nutzer und Betroffenen nach Art. 21 DSGVO das Recht auf Widerspruch gegen die künftige Verarbeitung der sie betreffenden Daten, sofern die Daten durch den Anbieter nach Maßgabe von Art. 6 Abs. 1 lit. f) DSGVO verarbeitet werden. Insbesondere ist ein Widerspruch gegen die Datenverarbeitung zum Zwecke der Direktwerbung statthaft.</strong></p>
        <h3>III. Informationen zur Datenverarbeitung</h3>
        <p>Ihre bei Nutzung unseres Internetauftritts verarbeiteten Daten werden gelöscht oder gesperrt, sobald der Zweck der Speicherung entfällt, der Löschung der Daten keine gesetzlichen Aufbewahrungspflichten entgegenstehen und nachfolgend keine anderslautenden Angaben zu einzelnen Verarbeitungsverfahren gemacht werden.</p>

        <h4>Serverdaten</h4>
        <p>Aus technischen Gründen, insbesondere zur Gewährleistung eines sicheren und stabilen Internetauftritts, werden Daten durch Ihren Internet-Browser an uns bzw. an unseren Webspace-Provider übermittelt. Mit diesen sog. Server-Logfiles werden u.a. Typ und Version Ihres Internetbrowsers, das Betriebssystem, die Website, von der aus Sie auf unseren Internetauftritt gewechselt haben (Referrer URL), die Website(s) unseres Internetauftritts, die Sie besuchen, Datum und Uhrzeit des jeweiligen Zugriffs sowie die IP-Adresse des Internetanschlusses, von dem aus die Nutzung unseres Internetauftritts erfolgt, erhoben.</p>
        <p>Diese so erhobenen Daten werden vorrübergehend gespeichert, dies jedoch nicht gemeinsam mit anderen Daten von Ihnen.</p>
        <p>Diese Speicherung erfolgt auf der Rechtsgrundlage von Art. 6 Abs. 1 lit. f) DSGVO. Unser berechtigtes Interesse liegt in der Verbesserung, Stabilität, Funktionalität und Sicherheit unseres Internetauftritts.</p>
        <p>Die Daten werden spätestens nach sieben Tage wieder gelöscht, soweit keine weitere Aufbewahrung zu Beweiszwecken erforderlich ist. Andernfalls sind die Daten bis zur endgültigen Klärung eines Vorfalls ganz oder teilweise von der Löschung ausgenommen.</p>

        <h4>Cookies</h4>
        <h5>a) Sitzungs-Cookies/Session-Cookies</h5>
        <p>Wir verwenden mit unserem Internetauftritt sog. Cookies. Cookies sind kleine Textdateien oder andere Speichertechnologien, die durch den von Ihnen eingesetzten Internet-Browser auf Ihrem Endgerät ablegt und gespeichert werden. Durch diese Cookies werden im individuellen Umfang bestimmte Informationen von Ihnen, wie beispielsweise Ihre Browser- oder Standortdaten oder Ihre IP-Adresse, verarbeitet. &nbsp;</p>
        <p>Durch diese Verarbeitung wird unser Internetauftritt benutzerfreundlicher, effektiver und sicherer, da die Verarbeitung bspw. die Wiedergabe unseres Internetauftritts in unterschiedlichen Sprachen oder das Angebot einer Warenkorbfunktion ermöglicht.</p>
        <p>Rechtsgrundlage dieser Verarbeitung ist Art. 6 Abs. 1 lit b.) DSGVO, sofern diese Cookies Daten zur Vertragsanbahnung oder Vertragsabwicklung verarbeitet werden.</p>
        <p>Falls die Verarbeitung nicht der Vertragsanbahnung oder Vertragsabwicklung dient, liegt unser berechtigtes Interesse in der Verbesserung der Funktionalität unseres Internetauftritts. Rechtsgrundlage ist in dann Art. 6 Abs. 1 lit. f) DSGVO.</p>
        <p>Mit Schließen Ihres Internet-Browsers werden diese Session-Cookies gelöscht.</p>
        <h5>b) Drittanbieter-Cookies</h5>
        <p>Gegebenenfalls werden mit unserem Internetauftritt auch Cookies von Partnerunternehmen, mit denen wir zum Zwecke der Werbung, der Analyse oder der Funktionalitäten unseres Internetauftritts zusammenarbeiten, verwendet.</p>
        <p>Die Einzelheiten hierzu, insbesondere zu den Zwecken und den Rechtsgrundlagen der Verarbeitung solcher Drittanbieter-Cookies, entnehmen Sie bitte den nachfolgenden Informationen.</p>
        <h5>c) Beseitigungsmöglichkeit</h5>
        <p>Sie können die Installation der Cookies durch eine Einstellung Ihres Internet-Browsers verhindern oder einschränken. Ebenfalls können Sie bereits gespeicherte Cookies jederzeit löschen. Die hierfür erforderlichen Schritte und Maßnahmen hängen jedoch von Ihrem konkret genutzten Internet-Browser ab. Bei Fragen benutzen Sie daher bitte die Hilfefunktion oder Dokumentation Ihres Internet-Browsers oder wenden sich an dessen Hersteller bzw. Support. Bei sog. Flash-Cookies kann die Verarbeitung allerdings nicht über die Einstellungen des Browsers unterbunden werden. Stattdessen müssen Sie insoweit die Einstellung Ihres Flash-Players ändern. Auch die hierfür erforderlichen Schritte und Maßnahmen hängen von Ihrem konkret genutzten Flash-Player ab. Bei Fragen benutzen Sie daher bitte ebenso die Hilfefunktion oder Dokumentation Ihres Flash-Players oder wenden sich an den Hersteller bzw. Benutzer-Support.</p>
        <p>Sollten Sie die Installation der Cookies verhindern oder einschränken, kann dies allerdings dazu führen, dass nicht sämtliche Funktionen unseres Internetauftritts vollumfänglich nutzbar sind. </p>

        <h4>Kundenkonto / Registrierungsfunktion</h4>
        <p>Falls Sie über unseren Internetauftritt ein Kundenkonto bei uns anlegen, werden wir die von Ihnen bei der Registrierung eingegebenen Daten (also bspw. Ihren Namen, Ihre Anschrift oder Ihre E-Mail-Adresse) ausschließlich für vorvertragliche Leistungen, für die Vertragserfüllung oder zum Zwecke der Kundenpflege (bspw. um Ihnen eine Übersicht über Ihre bisherigen Bestellungen bei uns zur Verfügung zu stellen oder um Ihnen die sog. Merkzettelfunktion anbieten zu können) erheben und speichern. Gleichzeitig speichern wir dann die IP-Adresse und das Datum Ihrer Registrierung nebst Uhrzeit. Eine Weitergabe dieser Daten an Dritte erfolgt natürlich nicht.</p>
        <p>Im Rahmen des weiteren Anmeldevorgangs wird Ihre Einwilligung in diese Verarbeitung eingeholt und auf diese Datenschutzerklärung verwiesen. Die dabei von uns erhobenen Daten werden ausschließlich für die Zurverfügungstellung des Kundenkontos verwendet.&nbsp;</p>
        <p>Soweit Sie in diese Verarbeitung einwilligen, ist Art. 6 Abs. 1 lit. a) DSGVO Rechtsgrundlage für die Verarbeitung.</p>
        <p>Sofern die Eröffnung des Kundenkontos zusätzlich auch vorvertraglichen Maßnahmen oder der Vertragserfüllung dient, so ist Rechtsgrundlage für diese Verarbeitung auch noch Art. 6 Abs. 1 lit. b) DSGVO.</p>
        <p>Die uns erteilte Einwilligung in die Eröffnung und den Unterhalt des Kundenkontos können Sie gemäß Art. 7 Abs. 3 DSGVO jederzeit mit Wirkung für die Zukunft widerrufen. Hierzu müssen Sie uns lediglich über Ihren Widerruf in Kenntnis setzen.</p>
        <p>Die insoweit erhobenen Daten werden gelöscht, sobald die Verarbeitung nicht mehr erforderlich ist. Hierbei müssen wir aber steuer- und handelsrechtliche Aufbewahrungsfristen beachten.</p>

        <h4>Google Analytics</h4>
        <p>In unserem Internetauftritt setzen wir Google Analytics ein. Hierbei handelt es sich um einen Webanalysedienst der Google LLC, 1600 Amphitheatre Parkway, Mountain View, CA 94043 USA, nachfolgend nur „Google“ genannt.</p>
        <p>Durch die Zertifizierung nach dem EU-US-Datenschutzschild („EU-US Privacy Shield“)</p>
        <p><a target="_blank" rel="nofollow" href="https://www.privacyshield.gov/participant?id=a2zt000000001L5AAI&amp;status=Active">https://www.privacyshield.gov/participant?id=a2zt000000001L5AAI&amp;status=Active</a></p>
        <p>garantiert Google, dass die Datenschutzvorgaben der EU auch bei der Verarbeitung von Daten in den USA eingehalten werden.</p>
        <p>Der Dienst Google Analytics dient zur Analyse des Nutzungsverhaltens unseres Internetauftritts. Rechtsgrundlage ist Art. 6 Abs. 1 lit. f) DSGVO. Unser berechtigtes Interesse liegt in der Analyse, Optimierung und dem wirtschaftlichen Betrieb unseres Internetauftritts.</p>
        <p>Nutzungs- und nutzerbezogene Informationen, wie bspw. IP-Adresse, Ort, Zeit oder Häufigkeit des Besuchs unseres Internetauftritts, werden dabei an einen Server von Google in den USA übertragen und dort gespeichert. Allerdings nutzen wir Google Analytics mit der sog. Anonymisierungsfunktion. Durch diese Funktion kürzt Google die IP-Adresse schon innerhalb der EU bzw. des EWR.</p>
        <p>Die so erhobenen Daten werden wiederum von Google genutzt, um uns eine Auswertung über den Besuch unseres Internetauftritts sowie über die dortigen Nutzungsaktivitäten zur Verfügung zu stellen. Auch können diese Daten genutzt werden, um weitere Dienstleistungen zu erbringen, die mit der Nutzung unseres Internetauftritts und der Nutzung des Internets zusammenhängen.</p>
        <p>Google gibt an, Ihre IP-Adresse nicht mit anderen Daten zu verbinden. Zudem hält Google unter</p>
        <p><a target="_blank" rel="nofollow" href="https://www.google.com/intl/de/policies/privacy/partners">https://www.google.com/intl/de/policies/privacy/partners</a></p>
        <p>weitere datenschutzrechtliche Informationen für Sie bereit, so bspw. auch zu den Möglichkeiten, die Datennutzung zu unterbinden.</p>
        <p>Zudem bietet Google unter</p>
        <p><a target="_blank" rel="nofollow" href="https://tools.google.com/dlpage/gaoptout?hl=de">https://tools.google.com/dlpage/gaoptout?hl=de</a></p>
        <p>ein sog. Deaktivierungs-Add-on nebst weiteren Informationen hierzu an. Dieses Add-on lässt sich mit den gängigen Internet-Browsern installieren und bietet Ihnen weitergehende Kontrollmöglichkeit über die Daten, die Google bei Aufruf unseres Internetauftritts erfasst. Dabei teilt das Add-on dem JavaScript (ga.js) von Google Analytics mit, dass Informationen zum Besuch unseres Internetauftritts nicht an Google Analytics übermittelt werden sollen. Dies verhindert aber nicht, dass Informationen an uns oder an andere Webanalysedienste übermittelt werden. Ob und welche weiteren Webanalysedienste von uns eingesetzt werden, erfahren Sie natürlich ebenfalls in dieser Datenschutzerklärung.</p>

        <h4>Google Fonts</h4>
        <p>In unserem Internetauftritt setzen wir Google Fonts zur Darstellung externer Schriftarten ein. Es handelt sich hierbei um einen Dienst der Google LLC, 1600 Amphitheatre Parkway, Mountain View, CA 94043 USA, nachfolgend nur „Google“ genannt.</p>
        <p>Durch die Zertifizierung nach dem EU-US-Datenschutzschild („EU-US Privacy Shield“)</p>
        <p><a target="_blank" rel="nofollow" href="https://www.privacyshield.gov/participant?id=a2zt000000001L5AAI&amp;status=Active">https://www.privacyshield.gov/participant?id=a2zt000000001L5AAI&amp;status=Active</a></p>
        <p>garantiert Google, dass die Datenschutzvorgaben der EU auch bei der Verarbeitung von Daten in den USA eingehalten werden.</p>
        <p>Um die Darstellung bestimmter Schriften in unserem Internetauftritt zu ermöglichen, wird bei Aufruf unseres Internetauftritts eine Verbindung zu dem Google-Server in den USA aufgebaut.</p>
        <p>Rechtsgrundlage ist Art. 6 Abs. 1 lit. f) DSGVO. Unser berechtigtes Interesse liegt in der Optimierung und dem wirtschaftlichen Betrieb unseres Internetauftritts.</p>
        <p>Durch die bei Aufruf unseres Internetauftritts hergestellte Verbindung zu Google kann Google ermitteln, von welcher Website Ihre Anfrage gesendet worden ist und an welche IP-Adresse die Darstellung der Schrift zu übermitteln ist.</p>
        <p>Google bietet unter</p>
        <p><a target="_blank" rel="nofollow" href="https://adssettings.google.com/authenticated">https://adssettings.google.com/authenticated</a></p>
        <p><a target="_blank" rel="nofollow" href="https://policies.google.com/privacy">https://policies.google.com/privacy</a></p>
        <p>weitere Informationen an und zwar insbesondere zu den Möglichkeiten der Unterbindung der Datennutzung.</p>

        <h4>„Facebook“-Social-Plug-in</h4>
        <p>In unserem Internetauftritt setzen wir das Plug-in des Social-Networks Facebook ein. Bei Facebook handelt es sich um einen Internetservice der facebook Inc., 1601 S. California Ave, Palo Alto, CA 94304, USA. In der EU wird dieser Service wiederum von der Facebook Ireland Limited, 4 Grand Canal Square, Dublin 2, Irland, betrieben, nachfolgend beide nur „Facebook“ genannt.</p>
        <p>Durch die Zertifizierung nach dem EU-US-Datenschutzschild („EU-US Privacy Shield“)</p>
        <p><a target="_blank" rel="nofollow" href="https://www.privacyshield.gov/participant?id=a2zt0000000GnywAAC&amp;status=Active">https://www.privacyshield.gov/participant?id=a2zt0000000GnywAAC&amp;status=Active</a></p>
        <p>garantiert Facebook, dass die Datenschutzvorgaben der EU auch bei der Verarbeitung von Daten in den USA eingehalten werden.</p>
        <p>Rechtsgrundlage ist Art. 6 Abs. 1 lit. f) DSGVO. Unser berechtigtes Interesse liegt in der Qualitätsverbesserung unseres Internetauftritts.</p>
        <p>Weitergehende Informationen über die möglichen Plug-ins sowie über deren jeweilige Funktionen hält Facebook unter</p>
        <p><a target="_blank" rel="nofollow" href="https://developers.facebook.com/docs/plugins/">https://developers.facebook.com/docs/plugins/</a></p>
        <p>für Sie bereit.</p>
        <p>Sofern das Plug-in auf einer der von Ihnen besuchten Seiten unseres Internetauftritts hinterlegt ist, lädt Ihr Internet-Browser eine Darstellung des Plug-ins von den Servern von Facebook in den USA herunter. Aus technischen Gründen ist es dabei notwendig, dass Facebook Ihre IP-Adresse verarbeitet. Daneben werden aber auch Datum und Uhrzeit des Besuchs unserer Internetseiten erfasst.</p>
        <p>Sollten Sie bei Facebook eingeloggt sein, während Sie eine unserer mit dem Plug-in versehenen Internetseite besuchen, werden die durch das Plug-in gesammelten Informationen Ihres konkreten Besuchs von Facebook erkannt. Die so gesammelten Informationen weist Facebook womöglich Ihrem dortigen persönlichen Nutzerkonto zu. Sofern Sie also bspw. den sog. „Gefällt mir“-Button von Facebook benutzen, werden diese Informationen in Ihrem Facebook-Nutzerkonto gespeichert und ggf. über die Plattform von Facebook veröffentlicht. Wenn Sie das verhindern möchten, müssen Sie sich entweder vor dem Besuch unseres Internetauftritts bei Facebook ausloggen oder durch den Einsatz eines Add-ons für Ihren Internetbrowser verhindern, dass das Laden des Facebook-Plug-in blockiert wird.</p>
        <p>Weitergehende Informationen über die Erhebung und Nutzung von Daten sowie Ihre diesbezüglichen Rechte und Schutzmöglichkeiten hält Facebook in den unter</p>
        <p><a target="_blank" rel="nofollow" href="https://www.facebook.com/policy.php">https://www.facebook.com/policy.php</a></p>
        <p>abrufbaren Datenschutzhinweisen bereit.</p>

        <h4>„Twitter“-Social-Plug-in</h4>
        <p>In unserem Internetauftritt setzen wir das Plug-in des Social-Networks Twitter ein. Bei Twitter handelt es sich um einen Internetservice der Twitter Inc., 795 Folsom St., Suite 600, San Francisco, CA 94107, USA, nachfolgend nur „Twitter“ genannt.</p>
        <p>Durch die Zertifizierung nach dem EU-US-Datenschutzschild („EU-US Privacy Shield“)</p>
        <p><a target="_blank" rel="nofollow" href="https://www.privacyshield.gov/participant?id=a2zt0000000TORzAAO&amp;status=Active">https://www.privacyshield.gov/participant?id=a2zt0000000TORzAAO&amp;status=Active</a></p>
        <p>garantiert Twitter, dass die Datenschutzvorgaben der EU auch bei der Verarbeitung von Daten in den USA eingehalten werden.</p>
        <p>Rechtsgrundlage ist Art. 6 Abs. 1 lit. f) DSGVO. Unser berechtigtes Interesse liegt in der Qualitätsverbesserung unseres Internetauftritts.</p>
        <p>Sofern das Plug-in auf einer der von Ihnen besuchten Seiten unseres Internetauftritts hinterlegt ist, lädt Ihr Internet-Browser eine Darstellung des Plug-ins von den Servern von Twitter in den USA herunter. Aus technischen Gründen ist es dabei notwendig, dass Twitter Ihre IP-Adresse verarbeitet. Daneben werden aber auch Datum und Uhrzeit des Besuchs unserer Internetseiten erfasst.</p>
        <p>Sollten Sie bei Twitter eingeloggt sein, während Sie eine unserer mit dem Plug-in versehenen Internetseite besuchen, werden die durch das Plug-in gesammelten Informationen Ihres konkreten Besuchs von Twitter erkannt. Die so gesammelten Informationen weist Twitter womöglich Ihrem dortigen persönlichen Nutzerkonto zu. Sofern Sie also bspw. den sog. „Teilen“-Button von Twitter benutzen, werden diese Informationen in Ihrem Twitter-Nutzerkonto gespeichert und ggf. über die Plattform von Twitter veröffentlicht. Wenn Sie das verhindern möchten, müssen Sie sich entweder vor dem Besuch unseres Internetauftritts bei Twitter ausloggen oder die entsprechenden Einstellungen in Ihrem Twitter-Benutzerkonto vornehmen.</p>
        <p>Weitergehende Informationen über die Erhebung und Nutzung von Daten sowie Ihre diesbezüglichen Rechte und Schutzmöglichkeiten hält Twitter in den unter</p>
        <p><a target="_blank" rel="nofollow" href="https://twitter.com/privacy">https://twitter.com/privacy</a></p>
        <p>abrufbaren Datenschutzhinweisen bereit.</p>

        <h4>YouTube</h4>
        <p>In unserem Internetauftritt setzen wir YouTube ein. Hierbei handelt es sich um ein Videoportal der YouTube LLC., 901 Cherry Ave., 94066 San Bruno, CA, USA, nachfolgend nur „YouTube“ genannt.</p>
        <p>YouTube ist ein Tochterunternehmen der Google LLC., 1600 Amphitheatre Parkway, Mountain View, CA 94043 USA, nachfolgend nur „Google“ genannt.</p>
        <p>Durch die Zertifizierung nach dem EU-US-Datenschutzschild („EU-US Privacy Shield“)</p>
        <p><a target="_blank" rel="nofollow" href="https://www.privacyshield.gov/participant?id=a2zt000000001L5AAI&amp;status=Active">https://www.privacyshield.gov/participant?id=a2zt000000001L5AAI&amp;status=Active</a></p>
        <p>garantiert Google und damit auch das Tochterunternehmen YouTube, dass die Datenschutzvorgaben der EU auch bei der Verarbeitung von Daten in den USA eingehalten werden.</p>
        <p>Wir nutzen YouTube im Zusammenhang mit der Funktion „Erweiterter Datenschutzmodus“, um Ihnen Videos anzeigen zu können. Rechtsgrundlage ist Art. 6 Abs. 1 lit. f) DSGVO. Unser berechtigtes Interesse liegt in der Qualitätsverbesserung unseres Internetauftritts. Die Funktion „Erweiterter Datenschutzmodus“ bewirkt laut Angaben von YouTube, dass die nachfolgend noch näher bezeichneten Daten nur dann an den Server von YouTube übermittelt werden, wenn Sie ein Video auch tatsächlich starten.</p>
        <p>Ohne diesen „Erweiterten Datenschutz“ wird eine Verbindung zum Server von YouTube in den USA hergestellt, sobald Sie eine unserer Internetseiten, auf der ein YouTube-Video eingebettet ist, aufrufen.</p>
        <p>Diese Verbindung ist erforderlich, um das jeweilige Video auf unserer Internetseite über Ihren Internet-Browser darstellen zu können. Im Zuge dessen wird YouTube zumindest Ihre IP-Adresse, das Datum nebst Uhrzeit sowie die von Ihnen besuchte Internetseite erfassen und verarbeiten. Zudem wird eine Verbindung zu dem Werbenetzwerk „DoubleClick“ von Google hergestellt.</p>
        <p>Sollten Sie gleichzeitig bei YouTube eingeloggt sein, weist YouTube die Verbindungsinformationen Ihrem YouTube-Konto zu. Wenn Sie das verhindern möchten, müssen Sie sich entweder vor dem Besuch unseres Internetauftritts bei YouTube ausloggen oder die entsprechenden Einstellungen in Ihrem YouTube-Benutzerkonto vornehmen.</p>
        <p>Zum Zwecke der Funktionalität sowie zur Analyse des Nutzungsverhaltens speichert YouTube dauerhaft Cookies über Ihren Internet-Browser auf Ihrem Endgerät. Falls Sie mit dieser Verarbeitung nicht einverstanden sind, haben Sie die Möglichkeit, die Speicherung der Cookies durch eine Einstellung in Ihrem Internet-Browsers zu verhindern. Nähere Informationen hierzu finden Sie vorstehend unter „Cookies“.</p>
        <p>Weitergehende Informationen über die Erhebung und Nutzung von Daten sowie Ihre diesbezüglichen Rechte und Schutzmöglichkeiten hält Google in den unter</p>
        <p><a target="_blank" rel="nofollow" href="https://policies.google.com/privacy">https://policies.google.com/privacy</a></p>
        <p>abrufbaren Datenschutzhinweisen bereit.</p>

        <h4>Online-Stellenbewerbungen / Veröffentlichung von Stellenanzeigen</h4>
        <p>Wir bieten Ihnen die Möglichkeit an, sich bei uns über unseren Internetauftritt bewerben zu können. Bei diesen digitalen Bewerbungen werden Ihre Bewerber- und Bewerbungsdaten von uns zur Abwicklung des Bewerbungsverfahrens elektronisch erhoben und verarbeitet.</p>
        <p>Rechtsgrundlage für diese Verarbeitung ist § 26 Abs. 1 S. 1 BDSG i.V.m. Art. 88 Abs. 1 DSGVO.</p>
        <p>Sofern nach dem Bewerbungsverfahren ein Arbeitsvertrag geschlossen wird, speichern wir Ihre bei der Bewerbung übermittelten Daten in Ihrer Personalakte zum Zwecke des üblichen Organisations- und Verwaltungsprozesses – dies natürlich unter Beachtung der weitergehenden rechtlichen Verpflichtungen.</p>
        <p>Rechtsgrundlage für diese Verarbeitung ist ebenfalls § 26 Abs. 1 S. 1 BDSG i.V.m. Art. 88 Abs. 1 DSGVO.</p>
        <p>Bei der Zurückweisung einer Bewerbung löschen wir die uns übermittelten Daten automatisch zwei Monate nach der Bekanntgabe der Zurückweisung. Die Löschung erfolgt jedoch nicht, wenn die Daten aufgrund gesetzlicher Bestimmungen, bspw. wegen der Beweispflichten nach dem AGG, eine längere Speicherung von bis zu vier Monaten oder bis zum Abschluss eines gerichtlichen Verfahrens erfordern.</p>
        <p>Rechtsgrundlage ist in diesem Fall Art. 6 Abs. 1 lit. f) DSGVO und § 24 Abs. 1 Nr. 2 BDSG. Unser berechtigtes Interesse liegt in der Rechtsverteidigung bzw. -durchsetzung.</p>
        <p>Sofern Sie ausdrücklich in eine längere Speicherung Ihrer Daten einwilligen, bspw. für Ihre Aufnahme in eine Bewerber- oder Interessentendatenbank, werden die Daten aufgrund Ihrer Einwilligung weiterverarbeitet. Rechtsgrundlage ist dann Art. 6 Abs. 1 lit. a) DSGVO. Ihre Einwilligung können Sie aber natürlich jederzeit nach Art. 7 Abs. 3 DSGVO durch Erklärung uns gegenüber mit Wirkung für die Zukunft widerrufen.</p>

        <p>
        <a target="_blank" href="https://www.ratgeberrecht.eu/leistungen/muster-datenschutzerklaerung.html">Muster-Datenschutzerklärung</a> der <a target="_blank" href="https://www.ratgeberrecht.eu/">Anwaltskanzlei Weiß &amp; Partner</a></p>



        <hr/>
        <p style="padding-top: 20px">
            memucho wird im Rahmen des EXIST-Programms durch das Bundesministerium für Wirtschaft und Energie und den Europäischen Sozialfonds gefördert.
        </p>

    </div>
</div>

</asp:Content>
