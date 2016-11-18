<%@ Page Title="Registrieren" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="ViewPage<RegisterModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <%= Scripts.Render("~/Views/Welcome/Js/Validation.js") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% using (Html.BeginForm("Register", "Welcome", null, FormMethod.Post, new { id = "RegistrationForm", enctype = "multipart/form-data" }))
    {%>
    
    <div class="row" style="padding-top:30px;">
        <div class="BackToHome col-md-3">
            <a href="/"><i class="fa fa-chevron-left">&nbsp;</i>Zur Startseite</a>
        </div>
        <div class="form-horizontal col-md-9">
            <fieldset>
                <legend>Jetzt registrieren. memucho ist kostenlos.</legend>

                <%= Html.ValidationSummary(true, "Bitte überprüfe deine Eingaben") %>
                
                <div class="alert alert-info">
                    <i class="fa fa-clock-o"></i> <b>Noch 20 Sekunden</b> und du kannst memucho nutzen :-)
                </div>
                
                <div class="form-group" style="margin-top: -5px; margin-bottom: 15px; padding-left: 12px;">
                    <div class="col-sm-12;" style="font-style: italic">
                        Wir gehen sorgfältig mit deinen Daten um. 
                        Wir schützen und respektieren deine Privatsphäre.
                    </div>
                </div>
       
                <div class="form-group">
                    <%: Html.LabelFor(model => model.Name, new { @class = "col-sm-2 control-label" }) %>
                    <div class="col-sm-6">
                        <%: Html.TextBoxFor(model => model.Name, new { @class="form-control", placeholder = Model.Name }) %>
                        <%: Html.ValidationMessageFor(model => model.Name) %>
                    </div>
                </div>
                
                <div class="form-group">
                    <%: Html.LabelFor(model => model.Email, new { @class = "col-sm-2 control-label" }) %>
                    <div class="col-sm-6">
                        <%: Html.TextBoxFor(model => model.Email, new { @class="form-control" }) %>
                        <%: Html.ValidationMessageFor(model => model.Email) %>
                    </div>
                </div>

                <div class="form-group">
                    <%: Html.LabelFor(model => model.Password, new { @class = "col-sm-2 control-label" }) %>
                    <div class="col-sm-6">
                        <%: Html.PasswordFor(model => model.Password, new { @class="form-control" }) %>
                        <%: Html.ValidationMessageFor(model => model.Password) %>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <label class="checkbox" style="white-space:nowrap;">
                            <%: Html.CheckBoxFor(model => model.TermsAndConditionsApproved, new { @class="" }) %>
                            Ich akzeptiere die <%= Html.ActionLink("Nutzungsbedingungen (AGBs)", Links.TermsAndConditions, Links.VariousController)%>.
                        </label>
                        <%: Html.ValidationMessageFor(model => model.TermsAndConditionsApproved) %>
                    </div>
                </div>
                
                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-6" style="border-top:0px; margin-top: 10px;">
                        
                        <a href="<%= Url.Action("Login", "Welcome") %>" class="btn btn-link">Ich bin schon Nutzer!</a>
                        <a href="#" onclick="$(this).closest('form').submit(); return false;" class="btn btn-success"><i class="fa fa-chevron-circle-right">&nbsp;</i> Jetzt Registrieren</a>

                    </div>
                </div>

            </fieldset>            
        </div>
    </div>
<% } %>

</asp:Content>

