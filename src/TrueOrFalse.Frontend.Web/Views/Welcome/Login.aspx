<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="ViewPage<LoginModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<% using (Html.BeginForm()) { %>

    <div class="row" style="padding-top:30px;">
        <div class="col-md-2" style="padding-top:7px;">
            <i class="icon-chevron-left"></i>&nbsp;<a href="/">zur Startseite</a>
        </div>
        <div class="form-horizontal col-md-8">
            <fieldset>
                <legend>Anmelden</legend>
                
                <% Html.Message(Model.Message); %>

                <div class="control-group">
                    <%: Html.LabelFor(model => model.EmailAddress, new { @class = "control-label" })%>
                    <div class="controls">
                        <%: Html.EditorFor(model => model.EmailAddress)%> 
                    </div>
                    <%: Html.ValidationMessageFor(model => model.EmailAddress)%>            
                </div>

                <div class="control-group">
                    <%: Html.LabelFor(model => model.Password, new { @class = "control-label" })%>
                    <div class="controls">
                        <%: Html.Password("Password") %> &nbsp; (grOß kLEinScHReiBunG beachten!)
                    </div>
                 </div>
                 
                <div class="control-group">
                    
                    <div class="controls">
                        <label class="checkbox" style="white-space:nowrap;">
                            <%: Html.CheckBoxFor(model => model.PersistentLogin) %> Angemeldet bleiben
                        </label>
                    </div>
                 </div>

                 <div class="form-actions">
                     <input type="submit" value="Anmelden" class="btn btn-primary" /> 
                     <%: Html.ActionLink("Passwort vergessen?", "PasswordRecovery", Links.VariousController, new {@style="vertical-align:bottom; margin-left:20px;"} )  %>
                 </div>
                 
                 <div class="form-actions" style="border-top:0px; background-color:white;">
                    Noch kein Benutzer?&nbsp;
                    <%: Html.ActionLink("Jetzt registrieren!", Links.Register) %><br/><br />
                 </div>

             </fieldset>
        </div>        
    </div>


<% } %>

</asp:Content>

