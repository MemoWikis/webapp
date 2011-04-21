<%@ Page Title="Register" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TrueOrFalse.Frontend.Web.Models.RegisterModel>" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Registrierung</h2>

<script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
<script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>

<% using (Html.BeginForm()) { %>
    <%: Html.ValidationSummary(true) %>

	<br />
    <fieldset>
        <legend>Registrieren Sie sich</legend>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.UserName)  %>
            
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.UserName) %> 
            <%: Html.ValidationMessageFor(model => model.UserName) %>
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
            <%: Html.EditorFor(model => model.Password) %>
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

<div>
    
</div>

</asp:Content>

