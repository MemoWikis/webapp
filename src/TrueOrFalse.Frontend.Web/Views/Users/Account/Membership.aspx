<%@ Page Title="Mitgliedschaft" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="ViewPage<MembershipModel>" %> 
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Scripts.Render("~/Views/Users/Account/Js/Membership.js") %>
    <%= Styles.Render("~/Views/Users/Account/Membership.css") %>
    <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Werde Unterstützer!", Url = "/Nutzer/Mitgliedschaft", TextStyles = "color:#FFD603;", ToolTipText = "Werde Unterstützer!"});
       Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <% if (Model.IsMember) {%>
        <h1 class="PageHeader">Danke, dass du uns unterstützt <i class="fa fa-smile-o"></i></h1>
            
        <% Html.Message(Model.Message); %>
        <div class="Box">
            <h3 style="margin-top: 0; margin-bottom: 20px;">Deine Mitgliedschaft:</h3>
    
            <div>
                <p><b>Monatlicher Preis:</b> <%= String.Format("{0:C}", Model.Membership.PricePerMonth) %></p>

                <p><b>Preiskategorie:</b> <%= String.Format("{0:d}", Model.Membership.PriceCategory.ToString()) %></p>

                <p><b>Gültig bis einschließlich:</b> <%= String.Format("{0:d}", Model.Membership.PeriodEnd) %></p>
            </div>
        </div>   
    <% } else { %>
        <h1 class="PageHeader">Fördermitglied werden</h1>
    
            
        <p>
            <strong>Du möchtest Fördermitglied werden?</strong>
            Du findest, dass memucho eine tolle Sache ist und möchtest uns unterstützen? 
            Dann werde Fördermitglied der ersten Stunde!
        </p>
        <p>
            Wir sind noch nicht ganz fertig, memucho befindet sich in der <a href="<%= Links.BetaInfo() %>">Beta-Phase</a>. 
            Einige Funktionen fehlen noch, andere Dinge müssen wir noch verbessern und 
            benutzerfreundlicher machen. 
            Außerdem haben wir noch sooo viele Ideen, die auf Verwirklichung warten. Wir arbeiten daran! 
            Aber gerade in der schwierigen Startphase brauchen wir dich und deine Unterstützung. 
            Wenn du an uns glaubst, dann werde jetzt Fördermitglied!
        </p>
    
    <% } %>
    
    <% if (Model.IsMember) {%>
        <h2>Welche Vorteile hast du als Mitglied? </h2>
    <% } else { %>
        <h2>Was bekommst du als Mitglied? </h2>
    <% } %>
            

    <ul>
        <li>Die wertvolle Trophäe "Fördermitglied der 1. Stunde" in Gold in deinem Profil auf Lebenszeit.</li>
        <li>Die memucho-Seite komplett werbefrei.</li>
        <li>Alle memucho-Funktionen auch zukünftig ohne Beschränkungen, insbesondere...
            <ul style="list-style: none;">
                <li>...unbegrenzt private Fragen</li>
                <li>...unbegrenzt Wunschwissen</li>
                <li>...unbegrenzte Nutzung von Lernterminen</li>     
                <li>...unbegrenzte Nutzung von Widgets (sonst max. 5)</li>
            </ul>
        </li>
        <li>Das gute Gefühl, eine tolle Idee zu unterstützen.</li>

    </ul>
    <% if (!Model.IsMember) {%>
        <p>
            In der Beta-Phase sind die Funktionalitäten für Nichtmitglieder teilweise noch nicht beschränkt. 
            Bis zum offiziellen Start unserer Plattform 
            winken für dich als Fördermitglied also neben der werbefreien Nutzung 
            "nur" Ruhm, Ehre und die besondere Trophäe 
            &#8211 und das gute Gefühl, eine tolle Sache zu unterstützen.
        </p>

        <h2>Dein Beitrag</h2>
        <p>
            Dein Beitrag richtet sich nach deinen finanziellen Möglichkeiten und deiner Motivation. Du kannst ihn selbst wählen.
        </p>
            

        <% using (Html.BeginForm("Membership", "Account", null, FormMethod.Post, new { id="BecomeMemberForm", enctype = "multipart/form-data" })){ %>
    
            <input type="hidden" id="hddSelectedPrice" name="SelectedPrice" value="0"/>

            <div id="PriceSelection" class="form-group">
                <input id="ChosenPrice" type="hidden"/>
                <div class="radio">
                    <label>
                        <input id="rdoPriceReduced" type="radio" name="PriceLevel" value="Reduced"/>
                        <span class="Title">Ermäßigt: </span>
                        <span class="MinPrice">1,00</span>&nbsp;€ bis 3,99&nbsp;€ im Monat, empfohlen: <span class="bold">3,00&nbsp;€ </span>&nbsp;(36,00&nbsp;€ im Jahr)
                    </label>
                    <a href="#PriceReducedInfo" class="MoreLink" data-toggle="collapse" aria-expanded="false" aria-controls="PriceReducedInfo"><i class="fa fa-chevron-down"></i></a>
                    <div id="PriceReducedInfo" class="PriceLevelInfo collapse">
                        <div class="Description">
                            Du bist zum Beispiel Schüler, Student, Geringverdiener oder Lebenskünstler 
                            mit geringem Budget und kannst auch deine Eltern nicht um Unterstützung bitten. 
                            Den Wert eines Kaffees im Monat oder von zwei Taschenbüchern im Jahr 
                            kannst du aber für memucho aufbringen.
                        </div>
                        <div class="ControlGroupInline JS-ValidationGroup" style="clear: left;">
                            <label class="control-label LabelInline">Dein Beitrag: </label>
                            <div class="ControlInline">
                                <input id="PriceReduced" name="PriceReduced" class="InputPrice NotInFocus form-control JS-ValidationGroupMember" type="text" value="3,00">
                            </div>
                            <label class="control-label LabelInline">€ &nbsp;&nbsp;(<span class="YearlyPrice">36,00</span>&nbsp;€ im Jahr)</label>
                        </div>
                    </div>
                </div>
                <div class="radio">
                    <label>
                        <input id="rdoPriceNormal" type="radio" name="PriceLevel" value="Normal" checked>
                        <span class="Title">Normal: </span>
                        <span class="MinPrice">4,00</span>&nbsp;€ bis 6,99&nbsp;€ im Monat, empfohlen: <span class="bold">5,00 € </span>&nbsp;(60,00&nbsp;€ im Jahr)
                    </label>
                    <a href="#PriceNormalInfo" class="MoreLink" data-toggle="collapse" aria-expanded="true" aria-controls="PriceNormalInfo"><i class="fa"></i></a>

                    <div id="PriceNormalInfo" class="PriceLevelInfo collapse in">
                        <div class="Description">
                            Du oder deine Eltern sind Normalverdiener.
                            5,00&nbsp;€ im Monat für Bildung gehen voll in Ordnung.
                        </div>
                        <div class="ControlGroupInline JS-ValidationGroup" style="clear: left;">
                            <label class="control-label LabelInline">Dein Beitrag: </label>
                            <div class="ControlInline">
                                <input id="PriceNormal" name="PriceNormal" class="InputPrice form-control JS-ValidationGroupMember" type="text" value="5,00">
                            </div>
                            <label class="control-label LabelInline">€ &nbsp;&nbsp;(<span class="YearlyPrice">60,00</span>&nbsp;€ im Jahr)</label>
                        </div>
                    </div>
                </div>
                <div class="radio">
                    <label>
                        <input id="rdoPriceSupporter" type="radio" name="PriceLevel" value="Supporter"/>
                        <span class="Title">Solibeitrag: </span>
                        <span class="MinPrice">7,00</span>&nbsp;€ oder mehr im Monat, empfohlen: <span class="bold">10,00&nbsp;€ </span>&nbsp;(120,00&nbsp;€ im Jahr)
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
                                <input id="PriceSupporter" class="InputPrice NotInFocus form-control JS-ValidationGroupMember" name="PriceSupporter" type="text" value="10,00">
                            </div>
                            <label class="control-label LabelInline">€ &nbsp;&nbsp;(<span class="YearlyPrice">120</span>&nbsp;€ im Jahr)</label>
                        </div>
                    </div>
                </div>
            </div>
    
            <% if(!Model.IsLoggedIn){ %>
                <div class="bs-callout bs-callout-danger" style="margin-top: 0;">
                    <h3>Einloggen oder registrieren</h3>
                    <p>
                        Um Mitglied zu werden 
                        musst du dich <a href="#" data-btn-login="true">einloggen</a> oder <a href="/Registrieren">registrieren</a>.
                    </p>
                </div>
            <% }%>
    
            <% if(Model.IsLoggedIn){ %>
    
                <h2>Deine Rechnungsdaten</h2>
    
                <p>
                    Du erhältst von uns eine Rechnung an die von dir angegebene E-Mail-Adresse: <b><%= Model.BillingEmail%></b>.
                    Bitte überweise uns den Betrag auf das in der Rechnung angegebene Konto. Andere Zahlungsoptionen bietet wir leider noch
                    nicht an.
                </p>
    
    
                <div id="invoiceForm">
                    <div class="form-group">
                        <label class="control-label RequiredField">Name Rechnungsempfänger</label>
                        <input class="form-control" name="BillingName" value="<%= Model.BillingName %>"/>
                    </div>

                    <div class="form-group">
                        <label class="control-label">Rechnungsadresse (optional)</label>
                        <textarea class="form-control" name="BillingAddress"></textarea>
                    </div>
                
                    <div>
                        <label class="radio-inline">
                            <input type="radio" name="PaymentPeriod" value="halfYear">
                            Halbjährlich zahlen
                        </label>
                        <label class="radio-inline">
                            <input type="radio" name="PaymentPeriod" value="fullYear" checked>
                            Jährlich zahlen
                        </label>
                    </div>

                    <div class="checkbox">
                        <label>
                            <%= Html.CheckBoxFor(m => Model.AutoRenewal) %>Automatisch verlängern 
                        </label>
                    </div>
                    <div class="form-group">
                        Du kannst jederzeit kündigen! Wir erstatten den Restbetrag für verbleibende volle Monate zurück.
                    </div>

                </div>
    
                <div style="text-align: center; margin-top: 30px;">
                    <input type="submit" value="Verbindlich Mitglied werden" class="btn btn-lg btn-success" name="btnSave" />
                </div>
    
            <% } %>
        <% } %>
    <% } %>           
            
    <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>

</asp:Content>
