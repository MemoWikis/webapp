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
            <i class="fa fa-chevron-left"></i>&nbsp;<a href="/">zur Startseite</a>
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
                    <div class="col-sm-3">
                        <%: Html.TextBoxFor(model => model.Email, new { @class="form-control" }) %>
                        <%: Html.ValidationMessageFor(model => model.Email) %>
                    </div>
                </div>
                
                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <input type="submit" value="Link anfordern" class="btn btn-primary" />&nbsp;
                        <%: Html.ActionLink("Mein Passwort ist mir wieder eingefallen.", Links.Login, Links.VariousController, new { @style = "vertical-align:bottom; margin-left:20px;" })%>
                    </div>
                </div>

            </fieldset>            
        </div>
    </div>
<% } %>

</asp:Content>
