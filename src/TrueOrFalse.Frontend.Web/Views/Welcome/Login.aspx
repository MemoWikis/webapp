<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="ViewPage<LoginModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<% using (Html.BeginForm()) { %>

    <div class="row" style="padding-top:30px;">
        <div class="BackToHome col-md-3">
            <i class="fa fa-chevron-left"></i>&nbsp;<a href="/">zur Startseite</a>
        </div>
        <div class="form-horizontal col-md-9" role="form">
            
            <fieldset>
                <legend>Anmelden</legend>
            
                <% Html.Message(Model.Message); %>
                
                <div class="form-group">    
                    <div class="col-sm-offset-2 col-sm-10">
                        <a class="zocial icon facebook"></a>
                        <a class="zocial icon google"></a>
                        <a class="zocial icon twitter"></a>
                        <p style="margin-top: 10px; margin-bottom: 0;">oder mit</p>
                    </div>
                </div>

                <div class="form-group">
                    <%: Html.LabelFor(model => model.EmailAddress, new { @class = "col-sm-2 control-label" })%>
                    <div class="col-sm-3">
                        <%= Html.TextBoxFor(m => m.EmailAddress, new { @class="form-control" })%> 
                    </div>
                    <%: Html.ValidationMessageFor(m => m.EmailAddress)%>            
                </div>

                <div class="form-group">
                    <label class = "col-sm-2 control-label" for="Password">Passwort</label>
                    <div class="col-sm-3">
                        <%: Html.PasswordFor(m => m.Password, new { @class="form-control" }) %>
                    </div>
                    <div class="col-sm-4" style="padding-top: 8px;">
                        (grOß kLEinScHReiBunG beachten!)
                    </div>
                 </div>
                 
                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <label class="checkbox" style="white-space:nowrap;">
                            <%: Html.CheckBoxFor(model => model.PersistentLogin) %> Angemeldet bleiben
                        </label>
                    </div>
                 </div>
                
                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <input type="submit" value="Anmelden" class="btn btn-primary" /> 
                        <%: Html.ActionLink("Passwort vergessen?", "PasswordRecovery", Links.VariousController, new {@style="vertical-align:bottom; margin-left:20px;"} )  %>
                    </div>
                </div>
                
                <div class="form-group"> 
                    <div class="col-sm-offset-2 col-sm-10">
                        Noch kein Benutzer?&nbsp;
                        <%: Html.ActionLink("Jetzt registrieren!", Links.Register) %><br/><br />
                    </div>
                </div>

             </fieldset>
        </div>        
    </div>


<% } %>

</asp:Content>

