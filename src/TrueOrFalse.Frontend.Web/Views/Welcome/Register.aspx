<%@ Page Title="Register" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="ViewPage<RegisterModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% using (Html.BeginForm())
   {%>
    
    <div class="row" style="padding-top:30px;">
        <div class="col-md-2" style="padding-top:7px;">
            <i class="icon-chevron-left"></i>&nbsp;<a href="/">zur Startseite</a>
        </div>
        <div class="form-horizontal col-md-8">
            <fieldset>
                <legend>Registriere Dich</legend>

                <% Html.ValidationSummary(true, "Bitte überprüfen Sie Ihre eingaben"); %>
                
                <div class="alert alert-info">
                    Wir gehen sorgfältig mit Deinen Daten um.
                </div>
       
                <div class="control-group">
                    <%: Html.LabelFor(model => model.Name, new { @class = "control-label" }) %>
                    <div class="controls">
                        <%: Html.EditorFor(model => model.Name) %>
                        <%: Html.ValidationMessageFor(model => model.Name) %>
                    </div>
                </div>
                
                <div class="control-group">
                    <%: Html.LabelFor(model => model.Email, new { @class = "control-label" }) %>
                    <div class="controls">
                        <%: Html.EditorFor(model => model.Email) %>
                        <%: Html.ValidationMessageFor(model => model.Email) %>
                    </div>
                </div>

                <div class="control-group">
                    <%: Html.LabelFor(model => model.Password, new { @class = "control-label" }) %>
                    <div class="controls">
                        <%: Html.Password("Password") %>
                        <%: Html.ValidationMessageFor(model => model.Password) %>
                    </div>
                </div>

                <div class="control-group">
                    <%: Html.LabelFor(model => model.TermsAndConditionsApproved, new { @class = "control-label" }) %>
                    <div class="controls">
                        <%: Html.CheckBoxFor(model => model.TermsAndConditionsApproved) %>
                        <%: Html.ValidationMessageFor(model => model.TermsAndConditionsApproved) %>
                    </div>
                </div>
                
                <div class="form-actions">
                    <input type="submit" value="Registrieren" class="btn btn-primary" />&nbsp;
                    <%: Html.ActionLink("Ich bin schon Benutzer!", Links.Login, Links.VariousController,
                                           new {@style = "vertical-align:bottom; margin-left:20px;"}) %>
                </div>

            </fieldset>            
        </div>
    </div>
<% } %>

</asp:Content>

