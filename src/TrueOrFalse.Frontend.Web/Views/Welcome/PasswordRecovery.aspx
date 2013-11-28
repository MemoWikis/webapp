<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="ViewPage<PasswordRecoveryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% using (Html.BeginForm()) { %>
    
    <div class="row" style="padding-top:30px;">
        <div class="col-md-2" style="padding-top:7px;">
            <i class="icon-chevron-left"></i>&nbsp;<a href="/">zur Startseite</a>
        </div>
        <div class="form-horizontal col-md-8">
            <fieldset>
                <legend>Ein neues Passwort setzen</legend>

                <% Html.ValidationSummary(true, "Bitte überprüfen Sie Ihre eingaben");  %>
                                
                <div class="alert alert-info">
                    Wir schicken einen Link an Deine Emailadresse. Folge dem Link und 
                    Du kannst Dir ein neues Passwort setzen. 
                </div>
                
                <% Html.Message(Model.Message); %>
                
                <div class="control-group">
                    <%: Html.LabelFor(model => model.Email, new { @class = "control-label" }) %>
                    <div class="controls">
                        <%: Html.EditorFor(model => model.Email) %>
                        <%: Html.ValidationMessageFor(model => model.Email) %>
                    </div>
                </div>
                
                <div class="form-actions">
                    <input type="submit" value="Link anfordern" class="btn btn-primary" />&nbsp;
                    <%: Html.ActionLink("Mein Password ist mir wieder eingefallen.", Links.Login, Links.VariousController, new { @style = "vertical-align:bottom; margin-left:20px;" })%>
                </div>

            </fieldset>            
        </div>
    </div>
<% } %>

</asp:Content>
