<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<HelpModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="col-md-9" style="">
    <div class="box box-main">
        <h2>Hilfe</h2>
        <div class="box-content" style="min-height: 120px; clear: both; padding-top: 10px;">
            <h5>Themen</h5>
            <ul>
                <li><a href="#">Wissen</a></li>
                <li><a href="<%= Url.Action("Reputation", "Help") %>">Reputation</a></li>
                <li><a href="<%= Url.Action("DatenSicherheit", "Help") %>">Datensicherheit & Datenschutz</a></li>
            </ul>
            
            <h5>Kontakt</h5>
            <p>
                Hast Du mehr Fragen, dann Freuen wir uns über Deine Email an: <a href="mailto:team@richtig-oder-falsch.de">team@richtig-oder-falsch.de</a>
            </p>
            <p>
                Robert kannst Du auch per Skype ansprechen (Skype-name: "robert-mischke"). Oder per Handy unter 0178 18 668 48.
            </p>
            <p>
                Jeden Montag ab 15Uhr kannst Du uns auch im Büro besuchen: Falls Du Fragen hast, prüfen möchtest, 
                ob was wir dokumentieren auch tatsächlich umgesetzt wird oder bei Anliegen anderer Art:
            </p>
                
            Wir sind Untermieter bei der<br/>
             "Raecke Schreiber GbR"
            Erkelenzdamm 59/61<br/>
            D-10999 Berlin<br/>
            Aufgang: Portal 3A, 4.OG<br/>
            <br/>
            (Damit wirklich jemand da ist, bitte meldet ich bitte etwas vorher.)
            
            
        </div>
    </div>
</div>

    
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
