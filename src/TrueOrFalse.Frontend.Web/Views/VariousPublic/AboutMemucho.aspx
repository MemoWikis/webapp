<%@ Page Title="Über memucho" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/VariousPublic/AboutMemucho.css" rel="stylesheet" />

<script type="text/javascript" >

    /* Source http://www.html-advisor.com/javascript/hide-email-with-javascript-jquery/ */
    $(function () {
        var spt = $('span.mailme');
        var at = / at /;
        var dot = / dot /g;
        var addr = $(spt).text().replace(at, "@").replace(dot, ".");
        $(spt).after('<a href="mailto:' + addr + '" title="Schreibe eine E-Mail">' + addr + '</a>').hover(function () { window.status = "Schreibe eine E-Mail!"; }, function () { window.status = ""; });
        $(spt).remove();
    });
</script>    
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<div class="row">
    <div class="col-xs-12">
        <h1 class="PageHeader" style="margin-bottom: 15px; margin-top: 0px;"><span class="ColoredUnderline Question">memucho: Die schnelle und sichere Art zu lernen</span></h1>
    </div>
</div>
<div class="row">
    <div class="col-xs-12">
        <div class="well">
            <p>
                Wir möchten, dass du mit memucho besser lernen kannst und mehr Spaß dabei hast, neues und altes Wissen zu entdecken. Das ist unser großes Ziel! 
                Noch sind wir in der <a href="<%= Links.BetaInfo() %>">Beta-Phase</a> und freuen uns, dass du von Anfang an dabei bist. 
                Hier erfährst du, welche Vorteile dir memucho bietet und welche Prinzipien uns leiten.
            </p>
        </div>

        <div class="well">
            <div class="row aboutRow">
                <div class="col-xs-3 aboutImg xxs-stack">
                    <div class="aboutImgInner">
                        <i class="fa fa-clock-o fa-3x"></i>
                    </div>
                </div>
                <div class="col-xs-9 xxs-stack aboutText">
                    <h3>Zeit sparen</h3>
                    <p>
                        memucho weiß, welche Fragen du am dringendsten lernen musst und welche (noch) nicht.
                        So lernst du immer das Richtige und musst nie wieder umsonst lernen. Dadurch sparst du Zeit.
                    </p>
                    <p>
                        Wie wir das machen? Wir analysieren, wie und wofür du lernst und wie andere Nutzer die Inhalte gelernt haben. 
                        Dadurch können wir vorhersagen, wie gut du etwas (noch) weißt. Nutze dafür unsere Funktion "Jetzt üben". 
                        Sie ist nur verfügbar, wenn du <a href="<%= Links.Login() %>">angemeldet bist</a>.
                        (<a href="<%= Links.AlgoInsightForecast() %>">Hier erfährst du mehr über unsere Technologie.</a>)
                    </p>
                </div>
            </div>

            <div class="row aboutRow">
                <div class="col-xs-3 xxs-stack aboutImg <%--col-xs-push-9--%>">
                    <div class="aboutImgInner">
                        <i class="fa fa-check-circle fa-3x"></i>
                    </div>
                </div>
                <div class="col-xs-9 xxs-stack aboutText <%--col-xs-pull-3--%>">
                    <h3>Sicherheit gewinnen</h3>
                    <p>
                        Mit memucho weißt du immer, wie viel Zeit du noch zum Lernen brauchst und ob du deine Lernziele erreichst. 
                        Du liegst im Plan? Dann kannst du mit gutem Gewissen ins Kino gehen.
                    </p>
                    <p>
                        Wo? Im Moment musst du dafür die einen Prüfungstermin anlegen (vergleiche den nächsten Punkt), aber ganz bald 
                        kannst du auch dein Wunschwissen optimiert lernen - also all das, was du dir immer merken möchtest.
                    </p>
                </div>
            </div>

            <div class="row aboutRow">
                <div class="col-xs-3 xxs-stack aboutImg">
                    <div class="aboutImgInner">
                        <i class="fa fa-line-chart fa-3x"></i>
                    </div>
                </div>
                <div class="col-xs-9 xxs-stack aboutText">
                    <h3>Optimiert für eine Prüfung lernen</h3>
                    <p>
                        memucho erstellt dir einen persönlichen Übungsplan, wenn du dich auf eine Prüfung vorbereitest. 
                        Intelligente Algorithmen sagen dir, wann du was am besten lernst und ob du deine Lernziele erreichen wirst. 
                        memucho ist dein persönlicher Coach beim Auswendiglernen.
                    </p>
                    <p>
                        Wie das geht? Gehe zu <a href="<%= Links.Dates() %>">Termine</a>, lege einen neuen Termin an und wähle die
                        zu lernenden Fragesätze aus. memucho erstellt dir dann automatisch einen persönlichen Übungsplan.
                    </p>
                </div>
            </div>

            <div class="row aboutRow">
                <div class="col-xs-3 xxs-stack aboutImg <%--col-xs-push-9--%>">
                    <div class="aboutImgInner">
                        <i class="fa fa-heart fa-3x" style="color:#b13a48"></i>
                    </div>
                </div>
                <div class="col-xs-9 xxs-stack aboutText <%--col-xs-pull-3--%>">
                    <h3>Wunschwissen sammeln und nie vergessen</h3>
                    <p>
                        Bei memucho kannst du dein <i class="fa fa-heart" style="color:#b13a48;"></i> Wunschwissen zusammenstellen. 
                        Entscheide, was du dir merken möchtest und memucho erinnert dich rechtzeitig daran, 
                        wenn du es wieder lernen musst, um es nicht zu vergessen.
                    </p>
                    <p>
                        Wo? In deiner <a href="<%= Links.Knowledge() %>">Wissenszentrale</a> bekommst du jederzeit einen
                        Überblick zu deinem Wunschwissen.
                    </p>
                </div>
            </div>

            <div class="row aboutRow">
                <div class="col-xs-3 xxs-stack aboutImg">
                    <div class="aboutImgInner">
                        <i class="fa fa-cogs fa-3x"></i>
                    </div>
                </div>
                <div class="col-xs-9 xxs-stack aboutText">
                    <h3>Und vieles mehr...</h3>
                    <p>
                        Mit memucho behältst du immer den Überblick, egal wie viel Wissen du dir merken möchtest oder wieviele Prüfungen du hast. 
                        memucho zeigt dir ausführliche <a href="<%= Links.Knowledge() %>">Lernauswertungen</a> und Statistiken 
                        und du kannst <a href="<%= Links.Users() %>">deinen Freunden folgen</a>, 
                        mit ihnen lernen und im <a href="<%= Links.Games() %>">Quiz-Spiel</a> gegen sie antreten.
                    </p>
                </div>
            </div>

        </div>
    </div>
</div>

</asp:Content>
