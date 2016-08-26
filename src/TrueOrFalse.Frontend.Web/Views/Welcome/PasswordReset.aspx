<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="ViewPage<PasswordResetModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% using (Html.BeginForm()) { %>
    
    <div class="row" style="padding-top:30px;">
        <div class="BackToHome col-md-3">
            <i class="fa fa-chevron-left"></i>&nbsp;<a href="/">zur Startseite</a>
        </div>
        <div class="form-horizontal col-md-9">
            <fieldset>
                <legend>Setze dein neues Passwort</legend>

                <% Html.Message(Model.Message); %>
                
                <% if(Model.TokenFound){ %> 
                
                    <%: Html.HiddenFor(x => x.Token) %>

                    <div class="form-group">
                        <%: Html.LabelFor(model => model.NewPassword1, new { @class = "col-sm-2 control-label" }) %>
                        <div class="col-sm-3">
                            <%: Html.PasswordFor(model => model.NewPassword1) %>
                            <%: Html.ValidationMessageFor(model => model.NewPassword1) %>
                        </div>
                    </div>
                
                    <div class="form-group">
                        <%: Html.LabelFor(model => model.NewPassword2, new { @class = "col-sm-2 control-label" }) %>
                        <div class="col-sm-3">
                            <%: Html.PasswordFor(model => model.NewPassword2) %>
                            <%: Html.ValidationMessageFor(model => model.NewPassword2) %>
                        </div>
                    </div>
                
                    <div class="form-group">
                        <div class="col-sm-offset-2 col-sm-10">
                            <input type="submit" value="Speichern" class="btn btn-primary" />&nbsp;
                    </div>
                </div>

                <%} %>

            </fieldset>            
        </div>
    </div>
<% } %>

</asp:Content>
