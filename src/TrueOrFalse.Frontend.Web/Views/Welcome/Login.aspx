<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TrueOrFalse.Frontend.Web.Models.LoginModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="TrueOrFalse.View.Web.Views.Shared" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<h2>Login</h2>

<% using (Html.BeginForm()) { %>

	<br />

    <% if(Model.IsError){
        Html.RenderPartial(UserControls.ErrorMesage, new ErrorMessageModel(Model.ErrorMessage));
    } %>

    <fieldset>

        <legend>Login</legend>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.UserName)  %><br/>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.UserName) %> <br/>
            <%: Html.ValidationMessageFor(model => model.UserName) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Password)  %><br/>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Password) %> <br/>
            <%: Html.ValidationMessageFor(model => model.Password) %>
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

