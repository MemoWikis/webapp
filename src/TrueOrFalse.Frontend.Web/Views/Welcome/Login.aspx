<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<LoginModel>" %>
<%@ Import Namespace="TrueOrFalse.Core.Web" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<h2>Login</h2>

<% using (Html.BeginForm()) { %>

	<br />

    <% Html.Message(Model.Message); %>

    <fieldset>

        <legend>Login</legend>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.EmailAddress)  %><br/>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.EmailAddress)%> <br/>
            <%: Html.ValidationMessageFor(model => model.EmailAddress)%>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Password)  %><br/>
        </div>
        <div class="editor-field">
            <%: Html.Password("Password") %> &nbsp; (grOß kLEinScHReiBunG beachten!)
        </div>

        <br/>
        <p>
            <input type="submit" value="Anmelden" />
        </p>

    </fieldset>
    
    <div class="span-8" >
        <p>
            Noch kein Benutzer?<br/>
            <%: Html.ActionLink("Jetzt registrieren!", Links.Register) %>

            <br/><br/>
            <%: Html.ActionLink("Passwort verloren?", Links.NotDoneYet, Links.VariousController)  %>
        </p>
    </div>

<% } %>

</asp:Content>

