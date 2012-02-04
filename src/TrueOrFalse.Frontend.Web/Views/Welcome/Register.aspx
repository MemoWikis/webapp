<%@ Page Title="Register" Language="C#" MasterPageFile="~/Views/Shared/Site.User.Master" Inherits="ViewPage<RegisterModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% using (Html.BeginForm()) { %>
    
    <div class="row" style="padding-top:30px;">
        <div class="span2" style="padding-top:7px;">
            <i class="icon-chevron-left"></i>&nbsp;<a href="/">zur Startseite</a>
        </div>
        <div class="form-horizontal span8">
            <fieldset>
                <legend>Registriere Dich</legend>

                <% Html.ValidationSummary(true, "Bitte überprüfen Sie Ihre eingaben");  %>
                
                <div class="alert alert-info">
                    Wir gehen sorgfältig mit Deinen Daten um.
                </div>
       
                <div class="control-group">
                    <%: Html.LabelFor(model => model.Name)  %>
                    <%: Html.EditorFor(model => model.Name) %>
                    <%: Html.ValidationMessageFor(model => model.Name) %>
                </div>
                
                <div class="control-group">
                    <%: Html.LabelFor(model => model.Email) %>
                    <%: Html.EditorFor(model => model.Email) %>
                    <%: Html.ValidationMessageFor(model => model.Email) %>
                </div>

                <div class="control-group">
                    <%: Html.LabelFor(model => model.Password) %>
                    <%: Html.Password("Password") %>
                    <%: Html.ValidationMessageFor(model => model.Password) %>
                </div>

                <div class="control-group">
                    <%: Html.LabelFor(model => model.TermsAndConditionsApproved ) %>
                    <%: Html.CheckBoxFor(model => model.TermsAndConditionsApproved)%>
                    <%: Html.ValidationMessageFor(model => model.TermsAndConditionsApproved) %>
                </div>
                
                <div class="form-actions">
                    <input type="submit" value="Registrieren" class="btn btn-primary" />&nbsp;
                    <%: Html.ActionLink("Anmeldung für Benutzer!", Links.Login, Links.VariousController, new { @style = "vertical-align:bottom; margin-left:20px;" })%>
                </div>

            </fieldset>            
        </div>
    </div>
<% } %>

    <div class="span-8" >
        <p>
            
        </p>
    </div>


<div>
    
</div>

</asp:Content>

