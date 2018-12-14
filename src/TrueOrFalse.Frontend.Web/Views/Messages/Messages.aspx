<%@ Page Title="Deine Nachrichten" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="ViewPage<MessageModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <link rel="canonical" href="<%= Settings.CanonicalHost %><%= Links.Messages(Url) %>">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/message") %>
    <%= Scripts.Render("~/bundles/js/Messages") %>
    <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Nutzer", Url = Links.Users(), ToolTipText = "Nutzer"});
        Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Profilseite", Url = Url.Action(Links.UserAction, Links.UserController, new { name = User.Identity.Name, id = Model.UserId}), ToolTipText = "Profilseite"});
        Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Nachrichten", Url = "/Nachrichten", ToolTipText = "Nachrichten"});
        Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <input type="hidden" id="urlMessageSetRead" value="<%= Links.MessageSetRead(Url) %>"/>
    <input type="hidden" id="urlMessageSetUnread" value="<%= Links.MessageSetUnread(Url) %>"/>
    
    <div class="row">
        <div class="col-md-9">
        
            <h1 style="margin-top: 0; margin-bottom: 20px;">
                <span class="ColoredUnderline Message" style="padding-right: 3px;">Nachrichten</span>
            </h1>    
            
            <div id="messagesWrapper">
                <% Html.RenderPartial("~/Views/Messages/Partials/MessagesRows.ascx", Model.Messages); %>

                <% if (Model.ReadMessagesCount > 0) { %>
                    <p>
                        Du hast <%= Model.ReadMessagesCount %> gelesene Nachricht<%= StringUtils.PluralSuffix(Model.ReadMessagesCount,"en") %>.
                        <a href="#" class="" id="btnShowAllMessages">Alle anzeigen</a>.
                    </p>
                <% } %>
            </div>

        </div>
        <div class="col-md-3"></div>
    </div>
</asp:Content>