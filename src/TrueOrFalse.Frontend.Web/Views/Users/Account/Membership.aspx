 <%@ Page Title="Mitgliedschaft" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<MembershipModel>" %> <%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Scripts.Render("~/Views/Users/Account/Js/Membership.js") %>
    <%= Styles.Render("~/Views/Users/Account/Membership.css") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="PageHeader">Fördermitglied werden</h2>
    
    <h3>Du möchtest Fördermitglied werden?</h3>
    <p>Du findest, dass MEMuchO eine tolle Sache ist und möchtest uns unterstützen? 
        Dann werde Fördermitglied der ersten Stunde!</p>
    <p>Wir sind noch nicht ganz fertig, MEMuchO befindet sich in der Beta-Phase. 
        Einige Funktionen fehlen noch (zum Beispiel das Lernen zu einem bestimmten Termin), 
        andere Dinge müssen wir noch verbessern und benutzerfreundlicher machen (zum Beispiel den Lernalgorithmus). 
        Außerdem haben wir noch sooo viele Ideen, die auf Verwirklichung warten. Wir arbeiten daran! 
        Aber gerade in der schwierigen Startphase brauchen wir dich und deine Unterstützung! 
        Wenn Du an uns glaubst, dann werde jetzt Fördermitglied!</p>
    
    <h3>Was bekommst Du? </h3>
    <ul>
        <li> Ein Förder-Medaille "Mitglied-der-1.-Stunde" in deinem Profil auf Lebenszeit.</li>
        <li>Das gute Gefühl, eine tolle Idee zu unterstützen.</li>
        <li>Alle MEMuchO-Funktionen ohne Beschränkungen, insbesondere...</li>
        <li> ...unbegrenzt private Fragen (sonst 20 Fragen)</li>
        <li>...unbegrenzt Wunschwissen (sonst 50 Fragen)</li>
        <li> ...Nutzung von Lernterminen.</li>
    </ul>
    <p>In der Beta-Phase sind die Funktionalitäten für Nichtmitglieder noch nicht beschränkt. 
        Bis zum offiziellen Start unserer Plattform 
        winken für dich als Fördermitglied also "nur" Ruhm, Ehre und die besondere Medaille 
        &#8211 und das gute Gefühl, eine tolle Sache zu unterstützen.</p>

    <h3>Dein Beitrag</h3>
    <p>Dein Beitrag richtet sich nach deinen finanziellen Möglichkeiten und deiner Motivation.</p>
    
<% using (Html.BeginForm("Create", "EditQuestion", null, FormMethod.Post, new { id="BecomeMemberForm", enctype = "multipart/form-data" })){ %>
    <div id="PriceSelection" class="form-group">
        <input id="ChosenPrice" type="hidden"/>
        <div class="radio">
            <label>
                <input id="rdoPriceReduced" type="radio" name="PriceLevel"/>
                <span class="Title">Ermäßigt: </span>
                <span class="MinPrice">0,80</span>&nbsp;€ bis 1,99&nbsp;€ im Monat, empfohlen: <span class="bold">1&nbsp;€ </span>&nbsp;(12,00&nbsp;€ im Jahr)
            </label>
            <a href="#PriceReducedInfo" class="MoreLink" data-toggle="collapse" aria-expanded="false" aria-controls="PriceReducedInfo"><i class="fa fa-chevron-down"></i></a>
            <div id="PriceReducedInfo" class="PriceLevelInfo collapse">
                <div class="Description">
                    Du bist zum Beispiel Schüler, Student, Geringverdiener oder Lebenskünstler 
                    mit geringem Budget und kannst auch deine Eltern nicht um Unterstützung bitten. 
                    Den Wert eines halben Kaffees im Monat oder eines Taschenbuchs im Jahr 
                    kannst du aber für MEMuchO aufbringen.
                </div>
                <div class="ControlGroupInline JS-ValidationGroup" style="clear: left;">
                    <label class="control-label LabelInline">Dein Beitrag: </label>
                    <div class="ControlInline">
                        <input id="PriceReduced" name="PriceReduced" class="InputPrice NotInFocus form-control JS-ValidationGroupMember" type="text" value="1,00">
                    </div>
                    <label class="control-label LabelInline">€ &nbsp;&nbsp;(<span class="YearlyPrice">12,00</span>&nbsp;€ im Jahr)</label>
                </div>
            </div>
        </div>
        <div class="radio">
            <label>
                <input id="rdoPriceNormal" type="radio" name="PriceLevel" checked>
                <span class="Title">Normal: </span>
                <span class="MinPrice">2,00</span>&nbsp;€ bis 3,99&nbsp;€ im Monat, empfohlen: <span class="bold">3,00 € </span>&nbsp;(36,00&nbsp;€ im Jahr)
            </label>
            <a href="#PriceNormalInfo" class="MoreLink" data-toggle="collapse" aria-expanded="true" aria-controls="PriceNormalInfo"><i class="fa"></i></a>

            <div id="PriceNormalInfo" class="PriceLevelInfo collapse in">
                <div class="Description">
                    Du oder deine Eltern sind Normalverdiener.
                    3,00&nbsp;€ im Monat für Bildung gehen voll in Ordnung.
                </div>
                <div class="ControlGroupInline JS-ValidationGroup" style="clear: left;">
                    <label class="control-label LabelInline">Dein Beitrag: </label>
                    <div class="ControlInline">
                        <input id="PriceNormal" name="PriceNormal" class="InputPrice form-control JS-ValidationGroupMember" type="text" value="3,00">
                    </div>
                    <label class="control-label LabelInline">€ &nbsp;&nbsp;(<span class="YearlyPrice">36,00</span>&nbsp;€ im Jahr)</label>
                </div>
            </div>
        </div>
        <div class="radio">
            <label>
                <input id="rdoPriceSupporter" type="radio" name="PriceLevel"/>
                <span class="Title">Solibeitrag: </span>
                <span class="MinPrice">4,00</span>&nbsp;€ oder mehr im Monat, empfohlen: <span class="bold">5&nbsp;€ </span>&nbsp;(60,00&nbsp;€ im Jahr)
            </label>
            <a href="#PriceSupporterInfo" class="MoreLink" data-toggle="collapse" aria-expanded="false" aria-controls="PriceSupporterInfo"><i class="fa fa-chevron-down"></i></a>
            <div id="PriceSupporterInfo" class="PriceLevelInfo collapse">
                <div class="Description">
                    Dir oder deinen Eltern geht es finanziell gut oder sehr gut 
                    oder du findest unsere Idee so toll, dass du uns mit einem höheren Beitrag unterstützen möchtest.
                </div>
                <div class="ControlGroupInline JS-ValidationGroup" style="clear: left;">
                    <label class="control-label LabelInline">Dein Beitrag: </label>
                    <div class="ControlInline">
                        <input id="PriceSupporter" class="InputPrice NotInFocus form-control JS-ValidationGroupMember" name="PriceSupporter" type="text" value="5,00">
                    </div>
                    <label class="control-label LabelInline">€ &nbsp;&nbsp;(<span class="YearlyPrice">60</span>&nbsp;€ im Jahr)</label>
                </div>
            </div>
        </div>
    </div>
    <div style="clear: left;">
        <input type="submit" value="Speichern" class="btn btn-primary" name="btnSave" />
    </div>
<% } %>    

</asp:Content>
