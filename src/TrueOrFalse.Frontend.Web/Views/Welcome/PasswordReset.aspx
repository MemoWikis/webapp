<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="ViewPage<PasswordResetModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% using (Html.BeginForm()) { %>
    
    <div class="row" style="padding-top:30px;">
        <div class="col-md-2" style="padding-top:7px;">
            <i class="icon-chevron-left"></i>&nbsp;<a href="/">zur Startseite</a>
        </div>
        <div class="form-horizontal col-md-8">
            <fieldset>
                <legend>Setze Dein neues Passwort</legend>

                <% Html.ValidationSummary(true, "Bitte überprüfe Deine Eingaben");  %>
                                                
                <% Html.Message(Model.Message); %>
                
                <% if(Model.TokenFound){ %> 
                
                    <%: Html.HiddenFor(x => x.Token) %>

                    <div class="control-group">
                        <%: Html.LabelFor(model => model.NewPassword1, new { @class = "control-label" }) %>
                        <div class="controls">
                            <%: Html.PasswordFor(model => model.NewPassword1) %>
                            <%: Html.ValidationMessageFor(model => model.NewPassword1) %>
                        </div>
                    </div>
                
                    <div class="control-group">
                        <%: Html.LabelFor(model => model.NewPassword2, new { @class = "control-label" }) %>
                        <div class="controls">
                            <%: Html.PasswordFor(model => model.NewPassword2) %>
                            <%: Html.ValidationMessageFor(model => model.NewPassword2) %>
                        </div>
                    </div>
                
                    <div class="form-actions">
                        <input type="submit" value="Speichern" class="btn btn-primary" />&nbsp;
                    </div>

                <%} %>

            </fieldset>            
        </div>
    </div>
<% } %>

</asp:Content>
