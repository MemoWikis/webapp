<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<HelpModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="box box-main">
    <h2>Hilfe</h2>
    <div class="box-content" style="min-height: 120px; clear: both; padding-top: 10px;">
        <h5>Themen</h5>
        <ul>
            <li><a href="<%= Url.Action("Wissen", "Help") %>">Wissen</a></li>
            <li><a href="<%= Url.Action("Reputation", "Help") %>">Reputation</a></li>
            <li><a href="<%= Url.Action("DatenSicherheit", "Help") %>">Datensicherheit & Datenschutz</a></li>
        </ul>
            
        <h5>Kontakt</h5>
        <p>
            Hast du mehr Fragen, dann Freuen wir uns über deine Email an: <a href="mailto:team@richtig-oder-falsch.de">team@richtig-oder-falsch.de</a>
        </p>
        <p>
            Robert kannst du auch per Skype ansprechen (Skype-name: "robert-mischke"). Oder per Handy unter 0178 18 668 48.
        </p>
        <p>
            Jeden Montag ab 15 Uhr kannst du uns auch im Büro besuchen: Falls du Fragen hast, du prüfen möchtest, 
            ob das, was wir dokumentieren auch tatsächlich umgesetzt wird oder bei Anliegen anderer Art:
        </p>
                
        Wir sind Untermieter bei der<br/>
            "Raecke Schreiber GbR"
        Erkelenzdamm 59/61<br/>
        D-10999 Berlin<br/>
        Aufgang: Portal 3A, 4.OG<br/>
        <br/>
        (Bitte melde dich vorher an.)
    </div>
</div>

    
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
