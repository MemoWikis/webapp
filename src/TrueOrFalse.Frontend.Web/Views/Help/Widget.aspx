<%@ Page Title="Widget-Integration von memucho" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Help/Widget.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="row">
        <div class="col-xs-12">

            <div class="well">

                <h1 class="PageHeader" style="margin-bottom: 15px; margin-top: 0;"><span class="ColoredUnderline GeneralMemucho">memucho als Widget in der eigenen Webseite</span></h1>
                <p class="teaserText">
                    Die Lerntechnologie und die Inhalte von memucho können als Widget leicht in bestehende Webseiten integriert werden. 
                    Egal ob auf dem privaten Blog, der Vereins- oder Unternehmensseite, oder dem Schul- oder Lernmanagementsystem: Nötig ist eine Textzeile HTML, 
                    die du einfach per Copy'n'Paste überträgst. Hier erfährst du Schritt für Schritt, wie das geht und was du dabei einstellen kannst.
                </p>
                    
            </div>
        </div>
    </div>




    <div class="row">
        <div class="col-xs-12">
            <div class="well">
                <h2 style="margin-bottom: 15px;" class="PageHeader">
                    <span class="ColoredUnderline GeneralMemucho">Einzelne Fragen per Widget einbinden</span>
                </h2>
                <p>
                    [beim Blogbeitrag Text auflockern, Nutzer zu Interaktion animieren...]
                </p>
                <p>
                    [Bild: So sieht's aus]
                </p>
                <p>
                    Du willst genauer wissen, wie ...
                    <button class="btn btn-secondary" data-toggle="collapse" data-target="#WidgetQuestionDetails">Schritt-für-Schritt-Anleitung anzeigen</button>
                </p>

                <div id="WidgetQuestionDetails" class="collapse">
                    <h4 style="margin-top: 25px;">
                        Schritt-für-Schritt-Anleitung [...]
                    </h4>
                    <ol>
                        <li>[bla]</li>
                        <li>[bla]</li>
                    </ol>
                </div>
            </div>
        </div>
    </div>
    

    <div class="row">
        <div class="col-xs-12">
            <div class="well">

                <h2 style="margin-bottom: 15px;" class="PageHeader">
                    <span class="ColoredUnderline GeneralMemucho">Fragesatz als Quiz mit Auswertung</span>
                </h2>
                <p class="subheader">
                    Nutze einen Fragesatz, erstelle selber einen...
                </p>
                <p>
                    [Bild: So sieht's aus]
                </p>
                
                <p>
                    Du willst genauer wissen, wie du das Fragesatz-Widget einbettest?
                    <button class="btn btn-secondary" data-toggle="collapse" data-target="#WidgetSetDetails">Schritt-für-Schritt-Anleitung anzeigen</button>
                </p>

                <div id="WidgetSetDetails" class="collapse">
                    <h4 style="margin-top: 25px;">
                        Schritt-für-Schritt-Anleitung zur Einbettung von Fragesätzen
                    </h4>
                    <p>
                        [bla...]
                    </p>

                </div>
            </div>
        </div>
    </div>
    

    <div class="row">
        <div class="col-xs-12">
            <div class="well">

                <h2 style="margin-bottom: 15px;" class="PageHeader">
                    <span class="ColoredUnderline GeneralMemucho">Video-Fragesatz mit Video und Fragen [...]</span>
                </h2>
                <p class="subheader">
                    [...]
                </p>
                <p>
                    [Bild: So sieht's aus]
                </p>
                
                <p>
                    Du willst genauer wissen, wie du einen Fragesatz mit Video einbettest?
                    <button class="btn btn-secondary" data-toggle="collapse" data-target="#WidgetSetVideoDetails">Schritt-für-Schritt-Anleitung anzeigen</button>
                </p>

                <div id="WidgetSetVideoDetails" class="collapse">
                    <h4 style="margin-top: 25px;">
                        Schritt-für-Schritt-Anleitung zur Einbettung von Fragesätzen
                    </h4>
                    <p>
                        [bla...]
                    </p>

                </div>
            </div>
        </div>
    </div>


</asp:Content>