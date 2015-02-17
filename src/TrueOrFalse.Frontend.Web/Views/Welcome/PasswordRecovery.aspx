<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="ViewPage<PasswordRecoveryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% using (Html.BeginForm()) { %>
    
    <div class="row" style="padding-top:30px;">
        <div class="BackToHome col-md-3">
            <i class="fa fa-chevron-left"></i>&nbsp;<a href="/">zur Startseite</a>
        </div>
        <div class="form-horizontal col-md-9">
            <fieldset>
                <legend>Ein neues Passwort setzen</legend>

                <% Html.ValidationSummary(true, "Bitte überprüfen Sie Ihre eingaben");  %>
                                
                <div class="alert alert-info">
                    Bitte gib deine Emailadresse ein. Wir schicken einen Link an deine Emailadresse. Folge dem Link und 
                    du kannst dir ein neues Passwort setzen. 
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
                        <%: Html.ActionLink("Mein Password ist mir wieder eingefallen.", Links.Login, Links.VariousController, new { @style = "vertical-align:bottom; margin-left:20px;" })%>
                    </div>
                </div>

            </fieldset>            
        </div>
    </div>
<% } %>

</asp:Content>
