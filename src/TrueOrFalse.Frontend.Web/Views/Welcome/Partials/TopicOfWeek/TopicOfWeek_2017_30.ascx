<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<TopicOfWeek_2017_30Model>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="categoryOfWeekContainer">
    <h1>Thema der Woche: Psychologie</h1>
    <div class="row">
        <div class="col-xs-12">
            <div class="" style="display: inline; float: left;">
                <div class="ImageContainer">
                    <%= Model.ImageFrontendData.RenderHtmlImageBasis(350, false, ImageType.Category, "ImageContainer") %>
                </div>
            </div>

            <p>
                Die Psychologie ist eine Wissenschaft, die sich mit den seelischen Vorgängen im Menschen befasst. 
                Sie ist nicht nur ein sehr beliebtes Studienfach, sondern auch ein sehr beliebtes populärwissenschaftliches Thema.
                Viele Menschen haben gewisse persönliche Grundannahmen darüber, warum sich Menschen wie verhalten. 
                Etwas abfällig oder (selbst-)ironisch spricht man von "Küchenpsychologie", wenn es um eine 
                naive und unreflektierte Verwendung alltagspsychologischer Kenntnisse geht. Für andere ist die Psychologie
                vor allem die Kunst, Menschen einzuschätzen und zu lenken.
            </p>
            <p>
                Dabei hat die Psychologie tatsächlich eine Vielzahl an spannenden Forschungsfeldern und Erkenntnissen zu bieten.
                Wie vielfältig das Fach ist, zeigt unsere <a href="/Kategorien/Psychologie-Studium/264">Themenseite zum Studienfach Psychologie</a>: 
                Von der Lern-, Bio- oder Entwicklungspsychologie über die Arbeits- und Organisations-Psychologie bis zur Klinischen Psychologie. 
                Ein weiterer Schwerpunkt in dem Fach sind die <a href="/Kategorien/Statistik-fuer-PsychologInnen/649">Methoden</a>, 
                denn viele Erkenntnisse werden über quantitative Studien gewonnen. Quantitativ bedeutet, dass viele Menschen/Fälle 
                betrachtet werden und die Ergebnisse statistisch ausgewertet werden.
            </p>
            <div style="text-align: center;">
                <a class="btn btn-lg btn-primary" href="/Kategorien/Psychologie-Studium/264">Zur Themenseite Psychologie</a>
            </div>
        </div>

    <div class="col-md-12 col-lg-7"  style="clear: both; margin-top: 40px;">
        <h3>Kleiner Quiz gefällig?</h3>
        <p>
            Interessierst du dich für ein Studium der Psychologie? Hier kanns du auf hohem Niveau dein Wissen zu den Grundlagen der Lernpsychologie testen:
        </p>
        <script src="https://memucho.de/views/widgets/w.js" data-t="set" data-id="116" data-width="100%" data-hideKnowledgeBtn="true"></script>

    </div>
    <div class="col-md-12 col-lg-5">
        Hier sind einige weitere interessante Lernsets aus der Psychologie:
        <div class="row">
            <% foreach (var setId in Model.AdditionalSets) { %>
                <div class="col-xs-6 col-md-3 col-lg-6">
                    <% Html.RenderPartial("WelcomeCardMiniSet", new WelcomeCardMiniSetModel(setId)); %>
                </div>
            <% } %>
        </div>
    </div>

    </div>

</div>
