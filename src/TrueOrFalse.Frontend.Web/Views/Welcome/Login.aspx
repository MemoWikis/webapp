<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.User.Master" Inherits="ViewPage<LoginModel>" %>
<%@ Import Namespace="TrueOrFalse.Core.Web" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


<% using (Html.BeginForm()) { %>
<% Html.Message(Model.Message); %>


    <div class="row" style="padding-top:30px;">
        <div class="span2">&nbsp;</div>
        <div class="form-horizontal span8">
            <fieldset>
                <legend>Anmelden</legend>
                <div class="control-group">
                    <%: Html.LabelFor(model => model.EmailAddress)%>
                    <%: Html.EditorFor(model => model.EmailAddress)%> 
                    <%: Html.ValidationMessageFor(model => model.EmailAddress)%>            
                </div>

                <div class="control-group">
                    <%: Html.LabelFor(model => model.Password)  %>
                    <%: Html.Password("Password") %> &nbsp; (grOß kLEinScHReiBunG beachten!)
                 </div>

                 <div class="form-actions">
                     <input type="submit" value="Anmelden" class="btn btn-primary" /> 
                     <%: Html.ActionLink("Passwort vergessen?", Links.NotDoneYet, Links.VariousController, new {@style="vertical-align:bottom; margin-left:20px;"} )  %>                     
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

