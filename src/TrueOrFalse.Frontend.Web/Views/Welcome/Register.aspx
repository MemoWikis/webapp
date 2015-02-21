<%@ Page Title="Registrieren" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="ViewPage<RegisterModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% using (Html.BeginForm())
   {%>
    
    <div class="row" style="padding-top:30px;">
        <div class="BackToHome col-md-3">
            <i class="fa fa-chevron-left"></i>&nbsp;<a href="/">zur Startseite</a>
        </div>
        <div class="form-horizontal col-md-9">
            <fieldset>
                <legend>Registriere dich</legend>

                <% Html.ValidationSummary(true, "Bitte überprüfe deine Eingaben"); %>
                
                <div class="alert alert-info">
                    Wir gehen sorgfältig mit deinen Daten um.
                </div>
       
                <div class="form-group">
                    <%: Html.LabelFor(model => model.Name, new { @class = "col-sm-2 control-label" }) %>
                    <div class="col-sm-3">
                        <%: Html.TextBoxFor(model => model.Name, new { @class="form-control" }) %>
                        <%: Html.ValidationMessageFor(model => model.Name) %>
                    </div>
                </div>
                
                <div class="form-group">
                    <%: Html.LabelFor(model => model.Email, new { @class = "col-sm-2 control-label" }) %>
                    <div class="col-sm-3">
                        <%: Html.TextBoxFor(model => model.Email, new { @class="form-control" }) %>
                        <%: Html.ValidationMessageFor(model => model.Email) %>
                    </div>
                </div>

                <div class="form-group">
                    <%: Html.LabelFor(model => model.Password, new { @class = "col-sm-2 control-label" }) %>
                    <div class="col-sm-3">
                        <%: Html.PasswordFor(model => model.Password, new { @class="form-control" }) %>
                        <%: Html.ValidationMessageFor(model => model.Password) %>
                    </div>
                </div>

<%-- AGB-Checkbox: Solange es keine AGBs gibt rauslassen; Akzeptieren der AGBs wird beim Registrieren noch nicht geprüft.

                    <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <%: Html.ValidationMessageFor(model => model.TermsAndConditionsApproved) %>
                        <label class="checkbox" style="white-space:nowrap;">
                            <%: Html.CheckBoxFor(model => model.TermsAndConditionsApproved, new { @class="" }) %>
                            Bitte bestätige unsere AGBs.
                        </label>
                    </div>
                </div>
--%>
                
                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10" style="border-top:0px; background-color:white;">
                        <input type="submit" value="Registrieren" class="btn btn-primary" />&nbsp;
                        <%: Html.ActionLink("Ich bin schon Benutzer!", Links.Login, Links.VariousController,
                                           new {@style = "vertical-align:bottom; margin-left:20px;"}) %>                        

                    </div>
                </div>

            </fieldset>            
        </div>
    </div>
<% } %>

</asp:Content>

