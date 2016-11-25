<%@ Page Title="Einloggen" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="ViewPage<LoginModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <link rel="canonical" href="<%= Settings.CanonicalHost %><%= Url.Action("Login", "Welcome") %>">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<% using (Html.BeginForm()) { %>

    <div class="row" style="padding-top:30px;">
        <div class="BackToHome col-md-3">
            <a href="/"><i class="fa fa-chevron-left">&nbsp;</i>Zur Startseite</a>
        </div>
        <div class="form-horizontal col-md-9" role="form">
            
            <fieldset>
                <legend>Einloggen</legend>
            
                <% Html.Message(Model.Message); %>

                <div class="form-group">
                    <%: Html.LabelFor(model => model.EmailAddress, new { @class = "col-sm-2 control-label" })%>
                    <div class="col-sm-6">
                        <%= Html.TextBoxFor(m => m.EmailAddress, new { @class="form-control" })%> 
                    </div>
                    <%: Html.ValidationMessageFor(m => m.EmailAddress)%>            
                </div>

                <div class="form-group">
                    <label class = "col-sm-2 control-label" for="Password">Passwort</label>
                    <div class="col-sm-6">
                        <%: Html.PasswordFor(m => m.Password, new { @class="form-control" }) %>
                    </div>
                    <div class="col-sm-4" style="padding-top: 8px;">
                        (grOß kLEinScHReiBunG beachten!)
                    </div>
                 </div>
                 
                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <label class="checkbox" style="white-space:nowrap;">
                            <%: Html.CheckBoxFor(model => model.PersistentLogin) %> Eingeloggt bleiben
                        </label>
                    </div>
                 </div>
                
                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <input type="submit" value="Einloggen" class="btn btn-primary" /> 
                        <a href="<%= Url.Action("PasswordRecovery", "Welcome") %>" class="btn btn-link">Passwort vergessen?</a>
                    </div>
                </div>
                
                <div class="form-group"> 
                    <div class="col-sm-offset-2 col-sm-10">
                        Noch kein Benutzer?&nbsp;
                        <%: Html.ActionLink("Jetzt registrieren!", Links.RegisterAction) %><br/><br />
                    </div>
                </div>

             </fieldset>
        </div>        
    </div>


<% } %>

</asp:Content>

