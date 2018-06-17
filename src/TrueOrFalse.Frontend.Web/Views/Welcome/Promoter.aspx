<%@ Page Title="Förderer" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master"
    Inherits="ViewPage<BaseModel>" %>



<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <meta name="description" content="Förderer für Lerntool und Wissensassistenten memucho">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Welcome/Promoter.css" rel="stylesheet" />

</asp:Content>


<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row" id="PromoterH2">
        <div>
            <h2 ><p>Förderer</p></h2>
        </div>
    </div>

    <div class="promoter-overview">
        <div class="row">
            <div class=" col-xs-5 col-xs-offset-5">
                <img id="efri" class="partnerImage" src="/Images/LogosPartners/efre.png" />
            </div>
        </div>

        <div class="row">
            <div class=" col-xs-8 col-xs-offset-2">
                <p>
                    memucho wird seit Juni 2018 im Rahmen des Programms „Gründung innovativ“ des Europäischen Fonds für regionale Entwicklung (EFRE) von der Investitionsbank
              des Landes Brandenburg gefördert. Der EFRE wird zur Schaffung nachhaltiger und selbsttragender Wirtschaftsstrukturen genutzt. www.efre.brandenburg.de
                </p>
            </div>
        </div>

        <div class="row">
            <div class=" col-xs-5 col-xs-offset-5">
                <img id="exist" class="partnerImage" src="/Images/LogosPartners/Logo-EXIST-eps.png" />
            </div>
        </div>
        <div class="row">
            <div class=" col-xs-8 col-xs-offset-2">
                <p>
                    memucho wurde von Oktober 2016 bis September 2017 mit einem EXIST-Gründerstipendium gefördert. 
              EXIST ist ein Förderprogramm des Bundesministeriums für Wirtschaft und Energie (BMWi). www.exist.de
                </p>
            </div>
        </div>
        <div class="row">
            <div class=" col-xs-5 col-xs-offset-5">
                <img id="profund" class="partnerImage" src="/Images/LogosPartners/profund-innovation-logo-t.png" />
            </div>
        </div>
        <div class="row">
            <div class=" col-xs-8 col-xs-offset-2 footer">
                <p>
                    Profund Innovation ist die Service-Einrichtung für die Förderung von Unternehmensgründungen
                und Innovationen in der Abteilung Forschung der Freien Universität. www.fu-berlin.de/sites/profund
                </p>
            </div>
        </div>
    </div>
</asp:Content>
