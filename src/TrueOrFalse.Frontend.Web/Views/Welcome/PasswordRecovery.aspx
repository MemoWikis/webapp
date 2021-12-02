<%@ Page Title="Passwort zurücksetzen" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="ViewPage<PasswordRecoveryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <%= Scripts.Render("~/Views/Welcome/Js/ValidationPasswordRecovery.js") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% using (Html.BeginForm("PasswordRecovery", "Welcome", null, FormMethod.Post, new { id = "PasswordRecoveryForm", enctype = "multipart/form-data" })) { %>
    
    <div class="row" style="padding-top:30px;">
        <div class="BackToHome col-md-3">
            <a href="/"><i class="fa fa-chevron-left">&nbsp;</i>Zur Startseite</a>
        </div>
        <div class="form-horizontal col-md-9">
            <fieldset>
                <legend>Ein neues Passwort setzen</legend>

                <div class="alert alert-info">
                    Gib hier die E-Mail-Adresse an, mit der du dich registriert hast. 
                    Wir schicken dir einen Link, mit dem du dir ein neues Passwort setzen kannst.
                </div>
                
                <% Html.Message(Model.Message); %>
                
                <div class="form-group">
                    <%: Html.LabelFor(model => model.Email, new { @class = "col-sm-2 control-label" }) %>
                    <div class="col-sm-6">
                        <%: Html.TextBoxFor(model => model.Email, new { @class="form-control" }) %>
                        <%: Html.ValidationMessageFor(model => model.Email) %>
                    </div>
                </div>
                
                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <input type="submit" value="Link anfordern" class="btn btn-primary" />&nbsp;
                        <a href="#" data-btn-login="true" onclick="eventBus.$emit('show-login-modal')" class="btn btn-link">Mein Passwort ist mir wieder eingefallen.</a>
                    </div>
                </div>

            </fieldset>            
        </div>
    </div>
<% } %>

</asp:Content>
