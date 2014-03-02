<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<NewsModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/news") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="col-md-6">
        
        <div class="row">
            <h2 style="margin-top: 0px;">Nachrichten</h2>    
        </div>
        

<%--        <ul>
            <li>Willkommen auf Richtig-oder-Falsch</li>
            <li>Fragesatz: "..." wurde aktualisiert (Du folgst dem Fragesatz)</li>
            <li>Frage: "..." wurde aktualisiert (Die Frage ist in Deinem <a href="<%= Url.Action(Links.HelpWunschwissen, Links.HelpController) %>">Wunschwissen)</a></li>
            <li>Person hat neue Frage hinzugefügt (Du folgst der Person)</li>
            <li>Der Kategorie "" wurde neue Frage hinzugefügt (Du folgst der Kategorie)</li>
            <li>Eine Frage die Du erstellt hast wurde geflaggt</li>
            <li>Eine Frage die Du erstellt hast wurde kommentiert</li>
            <li>Eine Frage die Du erstellt wurde duppliziert</li>
        </ul>--%>

        <% foreach(var msg in Model.Rows){ %>
    
            <div class="row msgRow rowBase ">
                <div class="header col-lg-12">
                    <h4><%: msg.Subject %></h4>
                </div>

                <div class="col-lg-12 body">
                    <%= msg.Body %>
                </div>

                <div class="col-lg-7 footer">
                    vor 12 min
                </div>
                <div class="col-lg-5  footer">
                    <a href="#" class="pull-right">als gelesen makieren</a>
                </div>
            </div>

        <% } %>        

    </div>
    


</asp:Content>