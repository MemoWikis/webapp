<%@ Page Title="Register" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<RegisterModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Registrierung</h2>

<% using (Html.BeginForm()) { %>
    
	<br />
    <fieldset>
        
        <legend>Registriere Dich</legend>

       <% Html.ValidationSummary(true, "Bitte überprüfen Sie Ihre eingaben");  %>

       <p>Wir gehen sorgfältig mit Deinen Daten um.</p>
       

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Name)  %><br/>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Name) %> <br/>
            <%: Html.ValidationMessageFor(model => model.Name) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Email) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Email) %>
            <%: Html.ValidationMessageFor(model => model.Email) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Password) %>
        </div>
        <div class="editor-field">
            <%: Html.Password("Password") %>
            <%: Html.ValidationMessageFor(model => model.Password) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.TermsAndConditionsApproved ) %>
        </div>
        <div class="editor-field">
            <%: Html.CheckBoxFor(model => model.TermsAndConditionsApproved)%>
            <%: Html.ValidationMessageFor(model => model.TermsAndConditionsApproved) %>
        </div>

        <br/>
        <p>
            <input type="submit" value="Registrieren" />
        </p>
    </fieldset>
<% } %>

    <div class="span-8" >
        <p>
            Schon Benutzer?<br/>
            <%: Html.ActionLink("Jetzt anmelden!", Links.Login) %>

            <br/><br/>
            <%: Html.ActionLink("Passwort verloren?", Links.NotDoneYet, Links.VariousController)  %>
        </p>
    </div>


<div>
    
</div>

</asp:Content>

