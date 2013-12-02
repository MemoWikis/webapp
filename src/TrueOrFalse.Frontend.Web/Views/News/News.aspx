<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<NewsModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="col-md-9">
        <h2 style="margin-top: 0px;">Neues</h2>
        
        
        <ul>
            <li>Willkommen auf Richtig-oder-Falsch</li>
            <li>Fragesatz: "..." wurde aktualisiert (Du folgst dem Fragesatz)</li>
            <li>Frage: "..." wurde aktualisiert (Die Frage ist in Deinem <a href="<%= Url.Action(Links.HelpWunschwissen, Links.HelpController) %>">Wunschwissen)</a></li>
            <li>Person hat neue Frage hinzugefügt (Du folgst der Person)</li>
            <li>Der Kategorie "" wurde neue Frage hinzugefügt (Du folgst der Kategorie)</li>
            <li>Eine Frage die Du erstellt hast wurde geflaggt</li>
            <li>Eine Frage die Du erstellt hast wurde kommentiert</li>
            <li>Eine Frage die Du erstellt wurde duppliziert</li>
        </ul>

    </div>
</asp:Content>