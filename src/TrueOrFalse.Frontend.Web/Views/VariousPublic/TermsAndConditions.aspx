<%@ Page Title="memucho Nutzungsbedingungen" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="System.Web.Mvc.ViewPage<BaseModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

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
    
    <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Nutzungsbedingungen", Url = "/AGB", ToolTipText = "Nutzungsbedingungen"});
       Model.TopNavMenu.IsCategoryBreadCrumb = false;%>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<div class="row" style="padding-top:30px;">
    <div class="BackToHome col-md-3">
        <p>
            <a href="<%= Url.Action(Links.RegisterAction, Links.RegisterController) %>"><i class="fa fa-chevron-left">&nbsp;</i>Zum Registrieren</a> 
        </p>
        <p>
            <a href="/"><i class="fa fa-chevron-left">&nbsp;</i>Zur Startseite</a>
        </p>
    </div>
    <div class="form-horizontal col-md-9">

        <h2 class="PageHeader">Nutzungsbedingungen</h2>
        
        <p style="font-style: italic; font-size: 120%;">
            Unser Ziel ist es, dass Lernen einfacher wird und mehr Spaß macht. Eine Grundlage dafür ist die Förderung von offenen Bildungsinhalten, 
            damit das Wissen dieser Welt für alle zugänglich ist. <strong>Wir freuen uns daher, dass du bei memucho mitmachst</strong> und wünschen dir, 
            dass du viele spannende Inhalte entdeckst und dein eigenes Wissen mit anderen teilst. Die wenigen Dinge, die du dabei beachten musst, 
            sind in diesen Nutzungsbedingungen gesammelt. Du musst sie akzeptieren, um dich bei memucho zu registrieren.
        </p>
        
        <div class="alert alert-info" role="alert" style="margin: 25px 0;">
            <h4>Das Wichtigste in Kürze:</h4>
            <ul>
                <li>Sei nett zu anderen memucho-Nutzern!</li>
                <li>Wenn du Inhalte (z.B. Fragen) selber einstellst, stelle sicher, dass du keine Urheberrechte verletzt.</li>
                <li>Wenn du Inhalte einstellst, achte auf Richtigkeit und Rechtschreibung. Stelle keine Inhalte ein, die falsch, beleidigend, verboten oder Werbung sind.</li>
                <li>Registrieren dürfen sich nur Privatpersonen und z.B. keine Unternehmen.</li>
            </ul>
            <p style="font-style: italic; margin-top: 20px;">Diese Zusammenfassung ist nicht Teil der rechtsverbindlichen Nutzungsbedingungen. Sie dient allein dem Überblick und ist informativ.</p>
        </div>
        
        <h3>1) Verhaltensregeln</h3>
        <p>
            memucho ist eine soziale Wissensplattform. Sie lebt davon, dass viele Menschen sich zusammentun, ihr Wissen bereitstellen, 
            zusammen lernen und sich gegenseitig unterstützen. Wo viele Menschen aufeinandertreffen, kann es aber auch zu Konflikten kommen, 
            deswegen ist es wichtig, verschiedene Regeln zu beachten. Wir haben nur eine Regel:<br/>
            <strong>Sei nett!</strong> Egal, was dich ärgert, gehe immer von guten Intentionen der anderen aus. 
            Denke daran, dass am anderen Ende der Internetleitung ein Mensch sitzt. Äußere Kritik sachlich und mit Respekt und biete deine Hilfe an, 
            wenn du helfen kannst.
        </p>
        

        <h3>2) Urheberrecht & Geistiges Eigentum</h3>
        <p>
            Urheberrecht und geistiges Eigentum sind sensible Themen im Internet. <strong>Du bist als Nutzer selbst verantwortlich für die Inhalte</strong>, 
            die du erstellst. memucho ist nicht für die Inhalte verantwortlich, die du oder andere Nutzer erstellt haben, sondern stellt diese lediglich bereit.
        </p>
        <p>
            Wenn du Texte oder Bilder bei memucho einstellst, musst du sicher sein, dass diese nicht urheberrechtlich geschützt sind. 
            Dies bestätigst du jedesmal, wenn du eine Frage erstellst oder änderst, es gilt aber auch bei allen anderen Bereichen 
            (Themen, Lernsets etc.). Denke daran, dass Inhalte bei memucho generell unter der Creative Commons-Lizenz “CC BY 4.0” stehen. 
            Wenn du sie einmal eingestellt hast, können sie von allen anderen (unter bestimmten Bedingungen) frei verwendet werden. Besondere 
            Vorsicht ist z.B. bei Schulbüchern geboten, hier darfst du nicht abschreiben. Wenn du dir nicht sicher bist, verzichte im Zweifelsfall 
            lieber auf eine bestimmte Frage oder ein Bild. Für Verstöße bist du verantwortlich. Erfahren wir von solchen Verstöße gegen das 
            Urheberrecht oder Verwertungsrechte, entfernen wir die betroffenen Inhalte umgehend. Solltest du auf Inhalte stoßen, bei denen du 
            ein Verstoß gegen das Urheberrecht vermutest, sage uns bitte Bescheid.
        </p>
        <p>
            Deine eigenen Bilder und deine eigenen Texte kannst du verwenden, hier bist du der Urheber. (Das gilt natürlich nur, 
            solange es wirklich deine eigenen Formulierungen sind und du nicht abgeschrieben hast.) Auch bei Wikipedia und Wikicommons 
            stehen fast alle Texte und Bilder unter offen Lizenzen (z.B. einer Creative Commons-Lizenz oder einer GNU-Lizenz) oder 
            sind sogar gemeinfrei (sie gehören also niemandem), so dass du sie sehr gut bei memucho nutzen kannst.
        </p>
        
        
        <h3>3) Die Inhalte: Was, wie & was nicht</h3>
        <p>
            Inhalte, die für die Allgemeinheit nicht relevant sind, müssen als private Fragen markiert sein (zum Beispiel, wann deine 
            Tante Geburtstag hat, wie die Telefonnummer von deinem Freund lautet oder welches dein Lieblingskuchen ist).
        </p>
        <p>
            Denke daran, dass alle anderen Inhalte von dir von allen memucho-Nutzern gesehen und genutzt werden können! <strong>Achte deshalb auf</strong>
            <ul>
                <li>Richtigkeit und Eindeutigkeit deiner Fragen und Antworten,</li>
                <li>Rechtschreibung, und</li>
                <li>Gib, wo dies möglich ist, Quellen an, damit andere im Zweifel die Antwort nachprüfen oder sich weiter informieren können</li>
            </ul>
        </p>
        <p>
            memucho ist eine offene Wissensplattform. Alles Wissen der Welt soll hier Platz haben! Allerdings darf die Plattform 
            nicht für andere Zwecke missbraucht werden. Daher darfst du <strong>keine Inhalte einstellen, die</strong>
            <ul>
                <li>offensichtlich falsch sind,</li>
                <li>offensichtlich und primär der (kommerziellen, religiösen etc.) Werbung dienen,</li>
                <li>absichtlich andere beleidigen oder</li>
                <li>gegen geltendes Recht verstoßen (öbszöne Inhalte, Kinderpornografie u.ä.).</li>
            </ul>
        </p>
        
        <h3>4) Nutzer & Mitgliedschaft</h3>
        <p>
            Als normale Nutzer registrieren dürfen sich nur Privatpersonen und z.B. keine Unternehmen. Für Unternehmen, gemeinnützige Vereine etc. 
            gibt es nach Absprache andere Modelle der Mitgliedschaft.
        </p>


    </div>
</div>

</asp:Content>
